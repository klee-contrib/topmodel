using TopModel.Core.FileModel;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.Loaders;

internal static class PropertyLoader
{
    public static IEnumerable<IProperty> LoadProperty(Parser parser, List<(object Target, Relation Relation)> relationships)
    {
        parser.Consume<MappingStart>();
        switch (parser.Current)
        {
            case Scalar { Value: "name" }:
                var rp = new RegularProperty();

                while (parser.Current is not MappingEnd)
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
                            relationships.Add((rp, new DomainRelation(value)));
                            break;
                        case "defaultValue":
                            rp.DefaultValue = value.Value;
                            break;
                        case "comment":
                            rp.Comment = value.Value;
                            break;
                        default:
                            throw new ModelException($"Propriété ${prop} inconnue pour une propriété");
                    }
                }

                if (rp.PrimaryKey)
                {
                    rp.Required = true;
                }

                yield return rp;
                break;

            case Scalar { Value: "association" }:
                var ap = new AssociationProperty();

                while (parser.Current is not MappingEnd)
                {
                    var prop = parser.Consume<Scalar>().Value;
                    var value = parser.Consume<Scalar>();

                    switch (prop)
                    {
                        case "association":
                            relationships.Add((ap, new ClassRelation(value)));
                            break;
                        case "asAlias":
                            ap.AsAlias = value.Value == "true";
                            break;
                        case "role":
                            ap.Role = value.Value;
                            break;
                        case "type":
                            ap.Type = value.Value switch
                            {
                                "oneToOne" => AssociationType.OneToOne,
                                "manyToOne" => AssociationType.ManyToOne,
                                "manyToMany" => AssociationType.ManyToMany,
                                _ => AssociationType.OneToMany
                            };
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
                            throw new ModelException($"Propriété ${prop} inconnue pour une propriété");
                    }
                }

                yield return ap;
                break;

            case Scalar { Value: "composition" }:
                var cp = new CompositionProperty();

                while (parser.Current is not MappingEnd)
                {
                    var prop = parser.Consume<Scalar>().Value;
                    var value = parser.Consume<Scalar>();

                    switch (prop)
                    {
                        case "composition":
                            relationships.Add((cp, new ClassRelation(value)));
                            break;
                        case "name":
                            cp.Name = value.Value;
                            break;
                        case "kind":
                            cp.Kind = value.Value;
                            if (cp.Kind != "object" && cp.Kind != "list" && cp.Kind != "async-list")
                            {
                                relationships.Add((cp, new DomainRelation(value)));
                            }

                            break;
                        case "comment":
                            cp.Comment = value.Value;
                            break;
                        default:
                            throw new ModelException($"Propriété ${prop} inconnue pour une propriété");
                    }
                }

                yield return cp;
                break;

            case Scalar { Value: "alias" }:
                var aliasRelation = new AliasRelation();

                parser.Consume<Scalar>();
                parser.Consume<MappingStart>();

                while (parser.Current is not MappingEnd)
                {
                    var prop = parser.Consume<Scalar>().Value;
                    var next = parser.Consume<ParsingEvent>();

                    switch (prop)
                    {
                        case "class":
                            aliasRelation.Reference = new Reference((Scalar)next);
                            break;
                        case "include" or "property" when next is Scalar pValue:
                            aliasRelation.AddInclude(pValue);
                            break;
                        case "exclude" when next is Scalar eValue:
                            aliasRelation.AddExclude(eValue);
                            break;
                        case "include" or "property" when next is SequenceStart:
                            while (parser.Current is not SequenceEnd)
                            {
                                aliasRelation.AddInclude(parser.Consume<Scalar>());
                            }

                            parser.Consume<SequenceEnd>();
                            break;
                        case "exclude" when next is SequenceStart:
                            while (parser.Current is not SequenceEnd)
                            {
                                aliasRelation.AddExclude(parser.Consume<Scalar>());
                            }

                            parser.Consume<SequenceEnd>();
                            break;
                        default:
                            throw new ModelException($"Propriété '{prop}' inconnue pour un alias");
                    }
                }

                parser.Consume<MappingEnd>();

                var alp = new AliasProperty();

                while (parser.Current is not MappingEnd)
                {
                    var prop = parser.Consume<Scalar>().Value;
                    var value = parser.Consume<Scalar>();

                    switch (prop)
                    {
                        case "prefix":
                            alp.Prefix = value.Value == "true" ? aliasRelation.ReferenceName : value.Value == "false" ? null : value.Value;
                            break;
                        case "suffix":
                            alp.Suffix = value.Value == "true" ? aliasRelation.ReferenceName : value.Value == "false" ? null : value.Value;
                            break;
                        case "label":
                            alp.Label = value.Value;
                            break;
                        case "required":
                            alp.Required = value.Value == "true";
                            break;
                        case "comment":
                            alp.Comment = value.Value;
                            break;
                        case "asListWithDomain":
                            relationships.Add((alp, new DomainRelation(value)));
                            break;
                        default:
                            throw new ModelException($"Propriété ${prop} inconnue pour une propriété");
                    }
                }

                relationships.Add((alp, aliasRelation));
                yield return alp;
                break;

            default:
                throw new ModelException($"Type de propriété inconnu.");
        }

        parser.Consume<MappingEnd>();
    }
}