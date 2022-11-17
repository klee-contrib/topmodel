using TopModel.Core.FileModel;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.Loaders;

public class EndpointLoader
{
    private readonly PropertyLoader _propertyLoader;

    public EndpointLoader(PropertyLoader propertyLoader)
    {
        _propertyLoader = propertyLoader;
    }

    public Endpoint LoadEndpoint(Parser parser)
    {
        var endpoint = new Endpoint();

        parser.ConsumeMapping(() =>
        {
            var prop = parser.Consume<Scalar>().Value;
            parser.TryConsume<Scalar>(out var value);

            switch (prop)
            {
                case "name":
                    endpoint.Name = new LocatedString(value);
                    break;
                case "method":
                    endpoint.Method = value!.Value;
                    break;
                case "route":
                    endpoint.Route = value!.Value;
                    break;
                case "description":
                    endpoint.Description = value!.Value;
                    break;
                case "params":
                    parser.ConsumeSequence(() =>
                    {
                        foreach (var property in _propertyLoader.LoadProperty(parser))
                        {
                            property.Endpoint = endpoint;
                            endpoint.Params.Add(property);
                        }
                    });
                    break;
                case "returns":
                    endpoint.Returns = _propertyLoader.LoadProperty(parser).First();
                    endpoint.Returns.Endpoint = endpoint;
                    parser.Consume<MappingEnd>();
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

                                endpoint.DecoratorReferences.Add(decorator);
                            });
                        }
                        else
                        {
                            endpoint.DecoratorReferences.Add(new DecoratorReference(parser.Consume<Scalar>()));
                        }
                    });
                    break;
                default:
                    throw new ModelException(endpoint, $"Propriété ${prop} inconnue pour un endpoint");
            }
        });

        return endpoint;
    }
}