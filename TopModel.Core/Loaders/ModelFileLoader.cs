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
                file.Domains.Add(_fileChecker.Deserialize<Domain>(parser));
            }
            else if (scalar.Value == "class")
            {
                file.Classes.Add(_classLoader.LoadClass(parser, filePath));
            }
            else if (scalar.Value == "endpoint")
            {
                file.Endpoints.Add(EndpointLoader.LoadEndpoint(parser));
            }
            else if (scalar.Value == "alias")
            {
                file.Aliases.Add(_fileChecker.Deserialize<Alias>(parser));
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