using System.Reflection;
using System.Text;
using System.Text.Json;
using Json.Schema;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TopModel.Utils;

public abstract class AbstractFileChecker<TConfig>
    where TConfig : ConfigBase
{
    private readonly JsonSchema? _configSchema;

    protected AbstractFileChecker(string? configSchemaPath, params IEnumerable<IYamlTypeConverter> typeConverters)
    {
        if (configSchemaPath != null)
        {
            _configSchema = JsonSchema.FromFile(Assembly.GetExecutingAssembly().GetFilePath(configSchemaPath));
        }

        var deserializerBuilder = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .WithNodeTypeResolver(new InferTypeFromValueResolver())
            .IgnoreUnmatchedProperties();

        foreach (var typeConverter in typeConverters)
        {
            deserializerBuilder.WithTypeConverter(typeConverter);
        }

        Deserializer = deserializerBuilder.Build();
    }

    protected IDeserializer Deserializer { get; }

    public void CheckConfigFile(string fileName)
    {
        if (_configSchema != null)
        {
            CheckCore(_configSchema, fileName);
        }
    }

    public T Deserialize<T>(string yaml)
    {
        return Deserializer.Deserialize<T>(yaml);
    }

    public T Deserialize<T>(TextReader yaml)
    {
        return Deserializer.Deserialize<T>(yaml);
    }

    public T Deserialize<T>(IParser parser)
    {
        return Deserializer.Deserialize<T>(parser);
    }

    public abstract TConfig DeserializeConfig(string yaml);

    protected static void Validate(string fileName, JsonSchema schema, JsonElement json)
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
            throw new LegitException(erreur.ToString());
        }
    }

    protected void CheckCore(JsonSchema schema, string fileName, string? content = null)
    {
        content ??= File.ReadAllText(fileName);

        var parser = new Parser(new StringReader(content));
        parser.Consume<YamlDotNet.Core.Events.StreamStart>();

        while (parser.Current is YamlDotNet.Core.Events.DocumentStart)
        {
            var yaml =
                Deserializer.Deserialize(parser)
                ?? throw new LegitException($"Impossible de lire le fichier {fileName.ToRelative()}.");
            var json = JsonSerializer.SerializeToElement(yaml);

            Validate(fileName, schema, json);
        }
    }
}
