using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Kinetix.NewGenerator.Config;
using Kinetix.NewGenerator.Model;
using Kinetix.Tools.Common;

namespace Kinetix.NewGenerator.Javascript
{
    /// <summary>
    /// Générateur de définitions Typescript.
    /// </summary>
    public static class TypescriptDefinitionGenerator
    {
        /// <summary>
        /// Génère les définitions Typescript.
        /// </summary>
        /// <param name="config">Paramètres.</param>
        /// <param name="classes">La liste des modèles.</param>
        public static void Generate(JavascriptConfig config, ICollection<Class> classes)
        {
            if (config.ModelOutputDirectory == null)
            {
                return;
            }

            var nameSpaceMap = classes.GroupBy(c => c.Namespace.Module).ToDictionary(g => g.Key, g => g.ToList());

            var staticLists = new List<Class>();

            foreach (var entry in nameSpaceMap)
            {
                foreach (var model in entry.Value)
                {
                    if (model.Stereotype != Stereotype.Statique)
                    {
                        if (!config.IsGenerateEntities && model.Trigram != null)
                        {
                            continue;
                        }

                        var fileName = model.Name.ToDashCase();
                        Console.Out.WriteLine($"Generating Typescript file: {fileName}.ts ...");

                        fileName = $"{config.ModelOutputDirectory}/{entry.Key.ToDashCase()}/{fileName}.ts";
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

                GenerateReferenceLists(config, staticLists, entry.Key);
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
