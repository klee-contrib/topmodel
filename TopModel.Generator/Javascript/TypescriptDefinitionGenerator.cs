using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TopModel.Core;
using TopModel.Core.Config;

namespace TopModel.Generator.Javascript
{
    /// <summary>
    /// Générateur de définitions Typescript.
    /// </summary>
    public class TypescriptDefinitionGenerator : IGenerator
    {
        private readonly JavascriptConfig? _config;
        private readonly ModelStore _modelStore;

        public TypescriptDefinitionGenerator(ModelStore modelStore, JavascriptConfig? config = null)
        {
            _config = config;
            _modelStore = modelStore;
        }

        /// <summary>
        /// Génère les définitions Typescript.
        /// </summary>
        public void Generate()
        {
            if (_config?.ModelOutputDirectory == null)
            {
                return;
            }

            var nameSpaceMap = _modelStore.Classes.GroupBy(c => c.Namespace.Module).ToDictionary(g => g.Key, g => g.ToList());

            var staticLists = new List<Class>();

            foreach (var entry in nameSpaceMap)
            {
                foreach (var model in entry.Value)
                {
                    if (model.Stereotype != Stereotype.Statique)
                    {
                        if (!_config.IsGenerateEntities && model.Trigram != null)
                        {
                            continue;
                        }

                        var fileName = model.Name.ToDashCase();
                        Console.Out.WriteLine($"Generating Typescript file: {fileName}.ts ...");

                        fileName = $"{_config.ModelOutputDirectory}/{entry.Key.ToDashCase()}/{fileName}.ts";
                        var fileInfo = new FileInfo(fileName);

                        var isNewFile = !fileInfo.Exists;

                        var directoryInfo = fileInfo.Directory;
                        if (!directoryInfo.Exists)
                        {
                            Directory.CreateDirectory(directoryInfo.FullName);
                        }

                        var template = new TypescriptTemplate(model);
                        var result = template.TransformText();
                        File.WriteAllText(fileName, result, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
                    }
                    else
                    {
                        staticLists.Add(model);
                    }
                }

                GenerateReferenceLists(_config, staticLists, entry.Key);
                staticLists.Clear();
            }
        }

        private static void GenerateReferenceLists(JavascriptConfig parameters, IList<Class> staticLists, string namespaceName)
        {
            if (staticLists.Any())
            {
                Console.Out.WriteLine($"Generating Typescript file: references.ts ...");

                var fileName = namespaceName != null
                    ? $"{parameters.ModelOutputDirectory}/{namespaceName.ToDashCase()}/references.ts"
                    : $"{parameters.ModelOutputDirectory}/references.ts";

                var fileInfo = new FileInfo(fileName);

                var isNewFile = !fileInfo.Exists;

                var directoryInfo = fileInfo.Directory;
                if (!directoryInfo.Exists)
                {
                    Directory.CreateDirectory(directoryInfo.FullName);
                }

                var template = new ReferenceTemplate(staticLists.OrderBy(r => r.Name));
                var result = template.TransformText();
                File.WriteAllText(fileName, result, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            }
        }
    }
}
