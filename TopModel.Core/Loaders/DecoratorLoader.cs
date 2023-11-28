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

    /// <inheritdoc cref="ILoader{T}.Load" />
    public Decorator Load(Parser parser)
    {
        var decorator = new Decorator();

        parser.ConsumeMapping(prop =>
        {
            _ = parser.TryConsume<Scalar>(out var value);

            switch (prop.Value)
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
                        decorator.Properties.Add(_propertyLoader.Load(parser));
                    });
                    break;
                default:
                    decorator.Implementations[prop.Value] = _fileChecker.Deserialize<DecoratorImplementation>(parser);
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