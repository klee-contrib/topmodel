using System;
using System.Collections.Generic;
using System.Linq;
using TopModel.Core.FileModel;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.Loaders
{
    public static class EndpointLoader
    {
        public static Endpoint LoadEndpoint(Parser parser, IDictionary<object, Relation> relationships)
        {
            parser.Consume<MappingStart>();

            var endpoint = new Endpoint();

            while (!(parser.Current is Scalar { Value: "params" }))
            {
                var prop = parser.Consume<Scalar>().Value;
                parser.TryConsume<Scalar>(out var value);

                switch (prop)
                {
                    case "name":
                        endpoint.Name = value!.Value;
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
                    case "returns":
                        endpoint.Returns = PropertyLoader.LoadProperty(parser, relationships).First();
                        endpoint.Returns.Endpoint = endpoint;
                        parser.Consume<MappingEnd>();
                        break;
                    case "body":
                        endpoint.Body = value!.Value;
                        break;
                    default:
                        throw new Exception($"Propriété ${prop} inconnue pour un endpoint");
                }
            }

            parser.Consume<Scalar>();
            parser.Consume<SequenceStart>();

            while (!(parser.Current is SequenceEnd))
            {
                foreach (var property in PropertyLoader.LoadProperty(parser, relationships))
                {
                    property.Endpoint = endpoint;
                    endpoint.Params.Add(property);
                }
            }

            parser.Consume<SequenceEnd>();
            parser.Consume<MappingEnd>();

            return endpoint;
        }
    }
}
