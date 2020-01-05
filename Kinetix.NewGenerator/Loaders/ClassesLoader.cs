using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public static void LoadClasses(FileDescriptor descriptor, Parser parser, Dictionary<string, Class> classes, Dictionary<(string Module, string Kind, string File), (FileDescriptor descriptor, Parser parser)> classFiles, IDictionary<string, Domain> domains, IDeserializer deserializer)
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
                        LoadClasses(a, b, classes, classFiles, domains, deserializer);
                    }
                }
            }

            var classesToResolve = new List<(object, string)>();
            var ns = new Namespace { Module = descriptor.Module, Kind = descriptor.Kind };

            while (parser.TryConsume<DocumentStart>(out _))
            {
                parser.Consume<MappingStart>();
                var scalar = parser.Consume<Scalar>();
                if (scalar.Value != "class")
                {
                    throw new Exception("Seuls des classes peuvent être définis dans un fichier de classes");
                }

                parser.Consume<MappingStart>();

                var classe = new Class { Namespace = ns };

                while (!(parser.Current is Scalar { Value: "properties" }))
                {
                    var prop = parser.Consume<Scalar>().Value;
                    var value = parser.Consume<Scalar>().Value;

                    switch (prop)
                    {
                        case "trigram":
                            classe.Trigram = value;
                            break;
                        case "name":
                            classe.Name = value;
                            break;
                        case "extends":
                            classesToResolve.Add((classe, value));
                            break;
                        case "label":
                            classe.Label = value;
                            break;
                        case "stereotype":
                            classe.Stereotype = value;
                            break;
                        case "orderProperty":
                            classe.OrderProperty = value;
                            break;
                        case "defaultProperty":
                            classe.DefaultProperty = value;
                            break;
                        case "comment":
                            classe.Comment = value;
                            break;
                        default:
                            throw new Exception($"Propriété ${prop} inconnue pour une classe");
                    }
                }

                parser.Consume<Scalar>();
                parser.Consume<SequenceStart>();

                while (!(parser.Current is SequenceEnd))
                {
                    parser.Consume<MappingStart>();
                    switch (parser.Current)
                    {
                        case Scalar { Value: "name" }:
                            var rp = new RegularProperty();

                            while (!(parser.Current is MappingEnd))
                            {
                                var prop = parser.Consume<Scalar>().Value;
                                var value = parser.Consume<Scalar>().Value;

                                switch (prop)
                                {
                                    case "name":
                                        rp.Name = value;
                                        break;
                                    case "label":
                                        rp.Label = value;
                                        break;
                                    case "primaryKey":
                                        rp.PrimaryKey = value == "true";
                                        if (rp.PrimaryKey) 
                                        {
                                            rp.Required = true;
                                        }
                                        break;
                                    case "required":
                                        rp.Required = value == "true";
                                        break;
                                    case "domain":
                                        rp.Domain = domains[value];
                                        break;
                                    case "defaultValue":
                                        rp.DefaultValue = value;
                                        break;
                                    case "comment":
                                        rp.Comment = value;
                                        break;
                                    default:
                                        throw new Exception($"Propriété ${prop} inconnue pour une propriété");
                                }
                            }

                            classe.Properties.Add(rp);
                            break;
                        case Scalar { Value: "association" }:
                            var ap = new AssociationProperty();

                            while (!(parser.Current is MappingEnd))
                            {
                                var prop = parser.Consume<Scalar>().Value;
                                var value = parser.Consume<Scalar>().Value;

                                switch (prop)
                                {
                                    case "association":
                                        classesToResolve.Add((ap, value));
                                        break;
                                    case "role":
                                        ap.Role = value;
                                        break;
                                    case "label":
                                        ap.Label = value;
                                        break;
                                    case "required":
                                        ap.Required = value == "true";
                                        break;
                                    case "comment":
                                        ap.Comment = value;
                                        break;
                                    default:
                                        throw new Exception($"Propriété ${prop} inconnue pour une propriété");
                                }
                            }

                            classe.Properties.Add(ap);
                            break;
                        case Scalar { Value: "composition" }:
                            var cp = new CompositionProperty();

                            while (!(parser.Current is MappingEnd))
                            {
                                var prop = parser.Consume<Scalar>().Value;
                                var value = parser.Consume<Scalar>().Value;

                                switch (prop)
                                {
                                    case "composition":
                                        classesToResolve.Add((cp, value));
                                        break;
                                    case "name":
                                        cp.Name = value;
                                        break;
                                    case "kind":
                                        cp.Kind = value;
                                        break;
                                    case "comment":
                                        cp.Comment = value;
                                        break;
                                    default:
                                        throw new Exception($"Propriété ${prop} inconnue pour une propriété");
                                }
                            }

                            classe.Properties.Add(cp);
                            break;
                        case Scalar { Value: "alias" }:
                            var alp = new AliasProperty();

                            parser.Consume<Scalar>();
                            parser.Consume<MappingStart>();

                            string aliasProp = null;
                            string aliasClass = null;
                            while (!(parser.Current is MappingEnd))
                            {
                                var prop = parser.Consume<Scalar>().Value;
                                var value = parser.Consume<Scalar>().Value;

                                switch (prop)
                                {
                                    case "property":
                                        aliasProp = value;
                                        break;
                                    case "class":
                                        aliasClass = value;
                                        break;
                                    default:
                                        throw new Exception($"Propriété ${prop} inconnue pour un alias");
                                }
                            }

                            classesToResolve.Add((alp, aliasProp + "|" + aliasClass));
                            parser.Consume<MappingEnd>();

                            while (!(parser.Current is MappingEnd))
                            {
                                var prop = parser.Consume<Scalar>().Value;
                                var value = parser.Consume<Scalar>().Value;

                                switch (prop)
                                {
                                    case "prefix":
                                        alp.Prefix = value;
                                        break;
                                    case "suffix":
                                        alp.Suffix = value;
                                        break;
                                    default:
                                        throw new Exception($"Propriété ${prop} inconnue pour une propriété");
                                }
                            }

                            classe.Properties.Add(alp);
                            break;
                        default:
                            throw new Exception($"Erreur lors du parsing des propriétés de la classe {classe.Name}");
                    }

                    parser.Consume<MappingEnd>();
                }

                parser.Consume<SequenceEnd>();
                parser.Consume<MappingEnd>();
                parser.Consume<MappingEnd>();
                parser.Consume<DocumentEnd>();

                classes.Add(classe.Name, classe);
            }

            foreach (var (obj, className) in classesToResolve)
            {
                switch (obj)
                {
                    case Class classe:
                        classe.Extends = classes[className];
                        break;
                    case AssociationProperty ap:
                        ap.Association = classes[className];
                        break;
                    case CompositionProperty cp:
                        cp.Composition = classes[className];
                        break;
                    case AliasProperty alp:
                        var aliasConf = className.Split("|");
                        alp.Property = (IFieldProperty)classes[aliasConf[1]].Properties.Single(p => p.Name == aliasConf[0]);
                        break;
                }
            }

            descriptor.Loaded = true;
        }
    }
}
