using TopModel.Core.FileModel;
using TopModel.Core.Model.Implementation;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.Loaders;

public class ConverterLoader : ILoader<Converter>
{
    private readonly FileChecker _fileChecker;

    public ConverterLoader(FileChecker fileChecker)
    {
        _fileChecker = fileChecker;
    }

    /// <inheritdoc cref="ILoader{T}.Load" />
    public Converter Load(Parser parser)
    {
        var converter = new Converter();

        parser.ConsumeMapping(() =>
        {
            var prop = parser.Consume<Scalar>().Value;
            _ = parser.TryConsume<Scalar>(out var value);

            switch (prop)
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
                    converter.Implementations[prop] = _fileChecker.Deserialize<ConverterImplementation>(parser);
                    break;
            }
        });

        return converter;
    }
}