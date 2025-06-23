using TopModel.Core.FileModel;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.Loaders;

public class EndpointLoader(PropertyLoader propertyLoader) : ILoader<Endpoint>
{
    /// <inheritdoc cref="ILoader{T}.Load" />
    public Endpoint Load(Parser parser)
    {
        var endpoint = new Endpoint();

        parser.ConsumeMapping(prop =>
        {
            parser.TryConsume<Scalar>(out var value);

            switch (prop.Value)
            {
                case "tags":
                    parser.ConsumeSequence(() => endpoint.OwnTags.Add(parser.Consume<Scalar>().Value));
                    break;
                case "name":
                    endpoint.Name = new LocatedString(value!);
                    break;
                case "method":
                    endpoint.Method = value!.Value;
                    break;
                case "route":
                    endpoint.RouteWithVariables = new StringWithVariables(value!);
                    break;
                case "description":
                    endpoint.Description = value!.Value;
                    break;
                case "preservePropertyCasing":
                    endpoint.PreservePropertyCasing = value!.Value == "true";
                    break;
                case "params":
                    parser.ConsumeSequence(() =>
                    {
                        var property = propertyLoader.Load(parser);
                        property.Endpoint = endpoint;
                        endpoint.Params.Add(property);
                    });
                    break;
                case "returns":
                    endpoint.Returns = propertyLoader.Load(parser);
                    endpoint.Returns.Endpoint = endpoint;
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
                                    decorator.ParameterReferences.Add(new ParameterReference(parser.Consume<Scalar>()));
                                });

                                endpoint.DecoratorReferences.Add(decorator);
                            });
                        }
                        else
                        {
                            endpoint.DecoratorReferences.Add(new DecoratorReference(parser.Consume<Scalar>()));
                        }
                    });
                    break;
                case "customProperties":
                    parser.ConsumeMapping(prop => endpoint.CustomProperties.Add(prop.Value, parser.Consume<Scalar>().Value));
                    break;
                default:
                    throw new ModelException(endpoint, $"Propriété ${prop} inconnue pour un endpoint");
            }
        });

        return endpoint;
    }
}