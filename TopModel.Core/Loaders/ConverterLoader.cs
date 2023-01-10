using TopModel.Core.FileModel;
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

    public Converter Load(Parser parser)
    {
        var converter = new Converter();

        parser.ConsumeMapping(() =>
        {
            var prop = parser.Consume<Scalar>().Value;
            _ = parser.TryConsume<Scalar>(out var value);

            switch (prop)
            {
                case "csharp":
                    converter.CSharp = _fileChecker.Deserialize<CSharpConverter>(parser);
                    break;
                case "java":
                    converter.Java = _fileChecker.Deserialize<JavaConverter>(parser);
                    break;
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
                    throw new ModelException(converter, $"Propriété ${prop} inconnue pour un décoteur");
            }
        });

        return converter;
    }
}