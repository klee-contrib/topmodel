using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NJsonSchema;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TopModel.Core.Loaders
{
    public class FileChecker
    {
        private readonly IDeserializer _deserializer;
        private readonly ISerializer _serialiazer;

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
            _serialiazer = new SerializerBuilder()
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

        public void CheckDomainFile(string fileName)
        {
            CheckCore(fileName, _modelSchema.OneOf.ToList()[1]);
        }

        public void CheckModelFile(string fileName)
        {
            CheckCore(fileName, _modelSchema, true);
        }

        public T Deserialize<T>(string yaml)
        {
            return _deserializer.Deserialize<T>(yaml);
        }

        public T Deserialize<T>(IParser parser)
        {
            return _deserializer.Deserialize<T>(parser);
        }

        private void CheckCore(string fileName, JsonSchema schema, bool isModel = false)
        {
            var parser = new Parser(new StringReader(File.ReadAllText(fileName)));
            parser.Consume<StreamStart>();

            var firstObject = true;
            while (parser.Current is DocumentStart)
            {
                var yaml = _deserializer.Deserialize(parser);
                if (yaml == null)
                {
                    throw new Exception($"Impossible de lire le fichier {fileName.ToRelative()}.");
                }

                var json = _serialiazer.Serialize(yaml);
                var finalSchema = isModel ? firstObject ? schema.OneOf.First() : schema.OneOf.Last() : schema;
                var errors = finalSchema.Validate(json);

                if (errors.Any())
                {
                    throw new Exception($@"Erreur dans le fichier {fileName.ToRelative()} :
{string.Join("\r\n", errors.Select(e => $"[{e.LinePosition}]: {e.Kind} - {e.Path}"))}.");
                }

                firstObject = false;
            }
        }
    }
}
