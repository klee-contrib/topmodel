using TopModel.Core.FileModel;
using TopModel.Utils;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.Loaders;

public class ClassLoader
{
    private readonly ModelConfig _modelConfig;
    private readonly PropertyLoader _propertyLoader;

    public ClassLoader(ModelConfig modelConfig, PropertyLoader propertyLoader)
    {
        _modelConfig = modelConfig;
        _propertyLoader = propertyLoader;
    }

    internal Class LoadClass(Parser parser)
    {
        var classe = new Class();

        parser.ConsumeMapping(() =>
        {
            var prop = parser.Consume<Scalar>();
            _ = parser.TryConsume<Scalar>(out var value);

            switch (prop.Value)
            {
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
                case "decorators":
                    parser.ConsumeSequence(() =>
                    {
                        if (parser.Current is MappingStart)
                        {
                            parser.ConsumeMapping(() =>
                            {
                                var decorator = new DecoratorReference(parser.Consume<Scalar>());

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
                        foreach (var property in _propertyLoader.LoadProperty(parser))
                        {
                            classe.Properties.Add(property);
                        }
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
                    parser.ConsumeMapping(() =>
                    {
                        var name = new Reference(parser.Consume<Scalar>());
                        var values = new Dictionary<Reference, string>();

                        classe.ReferenceValueReferences.Add(name, values);

                        parser.ConsumeMapping(() =>
                        {
                            values.Add(new Reference(parser.Consume<Scalar>()), parser.Consume<Scalar>().Value);
                        });
                    });
                    break;
                case "mappers":
                    parser.ConsumeMapping(() =>
                    {
                        var subProp = parser.Consume<Scalar>().Value;
                        switch (subProp)
                        {
                            case "from":
                                parser.ConsumeSequence(() =>
                                {
                                    var mapper = new FromMapper();
                                    classe.FromMappers.Add(mapper);

                                    parser.ConsumeMapping(() =>
                                    {
                                        var subSubProp = parser.Consume<Scalar>();
                                        switch (subSubProp.Value)
                                        {
                                            case "comment":
                                                mapper.Comment = parser.Consume<Scalar>().Value;
                                                break;
                                            case "params":
                                                mapper.Reference = new LocatedString(subSubProp);
                                                parser.ConsumeSequence(() =>
                                                {
                                                    var param = new ClassMappings();
                                                    mapper.Params.Add(param);

                                                    Scalar classScalar = null!;
                                                    parser.ConsumeMapping(() =>
                                                    {
                                                        var subSubSubProp = parser.Consume<Scalar>().Value;
                                                        switch (subSubSubProp)
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
                                                                parser.ConsumeMapping(() =>
                                                                {
                                                                    param.MappingReferences.Add(new Reference(parser.Consume<Scalar>()), new Reference(parser.Consume<Scalar>()));
                                                                });
                                                                break;
                                                        }
                                                    });

                                                    param.Name ??= new LocatedString(classScalar) { Value = param.ClassReference.ReferenceName.ToFirstLower() };
                                                });
                                                break;
                                        }
                                    });
                                });
                                break;

                            case "to":
                                parser.ConsumeSequence(() =>
                                {
                                    var mapper = new ClassMappings();
                                    classe.ToMappers.Add(mapper);

                                    parser.ConsumeMapping(() =>
                                    {
                                        var subSubProp = parser.Consume<Scalar>().Value;
                                        Scalar classScalar = null!;
                                        switch (subSubProp)
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
                                                parser.ConsumeMapping(() =>
                                                {
                                                    mapper.MappingReferences.Add(new Reference(parser.Consume<Scalar>()), new Reference(parser.Consume<Scalar>()));
                                                });
                                                break;
                                        }

                                        mapper.Name ??= new LocatedString(classScalar) { Value = $"To{mapper.ClassReference.ReferenceName}" };
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