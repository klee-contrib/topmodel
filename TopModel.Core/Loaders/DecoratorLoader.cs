using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Core.Model.Implementation;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.Loaders;

public class DecoratorLoader(FileChecker fileChecker, PropertyLoader propertyLoader) : ILoader
{
    /// <inheritdoc cref="ILoader.Load" />
    public void Load(Parser parser, ModelFile modelFile, ModelFileLoadConfig config, Reference location)
    {
        var decorator = new Decorator()
        {
            ModelFile = modelFile,
            Location = location,
            Namespace = modelFile.Namespace,
        };
        modelFile.Decorators.Add(decorator);
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
                case "target":
                    decorator.Target = value!.Value.ParseEnum<Target>();
                    break;
                case "decorators":
                    parser.ConsumeSequence(() =>
                    {
                        if (parser.Current is MappingStart)
                        {
                            parser.ConsumeMapping(prop =>
                            {
                                var decoratorRef = new DecoratorReference(prop)
                                {
                                    ParameterReferences = fileChecker.Deserialize<
                                        Dictionary<ParameterReference, StringWithVariables>
                                    >(parser),
                                };

                                decorator.DecoratorReferences.Add(decoratorRef);
                            });
                        }
                        else
                        {
                            decorator.DecoratorReferences.Add(new DecoratorReference(parser.Consume<Scalar>()));
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

                                decorator.AnnotationReferences.Add(annotation);
                            });
                        }
                        else
                        {
                            decorator.AnnotationReferences.Add(new AnnotationReference(parser.Consume<Scalar>()));
                        }
                    });
                    break;
                case "excludedAnnotations":
                    parser.ConsumeSequence(() =>
                    {
                        decorator.ExcludedAnnotationReferences.Add(new AnnotationReference(parser.Consume<Scalar>()));
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

                                decorator.PropertyAnnotationReferences.Add(annotation);
                            });
                        }
                        else
                        {
                            decorator.PropertyAnnotationReferences.Add(
                                new AnnotationReference(parser.Consume<Scalar>())
                            );
                        }
                    });
                    break;
                case "properties":
                    parser.ConsumeSequence(() =>
                    {
                        decorator.OwnProperties.Add(propertyLoader.Load(parser, modelFile, config));
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

        foreach (var prop in decorator.OwnProperties)
        {
            prop.Decorator = decorator;
        }
    }
}
