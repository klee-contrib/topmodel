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
        private readonly ModelConfig _config;

        public ModelFileLoader(FileChecker fileChecker, ModelConfig config)
        {
            _config = config;
            _fileChecker = fileChecker;
        }

        public ModelFile LoadModelFile(string filePath)
        {
            _fileChecker.CheckModelFile(filePath);

            var parser = new Parser(new StringReader(File.ReadAllText(filePath)));
            parser.Consume<StreamStart>();

            var file = _fileChecker.Deserialize<ModelFile>(parser);
            file.Path = filePath.ToRelative();
            file.Name = Path.GetRelativePath(Path.Combine(Directory.GetCurrentDirectory(), _config.ModelRoot), filePath)
                .Replace(".yml", string.Empty)
                .Replace("\\", "/");
            file.Classes = LoadClasses(parser, file.Relationships, file.Path).ToList();

            var ns = new Namespace { App = _config.App, Module = file.Module };
            foreach (var classe in file.Classes)
            {
                classe.ModelFile = file;
                classe.Namespace = ns;
            }

            return file;
        }

        private IEnumerable<Class> LoadClasses(Parser parser, IDictionary<object, Relation> relationships, string filePath)
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

                var classe = new Class();

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
                            relationships.Add(classe, new Relation(value));
                            break;
                        case "label":
                            classe.Label = value.Value;
                            break;
                        case "reference":
                            classe.Reference = value.Value == "true";
                            break;
                        case "orderProperty":
                            classe.OrderProperty = value.Value;
                            break;
                        case "defaultProperty":
                            classe.DefaultProperty = value.Value;
                            break;
                        case "flagProperty":
                            classe.FlagProperty = value.Value;
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
                                    case "required":
                                        rp.Required = value.Value == "true";
                                        break;
                                    case "domain":
                                        relationships.Add(rp, new Relation(value));
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
                                        relationships.Add(ap, new Relation(value));
                                        break;
                                    case "asAlias":
                                        ap.AsAlias = value.Value == "true";
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
                                        relationships.Add(cp, new Relation(value));
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
                            var alps = new List<(AliasProperty Alp, Scalar AliasProp)>();
                            Scalar? aliasClass = null;

                            parser.Consume<Scalar>();
                            parser.Consume<MappingStart>();

                            while (!(parser.Current is MappingEnd))
                            {
                                var prop = parser.Consume<Scalar>().Value;
                                var next = parser.Consume<ParsingEvent>();

                                if (next is Scalar value)
                                {
                                    switch (prop)
                                    {
                                        case "property":
                                            alps.Add((new AliasProperty(), value));
                                            break;
                                        case "class":
                                            aliasClass = value;
                                            break;
                                        default:
                                            throw new Exception($"Propriété ${prop} inconnue pour un alias");
                                    }
                                }
                                else if (next is SequenceStart)
                                {
                                    while (!(parser.Current is SequenceEnd))
                                    {
                                        alps.Add((new AliasProperty(), parser.Consume<Scalar>()));
                                    }

                                    parser.Consume<SequenceEnd>();
                                }
                            }

                            foreach (var (alp, aliasProp) in alps)
                            {
                                relationships.Add(alp, new Relation(aliasProp) { Peer = new Relation(aliasClass!) });
                            }

                            parser.Consume<MappingEnd>();

                            while (!(parser.Current is MappingEnd))
                            {
                                var prop = parser.Consume<Scalar>().Value;
                                var value = parser.Consume<Scalar>().Value;

                                foreach (var (alp, _) in alps)
                                {
                                    switch (prop)
                                    {
                                        case "prefix":
                                            alp.Prefix = value == "true" ? aliasClass!.Value : value == "false" ? null : value;
                                            break;
                                        case "suffix":
                                            alp.Suffix = value == "true" ? aliasClass!.Value : value == "false" ? null : value;
                                            break;
                                        case "label":
                                            alp.Label = value;
                                            break;
                                        case "required":
                                            alp.Required = value == "true";
                                            break;
                                        default:
                                            throw new Exception($"Propriété ${prop} inconnue pour une propriété");
                                    }
                                }
                            }

                            foreach (var (alp, _) in alps)
                            {
                                classe.Properties.Add(alp);
                            }

                            break;
                        default:
                            throw new Exception($"Erreur lors du parsing des propriétés de la classe {classe.Name}");
                    }

                    parser.Consume<MappingEnd>();
                }

                parser.Consume<SequenceEnd>();

                while (!(parser.Current is MappingEnd))
                {
                    var pos = $"[{parser.Current.Start.Line},{parser.Current.Start.Column}]";

                    if (parser.Current is Scalar { Value: "unique" } uks)
                    {
                        parser.Consume<Scalar>();
                        var uniqueKeys = _fileChecker.Deserialize<IList<IList<string>>>(parser);
                        classe.UniqueKeys = uniqueKeys.Select(uk => (IList<IFieldProperty>)uk.Select(propName =>
                        {
                            var regularProperty = classe.Properties.OfType<RegularProperty>().SingleOrDefault(rp => rp.Name == propName);
                            if (regularProperty != null)
                            {
                                return regularProperty;
                            }

                            var associationProperty = classe.Properties.OfType<AssociationProperty>().SingleOrDefault(ap => $"{relationships[ap].Value}{ap.Role ?? string.Empty}" == propName);
                            if (associationProperty != null)
                            {
                                return (IFieldProperty)associationProperty;
                            }

                            throw new Exception($@"{filePath}{pos}: La propriété ""{propName}"" n'existe pas sur la classe {classe}.");
                        }).ToList()).ToList();
                    }
                    else if (parser.Current is Scalar { Value: "values" } vs)
                    {
                        parser.Consume<Scalar>();
                        var references = _fileChecker.Deserialize<IDictionary<string, IDictionary<string, object>>>(parser);
                        classe.ReferenceValues = references.Select(reference => new ReferenceValue
                        {
                            Name = reference.Key,
                            Value = classe.Properties.OfType<IFieldProperty>().Select<IFieldProperty, (IFieldProperty Prop, object PropValue)>(prop =>
                            {
                                var propName = prop switch
                                {
                                    RegularProperty rp => rp.Name,
                                    AssociationProperty ap => $"{relationships[ap].Value}{ap.Role ?? string.Empty}",
                                    _ => throw new Exception($"{filePath}{pos}: Type de propriété non géré pour initialisation.")
                                };
                                reference.Value.TryGetValue(propName, out var propValue);
                                if (propValue == null && prop.Required && (!prop.PrimaryKey || relationships[prop].Value != "DO_ID"))
                                {
                                    throw new Exception($"{filePath}{pos}: L'initilisation {reference.Key} de la classe {classe.Name} n'initialise pas la propriété obligatoire '{propName}'.");
                                }

                                return (prop, propValue!);
                            }).ToDictionary(v => v.Prop, v => v.PropValue)
                        }).ToList();
                    }
                    else
                    {
                        throw new Exception($"Erreur dans la définition de la classe {classe.Name}.");
                    }
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
