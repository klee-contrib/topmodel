using TopModel.Core.FileModel;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.Loaders;

public class PropertyLoader : ILoader<IProperty>
{
    private readonly ModelConfig _modelConfig;

    public PropertyLoader(ModelConfig modelConfig)
    {
        _modelConfig = modelConfig;
    }

    /// <inheritdoc cref="ILoader{T}.Load" />
    public IProperty Load(Parser parser)
    {
        parser.Consume<MappingStart>();
        switch (parser.Current)
        {
            case Scalar { Value: "name" }:
                var rp = new RegularProperty { UseLegacyRoleName = _modelConfig.UseLegacyRoleNames };

                while (parser.Current is not MappingEnd)
                {
                    var prop = parser.Consume<Scalar>().Value;
                    _ = parser.TryConsume<Scalar>(out var value);

                    switch (prop)
                    {
                        case "name":
                            rp.Name = value!.Value;
                            rp.Location = new Reference(value);
                            break;
                        case "label":
                            rp.Label = value!.Value;
                            break;
                        case "primaryKey":
                            rp.PrimaryKey = value!.Value == "true";
                            break;
                        case "required":
                            rp.Required = value!.Value == "true";
                            break;
                        case "readonly":
                            rp.Readonly = value!.Value == "true";
                            break;
                        case "domain":
                            rp.DomainReference = parser.ConsumeDomain(value);
                            break;
                        case "defaultValue":
                            rp.DefaultValue = value!.Value;
                            break;
                        case "comment":
                            rp.Comment = value!.Value;
                            break;
                        case "trigram":
                            rp.Trigram = new LocatedString(value);
                            break;
                        default:
                            throw new ModelException($"Propriété ${prop} inconnue pour une propriété");
                    }
                }

                if (rp.PrimaryKey)
                {
                    rp.Required = true;
                }

                parser.Consume<MappingEnd>();
                return rp;

            case Scalar { Value: "association" } s:
                var ap = new AssociationProperty
                {
                    Location = new Reference(s),
                    UseLegacyRoleName = _modelConfig.UseLegacyRoleNames
                };

                while (parser.Current is not MappingEnd)
                {
                    var prop = parser.Consume<Scalar>().Value;
                    var value = parser.Consume<Scalar>();

                    switch (prop)
                    {
                        case "association":
                            ap.Reference = new ClassReference(value);
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
                        case "as":
                            ap.As = value.Value;
                            break;
                        case "label":
                            ap.Label = value.Value;
                            break;
                        case "required":
                            ap.Required = value.Value == "true";
                            break;
                        case "primaryKey":
                            ap.PrimaryKey = value.Value == "true";
                            break;
                        case "readonly":
                            ap.Readonly = value.Value == "true";
                            break;
                        case "defaultValue":
                            ap.DefaultValue = value.Value;
                            break;
                        case "comment":
                            ap.Comment = value.Value;
                            break;
                        case "property":
                            ap.PropertyReference = new Reference(value);
                            break;
                        case "trigram":
                            ap.Trigram = new LocatedString(value);
                            break;
                        default:
                            throw new ModelException($"Propriété ${prop} inconnue pour une propriété");
                    }
                }

                if (ap.PrimaryKey)
                {
                    ap.Required = true;
                }

                parser.Consume<MappingEnd>();
                return ap;

            case Scalar { Value: "composition" } s:
                var cp = new CompositionProperty
                {
                    Location = new Reference(s),
                    UseLegacyRoleName = _modelConfig.UseLegacyRoleNames
                };

                while (parser.Current is not MappingEnd)
                {
                    var prop = parser.Consume<Scalar>().Value;
                    _ = parser.TryConsume<Scalar>(out var value);

                    switch (prop)
                    {
                        case "composition":
                            cp.Reference = new ClassReference(value!);
                            break;
                        case "name":
                            cp.Name = value!.Value;
                            break;
                        case "label":
                            cp.Label = value!.Value;
                            break;
                        case "domain":
                            cp.DomainReference = parser.ConsumeDomain(value);
                            break;
                        case "comment":
                            cp.Comment = value!.Value;
                            break;
                        case "readonly":
                            cp.Readonly = value!.Value == "true";
                            break;
                        case "required":
                            cp.Required = value!.Value == "true";
                            break;
                        case "trigram":
                            cp.Trigram = new LocatedString(value);
                            break;
                        default:
                            throw new ModelException($"Propriété ${prop} inconnue pour une propriété");
                    }
                }

                parser.Consume<MappingEnd>();
                return cp;

            case Scalar { Value: "alias" } s:
                var aliasReference = new AliasReference();

                parser.Consume<Scalar>();
                parser.Consume<MappingStart>();

                while (parser.Current is not MappingEnd)
                {
                    var prop = parser.Consume<Scalar>().Value;
                    var next = parser.Consume<ParsingEvent>();

                    switch (prop)
                    {
                        case "class":
                            aliasReference.Start = ((Scalar)next).Start;
                            aliasReference.End = ((Scalar)next).End;
                            aliasReference.ReferenceName = ((Scalar)next).Value;
                            break;
                        case "include" or "property" when next is Scalar pValue:
                            aliasReference.AddInclude(pValue);
                            break;
                        case "exclude" when next is Scalar eValue:
                            aliasReference.AddExclude(eValue);
                            break;
                        case "include" or "property" when next is SequenceStart:
                            while (parser.Current is not SequenceEnd)
                            {
                                aliasReference.AddInclude(parser.Consume<Scalar>());
                            }

                            parser.Consume<SequenceEnd>();
                            break;
                        case "exclude" when next is SequenceStart:
                            while (parser.Current is not SequenceEnd)
                            {
                                aliasReference.AddExclude(parser.Consume<Scalar>());
                            }

                            parser.Consume<SequenceEnd>();
                            break;
                        default:
                            throw new ModelException($"Propriété '{prop}' inconnue pour un alias");
                    }
                }

                parser.Consume<MappingEnd>();

                var alp = new AliasProperty
                {
                    Location = new Reference(s),
                    UseLegacyRoleName = _modelConfig.UseLegacyRoleNames
                };

                while (parser.Current is not MappingEnd)
                {
                    var prop = parser.Consume<Scalar>().Value;
                    _ = parser.TryConsume<Scalar>(out var value);

                    switch (prop)
                    {
                        case "prefix":
                            alp.Prefix = value!.Value == "true" ? aliasReference.ReferenceName : value.Value == "false" ? null : value.Value;
                            break;
                        case "suffix":
                            alp.Suffix = value!.Value == "true" ? aliasReference.ReferenceName : value.Value == "false" ? null : value.Value;
                            break;
                        case "label":
                            alp.Label = value!.Value;
                            break;
                        case "domain":
                            alp.DomainReference = parser.ConsumeDomain(value);
                            break;
                        case "required":
                            alp.Required = value!.Value == "true";
                            break;
                        case "primaryKey":
                            alp.PrimaryKey = value!.Value == "true";
                            break;
                        case "readonly":
                            alp.Readonly = value!.Value == "true";
                            break;
                        case "comment":
                            alp.Comment = value!.Value;
                            break;
                        case "as":
                            alp.As = value!.Value;
                            break;
                        case "name":
                            alp.Name = value!.Value;
                            break;
                        case "trigram":
                            alp.Trigram = new LocatedString(value);
                            break;
                        case "defaultValue":
                            alp.DefaultValue = value!.Value;
                            break;
                        default:
                            throw new ModelException($"Propriété ${prop} inconnue pour une propriété");
                    }
                }

                alp.Reference = aliasReference;
                parser.Consume<MappingEnd>();
                return alp;

            default:
                throw new ModelException($"Type de propriété inconnu.");
        }
    }
}