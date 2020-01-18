using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using TopModel.Core.Config;
using TopModel.Core.FileModel;
using TopModel.Core.Loaders;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace TopModel.Core
{
    public class ModelStore
    {
        private readonly RootConfig _config;
        private readonly DomainFileLoader _domainFileLoader;
        private readonly IMemoryCache _fsCache;
        private readonly ILogger<ModelStore> _logger;
        private readonly ModelFileLoader _modelFileLoader;

        private readonly IDictionary<string, Domain> _domains = new Dictionary<string, Domain>();
        private readonly IDictionary<FileName, ModelFile> _modelFiles = new Dictionary<FileName, ModelFile>();

        private readonly ConcurrentDictionary<FileName, ModelFile> _pendingUpdates = new ConcurrentDictionary<FileName, ModelFile>();

        public ModelStore(DomainFileLoader domainFileLoader, IMemoryCache fsCache, ModelFileLoader modelFileLoader, ILogger<ModelStore> logger, RootConfig config)
        {
            _config = config;
            _domainFileLoader = domainFileLoader;
            _fsCache = fsCache;
            _logger = logger;
            _modelFileLoader = modelFileLoader;
        }

        public event ModelFileChangeEventHandler? FilesChanged;

        public IEnumerable<Class> Classes => _modelFiles.SelectMany(mf => mf.Value.Classes);
        public IEnumerable<ModelFile> Files => _modelFiles.Values;

        public string RootNamespace
        {
            get
            {
                var apps = _modelFiles.Select(f => f.Value.Descriptor.App).Distinct();
                if (apps.Count() != 1)
                {
                    throw new Exception("Tous les fichiers doivent être liés à la même 'app'.");
                }

                return apps.Single();
            }
        }

        public IDisposable BeginWatch()
        {
            _logger.LogInformation("Lancement du mode watch...");
            var fsWatcher = new FileSystemWatcher(_config.ModelRoot, "*.yml");
            fsWatcher.Changed += OnFSChangedEvent;
            fsWatcher.Created += OnFSChangedEvent;
            fsWatcher.IncludeSubdirectories = true;
            fsWatcher.EnableRaisingEvents = true;
            return fsWatcher;
        }

        public void LoadFromConfig()
        {
            _domains.Clear();
            _modelFiles.Clear();

            _logger.LogInformation("Chargement des domaines...");

            foreach (var domain in _domainFileLoader.LoadDomains(_config.Domains))
            {
                _domains.TryAdd(domain.Name, domain);
            }

            _logger.LogInformation($"{_domains.Count} domaines chargés.");
            _logger.LogInformation("Chargement des classes...");

            var files = Directory.EnumerateFiles(_config.ModelRoot, "*.yml", SearchOption.AllDirectories)
               .Where(f => f != _config.Domains);

            foreach (var file in files)
            {
                var modelFile = _modelFileLoader.LoadModelFile(file);
                _modelFiles.Add(modelFile.Name, modelFile);
            }

            var relationshipErrors = new List<string>();
            foreach (var modelFile in ModelUtils.Sort(_modelFiles.Values, GetDependencies))
            {
                relationshipErrors.AddRange(ResolveRelationships(modelFile));
            }
            if (relationshipErrors.Any())
            {
                foreach (var error in relationshipErrors)
                {
                    _logger.LogError(error);
                }
                throw new Exception("Erreur lors de la lecture du modèle.");
            }

            _logger.LogInformation($"{_modelFiles.SelectMany(mf => mf.Value.Classes).Count()} classes chargées.");

            LoadReferenceLists();
        }    

        private IEnumerable<ModelFile> GetDependencies(ModelFile modelFile)
        {
            return modelFile.Dependencies
               .Select(dep =>
               {
                   if (!_modelFiles.TryGetValue(dep, out var depFile))
                   {
                       throw new Exception($"Le fichier {dep}, référencé dans le fichier {modelFile}, est introuvable.");
                   }

                   return depFile;
               });
        }

        private void LoadReferenceLists()
        {
            _logger.LogInformation("Chargement des listes de référence...");

            var staticLists = ReferenceListsLoader.LoadReferenceLists(_config.StaticLists);
            var referenceLists = ReferenceListsLoader.LoadReferenceLists(_config.ReferenceLists);

            var classMap = _modelFiles.SelectMany(mf => mf.Value.Classes)
                .ToDictionary(c => c.Name, c => c);

            foreach (var (className, referenceValues) in staticLists.Concat(referenceLists))
            {
                if (classMap.TryGetValue(className, out var classe))
                {
                    ReferenceListsLoader.AddReferenceValues(classe, referenceValues);
                }
                else
                {
                    throw new Exception($"Une liste de référence pour la classe {className} a été définie, alors que cette classe est introuvable.");
                }
            }

            _logger.LogInformation($"{staticLists.Count()} listes statiques et {referenceLists.Count()} listes de références chargées.");
        }

        private void OnFSChangedEvent(object sender, FileSystemEventArgs e)
        {
            _fsCache.Set(e.FullPath, e, new MemoryCacheEntryOptions()
                .AddExpirationToken(new CancellationChangeToken(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token))
                .RegisterPostEvictionCallback((k, v, r, a) => OnModelFileChange((string)k, r)));
        }

        private void OnModelFileChange(string filePath, EvictionReason reason)
        {
            if (reason != EvictionReason.TokenExpired)
            {
                return;
            }

            try
            {
                _logger.LogInformation(string.Empty);
                _logger.LogInformation($"Fichier {filePath} modifié...");

                var file = _modelFileLoader.LoadModelFile(filePath);

                _modelFiles[file.Name] = file;
                _pendingUpdates.AddOrUpdate(file.Name, file, (_, __) => file);

                var relationshipErrors = new List<string>();
                var filesToGenerate = new List<ModelFile>();
                foreach (var update in _pendingUpdates.Values)
                {
                    relationshipErrors.AddRange(ResolveRelationships(update));
                    filesToGenerate.Add(update);
                    foreach (var dep in _modelFiles.Values.Where(f => f.Dependencies.Any(d => d.Equals(update.Name))))
                    {
                        relationshipErrors.AddRange(ResolveRelationships(dep));
                        filesToGenerate.Add(dep);
                    }
                }

                if (relationshipErrors.Any())
                {
                    foreach (var error in relationshipErrors)
                    {
                        _logger.LogError(error);
                    }
                    throw new Exception("Erreur lors de la lecture du modèle.");
                }

                if (_pendingUpdates.Values.SelectMany(f => f.Classes).Any(c => c.Stereotype != null))
                {
                    LoadReferenceLists();
                }

                FilesChanged?.Invoke(this, filesToGenerate);

                _logger.LogInformation($"Génération terminée avec succès.");

                _pendingUpdates.Clear();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

        private IEnumerable<string> ResolveRelationships(ModelFile modelFile)
        {
            var referencedClasses = GetDependencies(modelFile)
                .SelectMany(m => m.Classes)
                .Concat(modelFile.Classes)
                .ToDictionary(c => c.Name, c => c);

            foreach (var (obj, className) in modelFile.Relationships)
            {
                switch (obj)
                {
                    case Class classe:
                        if (!referencedClasses.TryGetValue(className, out var extends))
                        {
                            yield return $"La classe {className}, référencée par {classe.Name}, est introuvable dans les dépendances du fichier {modelFile}.";
                            break;
                        }
                        classe.Extends = extends;
                        break;
                    case RegularProperty rp:
                        if (!_domains.TryGetValue(className, out var domain))
                        {
                            yield return $"Le domaine {className}, référencé par la propriété {rp.Name} de la classe {rp.Class.Name} du fichier {modelFile}, est introuvable.";
                            break;
                        }
                        rp.Domain = domain;
                        break;
                    case AssociationProperty ap:
                        if (!referencedClasses.TryGetValue(className, out var association))
                        {
                            yield return $"La classe {className}, référencée sur une association de la classe {ap.Class.Name}, est introuvable dans les dépendances du fichier {modelFile}.";
                            break;
                        }
                        ap.Association = association;
                        break;
                    case CompositionProperty cp:
                        if (!referencedClasses.TryGetValue(className, out var composition))
                        {
                            yield return $"La classe {className}, référencée sur une composition de la classe {cp.Class.Name}, est introuvable dans les dépendances du fichier {modelFile}.";
                            break;
                        }
                        cp.Composition = composition;
                        break;
                    case AliasProperty alp:
                        var aliasConf = className.Split("|");
                        if (!referencedClasses.TryGetValue(aliasConf[1], out var aliasedClass))
                        {
                            yield return $"La classe {aliasConf[1]}, référencée sur un alias de la classe {alp.Class.Name}, est introuvable dans les dépendances du fichier {modelFile}.";
                            break;
                        }
                        var aliasedProperty = aliasedClass.Properties.SingleOrDefault(p => p.Name == aliasConf[0]);
                        if (aliasedProperty == null)
                        {
                            yield return $"La propriété {aliasConf[0]} est introuvable sur la classe {aliasedClass.Name}, référencée comme alias de la classe {alp.Class.Name} dans le fichier {modelFile}.";
                            break;
                        }
                        alp.Property = (IFieldProperty)aliasedProperty;
                        break;
                }
            }

            foreach (var classe in modelFile.Classes)
            {
                if (classe.Properties.Count(p => p.PrimaryKey) > 1)
                {
                    throw new Exception($"La classe {classe.Name} du fichier {modelFile} doit avoir une seule clé primaire ({string.Join(", ", classe.Properties.Where(p => p.PrimaryKey).Select(p => p.Name))} trouvées)");
                }
            }
        }
    }
}
