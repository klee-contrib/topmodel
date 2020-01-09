using System.Collections.Generic;
using System.IO;
using System.Linq;
using Kinetix.NewGenerator.Config;
using Kinetix.NewGenerator.FileModel;
using Kinetix.NewGenerator.Model;

namespace Kinetix.NewGenerator.CSharp
{
    using static CSharpUtils;

    /// <summary>
    /// Générateur de code C#.
    /// </summary>
    public static class CSharpCodeGenerator
    {
        /// <summary>
        /// Génère le code des classes.
        /// </summary>
        /// <param name="rootNamespace">Namespace de l'application.</param>
        /// <param name="config">Paramètres génération C#</param>
        /// <param name="classes">Classes.</param>
        public static void Generate(string rootNamespace, CSharpConfig config, ICollection<Class> classes)
        {
            if (config.DbContextProjectPath != null)
            {
                new DbContextGenerator(rootNamespace, config).Generate(classes);
            }

            if (config.OutputDirectory == null)
            {
                return;
            }

            var classGenerator = new CSharpClassGenerator(rootNamespace, config);

            foreach (var ns in classes.GroupBy(c => c.Namespace))
            {
                if (!Directory.Exists(ns.Key.CSharpName))
                {
                    var directoryForModelClass = GetDirectoryForModelClass(config.LegacyProjectPaths, config.OutputDirectory, ns.Key.Kind == Kind.Data, rootNamespace, ns.Key.CSharpName);
                    var projectDirectory = GetDirectoryForProject(config.LegacyProjectPaths, config.OutputDirectory, ns.Key.Kind == Kind.Data, rootNamespace, ns.Key.CSharpName);

                    foreach (var item in ns)
                    {
                        var currentDirectory = GetDirectoryForModelClass(config.LegacyProjectPaths, config.OutputDirectory, item.Trigram != null, rootNamespace, item.Namespace.CSharpName);
                        Directory.CreateDirectory(currentDirectory);
                        classGenerator.Generate(item);
                    }
                }
            }

            new ReferenceAccessorGenerator(rootNamespace, config).Generate(classes);
        }
    }
}
