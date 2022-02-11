using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.Loaders;

internal static class EndpointLoader
{
    public static Endpoint LoadEndpoint(Parser parser)
    {
        parser.Consume<MappingStart>();

        var endpoint = new Endpoint();

        while (parser.Current is not MappingEnd)
        {
            var prop = parser.Consume<Scalar>().Value;
            parser.TryConsume<Scalar>(out var value);

            switch (prop)
            {
                case "name":
                    endpoint.Name = new LocatedString(){
                        Value = value!.Value,
                        Location =  new FileModel.Reference(value)
                    };
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
                    parser.Consume<SequenceStart>();

                    while (parser.Current is not SequenceEnd)
                    {
                        foreach (var property in PropertyLoader.LoadProperty(parser))
                        {
                            property.Endpoint = endpoint;
                            endpoint.Params.Add(property);
                        }
                    }

                    parser.Consume<SequenceEnd>();
                    break;
                case "returns":
                    endpoint.Returns = PropertyLoader.LoadProperty(parser).First();
                    endpoint.Returns.Endpoint = endpoint;
                    parser.Consume<MappingEnd>();
                    break;
                default:
                    throw new ModelException(endpoint, $"Propriété ${prop} inconnue pour un endpoint");
            }
        }

        parser.Consume<MappingEnd>();

        return endpoint;
    }
}