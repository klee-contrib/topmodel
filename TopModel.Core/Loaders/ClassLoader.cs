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
        parser.Consume<MappingStart>();

        var classe = new Class();

        while (parser.Current is not MappingEnd)
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
                    parser.Consume<SequenceStart>();

                    while (parser.Current is not SequenceEnd)
                    {
                        classe.DecoratorReferences.Add(new DecoratorReference(parser.Consume<Scalar>()));
                    }

                    parser.Consume<SequenceEnd>();
                    break;
                case "properties":
                    parser.Consume<SequenceStart>();

                    while (parser.Current is not SequenceEnd)
                    {
                        foreach (var property in PropertyLoader.LoadProperty(parser))
                        {
                            classe.Properties.Add(property);
                        }
                    }

                    parser.Consume<SequenceEnd>();
                    break;
                case "unique":
                    parser.Consume<SequenceStart>();

                    while (parser.Current is not SequenceEnd)
                    {
                        var uniqueKeyRef = new List<Reference>();
                        classe.UniqueKeyReferences.Add(uniqueKeyRef);

                        parser.Consume<SequenceStart>();

                        while (parser.Current is not SequenceEnd)
                        {
                            uniqueKeyRef.Add(new Reference(parser.Consume<Scalar>()));
                        }

                        parser.Consume<SequenceEnd>();
                    }

                    parser.Consume<SequenceEnd>();
                    break;
                case "values":
                    parser.Consume<MappingStart>();

                    while (parser.Current is not MappingEnd)
                    {
                        var name = new Reference(parser.Consume<Scalar>());
                        var values = new Dictionary<Reference, string>();
                        classe.ReferenceValueReferences.Add(name, values);

                        parser.Consume<MappingStart>();

                        while (parser.Current is not MappingEnd)
                        {
                            values.Add(new Reference(parser.Consume<Scalar>()), parser.Consume<Scalar>().Value);
                        }

                        parser.Consume<MappingEnd>();
                    }

                    parser.Consume<MappingEnd>();

                    break;
                default:
                    throw new ModelException(classe, $"Propriété ${prop} inconnue pour une classe");
            }
        }

        classe.Label ??= classe.Name;
        classe.SqlName ??= ModelUtils.ConvertCsharp2Bdd(_modelConfig.PluralizeTableNames ? classe.PluralName : classe.Name);

        parser.Consume<MappingEnd>();

        foreach (var prop in classe.Properties)
        {
            prop.Class = classe;
        }

        return classe;
    }
}