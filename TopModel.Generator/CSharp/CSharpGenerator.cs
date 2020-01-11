using System.IO;
using System.Linq;
using TopModel.Core;
using TopModel.Core.Config;
using TopModel.Core.FileModel;

namespace TopModel.Generator.CSharp
{
    using static CSharpUtils;

    /// <summary>
    /// Générateur de code C#.
    /// </summary>
    public class CSharpGenerator : IGenerator
    {
        private readonly CSharpConfig? _config;
        private readonly ModelStore _modelStore;

        public CSharpGenerator(ModelStore modelStore, CSharpConfig? config = null)
        {
            _modelStore = modelStore;
            _config = config;
        }

        /// <summary>
        /// Génère le code des classes.
        /// </summary>
        /// <param name="rootNamespace">Namespace de l'application.</param>
        /// <param name="config">Paramètres génération C#</param>
        /// <param name="classes">Classes.</param>
        public void Generate()
        {
            if (_config == null)
            {
                return;
            }

            var rootNamespace = _modelStore.RootNamespace;
            var classes = _modelStore.Classes;

            if (_config.DbContextProjectPath != null)
            {
                new DbContextGenerator(rootNamespace, _config).Generate(classes);
            }

            if (_config.OutputDirectory == null)
            {
                return;
            }

            var classGenerator = new CSharpClassGenerator(rootNamespace, _config);

            foreach (var ns in classes.GroupBy(c => c.Namespace))
            {
                if (!Directory.Exists(ns.Key.CSharpName))
                {
                    var directoryForModelClass = GetDirectoryForModelClass(_config.LegacyProjectPaths, _config.OutputDirectory, ns.Key.Kind == Kind.Data, rootNamespace, ns.Key.CSharpName);
                    var projectDirectory = GetDirectoryForProject(_config.LegacyProjectPaths, _config.OutputDirectory, ns.Key.Kind == Kind.Data, rootNamespace, ns.Key.CSharpName);

                    foreach (var item in ns)
                    {
                        var currentDirectory = GetDirectoryForModelClass(_config.LegacyProjectPaths, _config.OutputDirectory, item.Trigram != null, rootNamespace, item.Namespace.CSharpName);
                        Directory.CreateDirectory(currentDirectory);
                        classGenerator.Generate(item);
                    }
                }
            }

            new ReferenceAccessorGenerator(rootNamespace, _config).Generate(classes);
        }
    }
}
