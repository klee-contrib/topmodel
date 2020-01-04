using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var rootDir = args[0];
            var deserializer = new DeserializerBuilder()
               .WithNamingConvention(CamelCaseNamingConvention.Instance)
               .Build();

            var files = Directory.GetFiles(rootDir, "*.yml", SearchOption.AllDirectories);
            var domainFiles = files.Where(f => f.Split(@"\").Last() == "domains.yml");
            var modelFiles = files.Where(f => f.Split(@"\").Last() != "domains.yml");

            var domains = domainFiles
                .SelectMany(file => DomainsLoader.LoadDomains(file, deserializer))
                .ToLookup(f => f.Name, f => f)
                .ToDictionary(f => f.Key, f => f.First());

            var classFiles = modelFiles
                .Select(file => ClassesLoader.GetFileDescriptor(file, deserializer))
                .ToDictionary(
                    file => (file.descriptor.Module, file.descriptor.Kind, file.descriptor.File),
                    file => file);

            var classes = new Dictionary<string, Class>();

            foreach(var (_, (descriptor, parser)) in classFiles)
            {
                ClassesLoader.LoadClasses(descriptor, parser, classes, classFiles, deserializer);
            }
        }        
    }
}
