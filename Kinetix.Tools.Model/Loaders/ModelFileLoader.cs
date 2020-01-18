using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopModel.Core.FileModel;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace TopModel.Core.Loaders
{
    public class ModelFileLoader
    {
        private readonly IDeserializer _deserializer;
        private readonly FileChecker _fileChecker;

        public ModelFileLoader(IDeserializer deserializer, FileChecker fileChecker)
        {
            _deserializer = deserializer;
            _fileChecker = fileChecker;
        }

        public ModelFile LoadModelFile(string filePath)
        {
            _fileChecker.CheckModelFile(filePath);

            var parser = new Parser(new StringReader(File.ReadAllText(filePath)));
            parser.Consume<StreamStart>();

            var descriptor = _deserializer.Deserialize<FileDescriptor>(parser);

            var relationships = new List<(object, string)>(); 
            var ns = new Namespace { Module = descriptor.Module, Kind = descriptor.Kind };
            var classes = LoadClasses(parser, relationships, ns).ToList();

            return new ModelFile
            {
                Descriptor = descriptor,
                Classes = classes,
                Relationships = relationships
            };
        }        

        private IEnumerable<Class> LoadClasses(Parser parser, List<(object, string)> relationships, Namespace ns)
        {
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
                        case "sqlName":
                            classe.SqlName = value;
                            break;
                        case "extends":
                            relationships.Add((classe, value));
                            break;
                        case "label":
                            classe.Label = value;
                            break;
                        case "stereotype":
                            classe.Stereotype = value == "Statique"
                                ? Stereotype.Statique
                                : value == "Reference"
                                ? Stereotype.Reference
                                : throw new Exception($"Stereotype inconnu: {value}");
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

                classe.Label ??= classe.Name;

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
                                        break;
                                    case "unique":
                                        rp.Unique = value == "true";
                                        break;
                                    case "required":
                                        rp.Required = value == "true";
                                        break;
                                    case "domain":
                                        relationships.Add((rp, value));
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

                            if (rp.PrimaryKey)
                            {
                                rp.Required = true;
                                rp.Unique = false;
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
                                        relationships.Add((ap, value));
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
                                    case "defaultValue":
                                        ap.DefaultValue = value;
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
                                        relationships.Add((cp, value));
                                        break;
                                    case "name":
                                        cp.Name = value;
                                        break;
                                    case "kind":
                                        cp.Kind = value == "list" ? Composition.List : Composition.Object;
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

                            string? aliasProp = null;
                            string? aliasClass = null;
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

                            relationships.Add((alp, aliasProp + "|" + aliasClass));
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

                foreach (var prop in classe.Properties)
                {
                    prop.Class = classe;
                }

                yield return classe;
            }
        }
    }
}
