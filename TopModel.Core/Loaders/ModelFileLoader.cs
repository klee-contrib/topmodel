using TopModel.Core.FileModel;
using TopModel.Utils;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.Loaders;

public class ModelFileLoader
{
    private readonly ClassLoader _classLoader;
    private readonly ModelConfig _config;
    private readonly FileChecker _fileChecker;

    public ModelFileLoader(ModelConfig config, ClassLoader classLoader, FileChecker fileChecker)
    {
        _classLoader = classLoader;
        _config = config;
        _fileChecker = fileChecker;
    }

    public ModelFile LoadModelFile(string filePath, string? content = null)
    {
        content ??= File.ReadAllText(filePath);

        _fileChecker.CheckModelFile(filePath, content);

        var parser = new Parser(new StringReader(content));
        parser.Consume<StreamStart>();

        parser.Consume<DocumentStart>();
        parser.Consume<MappingStart>();

        var file = new ModelFile
        {
            Name = _config.GetFileName(filePath),
            Path = filePath.ToRelative(),
        };

        while (parser.Current is not MappingEnd)
        {
            var prop = parser.Consume<Scalar>().Value;
            parser.TryConsume<Scalar>(out var value);

            switch (prop)
            {
                case "module":
                    file.Module = value!.Value;
                    break;
                case "tags":
                    parser.Consume<SequenceStart>();

                    while (parser.Current is not SequenceEnd)
                    {
                        file.Tags.Add(parser.Consume<Scalar>().Value);
                    }

                    parser.Consume<SequenceEnd>();
                    break;
                case "uses":
                    parser.Consume<SequenceStart>();

                    while (parser.Current is not SequenceEnd)
                    {
                        file.Uses.Add(new Reference(parser.Consume<Scalar>()));
                    }

                    parser.Consume<SequenceEnd>();
                    break;
            }
        }

        parser.Consume<MappingEnd>();
        parser.Consume<DocumentEnd>();

        while (parser.TryConsume<DocumentStart>(out var _))
        {
            parser.Consume<MappingStart>();
            var scalar = parser.Consume<Scalar>();

            if (scalar.Value == "domain")
            {
                var domain = _fileChecker.Deserialize<Domain>(parser);
                domain.ModelFile = file;
                domain.Location = new Reference(scalar);
                file.Domains.Add(domain);
            }
            else if (scalar.Value == "class")
            {
                var classe = _classLoader.LoadClass(parser, filePath);
                classe.Location = new Reference(scalar);
                file.Classes.Add(classe);
            }
            else if (scalar.Value == "endpoint")
            {
                var endpoint = EndpointLoader.LoadEndpoint(parser);
                endpoint.Location = new Reference(scalar);
                file.Endpoints.Add(endpoint);
            }
            else if (scalar.Value == "alias")
            {
                var alias = _fileChecker.Deserialize<Alias>(parser);
                alias.ModelFile = file;
                alias.Location = new Reference(scalar);
                file.Aliases.Add(alias);
            }
            else
            {
                throw new ModelException("Type de document inconnu.");
            }

            parser.Consume<MappingEnd>();
            parser.Consume<DocumentEnd>();
        }

        var ns = new Namespace { App = _config.App, Module = file.Module };
        foreach (var classe in file.Classes)
        {
            classe.ModelFile = file;
            classe.Namespace = ns;
        }

        foreach (var endpoint in file.Endpoints)
        {
            endpoint.ModelFile = file;
            endpoint.Namespace = ns;
        }

        return file;
    }
}