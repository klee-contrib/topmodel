using System;
using System.Collections.Generic;
using System.IO;
using Kinetix.NewGenerator.FileModel;
using Kinetix.NewGenerator.Model;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Kinetix.NewGenerator.Loaders
{
    public static class ClassesLoader
    {
        public static (FileDescriptor descriptor, Parser parser) GetFileDescriptor(string filePath, IDeserializer deserializer)
        {
            var parser = new Parser(new StringReader(File.ReadAllText(filePath)));
            parser.Consume<StreamStart>();

            var descriptor = deserializer.Deserialize<FileDescriptor>(parser);

            return (descriptor, parser);
        }

        public static void LoadClasses(FileDescriptor descriptor, Parser parser, Dictionary<string, Class> classes, Dictionary<(string Module, string Kind, string File), (FileDescriptor descriptor, Parser parser)> classFiles, IDeserializer deserializer)
        {
            if (descriptor.Loaded)
            {
                return;
            }

            if (descriptor.Uses != null)
            {
                foreach (var dep in descriptor.Uses)
                {
                    foreach (var depFile in dep.Files)
                    {
                        var (a, b) = classFiles[(dep.Module, dep.Kind, depFile.File)];
                        LoadClasses(a, b, classes, classFiles, deserializer);
                    }
                }
            }

            while (parser.TryConsume<DocumentStart>(out _))
            {
                parser.Consume<MappingStart>();
                var scalar = parser.Consume<Scalar>();
                if (scalar.Value != "class")
                {
                    throw new Exception("Seuls des classes peuvent être définis dans un fichier de classes");
                }

                parser.Consume<MappingStart>();

                var classe = new Class();

                while (!(parser.Current is Scalar { Value: "properties" } propScalar))
                {
                    var prop = parser.Consume<Scalar>().Value;
                    var value = parser.Consume<Scalar>().Value;

                    object _ = prop switch
                    {
                        "trigram" => classe.Trigram = value,
                        "name" => classe.Name = value,
                        "extends" => classe.Extends = classes[value],
                        "label" => classe.Label = value,
                        "stereotype" => classe.Stereotype = value,
                        "orderProperty" => classe.OrderProperty = value,
                        "defaultProperty" => classe.DefaultProperty = value,
                        "comment" => classe.Comment = value,
                        _ => throw new Exception($"Propriété ${prop} inconnue pour une classe")
                    };
                }

                parser.Consume<Scalar>();

                // TODO : Parser les propriétés
                parser.SkipThisAndNestedEvents();

                parser.Consume<MappingEnd>();
                parser.Consume<MappingEnd>();
                parser.Consume<DocumentEnd>();

                classes.Add(classe.Name, classe);
            }
            
            descriptor.Loaded = true;
        }
    }
}
