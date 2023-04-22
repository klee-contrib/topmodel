using TopModel.Core.Model.Implementation;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.Loaders;

public class DecoratorLoader : ILoader<Decorator>
{
    private readonly FileChecker _fileChecker;
    private readonly PropertyLoader _propertyLoader;

    public DecoratorLoader(FileChecker fileChecker, PropertyLoader propertyLoader)
    {
        _fileChecker = fileChecker;
        _propertyLoader = propertyLoader;
    }

    public Decorator Load(Parser parser)
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
                case "preservePropertyCasing":
                    decorator.PreservePropertyCasing = value!.Value == "true";
                    break;
                case "properties":
                    parser.ConsumeSequence(() =>
                    {
                        foreach (var property in _propertyLoader.Load(parser))
                        {
                            decorator.Properties.Add(property);
                        }
                    });
                    break;
                default:
                    decorator.Implementations[prop] = _fileChecker.Deserialize<DecoratorImplementation>(parser);
                    break;
            }
        });

        foreach (var prop in decorator.Properties)
        {
            prop.Decorator = decorator;
        }

        return decorator;
    }
}