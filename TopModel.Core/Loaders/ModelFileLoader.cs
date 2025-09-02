using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Utils;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.Loaders;

public class ModelFileLoader(
    ModelConfig config,
    AnnotationLoader annotationLoader,
    ClassLoader classLoader,
    DataFlowLoader dataFlowLoader,
    FileChecker fileChecker,
    DecoratorLoader decoratorLoader,
    ConverterLoader converterLoader,
    EndpointLoader endpointLoader,
    DomainLoader domainLoader
)
{
    public async Task<ModelFile?> LoadModelFile(string filePath, string? content = null, CancellationToken ct = default)
    {
        content ??= await File.ReadAllTextAsync(filePath, ct);

        fileChecker.CheckModelFile(filePath, content);

        var parser = new Parser(new StringReader(content));
        parser.Consume<StreamStart>();

        if (parser.Current is StreamEnd)
        {
            return null;
        }

        parser.Consume<DocumentStart>();

        var file = new ModelFile { Name = config.GetFileName(filePath), Path = filePath.ToRelative() };

        parser.ConsumeMapping(prop =>
        {
            parser.TryConsume<Scalar>(out var value);

            switch (prop.Value)
            {
                case "module":
                    file.Namespace = new Namespace { App = config.App, Module = value!.Value };
                    break;
                case "tags":
                    parser.ConsumeSequence(() => file.Tags.Add(parser.Consume<Scalar>().Value));
                    break;
                case "uses":
                    parser.ConsumeSequence(() => file.Uses.Add(new Reference(parser.Consume<Scalar>())));
                    break;
                case "options":
                    parser.Consume<MappingStart>();
                    var scalar = parser.Consume<Scalar>();
                    if (scalar.Value == "endpoints")
                    {
                        parser.ConsumeMapping(prop =>
                        {
                            parser.TryConsume<Scalar>(out var value);
                            switch (prop.Value)
                            {
                                case "fileName":
                                    file.Options.Endpoints.FileName = value!.Value;
                                    break;
                                case "prefix":
                                    file.Options.Endpoints.Prefix = new LocatedString(value!);
                                    break;
                            }
                        });
                    }

                    parser.Consume<MappingEnd>();
                    break;
            }
        });

        parser.Consume<DocumentEnd>();

        while (parser.TryConsume<DocumentStart>(out var _))
        {
            parser.Consume<MappingStart>();
            var scalar = parser.Consume<Scalar>();

            if (scalar.Value == "annotation")
            {
                var annotation = annotationLoader.Load(parser);
                annotation.Location = new Reference(scalar);
                file.Annotations.Add(annotation);
            }
            else if (scalar.Value == "domain")
            {
                var domain = domainLoader.Load(parser);
                domain.ModelFile = file;
                domain.Location = new Reference(scalar);
                file.Domains.Add(domain);
            }
            else if (scalar.Value == "decorator")
            {
                var decorator = decoratorLoader.Load(parser);
                decorator.Location = new Reference(scalar);
                file.Decorators.Add(decorator);
            }
            else if (scalar.Value == "converter")
            {
                var converter = converterLoader.Load(parser);
                converter.ModelFile = file;
                converter.Location = new Reference(scalar);
                file.Converters.Add(converter);
            }
            else if (scalar.Value == "class")
            {
                var classe = classLoader.Load(parser);
                classe.Location = new Reference(scalar);
                file.Classes.Add(classe);
            }
            else if (scalar.Value == "endpoint")
            {
                var endpoint = endpointLoader.Load(parser);
                endpoint.Location = new Reference(scalar);
                file.Endpoints.Add(endpoint);
            }
            else if (scalar.Value == "dataFlow")
            {
                var dataFlow = dataFlowLoader.Load(parser);
                dataFlow.ModelFile = file;
                dataFlow.Location = new Reference(scalar);
                file.DataFlows.Add(dataFlow);
            }
            else
            {
                throw new ModelException(file, $"Type de document inconnu ('{scalar.Value}').", new Reference(scalar));
            }

            parser.Consume<MappingEnd>();
            parser.Consume<DocumentEnd>();
        }

        foreach (var annotation in file.Annotations)
        {
            annotation.ModelFile = file;
            annotation.Namespace = file.Namespace;
        }

        foreach (var classe in file.Classes)
        {
            classe.ModelFile = file;
            classe.Namespace = file.Namespace;
        }

        foreach (var endpoint in file.Endpoints)
        {
            endpoint.ModelFile = file;
            endpoint.Namespace = file.Namespace;
        }

        foreach (var decorator in file.Decorators)
        {
            decorator.ModelFile = file;
            decorator.Namespace = file.Namespace;
        }

        if (file.Options.Endpoints.FileName == null)
        {
            var fileSplit = file.Name.Split("/")[^1];
            file.Options.Endpoints.FileName = string.Join(
                '_',
                fileSplit.Split("_").Skip(fileSplit.Contains('_') ? 1 : 0)
            );
        }

        return file;
    }
}
