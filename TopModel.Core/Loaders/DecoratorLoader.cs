using TopModel.Core.FileModel;
using TopModel.Core.Model.Implementation;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.Loaders;

public class DecoratorLoader(FileChecker fileChecker, PropertyLoader propertyLoader) : ILoader<Decorator>
{
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
                    decorator.Name = new LocatedString(value!);
                    break;
                case "description":
                    decorator.Description = value!.Value;
                    break;
                case "preservePropertyCasing":
                    decorator.PreservePropertyCasing = value!.Value == "true";
                    break;
                case "decorators":
                    parser.ConsumeSequence(() =>
                    {
                        if (parser.Current is MappingStart)
                        {
                            parser.ConsumeMapping(prop =>
                            {
                                var decoratorRef = new DecoratorReference(prop);

                                parser.ConsumeSequence(() =>
                                {
                                    decoratorRef.ParameterReferences.Add(new ParameterReference(parser.Consume<Scalar>()));
                                });

                                decorator.DecoratorReferences.Add(decoratorRef);
                            });
                        }
                        else
                        {
                            decorator.DecoratorReferences.Add(new DecoratorReference(parser.Consume<Scalar>()));
                        }
                    });
                    break;
                case "properties":
                    parser.ConsumeSequence(() =>
                    {
                        decorator.Properties.Add(propertyLoader.Load(parser));
                    });
                    break;
                case "parameters":
                    decorator.TemplateParameters = fileChecker.Deserialize<IList<TemplateParameter>>(parser);
                    foreach (var param in decorator.TemplateParameters)
                    {
                        param.Decorator = decorator;
                    }

                    break;
                default:
                    decorator.Implementations[prop.Value] = fileChecker.Deserialize<DecoratorImplementation>(parser);
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