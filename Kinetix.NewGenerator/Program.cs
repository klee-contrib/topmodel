using System.Collections.Generic;
using System.IO;
using System.Linq;
using Kinetix.NewGenerator.Config;
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

            var classes = new Dictionary<string, Class>();

            foreach(var (_, (descriptor, parser)) in classFiles)
            {
                ClassesLoader.LoadClasses(descriptor, parser, classes, classFiles, domains, deserializer);
            }
        }        
    }
}
