using System.Reflection;
using System.Text.Json;
using Json.Schema;
using TopModel.Core.Loaders.YamlUtils;
using TopModel.Utils;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;

namespace TopModel.Core.Loaders;

public class FileChecker()
    : AbstractFileChecker<ModelConfig>(
        "schema.config.json",
        new StringListTypeConverter(),
        new LocatedStringTypeConverter(),
        new ReferenceTypeConverter()
    )
{
    private readonly JsonSchema _modelSchema = JsonSchema.FromFile(
        Assembly.GetExecutingAssembly().GetFilePath("schema.json")
    );
    private readonly ISerializer _parsingSerializer = new SerializerBuilder().Build();
    private readonly ISerializer _serializer = new SerializerBuilder().JsonCompatible().Build();

    public void CheckModelFile(string fileName, string? content = null)
    {
        CheckCore(_modelSchema, fileName, content);
    }

    public override ModelConfig DeserializeConfig(string yaml)
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
            configType.Assembly.GetFilePath($"{configName}.config.json"),
            new() { SchemaRegistry = new SchemaRegistry() }
        );
        Validate(configName, schema, JsonSerializer.SerializeToElement(genConfigMap));
        return Deserializer.Deserialize(_serializer.Serialize(genConfigMap), configType)!;
    }

    public WatcherConfigBase GetWatcherConfigBase(IDictionary<string, object> genConfigMap)
    {
        return Deserializer.Deserialize<WatcherConfigBase>(_serializer.Serialize(genConfigMap))!;
    }

    private T ParseNode<T>(YamlNode node)
    {
        return Deserializer.Deserialize<T>(
            new MergingParser(new Parser(new StringReader(_parsingSerializer.Serialize(node))))
        );
    }
}
