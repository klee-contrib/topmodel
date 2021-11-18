using System.Reflection;
using NJsonSchema;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TopModel.Core.Loaders;

public class FileChecker
{
    private readonly IDeserializer _deserializer;
    private readonly ISerializer _serializer;

    private readonly JsonSchema? _configSchema;
    private readonly JsonSchema _modelSchema;

    public FileChecker(string? configSchemaPath = null)
    {
        if (configSchemaPath != null)
        {
            _configSchema = JsonSchema.FromFileAsync(
                Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, configSchemaPath))
                .Result;
        }

        _modelSchema = JsonSchema.FromFileAsync(
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "schema.json"))
            .Result;

        _deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .WithNodeTypeResolver(new InferTypeFromValueResolver())
            .IgnoreUnmatchedProperties()
            .Build();
        _serializer = new SerializerBuilder()
            .JsonCompatible()
            .Build();
    }

    public void CheckConfigFile(string fileName)
    {
        if (_configSchema != null)
        {
            CheckCore(fileName, _configSchema);
        }
    }

    public void CheckModelFile(string fileName)
    {
        CheckCore(fileName, _modelSchema);
    }

    public T Deserialize<T>(string yaml)
    {
        return _deserializer.Deserialize<T>(yaml);
    }

    public T Deserialize<T>(IParser parser)
    {
        return _deserializer.Deserialize<T>(parser);
    }

    private void CheckCore(string fileName, JsonSchema schema)
    {
        var parser = new Parser(new StringReader(File.ReadAllText(fileName)));
        parser.Consume<StreamStart>();

        var firstObject = true;
        while (parser.Current is DocumentStart)
        {
            var yaml = _deserializer.Deserialize(parser);
            if (yaml == null)
            {
                throw new ModelException($"Impossible de lire le fichier {fileName.ToRelative()}.");
            }

            var json = _serializer.Serialize(yaml);

            // La vérification du domaine ne marche pas à cause des nombres déserialisés en strings...
            if (!firstObject && json.StartsWith("{\"domain\":"))
            {
                continue;
            }

            var finalSchema = firstObject ? schema.OneOf.First() : schema;

            var errors = finalSchema.Validate(json);

            if (errors.Any())
            {
                throw new ModelException($@"Erreur dans le fichier {fileName.ToRelative()} :
{string.Join("\r\n", errors.Select(e => $"[{e.LinePosition}]: {e.Kind} - {e.Path}"))}.");
            }

            firstObject = false;
        }
    }
}