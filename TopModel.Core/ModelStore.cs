using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopModel.Core.Config;
using TopModel.Core.FileModel;
using TopModel.Core.Loaders;
using Microsoft.Extensions.Logging;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace TopModel.Core
{
    public class ModelStore
    {
        private readonly RootConfig _config;
        private readonly IDeserializer _deserializer;
        private readonly ILogger<ModelStore> _logger;

        private IDictionary<string, Class>? _classes;
        private IDictionary<(string Module, Kind Kind, string File), (FileDescriptor descriptor, Parser parser)>? _classFiles;
        private IDictionary<string, Domain>? _domains;
        private IEnumerable<string>? _files;
        private IEnumerable<(string className, IEnumerable<ReferenceValue> values)>? _referenceLists;
        private IEnumerable<(string className, IEnumerable<ReferenceValue> values)>? _staticLists;

        public ModelStore(RootConfig config, IDeserializer deserializer, ILogger<ModelStore> logger)
        {
            _config = config;
            _deserializer = deserializer;
            _logger = logger;
        }

        public IEnumerable<Class> Classes
        {
            get
            {
                if (_classes == null)
                {
                    _classes = new Dictionary<string, Class>();

                    _logger.LogInformation("Chargement des classes...");

                    foreach (var (_, (descriptor, parser)) in ClassFiles)
                    {
                        ClassesLoader.LoadClasses(descriptor, parser, _classes, ClassFiles, Domains, _deserializer);
                    }

                    foreach (var kvp in _classes)
                    {
                        var classe = kvp.Value;
                        if (classe.Properties.Count(p => p.PrimaryKey) > 1)
                        {
                            throw new Exception($"La classe {classe.Name} doit avoir une seule clé primaire ({string.Join(", ", classe.Properties.Where(p => p.PrimaryKey).Select(p => p.Name))} trouvés)");
                        }
                    }

                    _logger.LogInformation($"{_classes.Count} classes chargées.");
                    _logger.LogInformation("Chargement des listes de référence...");

                    foreach (var (className, referenceValues) in StaticLists)
                    {
                        ReferenceListsLoader.AddReferenceValues(_classes[className], referenceValues);
                    }

                    foreach (var (className, referenceValues) in ReferenceLists)
                    {
                        ReferenceListsLoader.AddReferenceValues(_classes[className], referenceValues);
                    }

                    _logger.LogInformation($"{StaticLists.Count()} listes statiques et {ReferenceLists.Count()} listes de références chargées.");
                }

                return _classes.Values;
            }
        }        

        public IDictionary<string, Domain> Domains
        {
            get
            {
                if (_domains == null)
                {
                    _logger.LogInformation("Chargement des domaines...");

                    _domains = DomainsLoader.LoadDomains(_config.Domains, _deserializer)
                        .ToLookup(f => f.Name, f => f)
                        .ToDictionary(f => f.Key, f => f.First());

                    _logger.LogInformation($"{_domains.Count} domaines chargés.");
                }             

                return _domains;
            }
        }

        public IDictionary<Class, IEnumerable<ReferenceValue>> StaticListsMap =>
            StaticLists.ToDictionary(s => _classes![s.className], s => s.values);

        public IDictionary<Class, IEnumerable<ReferenceValue>> ReferenceListsMap =>
            ReferenceLists.ToDictionary(s => _classes![s.className], s => s.values);

        public string RootNamespace
        {
            get
            {
                var apps = ClassFiles.Select(f => f.Value.descriptor.App).Distinct();
                if (apps.Count() != 1)
                {
                    throw new Exception("Tous les fichiers doivent être liés à la même 'app'.");
                }

                return apps.Single();
            }
        }

        private IDictionary<(string Module, Kind Kind, string File), (FileDescriptor descriptor, Parser parser)> ClassFiles
        {
            get
            {
                _classFiles ??= Files
                   .Select(file => ClassesLoader.GetFileDescriptor(file, _deserializer))
                   .ToDictionary(
                       file => (file.descriptor.Module, file.descriptor.Kind, file.descriptor.File),
                       file => file);

                return _classFiles;
            }
        }

        private IEnumerable<string> Files
        {
            get
            {
                _files ??= Directory.EnumerateFiles(_config.ModelRoot, "*.yml", SearchOption.AllDirectories)
                    .Where(f => f != _config.Domains);

                return _files;
            }
        }

        private IEnumerable<(string className, IEnumerable<ReferenceValue> values)> StaticLists
        {
            get
            {
                _staticLists ??= ReferenceListsLoader.LoadReferenceLists(_config.StaticLists);
                return _staticLists;
            }
        }

        private IEnumerable<(string className, IEnumerable<ReferenceValue> values)> ReferenceLists
        {
            get
            {
                _referenceLists ??= ReferenceListsLoader.LoadReferenceLists(_config.ReferenceLists);
                return _referenceLists;
            }
        }
    }
}
