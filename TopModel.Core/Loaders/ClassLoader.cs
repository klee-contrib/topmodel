using System;
using System.Collections.Generic;
using System.Linq;
using TopModel.Core.FileModel;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.Loaders
{
    public class ClassLoader
    {
        private readonly FileChecker _fileChecker;

        public ClassLoader(FileChecker fileChecker)
        {
            _fileChecker = fileChecker;
        }

        public Class LoadClass(Parser parser, IDictionary<object, Relation> relationships, string filePath)
        {
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
                foreach (var property in PropertyLoader.LoadProperty(parser, relationships))
                {
                    classe.Properties.Add(property);
                }
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

                        return associationProperty != null
                            ? (IFieldProperty)associationProperty
                            : throw new Exception($@"{filePath}{pos}: La propriété ""{propName}"" n'existe pas sur la classe {classe}.");
                    })
                    .ToList()).ToList();
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

                            return propValue == null && prop.Required && (!prop.PrimaryKey || relationships[prop].Value != "DO_ID")
                                ? throw new Exception($"{filePath}{pos}: L'initilisation {reference.Key} de la classe {classe.Name} n'initialise pas la propriété obligatoire '{propName}'.")
                                : (prop, propValue!);
                        })
                        .ToDictionary(v => v.Prop, v => v.PropValue)
                    }).ToList();
                }
                else
                {
                    throw new Exception($"Erreur dans la définition de la classe {classe.Name}.");
                }
            }

            parser.Consume<MappingEnd>();

            foreach (var prop in classe.Properties)
            {
                prop.Class = classe;
            }

            return classe;
        }
    }
}
