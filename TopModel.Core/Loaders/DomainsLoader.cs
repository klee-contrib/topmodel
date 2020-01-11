using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace TopModel.Core.Loaders
{
    public static class DomainsLoader
    {
        public static IEnumerable<Domain> LoadDomains(string domainFilePath, IDeserializer deserializer)
        {
            var parser = new Parser(new StringReader(File.ReadAllText(domainFilePath)));
            parser.Consume<StreamStart>();

            while (parser.TryConsume<DocumentStart>(out var _))
            {
                parser.Consume<MappingStart>();
                var scalar = parser.Consume<Scalar>();
                if (scalar.Value != "domain")
                {
                    throw new Exception("Seuls des domaines peuvent être définis dans le fichier de domaines");
                }

                var domain = deserializer.Deserialize<Domain>(parser);

                if (domain.Name == null)
                {
                    throw new Exception("Un domaine doit avoir un nom.");
                }

                if (domain.Label == null || domain.CsharpType == null)
                {
                    throw new Exception($"Les propriétés 'label' et 'csharpType' sont obligatoires sur le domaine {domain.Name}");
                }

                yield return domain;

                parser.Consume<MappingEnd>();
                parser.Consume<DocumentEnd>();
            }
        }
    }
}
