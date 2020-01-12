using System;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TopModel.Generator
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var fileName = args[0];
            var deserializer = new DeserializerBuilder()
               .WithNamingConvention(CamelCaseNamingConvention.Instance)
               .Build();

            using var provider = new ServiceCollection()
                .AddSingleton(deserializer)
                .AddSingleton<ModelStore>()
                .AddLogging()
                .BuildServiceProvider();

            var modelStore = provider.GetService<ModelStore>();

            var classes = modelStore.GetClassesFromFile(fileName, out var classesToResolve);

            var sb = new StringBuilder();
            foreach (var classe in classes)
            {
                sb.Append("[");
                sb.Append(classe.Name);

                var first = true;
                foreach (var prop in classe.Properties.OfType<IFieldProperty>())
                {
                    if (prop is AssociationProperty)
                    {
                        continue;
                    }

                    if (first)
                    {
                        first = false;
                        sb.Append("|");
                    }
                    else
                    {
                        sb.Append(";");
                    }

                    if (prop is AliasProperty alp)
                    {
                        var alias = classesToResolve[alp].Split("|");
                        sb.Append($"({alias[1]}).{alias[0]}");
                    }
                    else
                    {
                        sb.Append($"{prop.Name}{(prop.Required ? "?" : string.Empty)}: {classesToResolve[prop]}");
                    }
                }

                sb.Append("]\r\n");

                foreach (var prop in classe.Properties.OfType<AssociationProperty>())
                {
                    sb.Append($"[{classe.Name}]->{(prop.Required ? "1..1" : "0..1")}{(prop.Role != null ? $" {prop.Role}" : string.Empty)}[{classesToResolve[prop]}]\r\n");
                }

                foreach (var prop in classe.Properties.OfType<CompositionProperty>())
                {
                    sb.Append($"[{classe.Name}]{(prop.Kind == Composition.Object ? "1..1" : "0..n")}+->[{classesToResolve[prop]}]\r\n");
                }

                sb.Append("\r\n");
            }

            Console.WriteLine(sb.ToString());
        }
    }
}