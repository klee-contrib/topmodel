using System.Reflection;
using System.Text;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using NJsonSchema.Validation;
using Spectre.Console;
using TopModel.Core.Loaders.YamlUtils;
using TopModel.Utils;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TopModel.Core.Loaders;

public class FileChecker
{
    private readonly JsonSchema? _configSchema;
    private readonly IDeserializer _deserializer;
    private readonly JsonSchema _modelSchema;
    private readonly ISerializer _parsingSerializer;
    private readonly ISerializer _serializer;

    public FileChecker(string? configSchemaPath = null)
    {
        if (configSchemaPath != null)
        {
            _configSchema = JsonSchema
                .FromFileAsync(GetFilePath(Assembly.GetExecutingAssembly(), configSchemaPath))
                .Result;
        }

        _modelSchema = JsonSchema.FromFileAsync(GetFilePath(Assembly.GetExecutingAssembly(), "schema.json")).Result;

        _deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .WithNodeTypeResolver(new InferTypeFromValueResolver())
            .WithTypeConverter(new StringListTypeConverter())
            .WithTypeConverter(new LocatedStringTypeConverter())
            .WithTypeConverter(new ReferenceTypeConverter())
            .IgnoreUnmatchedProperties()
            .Build();
        _serializer = new SerializerBuilder().JsonCompatible().Build();
        _parsingSerializer = new SerializerBuilder().Build();
    }

    public static string GetFilePath(Assembly assembly, string fileName)
    {
        return Path.Combine(Path.GetDirectoryName(assembly.Location)!, fileName);
    }

    public void CheckConfigFile(string fileName)
    {
        if (_configSchema != null)
        {
            CheckCore(_configSchema, fileName);
        }
    }

    public void CheckModelFile(string fileName, string? content = null)
    {
        CheckCore(_modelSchema, fileName, content);
    }

    public T Deserialize<T>(string yaml)
    {
        return _deserializer.Deserialize<T>(yaml);
    }

    public T Deserialize<T>(TextReader yaml)
    {
        return _deserializer.Deserialize<T>(yaml);
    }

    public T Deserialize<T>(IParser parser)
    {
        return _deserializer.Deserialize<T>(parser);
    }

    public ModelConfig DeserializeConfig(string yaml)
    {
        var stream = new YamlStream();
        stream.Load(new StringReader(yaml));
        var config = new ModelConfig();

        foreach (var kv in (stream.Documents[0].RootNode as YamlMappingNode)!.Children)
        {
            switch (kv)
            {
                case (YamlScalarNode { Value: "app" }, YamlScalarNode { Value: var value }):
                    config.App = value;
                    break;
                case (YamlScalarNode { Value: "modelRoot" }, YamlScalarNode { Value: var value }):
                    config.ModelRoot = value;
                    break;
                case (YamlScalarNode { Value: "modelFilePaths" }, YamlSequenceNode seq):
                    config.ModelFilePathsGlobs = seq.OfType<YamlScalarNode>()
                        .Select(n => n.Value!.EndsWith(".tmd") ? n.Value : $"{n.Value}/*.tmd")
                        .ToList();
                    break;
                case (YamlScalarNode { Value: "lockFileName" }, YamlScalarNode { Value: var value }):
                    config.LockFileName = value;
                    break;
                case (YamlScalarNode { Value: "noWarn" }, YamlSequenceNode seq):
                    config.NoWarn.AddRange(seq.OfType<YamlScalarNode>().Select(n => Enum.Parse<ErrorType>(n.Value!)));
                    break;
                case (YamlScalarNode { Value: "pluralizeTableNames" }, YamlScalarNode { Value: var value }):
                    config.PluralizeTableNames = value == "true";
                    break;
                case (YamlScalarNode { Value: "useLegacyRoleNames" }, YamlScalarNode { Value: var value }):
                    config.UseLegacyRoleNames = value == "true";
                    break;
                case (YamlScalarNode { Value: "i18n" }, YamlMappingNode map):
                    config.I18n = ParseNode<I18nConfig>(map);
                    break;
                case (YamlScalarNode { Value: "generators" }, YamlSequenceNode seq):
                    config.CustomGenerators.AddRange(seq.OfType<YamlScalarNode>().Select(n => n.Value!));
                    break;
                case (YamlScalarNode { Value: "ignoredFiles" }, YamlSequenceNode seq):
                    config.IgnoredFiles = ParseNode<IList<IgnoredFile>>(seq);
                    break;
                case (YamlScalarNode { Value: var value }, YamlSequenceNode seq):
                    config.Generators.Add(value, ParseNode<IEnumerable<IDictionary<string, object>>>(seq));
                    break;
                default:
                    break;
            }
        }

        return config;
    }

    public async Task<object> GetGenConfig(
        string configName,
        Type configType,
        IDictionary<string, object> genConfigMap,
        CancellationToken ct = default
    )
    {
        var schema = await JsonSchema.FromFileAsync(GetFilePath(configType.Assembly, $"{configName}.config.json"), ct);
        Validate(configName, schema, JToken.FromObject(genConfigMap));
        return _deserializer.Deserialize(_serializer.Serialize(genConfigMap), configType)!;
    }

    public WatcherConfigBase GetWatcherConfigBase(IDictionary<string, object> genConfigMap)
    {
        return _deserializer.Deserialize<WatcherConfigBase>(_serializer.Serialize(genConfigMap))!;
    }

    private static void Validate(string fileName, JsonSchema schema, JToken json)
    {
        var errors = schema.Validate(json);

        if (errors.Any())
        {
            var erreur = new StringBuilder();
            erreur.Append($"Erreur dans le fichier {fileName.ToRelative()} :");

            void HandleErrors(IEnumerable<ValidationError> validationErrors, string indent = "")
            {
                foreach (var e in validationErrors)
                {
                    erreur.Append($"{Environment.NewLine}{indent}[{e.LinePosition}]: {e.Kind} - {e.Path}");
                    if (e is ChildSchemaValidationError csve)
                    {
                        foreach (var schema in csve.Errors)
                        {
                            var newIndent = indent + "  ";
                            if (csve.Errors.Count > 1)
                            {
                                erreur.Append($"{Environment.NewLine}{newIndent}{schema.Key.Description}");
                                newIndent += "  ";
                            }

                            HandleErrors(schema.Value, newIndent);
                        }
                    }
                }
            }

            HandleErrors(errors);
            throw new ModelException(erreur.ToString());
        }
    }

    private void CheckCore(JsonSchema schema, string fileName, string? content = null)
    {
        content ??= File.ReadAllText(fileName);

        var parser = new Parser(new StringReader(content));
        parser.Consume<YamlDotNet.Core.Events.StreamStart>();

        var firstObject = true;
        while (parser.Current is YamlDotNet.Core.Events.DocumentStart)
        {
            var yaml =
                _deserializer.Deserialize(parser)
                ?? throw new ModelException($"Impossible de lire le fichier {fileName.ToRelative()}.");
            var json = JToken.FromObject(yaml);

            var finalSchema = firstObject && schema.OneOf.Any() ? schema.OneOf.First() : schema;

            Validate(fileName, finalSchema, json);

            firstObject = false;
        }
    }

    private T ParseNode<T>(YamlNode node)
    {
        return _deserializer.Deserialize<T>(
            new MergingParser(new Parser(new StringReader(_parsingSerializer.Serialize(node))))
        );
    }
}
