using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Utils;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.Loaders;

public class ClassLoader(FileChecker fileChecker, PropertyLoader propertyLoader) : ILoader
{
    /// <inheritdoc cref="ILoader.Load" />
    public void Load(Parser parser, ModelFile modelFile, ModelFileLoadConfig config, Reference location)
    {
        var classe = new Class()
        {
            ModelFile = modelFile,
            Location = location,
            Namespace = modelFile.Namespace,
        };
        modelFile.Classes.Add(classe);

        parser.ConsumeMapping(prop =>
        {
            _ = parser.TryConsume<Scalar>(out var value);

            switch (prop.Value)
            {
                case "tags":
                    parser.ConsumeSequence(() => classe.OwnTags.Add(parser.Consume<Scalar>().Value));
                    break;
                case "trigram":
                    classe.Trigram = new LocatedString(value!);
                    break;
                case "name":
                    classe.Name = new LocatedString(value!);
                    break;
                case "pluralName":
                    classe.PluralName = value!.Value;
                    break;
                case "sqlName":
                    classe.SqlName = value!.Value;
                    break;
                case "extends":
                    classe.ExtendsReference = new ClassReference(value!);
                    break;
                case "label":
                    classe.Label = value!.Value;
                    break;
                case "reference":
                    classe.Reference = value!.Value == "true";
                    break;
                case "readonly":
                    classe.Readonly = value!.Value == "true";
                    break;
                case "enum":
                    classe.EnumOverride = new LocatedString(value!);
                    break;
                case "type":
                    classe.Type = Enum.Parse<ClassType>(value!.Value, ignoreCase: true);
                    break;
                case "orderProperty":
                    classe.OrderPropertyReference = new Reference(value!);
                    break;
                case "defaultProperty":
                    classe.DefaultPropertyReference = new Reference(value!);
                    break;
                case "flagProperty":
                    classe.FlagPropertyReference = new Reference(value!);
                    break;
                case "localeProperty":
                    classe.LocalePropertyReference = new Reference(value!);
                    break;
                case "translation":
                    classe.Translation = value!.Value == "true";
                    break;
                case "comment":
                    classe.Comment = value!.Value;
                    break;
                case "preservePropertyCasing":
                    classe.PreservePropertyCasing = value!.Value == "true";
                    break;
                case "implements":
                    parser.ConsumeSequence(() =>
                    {
                        classe.ImplementReferences.Add(new ClassReference(parser.Consume<Scalar>()));
                    });
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

                                classe.DecoratorReferences.Add(decorator);
                            });
                        }
                        else
                        {
                            classe.DecoratorReferences.Add(new DecoratorReference(parser.Consume<Scalar>()));
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

                                classe.AnnotationReferences.Add(annotation);
                            });
                        }
                        else
                        {
                            classe.AnnotationReferences.Add(new AnnotationReference(parser.Consume<Scalar>()));
                        }
                    });
                    break;
                case "excludedAnnotations":
                    parser.ConsumeSequence(() =>
                    {
                        classe.ExcludedAnnotationReferences.Add(new AnnotationReference(parser.Consume<Scalar>()));
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

                                classe.PropertyAnnotationReferences.Add(annotation);
                            });
                        }
                        else
                        {
                            classe.PropertyAnnotationReferences.Add(new AnnotationReference(parser.Consume<Scalar>()));
                        }
                    });
                    break;
                case "properties":
                    parser.ConsumeSequence(() =>
                    {
                        classe.OwnProperties.Add(propertyLoader.Load(parser, modelFile, config));
                    });
                    break;
                case "unique":
                    parser.ConsumeSequence(() =>
                    {
                        var index = new IndexDefinition { Unique = true, Class = classe };
                        classe.Indexes.Add(index);
                        parser.ConsumeSequence(() =>
                        {
                            index.PropertyReferences.Add(new Reference(parser.Consume<Scalar>()));
                        });
                    });
                    break;
                case "indexes":
                    parser.ConsumeSequence(() =>
                    {
                        if (parser.Current is SequenceStart)
                        {
                            var index = new IndexDefinition { Unique = false, Class = classe };
                            parser.ConsumeSequence(() =>
                            {
                                index.PropertyReferences.Add(new Reference(parser.Consume<Scalar>()));
                            });
                            classe.Indexes.Add(index);
                        }
                        else
                        {
                            bool indexUnique = false;
                            var propertyRefs = new List<Reference>();

                            parser.ConsumeMapping(prop =>
                            {
                                switch (prop.Value)
                                {
                                    case "unique":
                                        indexUnique = parser.Consume<Scalar>().Value == "true";
                                        break;
                                    case "properties":
                                        parser.ConsumeSequence(() =>
                                        {
                                            propertyRefs.Add(new Reference(parser.Consume<Scalar>()));
                                        });
                                        break;
                                    default:
                                        throw new ModelException(classe, $"Propriété ${prop} inconnue pour un index");
                                }
                            });

                            var idx = new IndexDefinition { Unique = indexUnique, Class = classe };

                            foreach (var r in propertyRefs)
                            {
                                idx.PropertyReferences.Add(r);
                            }

                            classe.Indexes.Add(idx);
                        }
                    });
                    break;
                case "values":
                    parser.ConsumeMapping(prop =>
                    {
                        var name = new Reference(prop);
                        var values = new Dictionary<Reference, string>();

                        classe.ValueReferences.Add(name, values);

                        parser.ConsumeMapping(prop =>
                        {
                            values.Add(new Reference(prop), parser.Consume<Scalar>().Value);
                        });
                    });
                    break;
                case "customProperties":
                    parser.ConsumeMapping(prop =>
                        classe.CustomProperties.Add(prop.Value, parser.Consume<Scalar>().Value)
                    );
                    break;
                case "mappers":
                    parser.ConsumeMapping(prop =>
                    {
                        switch (prop.Value)
                        {
                            case "from":
                                parser.ConsumeSequence(() =>
                                {
                                    var mapper = new FromMapper { Class = classe };
                                    classe.FromMappers.Add(mapper);

                                    parser.ConsumeMapping(prop =>
                                    {
                                        switch (prop.Value)
                                        {
                                            case "comment":
                                                mapper.Comment = parser.Consume<Scalar>().Value;
                                                break;
                                            case "params":
                                                mapper.Reference = new LocatedString(prop);
                                                parser.ConsumeSequence(() =>
                                                {
                                                    parser.Consume<MappingStart>();
                                                    if (parser.Current is Scalar { Value: "class" })
                                                    {
                                                        var param = new ClassMappings();
                                                        mapper.OwnParams.Add(param);

                                                        Scalar classScalar = null!;
                                                        while (parser.Current is not MappingEnd)
                                                        {
                                                            var prop = parser.Consume<Scalar>();
                                                            switch (prop.Value)
                                                            {
                                                                case "class":
                                                                    classScalar = parser.Consume<Scalar>();
                                                                    param.ClassReference = new ClassReference(
                                                                        classScalar
                                                                    );
                                                                    break;
                                                                case "required":
                                                                    param.Required =
                                                                        parser.Consume<Scalar>().Value == "true";
                                                                    break;
                                                                case "comment":
                                                                    param.Comment = parser.Consume<Scalar>().Value;
                                                                    break;
                                                                case "name":
                                                                    param.Name = new LocatedString(
                                                                        parser.Consume<Scalar>()
                                                                    );
                                                                    break;
                                                                case "mappings":
                                                                    parser.ConsumeMapping(prop =>
                                                                    {
                                                                        param.MappingReferences.Add(
                                                                            new Reference(prop),
                                                                            new Reference(parser.Consume<Scalar>())
                                                                        );
                                                                    });
                                                                    break;
                                                            }
                                                        }

                                                        param.Name ??= new LocatedString(classScalar)
                                                        {
                                                            Value = param.ClassReference.ReferenceName.ToCamelCase(
                                                                strictIfUppercase: true
                                                            ),
                                                        };
                                                    }
                                                    else if (parser.Current is Scalar { Value: "property" })
                                                    {
                                                        var param = new PropertyMapping { FromMapper = mapper };
                                                        mapper.OwnParams.Add(param);
                                                        while (parser.Current is not MappingEnd)
                                                        {
                                                            var prop = parser.Consume<Scalar>();
                                                            switch (prop.Value)
                                                            {
                                                                case "property":
                                                                    param.Property = propertyLoader.Load(
                                                                        parser,
                                                                        modelFile,
                                                                        config
                                                                    );
                                                                    param.Property.PropertyMapping = param;
                                                                    break;
                                                                case "target":
                                                                    param.TargetPropertyReference = new Reference(
                                                                        parser.Consume<Scalar>()
                                                                    );
                                                                    break;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        throw new ModelException(
                                                            classe,
                                                            $"Erreur dans la construction des paramètres du mapper 'from'."
                                                        );
                                                    }

                                                    parser.Consume<MappingEnd>();
                                                });
                                                break;
                                        }
                                    });
                                });
                                break;

                            case "to":
                                parser.ConsumeSequence(() =>
                                {
                                    var mapper = new ClassMappings { To = true };
                                    classe.ToMappers.Add(mapper);

                                    parser.ConsumeMapping(prop =>
                                    {
                                        Scalar classScalar = null!;
                                        switch (prop.Value)
                                        {
                                            case "class":
                                                classScalar = parser.Consume<Scalar>();
                                                mapper.ClassReference = new ClassReference(classScalar);
                                                break;
                                            case "name":
                                                mapper.Name = new LocatedString(parser.Consume<Scalar>());
                                                break;
                                            case "comment":
                                                mapper.Comment = parser.Consume<Scalar>().Value;
                                                break;
                                            case "mappings":
                                                parser.ConsumeMapping(prop =>
                                                {
                                                    mapper.MappingReferences.Add(
                                                        new Reference(prop),
                                                        new Reference(parser.Consume<Scalar>())
                                                    );
                                                });
                                                break;
                                        }

                                        mapper.Name ??= new LocatedString(classScalar)
                                        {
                                            Value =
                                                $"To{mapper.ClassReference.ReferenceName.ToPascalCase(strictIfUppercase: true)}",
                                        };
                                    });
                                });
                                break;
                        }
                    });
                    break;
                default:
                    throw new ModelException(classe, $"Propriété ${prop} inconnue pour une classe");
            }
        });

        classe.Label ??= classe.Name;
        classe.SqlName ??= (config.PluralizeTableNames ? classe.PluralName : classe.Name).ToConstantCase();

        foreach (var prop in classe.OwnProperties)
        {
            prop.Class = classe;
        }
    }
}
