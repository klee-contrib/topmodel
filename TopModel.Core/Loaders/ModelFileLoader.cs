using TopModel.Core.FileModel;
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

    public ModelFile LoadModelFile(string filePath)
    {
        _fileChecker.CheckModelFile(filePath);

        var parser = new Parser(new StringReader(File.ReadAllText(filePath)));
        parser.Consume<StreamStart>();

        var file = _fileChecker.Deserialize<ModelFile>(parser);
        file.Path = filePath.ToRelative();
        file.Name = Path.GetRelativePath(Path.Combine(Directory.GetCurrentDirectory(), _config.ModelRoot), filePath)
            .Replace(".yml", string.Empty)
            .Replace("\\", "/");
        file.Classes = new List<Class>();
        file.Domains = new List<Domain>();
        file.Endpoints = new List<Endpoint>();
        file.Aliases = new List<Alias>();

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