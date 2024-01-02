using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using TopModel.Core.FileModel;
using TopModel.Core.Loaders;
using TopModel.Utils;

namespace TopModel.Core;

public class ModelStore
{
    private readonly ModelConfig _config;
    private readonly IMemoryCache _fsCache;
    private readonly ILogger<ModelStore> _logger;
    private readonly ModelFileLoader _modelFileLoader;

    private readonly Dictionary<string, ModelFile> _modelFiles = new();
    private readonly IEnumerable<IModelWatcher> _modelWatchers;
    private readonly HashSet<string> _pendingUpdates = new();

    private readonly object _puLock = new();
    private readonly TopModelLock _topModelLock = new();

    private readonly TranslationStore _translationStore;

    private LoggingScope? _storeConfig;

    public ModelStore(IMemoryCache fsCache, ModelFileLoader modelFileLoader, ILogger<ModelStore> logger, ModelConfig config, IEnumerable<IModelWatcher> modelWatchers, TranslationStore translationStore)
    {
        _config = config;
        _fsCache = fsCache;
        _logger = logger;
        _modelFileLoader = modelFileLoader;
        _translationStore = translationStore;
        _modelWatchers = modelWatchers.Where(mw => !mw.Disabled);

        var lockFile = new FileInfo(Path.Combine(_config.ModelRoot, _config.LockFileName));
        if (lockFile.Exists)
        {
            try
            {
                using var file = lockFile.OpenText();
                _topModelLock = new FileChecker().Deserialize<TopModelLock>(file);
            }
            catch
            {
                logger.LogError($"Erreur à la lecture du fichier {_config.LockFileName}. Merci de rétablir la version générée automatiquement.");
            }
        }
    }

    public event Action<bool>? OnResolve;

    public bool DisableLockfile { get; set; }

    public IEnumerable<Class> Classes => _modelFiles.SelectMany(mf => mf.Value.Classes).Distinct();

    public IEnumerable<Endpoint> Endpoints => _modelFiles.SelectMany(mf => mf.Value.Endpoints).Distinct();

    public IEnumerable<DataFlow> DataFlows => _modelFiles.SelectMany(mf => mf.Value.DataFlows).Distinct();

    public IDictionary<string, Domain> Domains => _modelFiles.SelectMany(mf => mf.Value.Domains)
        .DistinctBy(d => (string)d.Name)
        .ToDictionary(d => (string)d.Name, d => d);

    public IList<Converter> Converters => _modelFiles.SelectMany(mf => mf.Value.Converters).ToList();

    public IEnumerable<Decorator> Decorators => _modelFiles.SelectMany(mf => mf.Value.Decorators).Distinct();

    public IEnumerable<ModelFile> Files => _modelFiles.Values;

    public IEnumerable<Class> GetAvailableClasses(ModelFile file)
    {
        return GetDependencies(file).SelectMany(m => m.Classes)
            .Concat(file.Classes);
    }

    public IEnumerable<DataFlow> GetAvailableDataFlows(ModelFile file)
    {
        return GetDependencies(file).SelectMany(m => m.DataFlows).Concat(file.DataFlows);
    }

    public IEnumerable<Decorator> GetAvailableDecorators(ModelFile file)
    {
        return GetDependencies(file).SelectMany(m => m.Decorators).Concat(file.Decorators);
    }

    public Dictionary<string, Class> GetReferencedClasses(ModelFile modelFile)
    {
        var dependencies = GetDependencies(modelFile).ToList();
        return dependencies
            .SelectMany(m => m.Classes)
            .Concat(modelFile.Classes)
            .Distinct()
            .GroupBy(c => c.Name.Value)
            .ToDictionary(c => c.Key, c => c.First());
    }

    public IDisposable? LoadFromConfig(bool watch = false, LoggingScope? storeConfig = null)
    {
        _storeConfig = storeConfig;

        using var scope = _logger.BeginScope(_storeConfig!);

        _topModelLock.Init(_logger);

        var watchers = _modelWatchers.Select(mw => mw.FullName.Split("@")).GroupBy(split => split[0]).Select(grp => $"{grp.Key}@{{{string.Join(",", grp.Select(split => split[1]))}}}");
        _logger.LogInformation($"Watchers enregistrés : \n                          - {string.Join("\n                          - ", watchers.OrderBy(x => x))}");

        FileSystemWatcher? fsWatcher = null;
        if (watch)
        {
            fsWatcher = new FileSystemWatcher(_config.ModelRoot, "*.tmd");
            fsWatcher.Changed += OnFSChangedEvent;
            fsWatcher.Created += OnFSChangedEvent;
            fsWatcher.Deleted += OnFSChangedEvent;
            fsWatcher.Renamed += OnFSChangedEvent;
            fsWatcher.IncludeSubdirectories = true;
            fsWatcher.EnableRaisingEvents = true;
        }

        _modelFiles.Clear();
        _pendingUpdates.Clear();

        _logger.LogInformation("Chargement du modèle...");

        var files = Directory.EnumerateFiles(_config.ModelRoot, "*.tmd", SearchOption.AllDirectories);

        lock (_puLock)
        {
            foreach (var file in files)
            {
                LoadFile(file);
            }
        }

        LoadTranslations();
        TryApplyUpdates();

        return fsWatcher;
    }

    public void OnModelFileChange(string filePath, string? content = null)
    {
        _logger.LogInformation(string.Empty);
        _logger.LogInformation($"Modifié:  {filePath.ToRelative()}");

        lock (_puLock)
        {
            LoadFile(filePath, content);
        }

        TryApplyUpdates();
    }

