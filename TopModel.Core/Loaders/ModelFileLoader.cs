using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopModel.Core.FileModel;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.Loaders
{
    public class ModelFileLoader
    {
        private readonly FileChecker _fileChecker;

        public ModelFileLoader(FileChecker fileChecker)
        {
            _fileChecker = fileChecker;
        }

        public ModelFile LoadModelFile(string filePath)
        {
            _fileChecker.CheckModelFile(filePath);

            var parser = new Parser(new StringReader(File.ReadAllText(filePath)));
            parser.Consume<StreamStart>();

            var descriptor = _fileChecker.Deserialize<FileDescriptor>(parser);

            var relationships = new List<(object, Relation)>();
            var ns = new Namespace { App = descriptor.App, Module = descriptor.Module, Kind = descriptor.Kind };
            var path = filePath.ToRelative();
            var classes = LoadClasses(parser, relationships, ns, path).ToList();

            var file = new ModelFile
            {
                Path = path,
                Descriptor = descriptor,
                Classes = classes,
                Relationships = relationships
            };

            foreach (var classe in classes)
            {
                classe.ModelFile = file;
            }

            return file;
        }

        private IEnumerable<Class> LoadClasses(Parser parser, List<(object Source, Relation Target)> relationships, Namespace ns, string filePath)
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
                    var value = parser.Consume<Scalar>();

                    switch (prop)
                    {
                        case "trigram":
                            classe.Trigram = value.Value;
                            break;
                        case "name":
                            classe.Name = value.Value;
                            break;
                        case "sqlName":
                            classe.SqlName = value.Value;
                            break;
                        case "extends":
                            relationships.Add((classe, new Relation(value)));
                            break;
                        case "label":
                            classe.Label = value.Value;
                            break;
                        case "stereotype":
                            classe.Stereotype = value.Value == "Statique"
                                ? Stereotype.Statique
                                : value.Value == "Reference"
                                ? Stereotype.Reference
                                : throw new Exception($"Stereotype inconnu: {value}");
                            break;
                        case "orderProperty":
                            classe.OrderProperty = value.Value;
                            break;
                        case "defaultProperty":
                            classe.DefaultProperty = value.Value;
                            break;
                        case "comment":
                            classe.Comment = value.Value;
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
                                var value = parser.Consume<Scalar>();

                                switch (prop)
                                {
                                    case "name":
                                        rp.Name = value.Value;
                                        break;
                                    case "label":
                                        rp.Label = value.Value;
                                        break;
                                    case "primaryKey":
                                        rp.PrimaryKey = value.Value == "true";
                                        break;
                                    case "unique":
                                        rp.Unique = value.Value == "true";
                                        break;
                                    case "required":
                                        rp.Required = value.Value == "true";
                                        break;
                                    case "domain":
                                        relationships.Add((rp, new Relation(value)));
                                        break;
                                    case "defaultValue":
                                        rp.DefaultValue = value.Value;
                                        break;
                                    case "comment":
                                        rp.Comment = value.Value;
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
                                var value = parser.Consume<Scalar>();

                                switch (prop)
                                {
                                    case "association":
                                        relationships.Add((ap, new Relation(value)));
                                        break;
                                    case "role":
                                        ap.Role = value.Value;
                                        break;
                                    case "label":
                                        ap.Label = value.Value;
                                        break;
                                    case "required":
                                        ap.Required = value.Value == "true";
                                        break;
                                    case "defaultValue":
                                        ap.DefaultValue = value.Value;
                                        break;
                                    case "comment":
                                        ap.Comment = value.Value;
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
                                var value = parser.Consume<Scalar>();

                                switch (prop)
                                {
                                    case "composition":
                                        relationships.Add((cp, new Relation(value)));
                                        break;
                                    case "name":
                                        cp.Name = value.Value;
                                        break;
                                    case "kind":
                                        cp.Kind = value.Value == "list" ? Composition.List : Composition.Object;
                                        break;
                                    case "comment":
                                        cp.Comment = value.Value;
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

                            Scalar? aliasProp = null;
                            Scalar? aliasClass = null;
                            while (!(parser.Current is MappingEnd))
                            {
                                var prop = parser.Consume<Scalar>().Value;
                                var value = parser.Consume<Scalar>();

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

                            relationships.Add((alp, new Relation(aliasProp!) { Peer = new Relation(aliasClass!) }));
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

                if (parser.Current is Scalar { Value: "values" })
                {
                    parser.Consume<Scalar>();
                    var references = _fileChecker.Deserialize<IDictionary<string, IDictionary<string, object>>>(parser);
                    classe.ReferenceValues = references.Select(reference => new ReferenceValue
                    {
                        Name = reference.Key,
                        Value = classe.Properties.OfType<IFieldProperty>().Select(prop =>
                        {
                            reference.Value.TryGetValue(
                                prop switch
                                {
                                    RegularProperty rp => rp.Name,
                                    AssociationProperty ap => $"{relationships.Single(r => r.Source == ap).Target.Value}{ap.Role ?? string.Empty}",
                                    _ => throw new Exception($"{filePath}: Type de propriété non géré pour initialisation.")
                                },
                                out var propValue);
                            if (propValue == null && prop.Required && (!prop.PrimaryKey || classe.Stereotype == Stereotype.Statique))
                            {
                                throw new Exception($"{filePath}: L'initilisation {reference.Key} de la classe {classe.Name} n'initialise pas la propriété obligatoire '{prop.Name}'.");
                            }

                            return (prop, propValue);
                        }).ToDictionary(v => v.prop, v => v.propValue)
                    }).ToList();
                }

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
