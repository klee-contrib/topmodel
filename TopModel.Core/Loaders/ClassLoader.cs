using TopModel.Core.FileModel;
using TopModel.Utils;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.Loaders;

public class ClassLoader
{
    private readonly FileChecker _fileChecker;
    private readonly ModelConfig _modelConfig;

    public ClassLoader(FileChecker fileChecker, ModelConfig modelConfig)
    {
        _fileChecker = fileChecker;
        _modelConfig = modelConfig;
    }

    internal Class LoadClass(Parser parser, string filePath)
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
                        classe.DecoratorReferences.Add(new DecoratorReference(parser.Consume<Scalar>()));
                    });
                    break;
                case "properties":
                    parser.ConsumeSequence(() =>
                    {
                        foreach (var property in PropertyLoader.LoadProperty(parser))
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
                                    parser.ConsumeMapping(() =>
                                    {
                                        var subSubProp = parser.Consume<Scalar>().Value;
                                        switch (subSubProp)
                                        {
                                            case "params":
                                                parser.ConsumeSequence(() =>
                                                {
                                                    parser.ConsumeMapping(() =>
                                                    {
                                                        var subSubSubProp = parser.Consume<Scalar>().Value;
                                                        switch (subSubSubProp)
                                                        {
                                                            case "class":
                                                                parser.Consume<Scalar>();
                                                                break;
                                                            case "name":
                                                                parser.Consume<Scalar>();
                                                                break;
                                                            case "mappings":
                                                                parser.ConsumeMapping(() =>
                                                                {
                                                                    parser.Consume<Scalar>();
                                                                    parser.Consume<Scalar>();
                                                                });
                                                                break;
                                                        }
                                                    });
                                                });
                                                break;
                                        }
                                    });
                                });
                                break;

                            case "to":
                                parser.ConsumeSequence(() =>
                                {
                                    parser.ConsumeMapping(() =>
                                    {
                                        var subSubProp = parser.Consume<Scalar>().Value;
                                        switch (subSubProp)
                                        {
                                            case "class":
                                                parser.Consume<Scalar>();
                                                break;
                                            case "name":
                                                parser.Consume<Scalar>();
                                                break;
                                            case "mappings":
                                                parser.ConsumeMapping(() =>
                                                {
                                                    parser.Consume<Scalar>();
                                                    parser.Consume<Scalar>();
                                                });
                                                break;
                                        }
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
        classe.SqlName ??= ModelUtils.ConvertCsharp2Bdd(_modelConfig.PluralizeTableNames ? classe.PluralName : classe.Name);

        foreach (var prop in classe.Properties)
        {
            prop.Class = classe;
        }

        return classe;
    }
}