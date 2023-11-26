using TopModel.Core.FileModel;
using TopModel.Utils;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.Loaders;

public class ClassLoader : ILoader<Class>
{
    private readonly ModelConfig _modelConfig;
    private readonly PropertyLoader _propertyLoader;

    public ClassLoader(ModelConfig modelConfig, PropertyLoader propertyLoader)
    {
        _modelConfig = modelConfig;
        _propertyLoader = propertyLoader;
    }

    /// <inheritdoc cref="ILoader{T}.Load" />
    public Class Load(Parser parser)
    {
        var classe = new Class();

        parser.ConsumeMapping(prop =>
        {
            _ = parser.TryConsume<Scalar>(out var value);

            switch (prop.Value)
            {
                case "tags":
                    parser.ConsumeSequence(() => classe.OwnTags.Add(parser.Consume<Scalar>().Value));
                    break;
                case "trigram":
                    classe.Trigram = new LocatedString(value);
                    break;
                case "name":
                    classe.Name = new LocatedString(value);
                    break;
                case "pluralName":
                    classe.PluralName = value!.Value;
                    break;
                case "sqlName":
                    classe.SqlName = value!.Value;
                    break;
                case "extends":
                    classe.ExtendsReference = new ClassReference(value!);
                    break;
                case "label":
                    classe.Label = value!.Value;
                    break;
                case "reference":
                    classe.Reference = value!.Value == "true";
                    break;
                case "enum":
                    classe.EnumOverride = new LocatedString(value);
                    break;
                case "abstract":
                    classe.Abstract = value!.Value == "true";
                    break;
                case "orderProperty":
                    classe.OrderPropertyReference = new Reference(value!);
                    break;
                case "defaultProperty":
                    classe.DefaultPropertyReference = new Reference(value!);
                    break;
                case "flagProperty":
                    classe.FlagPropertyReference = new Reference(value!);
                    break;
                case "comment":
                    classe.Comment = value!.Value;
                    break;
                case "preservePropertyCasing":
                    classe.PreservePropertyCasing = value!.Value == "true";
                    break;
                case "decorators":
                    parser.ConsumeSequence(() =>
                    {
                        if (parser.Current is MappingStart)
                        {
                            parser.ConsumeMapping(prop =>
                            {
                                var decorator = new DecoratorReference(prop);

                                parser.ConsumeSequence(() =>
                                {
                                    decorator.ParameterReferences.Add(new Reference(parser.Consume<Scalar>()));
                                });

                                classe.DecoratorReferences.Add(decorator);
                            });
                        }
                        else
                        {
                            classe.DecoratorReferences.Add(new DecoratorReference(parser.Consume<Scalar>()));
                        }
                    });
                    break;
                case "properties":
                    parser.ConsumeSequence(() =>
                    {
                        classe.Properties.Add(_propertyLoader.Load(parser));
                    });
                    break;
                case "unique":
                    parser.ConsumeSequence(() =>
                    {
                        var uniqueKeyRef = new List<Reference>();
                        classe.UniqueKeyReferences.Add(uniqueKeyRef);

                        parser.ConsumeSequence(() =>
                        {
                            uniqueKeyRef.Add(new Reference(parser.Consume<Scalar>()));
                        });
                    });
                    break;
                case "values":
                    parser.ConsumeMapping(prop =>
                    {
                        var name = new Reference(prop);
                        var values = new Dictionary<Reference, string>();

                        classe.ValueReferences.Add(name, values);

                        parser.ConsumeMapping(prop =>
                        {
                            values.Add(new Reference(prop), parser.Consume<Scalar>().Value);
                        });
                    });
                    break;
                case "mappers":
                    parser.ConsumeMapping(prop =>
                    {
                        switch (prop.Value)
                        {
                            case "from":
                                parser.ConsumeSequence(() =>
                                {
                                    var mapper = new FromMapper { Class = classe };
                                    classe.FromMappers.Add(mapper);

                                    parser.ConsumeMapping(prop =>
                                    {
                                        switch (prop.Value)
                                        {
                                            case "comment":
                                                mapper.Comment = parser.Consume<Scalar>().Value;
                                                break;
                                            case "params":
                                                mapper.Reference = new LocatedString(prop);
                                                parser.ConsumeSequence(() =>
                                                {
                                                    parser.Consume<MappingStart>();
                                                    if (parser.Current is Scalar { Value: "class" })
                                                    {
                                                        var param = new ClassMappings();
                                                        mapper.Params.Add(param);

                                                        Scalar classScalar = null!;
                                                        while (parser.Current is not MappingEnd)
                                                        {
                                                            var prop = parser.Consume<Scalar>();
                                                            switch (prop.Value)
                                                            {
                                                                case "class":
                                                                    classScalar = parser.Consume<Scalar>();
                                                                    param.ClassReference = new ClassReference(classScalar);
                                                                    break;
                                                                case "required":
                                                                    param.Required = parser.Consume<Scalar>().Value == "true";
                                                                    break;
                                                                case "comment":
                                                                    param.Comment = parser.Consume<Scalar>().Value;
                                                                    break;
                                                                case "name":
                                                                    param.Name = new LocatedString(parser.Consume<Scalar>());
                                                                    break;
                                                                case "mappings":
                                                                    parser.ConsumeMapping(prop =>
                                                                    {
                                                                        param.MappingReferences.Add(new Reference(prop), new Reference(parser.Consume<Scalar>()));
                                                                    });
                                                                    break;
                                                            }
                                                        }

                                                        param.Name ??= new LocatedString(classScalar) { Value = param.ClassReference.ReferenceName.ToCamelCase(strictIfUppercase: true) };
                                                    }
                                                    else if (parser.Current is Scalar { Value: "property" })
                                                    {
                                                        var param = new PropertyMapping { FromMapper = mapper };
                                                        mapper.Params.Add(param);
                                                        while (parser.Current is not MappingEnd)
                                                        {
                                                            var prop = parser.Consume<Scalar>();
                                                            switch (prop.Value)
                                                            {
                                                                case "property":
                                                                    param.Property = _propertyLoader.Load(parser);
                                                                    param.Property.PropertyMapping = param;
                                                                    break;
                                                                case "target":
                                                                    param.TargetPropertyReference = new Reference(parser.Consume<Scalar>());
                                                                    break;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        throw new ModelException(classe, $"Erreur dans la construction des paramètres du mapper 'from'.");
                                                    }

                                                    parser.Consume<MappingEnd>();
                                                });
                                                break;
                                        }
                                    });
                                });
                                break;

                            case "to":
                                parser.ConsumeSequence(() =>
                                {
                                    var mapper = new ClassMappings { To = true };
                                    classe.ToMappers.Add(mapper);

                                    parser.ConsumeMapping(prop =>
                                    {
                                        Scalar classScalar = null!;
                                        switch (prop.Value)
                                        {
                                            case "class":
                                                classScalar = parser.Consume<Scalar>();
                                                mapper.ClassReference = new ClassReference(classScalar);
                                                break;
                                            case "name":
                                                mapper.Name = new LocatedString(parser.Consume<Scalar>());
                                                break;
                                            case "comment":
                                                mapper.Comment = parser.Consume<Scalar>().Value;
                                                break;
                                            case "mappings":
                                                parser.ConsumeMapping(prop =>
                                                {
                                                    mapper.MappingReferences.Add(new Reference(prop), new Reference(parser.Consume<Scalar>()));
                                                });
                                                break;
                                        }

                                        mapper.Name ??= new LocatedString(classScalar) { Value = $"To{mapper.ClassReference.ReferenceName.ToPascalCase(strictIfUppercase: true)}" };
                                    });
                                });
                                break;
                        }
                    });
                    break;
                default:
                    throw new ModelException(classe, $"Propriété ${prop} inconnue pour une classe");
            }
        });

        classe.Label ??= classe.Name;
        classe.SqlName ??= (_modelConfig.PluralizeTableNames ? classe.PluralName : classe.Name).ToConstantCase();

        foreach (var prop in classe.Properties)
        {
            prop.Class = classe;
        }

        return classe;
    }
}