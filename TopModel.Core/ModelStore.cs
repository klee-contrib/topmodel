using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using TopModel.Core.FileModel;
using TopModel.Core.Loaders;

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

    public IEnumerable<Class> Classes => _modelFiles.SelectMany(mf => mf.Value.Classes);

    public IDictionary<string, Domain> Domains => _modelFiles.SelectMany(mf => mf.Value.Domains).ToDictionary(d => d.Name, d => d);

    public IEnumerable<ModelFile> Files => _modelFiles.Values;

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
            fsWatcher = new FileSystemWatcher(_config.ModelRoot, "*.yml");
            fsWatcher.Changed += OnFSChangedEvent;
            fsWatcher.Created += OnFSChangedEvent;
            fsWatcher.IncludeSubdirectories = true;
            fsWatcher.EnableRaisingEvents = true;
        }

        _modelFiles.Clear();
        _pendingUpdates.Clear();

        _logger.LogInformation("Chargement du modèle...");

        var files = Directory.EnumerateFiles(_config.ModelRoot, "*.yml", SearchOption.AllDirectories);

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

    private IEnumerable<ModelFile> GetDependencies(ModelFile modelFile)
    {
        return modelFile.Uses
           .Select(dep =>
           {
               if (!_modelFiles.TryGetValue(dep, out var depFile))
               {
                   _logger.LogError($"{modelFile.Path}[6,0] - Le fichier référencé '{dep}' est introuvable.");
                   throw new ModelException($"Erreur lors de la résolution des dépendances");
               }

               return depFile;
           });
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

    private void OnModelFileChange(string filePath)
    {
        _logger.LogInformation(string.Empty);
        _logger.LogInformation($"Modifié: {filePath.ToRelative()}");

        lock (_puLock)
        {
            LoadFile(filePath);
        }

        TryApplyUpdates();
    }

    private void LoadFile(string filePath)
    {
        try
        {
            var file = _modelFileLoader.LoadModelFile(filePath);
            _modelFiles[file.Name] = file;
            _pendingUpdates.Add(file.Name);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
    }

    private void TryApplyUpdates()
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

                var affectedFiles = _pendingUpdates.Select(pu => _modelFiles[pu]).Any(mf => mf.Domains.Any())
                    ? _modelFiles.Values
                    : _modelFiles.Values.Where(f => _pendingUpdates.Contains(f.Name)).SelectMany(pf => _modelFiles.Values.Where(f => f.Name.Equals(pf.Name) || f.Uses.Any(d => d.Equals(pf.Name)))).Distinct();

                var sortedFiles = ModelUtils.Sort(affectedFiles, f => GetDependencies(f).Where(d => affectedFiles.Any(af => af.Name.Equals(d.Name))));

                foreach (var affectedFile in sortedFiles)
                {
                    referenceErrors.AddRange(ResolveReferences(affectedFile));
                }

                if (referenceErrors.Any())
                {
                    foreach (var error in referenceErrors)
                    {
                        _logger.LogError(error.ToString());
                    }

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

    private IEnumerable<ModelError> ResolveReferences(ModelFile modelFile)
    {
        var dependencies = GetDependencies(modelFile).ToList();

        var referencedClasses = dependencies
            .SelectMany(m => m.Classes)
            .Concat(modelFile.Classes)
            .ToDictionary(c => c.Name, c => c);

        foreach (var classe in modelFile.Classes.Where(c => c.ExtendsReference != null))
        {
            if (!referencedClasses.TryGetValue(classe.ExtendsReference!.ReferenceName, out var extends))
            {
                yield return new ModelError(classe, "La classe '{0}' est introuvable dans le fichier ou l'un de ses dépendances.", classe.ExtendsReference!);
                break;
            }

            classe.Extends = extends;
        }

        foreach (var prop in modelFile.Properties)
        {
            switch (prop)
            {
                case RegularProperty rp:
                    if (!Domains.TryGetValue(rp.DomainReference.ReferenceName, out var domain))
                    {
                        yield return new ModelError(rp, "Le domaine '{0}' est introuvable.", rp.DomainReference);
                        break;
                    }

                    rp.Domain = domain;
                    break;

                case AssociationProperty ap:
                    if (!referencedClasses.TryGetValue(ap.Reference.ReferenceName, out var association))
                    {
                        yield return new ModelError(ap, "La classe '{0}' est introuvable dans le fichier ou l'une de ses dépendances.", ap.Reference);
                        break;
                    }

                    if (association.Properties.Count(p => p.PrimaryKey) != 1)
                    {
                        yield return new ModelError(ap, "La classe '{0}' doit avoir une (et une seule) clé primaire pour être référencée dans une association.", ap.Reference);
                        break;
                    }

                    ap.Association = association;
                    break;

                case CompositionProperty cp:
                    if (!referencedClasses.TryGetValue(cp.Reference.ReferenceName, out var composition))
                    {
                        yield return new ModelError(cp, "La classe '{0}' est introuvable dans le fichier ou l'une de ses dépendances.", cp.Reference);
                        break;
                    }

                    cp.Composition = composition;

                    if (cp.DomainKindReference != null)
                    {
                        if (!Domains.TryGetValue(cp.DomainKindReference.ReferenceName, out var domainKind))
                        {
                            yield return new ModelError(cp, "Le domaine '{0}' est introuvable.", cp.DomainKindReference);
                            break;
                        }

                        cp.DomainKind = domainKind;
                    }

                    break;

                case AliasProperty alp when alp.ListDomainReference != null:
                    if (!Domains.TryGetValue(alp.ListDomainReference.ReferenceName, out var listDomain))
                    {
                        yield return new ModelError(alp, "Le domaine '{0}' est introuvable.", alp.ListDomainReference);
                        break;
                    }

                    alp.ListDomain = listDomain;
                    break;
            }
        }

        foreach (var alp in modelFile.Properties.OfType<AliasProperty>().Where(alp => alp.Reference != null))
        {
            if (!referencedClasses.TryGetValue(alp.Reference!.ReferenceName, out var aliasedClass))
            {
                yield return new ModelError(alp, "La classe '{0}' est introuvable dans le fichier ou l'une de ses dépendances.", alp.Reference);
                break;
            }

            var shouldBreak = false;
            foreach (var propReference in alp.Reference.IncludeReferences.Concat(alp.Reference.ExcludeReferences))
            {
                var aliasedProperty = aliasedClass.Properties.SingleOrDefault(p => p.Name == propReference.ReferenceName);
                if (aliasedProperty == null)
                {
                    yield return new ModelError(alp, $"La propriété '{{0}}' est introuvable sur la classe '{aliasedClass}'.", propReference);
                    shouldBreak = true;
                }
            }

            if (shouldBreak)
            {
                break;
            }

            var propertiesToAlias =
                (alp.Reference.IncludeReferences.Any()
                    ? alp.Reference.IncludeReferences.Select(p => aliasedClass.Properties.Single(prop => prop.Name == p.ReferenceName))
                    : aliasedClass.Properties.Where(prop => !alp.Reference.ExcludeReferences.Select(p => p.ReferenceName).Contains(prop.Name)))
                .Reverse()
                .OfType<IFieldProperty>();

            foreach (var property in propertiesToAlias)
            {
                var prop = alp.Clone(property);
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
            }

            if (alp.Class != null)
            {
                alp.Class.Properties.Remove(alp);
            }
            else if (alp.Endpoint?.Params.Contains(alp) ?? false)
            {
                alp.Endpoint.Params.Remove(alp);
            }
        }

        foreach (var alias in modelFile.Aliases)
        {
            var referencedFile = dependencies.SingleOrDefault(dep => dep.Name == alias.File);
            if (referencedFile == null)
            {
                yield return new ModelError(alias, $"Le fichier '{alias.File}' est introuvable dans les dépendances du fichier.");
                break;
            }

            foreach (var className in alias.Classes)
            {
                var referencedClass = referencedFile.Classes.SingleOrDefault(classe => classe.Name == className);
                if (referencedClass == null)
                {
                    yield return new ModelError(alias, $"La classe '{className}' est introuvable dans le fichier '{alias.File}'.");
                    break;
                }

                if (!modelFile.Classes.Any(classe => classe.Name == referencedClass.Name))
                {
                    modelFile.Classes.Add(referencedClass);
                }
            }

            foreach (var endpointName in alias.Endpoints)
            {
                var referencedEndpoint = referencedFile.Endpoints.SingleOrDefault(endpoint => endpoint.Name == endpointName);
                if (referencedEndpoint == null)
                {
                    yield return new ModelError(alias, $"L'endpoint '{endpointName}' est introuvable dans le fichier '{alias.File}'.");
                    break;
                }

                if (!modelFile.Endpoints.Any(endpoint => endpoint.Name == referencedEndpoint.Name))
                {
                    modelFile.Endpoints.Add(referencedEndpoint);
                }
            }
        }

        if (!_config.AllowCompositePrimaryKey)
        {
            foreach (var classe in modelFile.Classes)
            {
                if (classe.Properties.Count(p => p.PrimaryKey) > 1)
                {
                    yield return new ModelError(classe, $"La classe '{classe.Name}' doit avoir une seule clé primaire ({string.Join(", ", classe.Properties.Where(p => p.PrimaryKey).Select(p => p.Name))} trouvées).");
                }
            }
        }
    }
}