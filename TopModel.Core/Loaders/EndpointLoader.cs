using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.Loaders;

internal static class EndpointLoader
{
    public static Endpoint LoadEndpoint(Parser parser)
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
                        foreach (var property in PropertyLoader.LoadProperty(parser))
                        {
                            property.Endpoint = endpoint;
                            endpoint.Params.Add(property);
                        }
                    });
                    break;
                case "returns":
                    endpoint.Returns = PropertyLoader.LoadProperty(parser).First();
                    endpoint.Returns.Endpoint = endpoint;
                    parser.Consume<MappingEnd>();
                    break;
                default:
                    throw new ModelException(endpoint, $"Propriété ${prop} inconnue pour un endpoint");
            }
        });

        return endpoint;
    }
}