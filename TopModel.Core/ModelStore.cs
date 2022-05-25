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
    private readonly IEnumerable<IModelWatcher> _modelWatchers;

    private readonly Dictionary<string, ModelFile> _modelFiles = new();

    private readonly object _puLock = new();
    private readonly HashSet<string> _pendingUpdates = new();

    public ModelStore(IMemoryCache fsCache, ModelFileLoader modelFileLoader, ILogger<ModelStore> logger, ModelConfig config, IEnumerable<IModelWatcher> modelWatchers)
    {
        _config = config;
        _fsCache = fsCache;
        _logger = logger;
        _modelFileLoader = modelFileLoader;
        _modelWatchers = modelWatchers;
    }

    public IEnumerable<Class> Classes => _modelFiles.SelectMany(mf => mf.Value.Classes).Distinct();

    public IEnumerable<Endpoint> Endpoints => _modelFiles.SelectMany(mf => mf.Value.Endpoints).Distinct();

    public IDictionary<string, Domain> Domains => _modelFiles.SelectMany(mf => mf.Value.Domains).ToDictionary(d => d.Name, d => d);

    public IEnumerable<Decorator> Decorators => _modelFiles.SelectMany(mf => mf.Value.Decorators).Distinct();

    public IEnumerable<ModelFile> Files => _modelFiles.Values;

    public IEnumerable<Class> GetAvailableClasses(ModelFile file)
    {
        return GetDependencies(file).SelectMany(m => m.Classes)
            .Concat(file.Classes.Where(c => !file.ResolvedAliases.Contains(c)));
    }

    public IEnumerable<Decorator> GetAvailableDecorators(ModelFile file)
    {
        return GetDependencies(file).SelectMany(m => m.Decorators).Concat(file.Decorators);
    }

    public IDisposable? LoadFromConfig(bool watch = false)
    {
        foreach (var mw in _modelWatchers)
        {
            var sameGeneratorList = _modelWatchers.Where(m => m.Name == mw.Name).ToList();
            mw.Number = sameGeneratorList.IndexOf(mw) + 1;
        }

        _logger.LogInformation($"Watchers enregistrés : {string.Join(", ", _modelWatchers.Select(mw => mw.FullName))}");

        FileSystemWatcher? fsWatcher = null;
        if (watch)
        {
            _logger.LogInformation("Lancement du mode watch...");
            fsWatcher = new FileSystemWatcher(_config.ModelRoot, "*.tmd");
            fsWatcher.Changed += OnFSChangedEvent;
            fsWatcher.Created += OnFSChangedEvent;
            fsWatcher.Deleted += OnFSChangedEvent;
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

        TryApplyUpdates();

        return fsWatcher;
    }

    public Dictionary<string, Class> GetReferencedClasses(ModelFile modelFile)
    {
        var dependencies = GetDependencies(modelFile).ToList();
        return dependencies
            .SelectMany(m => m.Classes)
            .Concat(modelFile.Classes.Where(c => !modelFile.ResolvedAliases.Contains(c)))
            .Distinct()
            .ToDictionary(c => c.Name.Value, c => c);
    }

    public void OnModelFileChange(string filePath, string? content = null)
    {
        _logger.LogInformation(string.Empty);
        _logger.LogInformation($"Modifié: {filePath.ToRelative()}");

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

                var affectedFiles = _pendingUpdates.Select(pu => _modelFiles.TryGetValue(pu, out var mf) ? mf : null).Any(mf => mf?.Domains.Any() ?? false)
                    ? _modelFiles.Values
                    : GetAffectedFiles(_pendingUpdates).Distinct();

                IList<ModelFile> sortedFiles = new List<ModelFile>(1);
                try
                {
                    sortedFiles = SortUtils.Sort(affectedFiles, f => GetDependencies(f).Where(d => affectedFiles.Any(af => af.Name == d.Name)));
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

                if (referenceErrors.Any(r => r.IsError))
                {
                    throw new ModelException("Erreur lors de la lecture du modèle.");
                }

                foreach (var modelWatcher in _modelWatchers)
                {
                    modelWatcher.OnFilesChanged(sortedFiles);
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
           .Where(dep => dep != null);
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

        var fileClasses = modelFile.Classes.Where(c => !modelFile.ResolvedAliases.Contains(c));
        var referencedClasses = dependencies
            .SelectMany(m => m.Classes)
            .Concat(fileClasses)
            .Distinct()
            .ToDictionary(c => c.Name.Value, c => c);

        var referencedDecorators = dependencies
            .SelectMany(m => m.Decorators)
            .Concat(modelFile.Decorators)
            .Distinct()
            .ToDictionary(d => d.Name, c => c);

        // Résolution des "extends" sur les classes.
        foreach (var classe in fileClasses.Where(c => c.ExtendsReference != null))
        {
            if (!referencedClasses.TryGetValue(classe.ExtendsReference!.ReferenceName, out var extends))
            {
                yield return new ModelError(classe, "La classe '{0}' est introuvable dans le fichier ou l'une de ses dépendances.", classe.ExtendsReference!) { ModelErrorType = ModelErrorType.TMD1002 };
                continue;
            }

            classe.Extends = extends;
        }

        // Résolution des décorateurs sur les classes.
        foreach (var classe in fileClasses.Where(c => c.DecoratorReferences.Any()))
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
                    if (classe.Decorators.Contains(decorator))
                    {
                        isError = true;
                        yield return new ModelError(classe, $"Le décorateur '{decoratorRef.ReferenceName}' est déjà présent dans la liste des décorateurs de la classe '{classe}'.", decoratorRef) { ModelErrorType = ModelErrorType.TMD1009 };
                    }
                    else
                    {
                        if ((decorator.CSharp?.Extends != null || decorator.Java?.Extends != null) && (classe.Extends != null || classe.Decorators.Any(d => decorator.CSharp?.Extends != null && d.CSharp?.Extends != null || decorator.Java?.Extends != null && d.Java?.Extends != null)))
                        {
                            isError = true;
                            yield return new ModelError(classe, $"Impossible d'appliquer le décorateur '{decoratorRef.ReferenceName}' à la classe '{classe}' : seul un 'extends' peut être spécifié.", decoratorRef) { ModelErrorType = ModelErrorType.TMD1010 };
                        }

                        classe.Decorators.Add(decorator);
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
                    if (!Domains.TryGetValue(rp.DomainReference.ReferenceName, out var domain))
                    {
                        yield return new ModelError(rp, "Le domaine '{0}' est introuvable.", rp.DomainReference) { ModelErrorType = ModelErrorType.TMD1005 };
                        break;
                    }

                    rp.Domain = domain;
                    break;

                case AssociationProperty ap:
                    if (!referencedClasses.TryGetValue(ap.Reference.ReferenceName, out var association))
                    {
                        yield return new ModelError(ap, "La classe '{0}' est introuvable dans le fichier ou l'une de ses dépendances.", ap.Reference) { ModelErrorType = ModelErrorType.TMD1002 };
                        break;
                    }

                    if (association.Properties.Count(p => p.PrimaryKey) != 1)
                    {
                        yield return new ModelError(ap, "La classe '{0}' doit avoir une (et une seule) clé primaire pour être référencée dans une association.", ap.Reference) { ModelErrorType = ModelErrorType.TMD1001 };
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

                    if (cp.DomainKindReference != null)
                    {
                        if (!Domains.TryGetValue(cp.DomainKindReference.ReferenceName, out var domainKind))
                        {
                            yield return new ModelError(cp, "Le domaine '{0}' est introuvable.", cp.DomainKindReference) { ModelErrorType = ModelErrorType.TMD1005 };
                            break;
                        }

                        cp.DomainKind = domainKind;
                    }

                    break;

                case AliasProperty alp when alp.ListDomainReference != null:
                    if (!Domains.TryGetValue(alp.ListDomainReference.ReferenceName, out var listDomain))
                    {
                        yield return new ModelError(alp, "Le domaine '{0}' est introuvable.", alp.ListDomainReference) { ModelErrorType = ModelErrorType.TMD1005 };
                        break;
                    }

                    alp.ListDomain = listDomain;
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
                    classe.Properties.RemoveAt(classe.Properties.IndexOf(alp));
                    if (!classe.Properties.Contains(alp.OriginalAliasProperty))
                    {
                        classe.Properties.Insert(index, alp.OriginalAliasProperty);
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
                }

                if (alp.Class != null)
                {
                    alp.Class.Properties.Remove(alp);
                }
                else if (alp.Endpoint?.Params.Contains(alp) ?? false)
                {
                    alp.Endpoint.Params.Remove(alp);
                }
                else if (alp.Decorator != null)
                {
                    alp.Decorator.Properties.Remove(alp);
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

                foreach (var prop in classe.Decorators.SelectMany(d => d.Properties).Reverse())
                {
                    classe.Properties.Insert(0, prop.CloneWithClass(classe));
                }
            }
        }

        // Résolution des alias des classes et endpoints.
        foreach (var modelError in ResolveAliases(modelFile.Properties.OfType<AliasProperty>().Where(alp => alp.Reference is not null)))
        {
            yield return modelError;
        }

        // Résolution des alias de classes et endpoints dans le fichier.
        foreach (var alias in modelFile.Aliases)
        {
            var referencedFile = dependencies.SingleOrDefault(dep => dep.Name == alias.File.ReferenceName);
            if (referencedFile == null)
            {
                yield return new ModelError(alias, $"Le fichier '{alias.File.ReferenceName}' est introuvable dans les dépendances du fichier.", alias.File) { ModelErrorType = ModelErrorType.TMD1003 };
                continue;
            }

            foreach (var aliasClass in alias.Classes)
            {
                var referencedClass = referencedFile.Classes.SingleOrDefault(classe => classe.Name == aliasClass.ReferenceName);
                if (referencedClass == null)
                {
                    yield return new ModelError(alias, $"La classe '{aliasClass.ReferenceName}' est introuvable dans le fichier '{alias.File.ReferenceName}'.", aliasClass) { ModelErrorType = ModelErrorType.TMD1002 };
                    continue;
                }

                var existingClasse = modelFile.Classes.SingleOrDefault(classe => classe.Name == referencedClass.Name);
                if (existingClasse == null)
                {
                    modelFile.Classes.Add(referencedClass);
                }
                else
                {
                    modelFile.Classes.Insert(modelFile.Classes.IndexOf(existingClasse), referencedClass);
                    modelFile.Classes.Remove(existingClasse);
                }

                modelFile.ResolvedAliases.Add(referencedClass);
            }

            foreach (var aliasEndpoint in alias.Endpoints)
            {
                var referencedEndpoint = referencedFile.Endpoints.SingleOrDefault(endpoint => endpoint.Name == aliasEndpoint.ReferenceName);
                if (referencedEndpoint == null)
                {
                    yield return new ModelError(alias, $"L'endpoint '{aliasEndpoint.ReferenceName}' est introuvable dans le fichier '{alias.File.ReferenceName}'.", aliasEndpoint) { ModelErrorType = ModelErrorType.TMD1006 };
                    continue;
                }

                var existingEndpoint = modelFile.Endpoints.SingleOrDefault(endpoint => endpoint.Name == referencedEndpoint.Name);
                if (existingEndpoint == null)
                {
                    modelFile.Endpoints.Add(referencedEndpoint);
                }
                else
                {
                    modelFile.Endpoints.Insert(modelFile.Endpoints.IndexOf(existingEndpoint), referencedEndpoint);
                    modelFile.Endpoints.Remove(existingEndpoint);
                }

                modelFile.ResolvedAliases.Add(referencedEndpoint);
            }
        }

        // Résolution des clés d'unicités.
        foreach (var classe in fileClasses.Where(c => c.UniqueKeyReferences.Any()))
        {
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

        // Résolution des valeurs de références
        foreach (var classe in fileClasses.Where(c => c.ReferenceValueReferences.Any()))
        {
            foreach (var valueRef in classe.ReferenceValueReferences)
            {
                var referenceValue = new ReferenceValue { Name = valueRef.Key.ReferenceName };
                classe.ReferenceValues.Add(referenceValue);

                foreach (var value in valueRef.Value)
                {
                    var property = classe.Properties.OfType<IFieldProperty>().FirstOrDefault(p => p.Name == value.Key.ReferenceName);

                    if (property == null)
                    {
                        yield return new ModelError(classe, $"La propriété '{value.Key.ReferenceName}' n'existe pas sur la classe '{classe}'.", value.Key) { ModelErrorType = ModelErrorType.TMD1011 };
                    }
                    else
                    {
                        referenceValue.Value.Add(property, value.Value);
                    }
                }

                var missingRequiredProperties = classe.Properties.OfType<IFieldProperty>().Where(p => p.Required && !p.Domain.AutoGeneratedValue && !valueRef.Value.Any(v => v.Key.ReferenceName == p.Name));

                if (missingRequiredProperties.Any())
                {
                    yield return new ModelError(classe, $"La valeur '{valueRef.Key.ReferenceName}' n'initialise pas les propriétés obligatoires suivantes : {string.Join(", ", missingRequiredProperties.Select(p => p.Name))}.", valueRef.Key) { ModelErrorType = ModelErrorType.TMD1012 };
                }
            }
        }

        // Résolution des propriétés spéciales de classe.
        foreach (var classe in fileClasses)
        {
            if (classe.DefaultPropertyReference != null)
            {
                classe.DefaultProperty = classe.Properties.OfType<IFieldProperty>().FirstOrDefault(fp => fp.Name == classe.DefaultPropertyReference.ReferenceName);
                if (classe.DefaultProperty == null)
                {
                    yield return new ModelError(classe, $"La propriété '{classe.DefaultPropertyReference.ReferenceName}' n'existe pas sur la classe '{classe}'.", classe.DefaultPropertyReference) { ModelErrorType = ModelErrorType.TMD1011 };
                }
            }
            else
            {
                // Si la classe a une propriété "Label" ou "Libelle", alors on la considère par défaut (sic) comme propriété par défaut.
                classe.DefaultProperty = classe.Properties.OfType<IFieldProperty>().FirstOrDefault(fp => fp.Name == "Label" || fp.Name == "Libelle");
            }

            if (classe.OrderPropertyReference != null)
            {
                classe.OrderProperty = classe.Properties.OfType<IFieldProperty>().FirstOrDefault(fp => fp.Name == classe.OrderPropertyReference.ReferenceName);
                if (classe.OrderProperty == null)
                {
                    yield return new ModelError(classe, $"La propriété '{classe.OrderPropertyReference.ReferenceName}' n'existe pas sur la classe '{classe}'.", classe.OrderPropertyReference) { ModelErrorType = ModelErrorType.TMD1011 };
                }
            }
            else
            {
                // Si la classe a une propriété "Order" ou "Ordre", alors on la considère par défaut comme propriété d'ordre.
                classe.OrderProperty = classe.Properties.OfType<IFieldProperty>().FirstOrDefault(fp => fp.Name == "Order" || fp.Name == "Ordre");
            }

            if (classe.FlagPropertyReference != null)
            {
                classe.FlagProperty = classe.Properties.OfType<IFieldProperty>().FirstOrDefault(fp => fp.Name == classe.FlagPropertyReference.ReferenceName);
                if (classe.FlagProperty == null)
                {
                    yield return new ModelError(classe, $"La propriété '{classe.FlagPropertyReference.ReferenceName}' n'existe pas sur la classe '{classe}'.", classe.FlagPropertyReference) { ModelErrorType = ModelErrorType.TMD1011 };
                }
            }
            else
            {
                // Si la classe a une propriété "Flag", alors on la considère par défaut comme propriété de flag.
                classe.FlagProperty = classe.Properties.OfType<IFieldProperty>().FirstOrDefault(fp => fp.Name == "Flag");
            }
        }

        // Vérifications de cohérence sur les fichiers.
        if (!_config.AllowCompositePrimaryKey)
        {
            foreach (var classe in modelFile.Classes)
            {
                if (classe.Properties.Count(p => p.PrimaryKey) > 1)
                {
                    yield return new ModelError(classe, $"La classe '{classe.Name}' doit avoir une seule clé primaire ({string.Join(", ", classe.Properties.Where(p => p.PrimaryKey).Select(p => p.Name))} trouvées).") { ModelErrorType = ModelErrorType.TMD0001 };
                }
            }
        }

        foreach (var classe in fileClasses)
        {
            foreach (var property in classe.Properties.Where((e, i) => classe.Properties.Where((p, j) => p.Name == e.Name && j < i).Any()))
            {
                yield return new ModelError(modelFile, $"Le nom '{property.Name}' est déjà utilisé.", property is AliasProperty { PropertyReference: var pr } ? pr : property.GetLocation()) { IsError = true, ModelErrorType = ModelErrorType.TMD0003 };
            }

            if (classe.ReferenceValues.Any() && classe.PrimaryKey?.Domain.AutoGeneratedValue == true)
            {
                var firstUk = classe.UniqueKeys.FirstOrDefault();
                if (firstUk == null || firstUk.Count != 1)
                {
                    yield return new ModelError(classe, $"La classe '{classe}' doit avoir une clé primaire non auto-générée ou une contrainte d'unicité sur une seule propriété en première position pour pouvoir définir des valeurs de références.") { ModelErrorType = ModelErrorType.TMD1013 };
                }
            }
        }

        foreach (var endpoint in modelFile.Endpoints.Where((e, i) => modelFile.Endpoints.Where((p, j) => p.Name == e.Name && j < i).Any()))
        {
            yield return new ModelError(modelFile, $"Le nom '{endpoint.Name}' est déjà utilisé.", endpoint.Name.GetLocation()) { IsError = true, ModelErrorType = ModelErrorType.TMD0003 };
        }

        foreach (var use in modelFile.UselessImports)
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
        }
    }

    private IEnumerable<ModelError> GetGlobalErrors()
    {
        foreach (var classe in Classes.Where(c => c.Trigram != null && Classes.Any(u => u.Trigram == c.Trigram && u != c)))
        {
            yield return new ModelError(classe.ModelFile, $"Le trigram '{classe.Trigram}' est déjà utilisé dans la (les) classe(s) suivantes : {string.Join(", ", Classes.Where(u => u.Trigram == classe.Trigram && u != classe).Select(c => c.Name))}", classe.Trigram.GetLocation()) { IsError = false, ModelErrorType = ModelErrorType.TMD9002 };
        }

        foreach (var domain in Domains.Values.Where(domain => !this.GetDomainReferences(domain).Any()))
        {
            yield return new ModelError(domain, $"Le domaine '{domain.Name}' n'est pas utilisé.") { IsError = false, ModelErrorType = ModelErrorType.TMD9004 };
        }
    }
}