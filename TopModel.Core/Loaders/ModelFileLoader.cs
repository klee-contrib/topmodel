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
            var location = new Reference(scalar);
            ILoader loader = scalar.Value switch
            {
                "annotation" => annotationLoader,
                "class" => classLoader,
                "converter" => converterLoader,
                "dataFlow" => dataFlowLoader,
                "decorator" => decoratorLoader,
                "domain" => domainLoader,
                "endpoint" => endpointLoader,
                _ => throw new ModelException(
                    file,
                    $"Type de document inconnu ('{scalar.Value}').",
                    new Reference(scalar)
                ),
            };

            loader.Load(parser, file, location);
            parser.Consume<MappingEnd>();
            parser.Consume<DocumentEnd>();
        }

        if (file.Options.Endpoints.FileName == null)
        {
            var fileSplit = file.Name.Split("/")[^1];
            file.Options.Endpoints.FileName = string.Join(
                '_',
                fileSplit.Split("_").Skip(fileSplit.Contains('_') ? 1 : 0)
            );
        }

        var lines = content.ReplaceLineEndings(Environment.NewLine).Split(Environment.NewLine);

        for (var i = 0; i < lines.Length; i++)
        {
            var commentIndex = lines[i].IndexOf('#');
            if (commentIndex >= 0)
            {
                file.Comments.Add(i + 1, lines[i][commentIndex..].TrimStart('#').Trim());
            }
        }

        return file;
    }
}