    public void TryApplyUpdates()
    {
        if (!_pendingUpdates.Any())
        {
            return;
        }

        lock (_puLock)
        {
            try
            {
                var referenceErrors = new List<ModelError>();

                var affectedFiles = _pendingUpdates.Select(pu => _modelFiles.TryGetValue(pu, out var mf) ? mf : null).Any(mf => (mf?.Domains.Any() ?? false) || (mf?.Converters.Any() ?? false))
                    ? _modelFiles.Values
                    : GetAffectedFiles(_pendingUpdates).Distinct();

                IList<ModelFile> sortedFiles = new List<ModelFile>(1);
                try
                {
                    sortedFiles = CoreUtils.Sort(affectedFiles, f => GetDependencies(f).Where(d => affectedFiles.Any(af => af.Name == d.Name)));
                }

                // Dépendance circulaire.
                catch (ModelException e) when (e.ModelError is not null)
                {
                    referenceErrors.Add(e.ModelError);
                }

                foreach (var affectedFile in sortedFiles)
                {
                    referenceErrors.AddRange(ResolveReferences(affectedFile));
                }

                referenceErrors.AddRange(GetGlobalErrors());

                foreach (var modelWatcher in _modelWatchers)
                {
                    modelWatcher.OnErrors(affectedFiles
                        .Select(file => (file, errors: referenceErrors.Where(e => e.File == file && !_config.NoWarn.Contains(e.ModelErrorType))))
                        .ToDictionary(i => i.file, i => i.errors));
                }

                foreach (var error in referenceErrors.Where(e => e.IsError))
                {
                    _logger.LogError(error.ToString());
                }

                foreach (var error in referenceErrors.Where(e => !e.IsError && !_config.NoWarn.Contains(e.ModelErrorType)))
                {
                    _logger.LogWarning(error.ToString());
                }

                var hasError = referenceErrors.Any(r => r.IsError);
                OnResolve?.Invoke(hasError);

                if (hasError)
                {
                    throw new ModelException("Erreur lors de la lecture du modèle.");
                }
                else
                {
                    _logger.LogInformation("Modèle chargé avec succès.");
                }

                foreach (var modelWatcher in _modelWatchers)
                {
                    modelWatcher.OnFilesChanged(sortedFiles, _storeConfig);
                }

                var generatedFiles = _modelWatchers.Where(m => m.GeneratedFiles != null).SelectMany(m => m.GeneratedFiles!);
                if (generatedFiles.Any() && !DisableLockfile)
                {
                    _topModelLock.Update(_config.ModelRoot, _config.LockFileName, _logger, generatedFiles);
                }

                _logger.LogInformation($"Mise à jour terminée avec succès.");

                _pendingUpdates.Clear();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }
    }

    private IEnumerable<ModelFile> GetAffectedFiles(IEnumerable<string> fileNames, HashSet<string>? foundFiles = null)
    {
        foundFiles ??= new();

        foreach (var file in _modelFiles.Values.Where(f => fileNames.Contains(f.Name) || f.Uses.Any(d => fileNames.Contains(d.ReferenceName))))
        {
            if (!foundFiles.Contains(file.Name))
            {
                foundFiles.Add(file.Name);
                yield return file;

                foreach (var use in GetAffectedFiles(new[] { file.Name }, foundFiles))
                {
                    yield return use;
                }
            }
        }
    }

    private IEnumerable<ModelFile> GetDependencies(ModelFile modelFile)
    {
        return modelFile.Uses
            .Select(dep => _modelFiles.TryGetValue(dep.ReferenceName, out var depFile) ? depFile : null!)
            .Where(dep => dep != null)
            .Concat(Files.Where(f => f != modelFile && f.Converters.Any() && modelFile.Classes.Any(c => c.FromMappers.Any() || c.ToMappers.Any())))
            .Concat(Files.Where(f => f != modelFile && f.Domains.Any() && (!modelFile.Domains.Any() || modelFile.Domains.Any(d => d.AsDomainReferences.Any()))));
    }

    private IEnumerable<ModelError> GetGlobalErrors()
    {
        foreach (var g in Files.SelectMany(f => f.Domains).GroupBy(f => (string)f.Name).Where(g => g.Count() > 1))
        {
            foreach (var domain in g.Skip(1))
            {
                yield return new ModelError(domain, $"Le domaine '{domain}' est déjà défini") { ModelErrorType = ModelErrorType.TMD0007 };
            }
        }

        foreach (var converter in Converters)
        {
            var dup = Converters.FirstOrDefault(c => Converters.IndexOf(c) < Converters.IndexOf(converter) && c.Conversions.Intersect(converter.Conversions).Any());
            if (dup != null)
            {
                foreach (var (from, to) in dup.Conversions.Intersect(converter.Conversions))
                {
                    yield return new ModelError(converter, $"La définition de la conversion entre {from.Name} et {to.Name} est déjà définie dans un autre converter") { ModelErrorType = ModelErrorType.TMD1022 };
                }
            }
        }

        foreach (var classe in Classes.Where(c => c.Trigram != null && Classes.Any(u => u.Trigram == c.Trigram && u != c)))
        {
            yield return new ModelError(classe.ModelFile, $"Le trigram '{classe.Trigram}' est déjà utilisé dans la (les) classe(s) suivantes : {string.Join(", ", Classes.Where(u => u.Trigram == classe.Trigram && u != classe).Select(c => c.Name))}", classe.Trigram.GetLocation()) { IsError = false, ModelErrorType = ModelErrorType.TMD9002 };
        }

        foreach (var domain in Domains.Values.Where(domain => !this.GetDomainReferences(domain).Any()))
        {
            yield return new ModelError(domain, $"Le domaine '{domain.Name}' n'est pas utilisé.") { IsError = false, ModelErrorType = ModelErrorType.TMD9004 };
        }

        foreach (var decorator in Decorators.Where(decorator => !this.GetDecoratorReferences(decorator).Any()))
        {
            yield return new ModelError(decorator, $"Le décorateur '{decorator.Name}' n'est pas utilisé.") { IsError = false, ModelErrorType = ModelErrorType.TMD9005 };
        }

        foreach (var files in Files.GroupBy(file => new { file.Options.Endpoints.FileName, file.Namespace.Module }))
        {
            var endpoints = files.SelectMany(f => f.Endpoints);

            foreach (var endpoint in endpoints.Where((e, i) => files.SelectMany(f => f.Endpoints).Where((p, j) => p.Name == e.Name && j < i).Any()))
            {
                yield return new ModelError(endpoint, $"Le nom '{endpoint.Name}' est déjà utilisé.", endpoint.Name.GetLocation()) { IsError = true, ModelErrorType = ModelErrorType.TMD0003 };
            }

            if (files.Select(file => file.Options.Endpoints.Prefix).Distinct().Count() > 1)
            {
                foreach (var file in files)
                {
                    if (file.Options.Endpoints.Prefix != null)
                    {
                        yield return new ModelError(file, $"Le préfixe d'endpoint '{file.Options.Endpoints.Prefix}' doit être identique à celui de tous les fichiers de même nom et de même module.", file.Options.Endpoints.Prefix?.GetLocation()) { ModelErrorType = ModelErrorType.TMD1021 };
                    }
                    else
                    {
                        yield return new ModelError(file, $"Le fichier ne définit pas de préfixe d'endpoint alors que d'autres fichiers de même nom et de même module le font.") { ModelErrorType = ModelErrorType.TMD1021 };
                    }
                }
            }
        }
    }

    private void LoadFile(string filePath, string? content = null)
    {
        try
        {
            ModelFile? file = null;
            if (File.Exists(filePath))
            {
                file = _modelFileLoader.LoadModelFile(filePath, content);
            }

            if (file != null)
            {
                _modelFiles[file.Name] = file;
                _pendingUpdates.Add(file.Name);
            }
            else
            {
                var fileName = _config.GetFileName(filePath);
                _modelFiles.Remove(fileName);
                _pendingUpdates.Add(fileName);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
    }

    private void LoadTranslations()
    {
        _translationStore.Translations[_config.I18n.DefaultLang] = new Dictionary<string, string>();

        foreach (var lang in _config.I18n.Langs)
        {
            var langMap = new Dictionary<string, string>();
            var directoryPath = _config.I18n.RootPath.Replace("{lang}", lang);
            var exists = Directory.Exists(directoryPath);
            if (exists)
            {
                var files = Directory.GetFiles(directoryPath, "*.properties", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    var lines = File.ReadAllLines(file);
                    foreach (var line in lines)
                    {
                        if (line != null && line != string.Empty)
                        {
                            langMap[line.Split("=")[0]] = line.Split("=")[1];
                        }
                    }
                }
            }

            _translationStore.Translations[lang] = langMap;
        }
    }

    private void OnFSChangedEvent(object sender, FileSystemEventArgs e)
    {
        _fsCache.Set(e.FullPath, e, new MemoryCacheEntryOptions()
            .AddExpirationToken(new CancellationChangeToken(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token))
            .RegisterPostEvictionCallback((k, v, r, a) =>
            {
                if (r != EvictionReason.TokenExpired)
                {
                    return;
                }

                OnModelFileChange((string)k);
            }));
    }

    private IEnumerable<ModelError> ResolveReferences(ModelFile modelFile)
    {
        var nonExistingFiles = modelFile.Uses.Where(use => !_modelFiles.TryGetValue(use.ReferenceName, out var _));
        foreach (var use in nonExistingFiles)
        {
            yield return new ModelError(modelFile, $"Le fichier référencé '{use.ReferenceName}' est introuvable.", use) { ModelErrorType = ModelErrorType.TMD1007 };
        }

        var duplicatedUses = modelFile.Uses
            .GroupBy(u => u.ReferenceName)
            .Select(u => new { ReferenceName = u.Key, Count = u.Count() })
            .Where(r => r.Count > 1)
            .Select(u => u.ReferenceName);

        foreach (var use in modelFile.Uses.Where(u => duplicatedUses.Contains(u.ReferenceName)).Skip(1))
        {
            yield return new ModelError(modelFile, $"L'import '{use.ReferenceName}' ne doit être spécifié qu'une seule fois", use) { IsError = true, ModelErrorType = ModelErrorType.TMD0002 };
        }

        var dependencies = GetDependencies(modelFile).ToList();

        var referencedClassesRaw = dependencies
            .SelectMany(m => m.Classes)
            .Concat(modelFile.Classes)
            .Distinct();

        var duplicateClasses = referencedClassesRaw
            .GroupBy(c => c.Name.Value)
            .Where(g => g.Count() > 1)
            .Select(g => g.OrderByDescending(c => (c.ModelFile == modelFile ? 1_000_000 : 0) + c.Name.Location.Start.Line).First());

        foreach (var classe in duplicateClasses.Where(c => c.ModelFile == modelFile))
        {
            yield return new ModelError(classe, $"La classe '{classe}' est définie plusieurs fois dans le fichier ou une de ses dépendences.", classe.Name.Location) { ModelErrorType = ModelErrorType.TMD0005 };
        }

        var referencedClasses = referencedClassesRaw
            .Where(c => !duplicateClasses.Select(c => c.Name.Value).Contains(c.Name.Value))
            .ToDictionary(c => c.Name.Value, c => c);

        var referencedDecorators = dependencies
            .SelectMany(m => m.Decorators)
            .Concat(modelFile.Decorators)
            .Distinct()
            .ToDictionary(d => (string)d.Name, c => c);

        var referencedDataFlowsRaw = dependencies
             .SelectMany(m => m.DataFlows)
             .Concat(modelFile.DataFlows)
             .Distinct();

        var duplicateDataFlows = referencedDataFlowsRaw
            .GroupBy(c => c.Name.Value)
            .Where(g => g.Count() > 1)
            .Select(g => g.OrderByDescending(c => (c.ModelFile == modelFile ? 1_000_000 : 0) + c.Name.Location.Start.Line).First());

        foreach (var dataFlow in duplicateDataFlows.Where(c => c.ModelFile == modelFile))
        {
            yield return new ModelError(dataFlow, $"Le flux de données '{dataFlow}' est défini plusieurs fois dans le fichier ou une de ses dépendences.", dataFlow.Name.Location) { ModelErrorType = ModelErrorType.TMD0008 };
        }

        var referencedDataFlows = referencedDataFlowsRaw
            .Where(c => !duplicateDataFlows.Select(c => c.Name.Value).Contains(c.Name.Value))
            .ToDictionary(c => c.Name.Value, c => c);

        // Résolution des "asDomains" sur les domaines
        foreach (var domain in modelFile.Domains)
        {
            foreach (var (asName, domainReference) in domain.AsDomainReferences)
            {
                if (!Domains.TryGetValue(domainReference.ReferenceName, out var asDomain))
                {
                    yield return new ModelError(domain, "Le domaine '{0}' est introuvable.", domainReference) { ModelErrorType = ModelErrorType.TMD1005 };
                    continue;
                }

                domain.AsDomains[asName] = asDomain;
            }
        }

        // Résolution des "extends" sur les classes.
        foreach (var classe in modelFile.Classes.Where(c => c.ExtendsReference != null))
        {
            if (classe.Abstract)
            {
                yield return new ModelError(classe, $"Impossible de définir un 'extends' sur la classe '{classe}' abstraite.", classe.ExtendsReference!) { ModelErrorType = ModelErrorType.TMD1026 };
                continue;
            }

            if (!referencedClasses.TryGetValue(classe.ExtendsReference!.ReferenceName, out var extends))
            {
                yield return new ModelError(classe, "La classe '{0}' est introuvable dans le fichier ou l'une de ses dépendances.", classe.ExtendsReference!) { ModelErrorType = ModelErrorType.TMD1002 };
                continue;
            }

            if (extends.Abstract)
            {
                yield return new ModelError(classe, $"Impossible de définir la classe '{extends}' abstraite comme 'extends' sur la classe '{classe}'.", classe.ExtendsReference!) { ModelErrorType = ModelErrorType.TMD1026 };
                continue;
            }

            if (extends.PrimaryKey.Count() > 1)
            {
                yield return new ModelError(classe, $"Impossible de définir la classe '{extends}' comme 'extends' sur la classe '{classe}' car elle a une clé primaire composite.", classe.ExtendsReference!) { ModelErrorType = ModelErrorType.TMD1030 };
                continue;
            }

            classe.Extends = extends;
        }

        // Résolution des décorateurs sur les classes.
        foreach (var classe in modelFile.Classes.Where(c => c.DecoratorReferences.Any()))
        {
            classe.Decorators.Clear();

            var isError = false;
            foreach (var decoratorRef in classe.DecoratorReferences)
            {
                if (!referencedDecorators.TryGetValue(decoratorRef.ReferenceName, out var decorator))
                {
                    isError = true;
                    yield return new ModelError(classe, $"Le décorateur '{decoratorRef.ReferenceName}' est introuvable dans le fichier ou l'une de ses dépendances.", decoratorRef) { ModelErrorType = ModelErrorType.TMD1008 };
                }
                else
                {
                    if (classe.Decorators.Any(d => d.Decorator == decorator))
                    {
                        isError = true;
                        yield return new ModelError(classe, $"Le décorateur '{decoratorRef.ReferenceName}' est déjà présent dans la liste des décorateurs de la classe '{classe}'.", decoratorRef) { ModelErrorType = ModelErrorType.TMD1009 };
                    }
                    else
                    {
                        if (decorator.Implementations.Any(impl => impl.Value.Extends != null && (classe.Extends != null || classe.Decorators.Any(d => d.Decorator.Implementations.TryGetValue(impl.Key, out var dImpl) && dImpl.Extends != null))))
                        {
                            isError = true;
                            yield return new ModelError(classe, $"Impossible d'appliquer le décorateur '{decoratorRef.ReferenceName}' à la classe '{classe}' : seul un 'extends' peut être spécifié.", decoratorRef) { ModelErrorType = ModelErrorType.TMD1010 };
                        }

                        classe.Decorators.Add((decorator, decoratorRef.ParameterReferences.Select(p => p.ReferenceName).ToArray()));
                    }
                }
            }

            if (isError)
            {
                continue;
            }
        }

        // Résolution des décorateurs sur les endpoints.
        foreach (var endpoint in modelFile.Endpoints.Where(c => c.DecoratorReferences.Any()))
        {
            endpoint.Decorators.Clear();

            var isError = false;
            foreach (var decoratorRef in endpoint.DecoratorReferences)
            {
                if (!referencedDecorators.TryGetValue(decoratorRef.ReferenceName, out var decorator))
                {
                    isError = true;
                    yield return new ModelError(endpoint, $"Le décorateur '{decoratorRef.ReferenceName}' est introuvable dans le fichier ou l'une de ses dépendances.", decoratorRef) { ModelErrorType = ModelErrorType.TMD1008 };
                }
                else
                {
                    if (endpoint.Decorators.Any(d => d.Decorator == decorator))
                    {
                        isError = true;
                        yield return new ModelError(endpoint, $"Le décorateur '{decoratorRef.ReferenceName}' est déjà présent dans la liste des décorateurs du endpoint '{endpoint}'.", decoratorRef) { ModelErrorType = ModelErrorType.TMD1009 };
                    }
                    else
                    {
                        endpoint.Decorators.Add((decorator, decoratorRef.ParameterReferences.Select(p => p.ReferenceName).ToArray()));
                    }
                }
            }

            if (isError)
            {
                continue;
            }
        }

        // Résolutions des références sur les propriétés (hors alias).
        // On ne touche pas aux propriétés liées à une classe et un décorateur en même temps car
        // ces propriétés sont déjà résolues sur les décorateurs avant d'être recopiées sur les classes.
        foreach (var prop in modelFile.Properties.Where(p => p.Decorator is null || p.Class is null))
        {
            switch (prop)
            {
                case RegularProperty rp:
                    if (rp.DomainReference == null || !Domains.TryGetValue(rp.DomainReference.ReferenceName, out var domain))
                    {
                        yield return new ModelError(rp, "Le domaine '{0}' est introuvable.", rp.DomainReference) { ModelErrorType = ModelErrorType.TMD1005 };
                        break;
                    }

                    rp.Domain = domain;
                    rp.DomainParameters = rp.DomainReference.ParameterReferences.Select(p => p.ReferenceName).ToArray();
                    break;

                case AssociationProperty ap:
                    if (!referencedClasses.TryGetValue(ap.Reference.ReferenceName, out var association))
                    {
                        yield return new ModelError(ap, "La classe '{0}' est introuvable dans le fichier ou l'une de ses dépendances.", ap.Reference) { ModelErrorType = ModelErrorType.TMD1002 };
                        break;
                    }

                    if (ap.PropertyReference == null && !association.ExtendedProperties.Any(p => p.PrimaryKey))
                    {
                        yield return new ModelError(ap, "La classe '{0}' doit avoir au moins une clé primaire pour être référencée dans une association.", ap.Reference) { ModelErrorType = ModelErrorType.TMD1001 };
                        break;
                    }

                    if (ap.PropertyReference == null && association.Properties.Count(p => p.PrimaryKey) > 1 && ap.PropertyReference == null)
                    {
                        yield return new ModelError(ap, "La classe '{0}' a plusieurs clés primaires, vous devez obligatoirement référencer une propriété cible.", ap.Reference) { ModelErrorType = ModelErrorType.TMD1001 };
                        break;
                    }

                    ap.Association = association;
                    break;

                case CompositionProperty cp:
                    if (!referencedClasses.TryGetValue(cp.Reference.ReferenceName, out var composition))
                    {
                        yield return new ModelError(cp, "La classe '{0}' est introuvable dans le fichier ou l'une de ses dépendances.", cp.Reference) { ModelErrorType = ModelErrorType.TMD1002 };
                        break;
                    }

                    cp.Composition = composition;

                    if (cp.DomainReference != null)
                    {
                        if (!Domains.TryGetValue(cp.DomainReference.ReferenceName, out var cpDomain))
                        {
                            yield return new ModelError(cp, "Le domaine '{0}' est introuvable.", cp.DomainReference) { ModelErrorType = ModelErrorType.TMD1005 };
                            break;
                        }

                        cp.Domain = cpDomain;
                        cp.DomainParameters = cp.DomainReference.ParameterReferences.Select(p => p.ReferenceName).ToArray();
                    }

                    break;

                case AliasProperty alp when alp.DomainReference != null:
                    if (!Domains.TryGetValue(alp.DomainReference.ReferenceName, out var aliasDomain))
                    {
                        yield return new ModelError(alp, "Le domaine '{0}' est introuvable.", alp.DomainReference) { ModelErrorType = ModelErrorType.TMD1005 };
                        break;
                    }

                    alp.Domain = aliasDomain;
                    alp.DomainParameters = alp.DomainReference.ParameterReferences.Select(p => p.ReferenceName).ToArray();
                    break;
            }
        }

        // Reset des alias déjà résolus sur les classes.
        foreach (var classe in modelFile.Classes)
        {
            foreach (var alp in classe.Properties.OfType<AliasProperty>().ToList())
            {
                if (alp.OriginalAliasProperty is not null)
                {
                    var index = classe.Properties.IndexOf(alp);
                    classe.Properties.RemoveAt(index);
                    if (!classe.Properties.Contains(alp.OriginalAliasProperty))
                    {
                        classe.Properties.Insert(index, alp.OriginalAliasProperty);
                    }
                }
            }

            foreach (var alp in classe.FromMapperProperties.OfType<AliasProperty>().ToList())
            {
                if (alp.OriginalAliasProperty is not null)
                {
                    var index = alp.PropertyMapping.FromMapper.Params.IndexOf(alp.PropertyMapping);
                    alp.PropertyMapping.FromMapper.Params.RemoveAt(index);
                    if (!alp.PropertyMapping.FromMapper.Params.Contains(alp.OriginalAliasProperty.PropertyMapping))
                    {
                        alp.PropertyMapping.FromMapper.Params.Insert(index, new PropertyMapping
                        {
                            FromMapper = alp.PropertyMapping.FromMapper,
                            Property = alp.OriginalAliasProperty,
                            TargetProperty = alp.PropertyMapping.TargetProperty,
                            TargetPropertyReference = alp.PropertyMapping.TargetPropertyReference
                        });
                    }
                }
            }
        }

        // Reset des alias déjà résolus sur les endpoints.
        foreach (var endpoint in modelFile.Endpoints)
        {
            foreach (var alp in endpoint.Params.OfType<AliasProperty>().ToList())
            {
                if (alp.OriginalAliasProperty is not null)
                {
                    var index = endpoint.Params.IndexOf(alp);
                    endpoint.Params.RemoveAt(endpoint.Params.IndexOf(alp));
                    if (!endpoint.Params.Contains(alp.OriginalAliasProperty))
                    {
                        endpoint.Params.Insert(index, alp.OriginalAliasProperty);
                    }
                }
            }

            if (endpoint.Returns is AliasProperty ralp && ralp.OriginalAliasProperty is not null)
            {
                endpoint.Returns = ralp.OriginalAliasProperty;
            }
        }

        // Reset des alias déjà résolus sur les décorateurs.
        foreach (var decorator in modelFile.Decorators)
        {
            foreach (var alp in decorator.Properties.OfType<AliasProperty>().ToList())
            {
                if (alp.OriginalAliasProperty is not null)
                {
                    var index = decorator.Properties.IndexOf(alp);
                    decorator.Properties.RemoveAt(decorator.Properties.IndexOf(alp));
                    if (!decorator.Properties.Contains(alp.OriginalAliasProperty))
                    {
                        decorator.Properties.Insert(index, alp.OriginalAliasProperty);
                    }
                }
            }
        }

        /// <summary>
        /// Résout les alias d'une liste de propriétés.
        /// </summary>
        IEnumerable<ModelError> ResolveAliases(IEnumerable<AliasProperty> alps)
        {
            foreach (var alp in alps)
            {
                if (!referencedClasses!.TryGetValue(alp.Reference!.ReferenceName, out var aliasedClass))
                {
                    yield return new ModelError(alp, "La classe '{0}' est introuvable dans le fichier ou l'une de ses dépendances.", alp.Reference) { ModelErrorType = ModelErrorType.TMD1002 };
                    continue;
                }

                var shouldBreak = false;
                foreach (var propReference in alp.Reference.IncludeReferences.Concat(alp.Reference.ExcludeReferences))
                {
                    var aliasedProperty = aliasedClass.Properties.FirstOrDefault(p => p.Name == propReference.ReferenceName);
                    if (aliasedProperty == null)
                    {
                        yield return new ModelError(alp, $"La propriété '{{0}}' est introuvable sur la classe '{aliasedClass}'.", propReference) { ModelErrorType = ModelErrorType.TMD1004 };
                        shouldBreak = true;
                    }
                }

                foreach (var include in alp.Reference.IncludeReferences.Where((e, i) => alp.Reference.IncludeReferences.Where((p, j) => p.ReferenceName == e.ReferenceName && j < i).Any()))
                {
                    yield return new ModelError(modelFile, $"La propriété '{include.ReferenceName}' est déjà référencée dans la définition de l'alias.", include) { IsError = true, ModelErrorType = ModelErrorType.TMD0004 };
                    shouldBreak = true;
                }

                foreach (var exclude in alp.Reference.ExcludeReferences.Where((e, i) => alp.Reference.ExcludeReferences.Where((p, j) => p.ReferenceName == e.ReferenceName && j < i).Any()))
                {
                    yield return new ModelError(modelFile, $"La propriété '{exclude.ReferenceName}' est déjà référencée dans la définition de l'alias.", exclude) { IsError = true, ModelErrorType = ModelErrorType.TMD0004 };
                    shouldBreak = true;
                }

                if (shouldBreak)
                {
                    continue;
                }

                var propertiesToAlias =
                    (alp.Reference.IncludeReferences.Any()
                        ? alp.Reference.IncludeReferences.Select(p => aliasedClass.Properties.First(prop => prop.Name == p.ReferenceName))
                        : aliasedClass.Properties.Where(prop => !alp.Reference.ExcludeReferences.Select(p => p.ReferenceName).Contains(prop.Name)))
                    .Reverse()
                    .OfType<IFieldProperty>();

                foreach (var property in propertiesToAlias)
                {
                    var prop = alp.Clone(property, alp.Reference.IncludeReferences.FirstOrDefault(ir => ir.ReferenceName == property.Name));

                    if (prop.As != null && prop.Domain == null)
                    {
                        yield return new ModelError(modelFile, $"Le domaine '{prop.OriginalProperty?.Domain}' doit définir un domaine 'as' pour '{prop.As}' pour définir un alias '{prop.As}' sur la propriété '{prop.OriginalProperty}' de la classe '{prop.OriginalProperty?.Class}'", prop.PropertyReference ?? prop.Reference) { IsError = true, ModelErrorType = ModelErrorType.TMD1023 };
                    }

                    if (alp.Class != null)
                    {
                        var index = alp.Class.Properties.IndexOf(alp);
                        if (index >= 0)
                        {
                            alp.Class.Properties.Insert(index + 1, prop);
                        }
                    }
                    else if (alp.Endpoint?.Params.Contains(alp) ?? false)
                    {
                        var index = alp.Endpoint.Params.IndexOf(alp);
                        if (index >= 0)
                        {
                            alp.Endpoint.Params.Insert(index + 1, prop);
                        }
                    }
                    else if (alp.Endpoint?.Returns == alp)
                    {
                        alp.Endpoint.Returns = prop;
                    }
                    else if (alp.Decorator != null)
                    {
                        var index = alp.Decorator.Properties.IndexOf(alp);
                        if (index >= 0)
                        {
                            alp.Decorator.Properties.Insert(index + 1, prop);
                        }
                    }
                    else if (alp.PropertyMapping != null)
                    {
                        var index = alp.PropertyMapping.FromMapper.Params.IndexOf(alp.PropertyMapping);
                        if (index >= 0)
                        {
                            var mapping = new PropertyMapping
                            {
                                FromMapper = alp.PropertyMapping.FromMapper,
                                Property = prop,
                                TargetProperty = alp.PropertyMapping.TargetProperty,
                                TargetPropertyReference = alp.PropertyMapping.TargetPropertyReference,
                            };
                            prop.PropertyMapping = mapping;
                            alp.PropertyMapping.FromMapper.Params.Insert(index + 1, mapping);
                        }
                    }
                }

                if (alp.Class != null)
                {
                    alp.Class.Properties.Remove(alp);
                }
                else if (alp.Endpoint?.Params.Contains(alp) ?? false)
                {
                    alp.Endpoint.Params.Remove(alp);
                }
                else if (alp.PropertyMapping != null)
                {
                    alp.PropertyMapping.FromMapper.Params.Remove(alp.PropertyMapping);
                }
                else
                {
                    alp.Decorator?.Properties.Remove(alp);
                }
            }
        }

        // Résolution des alias des décorateurs
        foreach (var modelError in ResolveAliases(modelFile.Properties.OfType<AliasProperty>().Where(alp => alp.Decorator is not null && alp.Reference is not null)))
        {
            yield return modelError;
        }

        // Recopie des propriétés des décorateurs dans les classes.
        foreach (var classe in modelFile.Classes)
        {
            if (classe.Decorators.Any())
            {
                foreach (var prop in classe.Properties.Where(p => p.Decorator is not null).ToList())
                {
                    classe.Properties.Remove(prop);
                }

                foreach (var prop in classe.Decorators.SelectMany(d => d.Decorator.Properties))
                {
                    classe.Properties.Add(prop.CloneWithClassOrEndpoint(classe: classe));
                }
            }
        }

        // Recopie des propriétés des décorateurs dans les endpoints.
        foreach (var endpoint in modelFile.Endpoints)
        {
            if (endpoint.Decorators.Any())
            {
                foreach (var prop in endpoint.Params.Where(p => p.Decorator is not null).ToList())
                {
                    endpoint.Params.Remove(prop);
                }

                foreach (var prop in endpoint.Decorators.SelectMany(d => d.Decorator.Properties))
                {
                    endpoint.Params.Add(prop.CloneWithClassOrEndpoint(endpoint: endpoint));
                }
            }
        }

        // Résolution des alias des classes et endpoints.
        foreach (var modelError in ResolveAliases(modelFile.Properties.OfType<AliasProperty>().Where(alp => alp.Decorator is null && alp.Reference is not null)))
        {
            yield return modelError;
        }

        // Résolution des propriétés d'association (pour clé étrangère).
        foreach (var ap in modelFile.Classes.SelectMany(c => c.Properties.OfType<AssociationProperty>()).Where(ap => ap.Association != null))
        {
            if (ap.Type.IsToMany() && !(ap.Property?.Domain?.AsDomains.ContainsKey(ap.As) ?? false))
            {
                yield return new ModelError(ap, $@"Cette association ne peut pas avoir le type {ap.Type} car le domain {ap.Property?.Domain} ne contient pas de définition de domaine 'as' pour '{ap.As}'.", ap.Reference) { ModelErrorType = ModelErrorType.TMD1028 };
                continue;
            }

            if (ap.PropertyReference != null)
            {
                var referencedProperty = ap.Association.ExtendedProperties.OfType<IFieldProperty>().FirstOrDefault(p => p.Name == ap.PropertyReference!.ReferenceName);
                if (referencedProperty == null)
                {
                    yield return new ModelError(ap, $"La propriété '{{0}}' est introuvable sur la classe '{ap.Association}'.", ap.PropertyReference) { ModelErrorType = ModelErrorType.TMD1004 };
                }

                ap.Property = referencedProperty;
            }
        }

        // Résolution des clés d'unicités.
        foreach (var classe in modelFile.Classes.Where(c => c.UniqueKeyReferences.Any()))
        {
            classe.UniqueKeys.Clear();

            foreach (var ukRef in classe.UniqueKeyReferences)
            {
                var uk = new List<IFieldProperty>();
                classe.UniqueKeys.Add(uk);

                foreach (var ukPropRef in ukRef)
                {
                    var property = classe.Properties.OfType<IFieldProperty>().FirstOrDefault(p => p.Name == ukPropRef.ReferenceName);

                    if (property == null)
                    {
                        yield return new ModelError(classe, $"La propriété '{ukPropRef.ReferenceName}' n'existe pas sur la classe '{classe}'.", ukPropRef) { ModelErrorType = ModelErrorType.TMD1011 };
                    }
                    else
                    {
                        uk.Add(property);
                    }
                }
            }
        }

        // Résolution des valeurs
        foreach (var classe in modelFile.Classes.Where(c => c.ValueReferences.Any()))
        {
            classe.Values.Clear();

            foreach (var valueRef in classe.ValueReferences)
            {
                var classValue = new ClassValue { Name = valueRef.Key.ReferenceName, Class = classe, Reference = valueRef.Key };
                classe.Values.Add(classValue);

                foreach (var value in valueRef.Value)
                {
                    var property = classe.ExtendedProperties.OfType<IFieldProperty>().FirstOrDefault(p => p.Name == value.Key.ReferenceName);

                    if (property == null)
                    {
                        yield return new ModelError(classe, $"La propriété '{value.Key.ReferenceName}' n'existe pas sur la classe '{classe}'.", value.Key) { ModelErrorType = ModelErrorType.TMD1011 };
                    }
                    else
                    {
                        classValue.Value.Add(property, value.Value);
                    }
                }

                var missingRequiredProperties = classe.ExtendedProperties.OfType<IFieldProperty>()
                    .Where(p =>
                        p.Required
                        && (!(p.Domain?.AutoGeneratedValue ?? false) || p is AssociationProperty or AliasProperty { Property: AssociationProperty })
                        && !valueRef.Value.Any(v => v.Key.ReferenceName == p.Name));

                if (missingRequiredProperties.Any())
                {
                    yield return new ModelError(classe, $"La valeur '{valueRef.Key.ReferenceName}' n'initialise pas les propriétés obligatoires suivantes : {string.Join(", ", missingRequiredProperties.Select(p => p.Name))}.", valueRef.Key) { ModelErrorType = ModelErrorType.TMD1012 };
                }
            }
        }

        // Résolution des propriétés spéciales de classe.
        foreach (var classe in modelFile.Classes)
        {
            if (classe.DefaultPropertyReference != null)
            {
                classe.DefaultProperty = classe.ExtendedProperties.OfType<IFieldProperty>().FirstOrDefault(fp => fp.Name == classe.DefaultPropertyReference.ReferenceName);
                if (classe.DefaultProperty == null)
                {
                    yield return new ModelError(classe, $"La propriété '{classe.DefaultPropertyReference.ReferenceName}' n'existe pas sur la classe '{classe}'.", classe.DefaultPropertyReference) { ModelErrorType = ModelErrorType.TMD1011 };
                }
            }
            else
            {
                // Si la classe a une propriété "Label" ou "Libelle", alors on la considère par défaut (sic) comme propriété par défaut.
                classe.DefaultProperty = classe.ExtendedProperties.OfType<IFieldProperty>().FirstOrDefault(fp => fp.NamePascal == "Label" || fp.NamePascal == "Libelle");
            }

            if (classe.OrderPropertyReference != null)
            {
                classe.OrderProperty = classe.ExtendedProperties.OfType<IFieldProperty>().FirstOrDefault(fp => fp.Name == classe.OrderPropertyReference.ReferenceName);
                if (classe.OrderProperty == null)
                {
                    yield return new ModelError(classe, $"La propriété '{classe.OrderPropertyReference.ReferenceName}' n'existe pas sur la classe '{classe}'.", classe.OrderPropertyReference) { ModelErrorType = ModelErrorType.TMD1011 };
                }
            }
            else
            {
                // Si la classe a une propriété "Order" ou "Ordre", alors on la considère par défaut comme propriété d'ordre.
                classe.OrderProperty = classe.ExtendedProperties.OfType<IFieldProperty>().FirstOrDefault(fp => fp.NamePascal == "Order" || fp.NamePascal == "Ordre");
            }

            if (classe.FlagPropertyReference != null)
            {
                classe.FlagProperty = classe.ExtendedProperties.OfType<IFieldProperty>().FirstOrDefault(fp => fp.Name == classe.FlagPropertyReference.ReferenceName);
                if (classe.FlagProperty == null)
                {
                    yield return new ModelError(classe, $"La propriété '{classe.FlagPropertyReference.ReferenceName}' n'existe pas sur la classe '{classe}'.", classe.FlagPropertyReference) { ModelErrorType = ModelErrorType.TMD1011 };
                }
            }
            else
            {
                // Si la classe a une propriété "Flag", alors on la considère par défaut comme propriété de flag.
                classe.FlagProperty = classe.ExtendedProperties.OfType<IFieldProperty>().FirstOrDefault(fp => fp.NamePascal == "Flag");
            }
        }

        // Check de `reference` et `enum`
        foreach (var classe in modelFile.Classes.Where(c => c.Reference && c.ReferenceKey == null))
        {
            yield return new ModelError(classe, $"La classe '{classe}' doit avoir au moins une propriété non composée et au plus une clé primaire pour être définie comme `reference`.") { ModelErrorType = ModelErrorType.TMD0001 };
        }

        foreach (var classe in modelFile.Classes)
        {
            if (classe.EnumOverride != null)
            {
                if (classe.EnumOverride == "true" && (!classe.Values.Any() || classe.ReferenceKey == null))
                {
                    yield return new ModelError(classe, $"La classe '{classe}' doit avoir au moins une propriété non composée, au plus une clé primaire et au moins une `value` pour être définie comme `enum`.", classe.EnumOverride.Location) { ModelErrorType = ModelErrorType.TMD0006 };
                }
                else
                {
                    classe.Enum = classe.EnumOverride == "true";
                }
            }
            else
            {
                classe.Enum = classe.Values.Any() && classe.ReferenceKey != null && !(classe.ReferenceKey.Domain?.AutoGeneratedValue ?? false);
            }

            if (classe.Extends != null && classe.Enum != classe.Extends.Enum)
            {
                yield return new ModelError(classe, $"La classe '{classe}' et sa classe parente '{classe.Extends}' doivent toutes les deux être des `enum`.") { ModelErrorType = ModelErrorType.TMD1031 };
            }
        }

        // Résolution des domaines converters
        foreach (var converter in Converters)
        {
            converter.From.Clear();
            converter.To.Clear();

            foreach (var domain in Domains.Values)
            {
                domain.ConvertersFrom.Remove(converter);
                domain.ConvertersTo.Remove(converter);
            }

            foreach (var dom in converter.DomainsFromReferences)
            {
                if (!Domains.TryGetValue(dom.ReferenceName, out var domain))
                {
                    yield return new ModelError(converter, "Le domaine '{0}' est introuvable.", dom) { ModelErrorType = ModelErrorType.TMD1005 };
                    break;
                }

                converter.From.Add(domain);
            }

            foreach (var dom in converter.DomainsToReferences)
            {
                if (!Domains.TryGetValue(dom.ReferenceName, out var domain))
                {
                    yield return new ModelError(converter, "Le domaine '{0}' est introuvable.", dom) { ModelErrorType = ModelErrorType.TMD1005 };
                    break;
                }

                converter.To.Add(domain);
            }

            foreach (var f in converter.From)
            {
                f.ConvertersFrom.Add(converter);
            }

            foreach (var f in converter.To)
            {
                f.ConvertersTo.Add(converter);
            }
        }

        // Résolutions des mappers
        foreach (var classe in modelFile.Classes)
        {
            foreach (var mappings in classe.FromMappers.SelectMany(m => m.ClassParams).Concat(classe.ToMappers))
            {
                if (!referencedClasses.TryGetValue(mappings.ClassReference.ReferenceName, out var mappedClass))
                {
                    yield return new ModelError(classe, "La classe '{0}' est introuvable dans le fichier ou l'une de ses dépendances.", mappings.ClassReference) { ModelErrorType = ModelErrorType.TMD1002 };
                    continue;
                }

                mappings.Class = mappedClass;

                mappings.Mappings.Clear();

                foreach (var mapping in mappings.MappingReferences)
                {
                    var currentProperty = classe.ExtendedProperties.FirstOrDefault(p => p.Name == mapping.Key.ReferenceName);
                    if (currentProperty == null)
                    {
                        yield return new ModelError(classe, $"La propriété '{{0}}' est introuvable sur la classe '{classe}'.", mapping.Key) { ModelErrorType = ModelErrorType.TMD1004 };
                    }

                    if (mapping.Value.ReferenceName == "false")
                    {
                        continue;
                    }

                    var mappedProperty = mappedClass.ExtendedProperties.OfType<IFieldProperty>().FirstOrDefault(p => p.Name == mapping.Value.ReferenceName);
                    if (mappedProperty == null)
                    {
                        yield return new ModelError(classe, $"La propriété '{{0}}' est introuvable sur la classe '{mappedClass}'.", mapping.Value) { ModelErrorType = ModelErrorType.TMD1004 };
                    }

                    if (currentProperty != null && mappedProperty != null)
                    {
                        mappings.Mappings.Add(currentProperty, mappedProperty);

                        if (mappings.To && mappedProperty.Readonly)
                        {
                            yield return new ModelError(classe, $"La propriété '{mappedProperty.Name}' ne peut pas être la cible d'un mapping car elle a été marquée comme 'readonly'.", mapping.Value) { ModelErrorType = ModelErrorType.TMD1024 };
                        }
                        else if (!mappings.To && currentProperty.Readonly)
                        {
                            yield return new ModelError(classe, $"La propriété '{currentProperty.Name}' ne peut pas être la cible d'un mapping car elle a été marquée comme 'readonly'.", mapping.Key) { ModelErrorType = ModelErrorType.TMD1024 };
                        }

                        if (currentProperty is IFieldProperty fp)
                        {
                            if (fp.Domain != mappedProperty.Domain
                                && !Converters.Any(c => c.From.Any(cf => cf == (mappings.To ? fp.Domain : mappedProperty.Domain)) && c.To.Any(ct => ct == (mappings.To ? mappedProperty.Domain : fp.Domain))))
                            {
                                yield return new ModelError(classe, $"La propriété '{mappedProperty.Name}' ne peut pas être mappée à '{currentProperty.Name}' car elle n'a pas le même domaine ('{mappedProperty.Domain.Name}' au lieu de '{fp.Domain.Name}') et qu'il n'existe pas de convertisseur entre les deux.", mapping.Value) { ModelErrorType = ModelErrorType.TMD1014 };
                            }
                        }
                        else if (currentProperty is CompositionProperty cp)
                        {
                            var mappedAp = mappedProperty switch
                            {
                                AssociationProperty ap => ap,
                                AliasProperty { Property: AssociationProperty ap } => ap,
                                _ => null
                            };

                            if (mappedAp == null)
                            {
                                yield return new ModelError(classe, $"La propriété '{mappedProperty.Name}' ne peut pas être mappée à la composition '{currentProperty.Name}' car ce n'est pas une association.", mapping.Value) { ModelErrorType = ModelErrorType.TMD1017 };
                            }
                            else if (!_config.UseLegacyAssociationCompositionMappers && (mappedAp.Type.IsToMany() || cp.Domain != null))
                            {
                                yield return new ModelError(classe, $"L'association '{mappedProperty.Name}' ne peut pas être mappée à la composition '{currentProperty.Name}' car l'association et la composition doivent toutes les deux être simples.", mapping.Value) { ModelErrorType = ModelErrorType.TMD1018 };
                            }
                            else
                            {
                                var cpPks = cp.Composition.ExtendedProperties.OfType<IFieldProperty>().Where(p => p.PrimaryKey);
                                var cpPk = cpPks.Count() == 1 ? cpPks.Single() : null;

                                if (!_config.UseLegacyAssociationCompositionMappers && cpPk?.Domain != mappedAp.Domain && !Converters.Any(c => c.From.Any(cf => cf == cpPk?.Domain) && c.To.Any(ct => ct == mappedAp.Domain)))
                                {
                                    yield return new ModelError(classe, $"La propriété '{mappedProperty.Name}' ne peut pas être mappée à la composition '{currentProperty.Name}' car elle n'a pas le même domaine que la composition '{cp.Composition.Name}' ('{mappedProperty.Domain.Name}' au lieu de '{cpPk?.Domain?.Name ?? string.Empty}').", mapping.Value) { ModelErrorType = ModelErrorType.TMD1019 };
                                }
                            }
                        }
                    }
                }
            }

            foreach (var mapping in classe.FromMappers.SelectMany(fm => fm.PropertyParams))
            {
                if (mapping.Property != null)
                {
                    if (mapping.TargetPropertyReference != null)
                    {
                        var currentProperty = classe.ExtendedProperties.FirstOrDefault(p => p.Name == mapping.TargetPropertyReference.ReferenceName);
                        if (currentProperty == null)
                        {
                            yield return new ModelError(classe, $"La propriété '{{0}}' est introuvable sur la classe '{classe}'.", mapping.TargetPropertyReference) { ModelErrorType = ModelErrorType.TMD1004 };
                        }

                        mapping.TargetProperty = currentProperty;
                    }
                    else
                    {
                        var mappedProperty = classe.ExtendedProperties.FirstOrDefault(p => p.Name == mapping.Property.Name);
                        if (mappedProperty == null)
                        {
                            yield return new ModelError(classe, $"La propriété '{mapping.Property.Name}' est introuvable sur la classe '{classe}'.", mapping.Property.GetLocation()) { ModelErrorType = ModelErrorType.TMD1004 };
                        }

                        mapping.TargetProperty = mappedProperty;
                    }

                    if (mapping.TargetProperty != null)
                    {
                        if (mapping.TargetProperty is not CompositionProperty && mapping.Property is CompositionProperty)
                        {
                            yield return new ModelError(classe, $"La propriété '{mapping.Property.Name}' ne peut pas être une composition pour définir un mapping vers '{mapping.TargetProperty.Name}'.", mapping.Property.GetLocation()) { ModelErrorType = ModelErrorType.TMD1033 };
                        }

                        if (mapping.TargetProperty is CompositionProperty { Composition: Class cpClass } && (mapping.Property is not CompositionProperty || ((CompositionProperty)mapping.Property).Composition != cpClass))
                        {
                            yield return new ModelError(classe, $"La propriété '{mapping.Property.Name}' doit être une composition de la même classe que '{mapping.TargetProperty.Name}' pour définir un mapping entre les deux.", mapping.Property.GetLocation()) { ModelErrorType = ModelErrorType.TMD1032 };
                        }

                        if (mapping.Property.Domain != mapping.TargetProperty.Domain
                            && !Converters.Any(c => c.From.Any(cf => cf == mapping.Property.Domain) && c.To.Any(ct => ct == mapping.TargetProperty.Domain)))
                        {
                            yield return new ModelError(classe, $"La propriété '{mapping.Property.Name}' ne peut pas être mappée à '{mapping.TargetProperty.Name}' car elle n'a pas le même domaine ('{mapping.Property.Domain?.Name}' au lieu de '{mapping.TargetProperty.Domain?.Name}') et qu'il n'existe pas de convertisseur entre les deux.", mapping.Property.GetLocation()) { ModelErrorType = ModelErrorType.TMD1014 };
                        }
                    }
                }
            }

            foreach (var mapper in classe.FromMappers)
            {
                foreach (var param in mapper.Params.Where((e, i) => mapper.Params.Where((p, j) => p.GetName() == e.GetName() && j < i).Any()))
                {
                    yield return new ModelError(classe, $"Le nom '{param.GetName()}' est déjà utilisé.", param.GetLocation()) { ModelErrorType = ModelErrorType.TMD0003 };
                }

                var mappedProperties = mapper.Params.SelectMany(p => p.Match(
                    p => p.MappingReferences.Where(e => e.Value.ReferenceName != "false").Select(e => e.Key),
                    p =>
                    {
                        if (p.TargetPropertyReference != null)
                        {
                            return [p.TargetPropertyReference];
                        }

                        var propRef = p.Property.GetLocation() ?? new Reference();
                        propRef.ReferenceName = p.Property.Name;
                        return [propRef];
                    }));

                var hasDoublon = false;
                foreach (var mapping in mappedProperties.Where((e, i) => mappedProperties.Where((p, j) => p.ReferenceName == e.ReferenceName && j < i).Any()))
                {
                    hasDoublon = true;
                    yield return new ModelError(classe, $"La propriété '{mapping.ReferenceName}' est déjà initialisée dans ce mapper.", mapping) { ModelErrorType = ModelErrorType.TMD1015 };
                }

                if (!hasDoublon)
                {
                    var explicitMappings = mapper.ClassParams.SelectMany(p => p.Mappings)
                        .Concat(mapper.PropertyParams.Select(p => new KeyValuePair<IProperty, IProperty>(p.TargetProperty, p.Property)))
                        .ToList();

                    foreach (var param in mapper.ClassParams.Where(p => p.Class != null))
                    {
                        foreach (var property in classe.ExtendedProperties.OfType<AliasProperty>().Where(property => !property.Readonly && !explicitMappings.Any(m => m.Key == property) && !param.MappingReferences.Any(m => m.Key.ReferenceName == property.Name && m.Value.ReferenceName == "false")))
                        {
                            var matchingProperties = param.Class.ExtendedProperties.OfType<IFieldProperty>().Where(p => property.Property == p || p is AliasProperty alp && property == alp.Property || p is AliasProperty alp2 && property.Property == alp2.Property);
                            if (matchingProperties.Count() == 1)
                            {
                                var p = matchingProperties.First();
                                if (p.Domain != null && (p.Domain == property.Domain || Converters.Any(c => c.From.Any(cf => cf == p.Domain) && c.To.Any(ct => ct == property.Domain))))
                                {
                                    param.Mappings.Add(property, p);
                                }
                            }
                        }
                    }

                    var explicitAndAliasMappings = mapper.ClassParams.SelectMany(p => p.Mappings)
                        .Concat(mapper.PropertyParams.Select(p => new KeyValuePair<IProperty, IProperty>(p.TargetProperty, p.Property)))
                        .ToList();

                    foreach (var param in mapper.ClassParams.Where(p => p.Class != null))
                    {
                        foreach (var property in classe.ExtendedProperties.OfType<IFieldProperty>().Where(property => !property.Readonly && !explicitAndAliasMappings.Any(m => m.Key == property) && !param.MappingReferences.Any(m => m.Key.ReferenceName == property.Name && m.Value.ReferenceName == "false")))
                        {
                            foreach (var p in param.Class.ExtendedProperties.OfType<IFieldProperty>())
                            {
                                if (!param.Mappings.ContainsKey(property) && p.Name == property.Name && p.Domain != null && (p.Domain == property.Domain || Converters.Any(c => c.From.Any(cf => cf == p.Domain) && c.To.Any(ct => ct == property.Domain))))
                                {
                                    param.Mappings.Add(property, p);
                                }
                            }
                        }
                    }

                    var finalMappings = mapper.ClassParams.SelectMany(p => p.Mappings)
                        .Concat(mapper.PropertyParams.Select(p => new KeyValuePair<IProperty, IProperty>(p.TargetProperty, p.Property)))
                        .ToList();

                    foreach (var mapping in finalMappings.Where((e, i) => finalMappings.Where((p, j) => p.Key == e.Key && j < i).Any()))
                    {
                        yield return new ModelError(classe, $"Plusieurs propriétés de la classe peuvent être mappées sur '{mapping.Key.Name}' : {string.Join(", ", mapper.ClassParams.SelectMany(p => p.Mappings.Where(m => m.Key == mapping.Key).Select(m => $"'{p.Name}.{m.Value}'")))}.", mapper.GetLocation()) { ModelErrorType = ModelErrorType.TMD1016 };
                    }

                    foreach (var param in mapper.Params.Where((p, i) => p.GetRequired() && mapper.Params.Where((q, j) => !q.GetRequired() && j < i).Any()))
                    {
                        yield return new ModelError(classe, $"Le paramètre '{param.GetName()}' du mapper ne peut pas être obligatoire si l'un des paramètres précédents ne l'est pas.", param.GetLocation()) { ModelErrorType = ModelErrorType.TMD1034 };
                    }
                }
            }

            foreach (var mapper in classe.ToMappers.Where((e, i) => classe.ToMappers.Where((p, j) => p.Name == e.Name && j < i).Any()))
            {
                yield return new ModelError(classe, $"Le nom '{mapper.Name}' est déjà utilisé.", mapper.GetLocation()) { ModelErrorType = ModelErrorType.TMD0003 };
            }

            foreach (var mapper in classe.ToMappers.Where(m => m.Class != null))
            {
                var explicitMappings = mapper.Mappings.ToDictionary(p => p.Key, p => p.Value);

                foreach (var property in classe.ExtendedProperties.OfType<AliasProperty>().Where(property => !explicitMappings.ContainsKey(property) && !mapper.MappingReferences.Any(m => m.Key.ReferenceName == property.Name && m.Value.ReferenceName == "false")))
                {
                    var matchingProperties = mapper.Class.ExtendedProperties.OfType<IFieldProperty>().Where(p => property.Property == p || p is AliasProperty alp && property == alp.Property || p is AliasProperty alp2 && property.Property == alp2.Property);
                    if (matchingProperties.Count() == 1)
                    {
                        var p = matchingProperties.First();
                        if (!p.Readonly && p.Domain != null && (p.Domain == property.Domain || Converters.Any(c => c.From.Any(cf => cf == p.Domain) && c.To.Any(ct => ct == property.Domain))))
                        {
                            mapper.Mappings.Add(property, p);
                        }
                    }
                }

                var explicitAndAliasMappings = mapper.Mappings.ToDictionary(p => p.Key, p => p.Value);

                foreach (var property in classe.ExtendedProperties.OfType<IFieldProperty>().Where(property => !explicitAndAliasMappings.ContainsKey(property) && !mapper.MappingReferences.Any(m => m.Key.ReferenceName == property.Name && m.Value.ReferenceName == "false")))
                {
                    foreach (var p in mapper.Class.ExtendedProperties.OfType<IFieldProperty>())
                    {
                        if (p.Name == property.Name
                            && p.Domain != null
                            && (p.Domain == property.Domain
                                || Converters.Any(c => c.From.Any(cf => cf == p.Domain)
                                    && c.To.Any(ct => ct == property.Domain))))
                        {
                            if (!p.Readonly)
                            {
                                mapper.Mappings.Add(property, p);
                            }
                        }
                    }
                }

                foreach (var mapping in mapper.Mappings.Where((e, i) => mapper.Mappings.Where((p, j) => p.Value == e.Value && j < i).Any()))
                {
                    yield return new ModelError(classe, $"Plusieurs propriétés de la classe peuvent être mappées sur '{mapper.Class}.{mapping.Value?.Name}' : {string.Join(", ", mapper.Mappings.Where(p => p.Value == mapping.Value).Select(p => $"'{p.Key.Name}'"))}.", mapper.GetLocation()) { ModelErrorType = ModelErrorType.TMD1016 };
                }
            }
        }

        // Vérification qu'aucun mapper n'est vide
        foreach (var classe in modelFile.Classes)
        {
            foreach (var mapper in classe.FromMappers)
            {
                if (!mapper.ClassParams.SelectMany(p => p.Mappings).Any() && !mapper.PropertyParams.Any())
                {
                    yield return new ModelError(classe, "Aucun mapping n'a été trouvé sur ce mapper.", mapper.GetLocation()) { ModelErrorType = ModelErrorType.TMD1025 };
                }
            }

            foreach (var mapper in classe.ToMappers)
            {
                if (!mapper.Mappings.Any())
                {
                    yield return new ModelError(classe, "Aucun mapping n'a été trouvé sur ce mapper.", mapper.GetLocation()) { ModelErrorType = ModelErrorType.TMD1025 };
                }
            }
        }

        // Résolution des flux de données.
        foreach (var dataFlow in modelFile.DataFlows)
        {
            if (!referencedClasses.TryGetValue(dataFlow.ClassReference.ReferenceName, out var classe))
            {
                yield return new ModelError(dataFlow, "La classe '{0}' est introuvable dans le fichier ou l'une de ses dépendances.", dataFlow.ClassReference) { ModelErrorType = ModelErrorType.TMD1002 };
                continue;
            }

            dataFlow.Class = classe;

            if (dataFlow.ActivePropertyReference != null)
            {
                dataFlow.ActiveProperty = classe.ExtendedProperties.OfType<IFieldProperty>().FirstOrDefault(fp => fp.Name == dataFlow.ActivePropertyReference.ReferenceName);
                if (dataFlow.ActiveProperty == null)
                {
                    yield return new ModelError(dataFlow, $"La propriété '{dataFlow.ActivePropertyReference.ReferenceName}' n'existe pas sur la classe '{classe}'.", dataFlow.ActivePropertyReference) { ModelErrorType = ModelErrorType.TMD1011 };
                }
            }

            dataFlow.DependsOn.Clear();

            foreach (var dependsOnReference in dataFlow.DependsOnReference)
            {
                if (!referencedDataFlows.TryGetValue(dependsOnReference.ReferenceName, out var referencedDataFlow))
                {
                    yield return new ModelError(dataFlow, "Le flux de données '{0}' est introuvable dans le fichier ou l'une de ses dépendances.", dependsOnReference) { ModelErrorType = ModelErrorType.TMD2000 };
                    continue;
                }

                dataFlow.DependsOn.Add(referencedDataFlow);
            }

            foreach (var source in dataFlow.Sources)
            {
                if (!referencedClasses.TryGetValue(source.ClassReference.ReferenceName, out var sourceClass))
                {
                    yield return new ModelError(dataFlow, "La classe '{0}' est introuvable dans le fichier ou l'une de ses dépendances.", source.ClassReference) { ModelErrorType = ModelErrorType.TMD1002 };
                    continue;
                }

                source.Class = sourceClass;
                source.JoinProperties.Clear();

                foreach (var joinPropertyReference in source.JoinPropertyReferences)
                {
                    var joinProperty = sourceClass.ExtendedProperties.OfType<IFieldProperty>().FirstOrDefault(fp => fp.Name == joinPropertyReference.ReferenceName);
                    if (joinProperty == null)
                    {
                        yield return new ModelError(dataFlow, $"La propriété '{joinPropertyReference.ReferenceName}' n'existe pas sur la classe '{sourceClass}'.", joinPropertyReference) { ModelErrorType = ModelErrorType.TMD1011 };
                    }

                    source.JoinProperties.Add(joinProperty);
                }
            }
        }

        // Vérifications de cohérence sur les fichiers.
        foreach (var classe in modelFile.Classes)
        {
            foreach (var property in classe.ExtendedProperties.Where((e, i) => classe.ExtendedProperties.Where((p, j) => p.Name == e.Name && j < i).Any()))
            {
                yield return new ModelError(modelFile, $"Le nom '{property.Name}' est déjà utilisé.", property.Decorator is not null ? classe.DecoratorReferences.FirstOrDefault(dr => dr.ReferenceName == property.Decorator.Name) : property.GetLocation())
                {
                    IsError = true,
                    ModelErrorType = ModelErrorType.TMD0003
                };
            }

            foreach (var property in classe.Properties.OfType<AssociationProperty>().Where(p => (p.Association == classe || p.Association == classe.Extends) && string.IsNullOrEmpty(p.Role)))
            {
                yield return new ModelError(modelFile, $"Cette association sur la classe '{classe}' doit définir un rôle.", property.Decorator is not null ? classe.DecoratorReferences.FirstOrDefault(dr => dr.ReferenceName == property.Decorator.Name) : property.GetLocation())
                {
                    IsError = true,
                    ModelErrorType = ModelErrorType.TMD1029
                };
            }
        }

        foreach (var classe in modelFile.Classes.Where(c => c.Values.Any() && (c.IsPersistent || c.UniqueKeys.Any())))
        {
            var uks = new List<IEnumerable<IFieldProperty>>();
            uks.AddRange(classe.UniqueKeys);
            if (classe.IsPersistent)
            {
                uks.Add(classe.PrimaryKey.Any() ? classe.PrimaryKey : (classe.Extends?.PrimaryKey ?? Array.Empty<IFieldProperty>()));
            }

            foreach (var uk in uks)
            {
                if (!classe.Values.All(value => uk.All(p => value.Value.ContainsKey(p))))
                {
                    continue;
                }

                var ukValues = classe.Values.Select(value => string.Concat(uk.Select(p => value.Value[p]))).ToList();
                for (int i = 0; i < classe.Values.Count; i++)
                {
                    var ukValue = string.Concat(uk.Select(p => classe.Values[i].Value[p]));
                    if (ukValues.IndexOf(ukValue) < i)
                    {
                        var duplicateValue = classe.Values[i];
                        yield return new ModelError(duplicateValue, $"La valeur viole la contrainte d'unicité [{string.Join(", ", uk.Select(u => u.Name))}]", duplicateValue.Reference) { IsError = true, ModelErrorType = ModelErrorType.TMD1013 };
                    }
                }
            }
        }

        foreach (var use in modelFile.UselessImports.Where(u => dependencies.Any(d => d.Name == u.ReferenceName)))
        {
            yield return new ModelError(modelFile, $"L'import '{use.ReferenceName}' n'est pas utilisé.", use) { IsError = false, ModelErrorType = ModelErrorType.TMD9001 };
        }

        foreach (var endpoint in modelFile.Endpoints)
        {
            foreach (var queryParam in endpoint.GetQueryParams())
            {
                var index = endpoint.Params.IndexOf(queryParam);

                if (endpoint.Params.Any(param => !param.IsQueryParam() && endpoint.Params.IndexOf(param) > index))
                {
                    yield return new ModelError(endpoint, $"Le paramètre de requête '{queryParam.GetParamName()}' doit suivre tous les paramètres de route ou de body dans un endpoint.", queryParam.GetLocation()) { IsError = false, ModelErrorType = ModelErrorType.TMD9003 };
                }
            }

            var split = endpoint.FullRoute.Split("/");

            for (var i = 0; i < split.Length; i++)
            {
                if (split[i].StartsWith("{"))
                {
                    var routeParamName = split[i][1..^1];
                    var param = endpoint.Params.OfType<IFieldProperty>().SingleOrDefault(param => param.GetParamName() == routeParamName);

                    if (param == null)
                    {
                        yield return new ModelError(endpoint, $"Le endpoint '{endpoint.Name}' définit un paramètre '{routeParamName}' dans sa route qui n'existe pas dans la liste des paramètres.") { ModelErrorType = ModelErrorType.TMD1027 };
                    }
                }
            }
        }

        // Résolution des traductions
        foreach (var classe in modelFile.Classes)
        {
            foreach (var p in classe.Properties.OfType<IFieldProperty>().Where(p => p.Label != null))
            {
                _translationStore.Translations[_config.I18n.DefaultLang][p.ResourceKey] = p.Label!;
            }

            if (classe.DefaultProperty != null)
            {
                foreach (var r in classe.Values)
                {
                    if (r.Value.TryGetValue(classe.DefaultProperty, out var labelProperty))
                    {
                        _translationStore.Translations[_config.I18n.DefaultLang][r.ResourceKey] = labelProperty;
                    }
                }
            }
        }
    }
}