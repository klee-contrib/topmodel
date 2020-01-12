using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace TopModel.Core.Loaders
{
    public class FileChecker
    {
        private readonly IDeserializer _deserializer;
        private readonly ISerializer _serialiazer;

        private readonly JSchema _configSchema = JSchema.Parse(File.ReadAllText(
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "schema.config.json")));
        private readonly JSchema _modelSchema = JSchema.Parse(File.ReadAllText(
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "schema.json")));

        public FileChecker(IDeserializer deserializer)
        {
            _deserializer = deserializer;
            _serialiazer = new SerializerBuilder()
                .JsonCompatible()
                .Build();
        }

        public void CheckConfigFile(string fileName)
        {
            CheckCore(fileName, _configSchema);
        }

        public void CheckDomainFile(string fileName)
        {
            CheckCore(fileName, _modelSchema.OneOf[1]);
        }

        public void CheckModelFile(string fileName)
        {
            CheckCore(fileName, _modelSchema, true);
        }

        private void CheckCore(string fileName, JSchema schema, bool isModel = false)
        {
            var parser = new Parser(new StringReader(File.ReadAllText(fileName)));
            parser.Consume<StreamStart>();

            var firstObject = true;
            while (parser.Current is DocumentStart)
            {
                var yaml = _deserializer.Deserialize(parser);
                if (yaml == null)
                {
                    throw new Exception($"Impossible de lire le fichier {fileName}.");
                }

                var jsonYaml = _serialiazer.Serialize(yaml);
                var json = JObject.Parse(jsonYaml);

                var finalSchema = isModel ? firstObject ? schema.OneOf[0] : schema.OneOf[2] : schema;

                if (!json.IsValid(schema, out IList<ValidationError> errors))
                {
                    throw new Exception($@"Erreur dans le fichier {fileName} :
{string.Join("\r\n", errors.Select(e => $"[{e.LinePosition}]: {e.Message}"))}.");
                }

                firstObject = false;
            }
        }
    }   
}
