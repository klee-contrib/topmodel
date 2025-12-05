using TopModel.Core.FileModel;
using TopModel.Core.Model;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.Loaders;

public class EndpointLoader(FileChecker fileChecker, PropertyLoader propertyLoader) : ILoader
{
    /// <inheritdoc cref="ILoader.Load" />
    public void Load(Parser parser, ModelFile modelFile, Reference location)
    {
        var endpoint = new Endpoint()
        {
            ModelFile = modelFile,
            Location = location,
            Namespace = modelFile.Namespace,
        };

        modelFile.Endpoints.Add(endpoint);
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
                    endpoint.Route = new StringWithVariables(value!);
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
                        var property = propertyLoader.Load(parser, modelFile);
                        property.Endpoint = endpoint;
                        endpoint.Params.Add(property);
                    });
                    break;
                case "returns":
                    endpoint.Returns = propertyLoader.Load(parser, modelFile);
                    endpoint.Returns.Endpoint = endpoint;
                    break;
                case "decorators":
                    parser.ConsumeSequence(() =>
                    {
                        if (parser.Current is MappingStart)
                        {
                            parser.ConsumeMapping(prop =>
                            {
                                var decorator = new DecoratorReference(prop)
                                {
                                    ParameterReferences = fileChecker.Deserialize<
                                        Dictionary<ParameterReference, StringWithVariables>
                                    >(parser),
                                };

                                endpoint.DecoratorReferences.Add(decorator);
                            });
                        }
                        else
                        {
                            endpoint.DecoratorReferences.Add(new DecoratorReference(parser.Consume<Scalar>()));
                        }
                    });
                    break;
                case "annotations":
                    parser.ConsumeSequence(() =>
                    {
                        if (parser.Current is MappingStart)
                        {
                            parser.ConsumeMapping(prop =>
                            {
                                var annotation = new AnnotationReference(prop)
                                {
                                    ParameterReferences = fileChecker.Deserialize<
                                        Dictionary<ParameterReference, StringWithVariables>
                                    >(parser),
                                };

                                endpoint.AnnotationReferences.Add(annotation);
                            });
                        }
                        else
                        {
                            endpoint.AnnotationReferences.Add(new AnnotationReference(parser.Consume<Scalar>()));
                        }
                    });
                    break;
                case "excludedAnnotations":
                    parser.ConsumeSequence(() =>
                    {
                        endpoint.ExcludedAnnotationReferences.Add(new AnnotationReference(parser.Consume<Scalar>()));
                    });
                    break;
                case "propertyAnnotations":
                    parser.ConsumeSequence(() =>
                    {
                        if (parser.Current is MappingStart)
                        {
                            parser.ConsumeMapping(prop =>
                            {
                                var annotation = new AnnotationReference(prop)
                                {
                                    ParameterReferences = fileChecker.Deserialize<
                                        Dictionary<ParameterReference, StringWithVariables>
                                    >(parser),
                                };

                                endpoint.PropertyAnnotationReferences.Add(annotation);
                            });
                        }
                        else
                        {
                            endpoint.PropertyAnnotationReferences.Add(
                                new AnnotationReference(parser.Consume<Scalar>())
                            );
                        }
                    });
                    break;
                case "customProperties":
                    parser.ConsumeMapping(prop =>
                        endpoint.CustomProperties.Add(prop.Value, parser.Consume<Scalar>().Value)
                    );
                    break;
                default:
                    throw new ModelException(endpoint, $"Propriété ${prop} inconnue pour un endpoint");
            }
        });
    }
}
