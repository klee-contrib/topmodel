using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Core.Model.Implementation;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.Loaders;

public class ConverterLoader(FileChecker fileChecker) : ILoader
{
    /// <inheritdoc cref="ILoader.Load" />
    public void Load(Parser parser, ModelFile modelFile, Reference location)
    {
        var converter = new Converter() { ModelFile = modelFile, Location = location };
        modelFile.Converters.Add(converter);
        parser.ConsumeMapping(prop =>
        {
            _ = parser.TryConsume<Scalar>(out var _);

            switch (prop.Value)
            {
                case "from":
                    parser.ConsumeSequence(() =>
                    {
                        var domainRef = new DomainReference(parser.Consume<Scalar>());
                        converter.DomainsFromReferences.Add(domainRef);
                    });
                    break;
                case "to":
                    parser.ConsumeSequence(() =>
                    {
                        var domainRef = new DomainReference(parser.Consume<Scalar>());
                        converter.DomainsToReferences.Add(domainRef);
                    });
                    break;
                default:
                    converter.Implementations[prop.Value] = fileChecker.Deserialize<ConverterImplementation>(parser);
                    break;
            }
        });
    }
}
