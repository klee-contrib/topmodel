using System.Reflection;
using System.Text;
using System.Text.Json;
using Json.Schema;
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
            _configSchema = JsonSchema.FromFile(GetFilePath(Assembly.GetExecutingAssembly(), configSchemaPath));
        }

        _modelSchema = JsonSchema.FromFile(GetFilePath(Assembly.GetExecutingAssembly(), "schema.json"));

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
                case (YamlScalarNode { Value: "defaultAssociationUseClass" }, YamlScalarNode { Value: var value }):
                    config.DefaultAssociationUseClass = value == "true";
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

    public object GetGenConfig(string configName, Type configType, IDictionary<string, object> genConfigMap)
    {
        var schema = JsonSchema.FromFile(
            GetFilePath(configType.Assembly, $"{configName}.config.json"),
            new() { SchemaRegistry = new SchemaRegistry() }
        );
        Validate(configName, schema, JsonSerializer.SerializeToElement(genConfigMap));
        return _deserializer.Deserialize(_serializer.Serialize(genConfigMap), configType)!;
    }

    public WatcherConfigBase GetWatcherConfigBase(IDictionary<string, object> genConfigMap)
    {
        return _deserializer.Deserialize<WatcherConfigBase>(_serializer.Serialize(genConfigMap))!;
    }

    private static void Validate(string fileName, JsonSchema schema, JsonElement json)
    {
        var result = schema.Evaluate(json, new() { OutputFormat = OutputFormat.Hierarchical });

        if (!result.IsValid)
        {
            var erreur = new StringBuilder();
            erreur.AppendLine($"Erreur dans le fichier {fileName.ToRelative()} :");

            void HandleErrors(EvaluationResults r)
            {
                if (r.IsValid)
                {
                    return;
                }

                if (r.Errors != null)
                {
                    erreur.AppendLine($"{r.EvaluationPath}:{string.Join(", ", r.Errors)}");
                }
                else if (r.Details != null)
                {
                    foreach (var d in r.Details)
                    {
                        HandleErrors(d);
                    }
                }
            }

            HandleErrors(result);
            throw new ModelException(erreur.ToString());
        }
    }

    private void CheckCore(JsonSchema schema, string fileName, string? content = null)
    {
        content ??= File.ReadAllText(fileName);

        var parser = new Parser(new StringReader(content));
        parser.Consume<YamlDotNet.Core.Events.StreamStart>();

        while (parser.Current is YamlDotNet.Core.Events.DocumentStart)
        {
            var yaml =
                _deserializer.Deserialize(parser)
                ?? throw new ModelException($"Impossible de lire le fichier {fileName.ToRelative()}.");
            var json = JsonSerializer.SerializeToElement(yaml);

            Validate(fileName, schema, json);
        }
    }

    private T ParseNode<T>(YamlNode node)
    {
        return _deserializer.Deserialize<T>(
            new MergingParser(new Parser(new StringReader(_parsingSerializer.Serialize(node))))
        );
    }
}
