using TopModel.Core.Types;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.Loaders;

public class DecoratorLoader
{
    private readonly FileChecker _fileChecker;

    public DecoratorLoader(FileChecker fileChecker)
    {
        _fileChecker = fileChecker;
    }

    public Decorator LoadDecorator(Parser parser)
    {
        var decorator = new Decorator();

        parser.ConsumeMapping(() =>
        {
            var prop = parser.Consume<Scalar>().Value;
            _ = parser.TryConsume<Scalar>(out var value);

            switch (prop)
            {
                case "name":
                    decorator.Name = new LocatedString(value);
                    break;
                case "description":
                    decorator.Description = value!.Value;
                    break;
                case "csharp":
                    decorator.CSharp = _fileChecker.Deserialize<CSharpDecorator>(parser);
                    break;
                case "java":
                    decorator.Java = _fileChecker.Deserialize<JavaDecorator>(parser);
                    break;
                case "properties":
                    parser.ConsumeSequence(() =>
                    {
                        foreach (var property in PropertyLoader.LoadProperty(parser))
                        {
                            decorator.Properties.Add(property);
                        }
                    });
                    break;
                default:
                    throw new ModelException(decorator, $"Propriété ${prop} inconnue pour un décoteur");
            }
        });

        foreach (var prop in decorator.Properties)
        {
            prop.Decorator = decorator;
        }

        return decorator;
    }
}