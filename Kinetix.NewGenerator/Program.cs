using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Kinetix.NewGenerator.Config;
using Kinetix.NewGenerator.CSharp;
using Kinetix.NewGenerator.Javascript;
using Kinetix.NewGenerator.Loaders;
using Kinetix.NewGenerator.Model;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Kinetix.NewGenerator
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var deserializer = new DeserializerBuilder()
               .WithNamingConvention(CamelCaseNamingConvention.Instance)
               .Build();

            var configFile = new FileInfo(args[0]);
            var config = deserializer.Deserialize<RootConfig>(configFile.OpenText().ReadToEnd());
            config.ModelRoot = Path.Combine(configFile.DirectoryName, config.ModelRoot ?? string.Empty);
            config.Domains = Path.Combine(configFile.DirectoryName, config.Domains ?? "domains.yml");
            config.StaticLists = config.StaticLists != null ?
                Path.Combine(configFile.DirectoryName, config.StaticLists)
                : null;
            config.ReferenceLists = config.ReferenceLists != null ?
                Path.Combine(configFile.DirectoryName, config.ReferenceLists)
                : null;

            var files = Directory.EnumerateFiles(config.ModelRoot, "*.yml", SearchOption.AllDirectories)
                .Where(f => f != config.Domains && f != configFile.FullName);

            var domains = DomainsLoader.LoadDomains(config.Domains, deserializer)
                .ToLookup(f => f.Name, f => f)
                .ToDictionary(f => f.Key, f => f.First());

            var classFiles = files
                .Select(file => ClassesLoader.GetFileDescriptor(file, deserializer))
                .ToDictionary(
                    file => (file.descriptor.Module, file.descriptor.Kind, file.descriptor.File),
                    file => file);

            var apps = classFiles.Select(f => f.Value.descriptor.App).Distinct();
            if (apps.Count() != 1)
            {
                throw new Exception("Tous les fichiers doivent être liés à la même 'app'.");
            }
            var rootNamespace = apps.Single();

            var classes = new Dictionary<string, Class>();

            foreach (var (_, (descriptor, parser)) in classFiles)
            {
                ClassesLoader.LoadClasses(descriptor, parser, classes, classFiles, domains, deserializer);
            }

            foreach (var kvp in classes)
            {
                var classe = kvp.Value;
                if (classe.Properties.Count(p => p.PrimaryKey) > 1)
                {
                    throw new Exception($"La classe {classe.Name} doit avoir une seule clé primaire ({string.Join(", ", classe.Properties.Where(p => p.PrimaryKey).Select(p => p.Name))} trouvés)");
                }
            }

            if (config.StaticLists != null)
            {
                var staticLists = ReferenceListsLoader.LoadReferenceLists(config.StaticLists);
                foreach (var (className, referenceValues) in staticLists)
                {
                    ReferenceListsLoader.AddReferenceValues(classes[className], referenceValues);
                }
            }

            if (config.ReferenceLists != null)
            {
                var referenceLists = ReferenceListsLoader.LoadReferenceLists(config.ReferenceLists);
                foreach (var (className, referenceValues) in referenceLists)
                {
                    ReferenceListsLoader.AddReferenceValues(classes[className], referenceValues);
                }
            }

            if (config.Csharp != null)
            {
                if (config.Csharp.OutputDirectory != null)
                {
                    config.Csharp.OutputDirectory = Path.Combine(configFile.DirectoryName, config.Csharp.OutputDirectory);
                }

                CSharpCodeGenerator.Generate(rootNamespace, config.Csharp, classes.Values);
            }

            if (config.Javascript != null)
            {
                if (config.Javascript.ModelOutputDirectory != null)
                {
                    config.Javascript.ModelOutputDirectory = Path.Combine(configFile.DirectoryName, config.Javascript.ModelOutputDirectory);
                }
                if (config.Javascript.ResourceOutputDirectory != null)
                {
                    config.Javascript.ResourceOutputDirectory = Path.Combine(configFile.DirectoryName, config.Javascript.ResourceOutputDirectory);
                }

                TypescriptDefinitionGenerator.Generate(config.Javascript, classes.Values.ToList());
                JavascriptResourceGenerator.Generate(config.Javascript, classes.Values.ToList());
            }
        }
    }
}
