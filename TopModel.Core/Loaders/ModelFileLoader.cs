using TopModel.Core.FileModel;
using TopModel.Utils;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.Loaders;

public class ModelFileLoader
{
    private readonly ClassLoader _classLoader;
    private readonly ModelConfig _config;
    private readonly ConverterLoader _converterLoader;
    private readonly DecoratorLoader _decoratorLoader;
    private readonly DomainLoader _domainLoader;
    private readonly EndpointLoader _endpointLoader;
    private readonly FileChecker _fileChecker;

    public ModelFileLoader(ModelConfig config, ClassLoader classLoader, FileChecker fileChecker, DecoratorLoader decoratorLoader, ConverterLoader converterLoader, EndpointLoader endpointLoader, DomainLoader domainLoader)
    {
        _classLoader = classLoader;
        _config = config;
        _converterLoader = converterLoader;
        _decoratorLoader = decoratorLoader;
        _domainLoader = domainLoader;
        _endpointLoader = endpointLoader;
        _fileChecker = fileChecker;
    }

    public ModelFile? LoadModelFile(string filePath, string? content = null)
    {
        content ??= File.ReadAllText(filePath);

        _fileChecker.CheckModelFile(filePath, content);

        var parser = new Parser(new StringReader(content));
        parser.Consume<StreamStart>();

        if (parser.Current is StreamEnd)
        {
            return null;
        }

        parser.Consume<DocumentStart>();

        var file = new ModelFile
        {
            Name = _config.GetFileName(filePath),
            Path = filePath.ToRelative(),
        };

        parser.ConsumeMapping(() =>
        {
            var prop = parser.Consume<Scalar>().Value;
            parser.TryConsume<Scalar>(out var value);

            switch (prop)
            {
                case "module":
                    file.Module = value!.Value;
                    break;
                case "tags":
                    parser.ConsumeSequence(() =>
                    {
                        file.Tags.Add(parser.Consume<Scalar>().Value);
                    });
                    break;
                case "uses":
                    parser.ConsumeSequence(() =>
                    {
                        file.Uses.Add(new Reference(parser.Consume<Scalar>()));
                    });
                    break;
                case "options":
                    parser.Consume<MappingStart>();
                    var scalar = parser.Consume<Scalar>();
                    if (scalar.Value == "endpoints")
                    {
                        parser.ConsumeMapping(() =>
                        {
                            var prop = parser.Consume<Scalar>().Value;
                            parser.TryConsume<Scalar>(out var value);
                            switch (prop)
                            {
                                case "fileName":
                                    file.Options.Endpoints.FileName = value!.Value;
                                    break;
                                case "prefix":
                                    file.Options.Endpoints.Prefix = new LocatedString(value!);
                                    break;
                            }
                        });
                    }

                    parser.Consume<MappingEnd>();
                    break;
            }
        });

        parser.Consume<DocumentEnd>();

        while (parser.TryConsume<DocumentStart>(out var _))
        {
            parser.Consume<MappingStart>();
            var scalar = parser.Consume<Scalar>();

            if (scalar.Value == "domain")
            {
                var domain = _domainLoader.Load(parser);
                domain.ModelFile = file;
                domain.Location = new Reference(scalar);
                file.Domains.Add(domain);
            }
            else if (scalar.Value == "decorator")
            {
                var decorator = _decoratorLoader.Load(parser);
                decorator.ModelFile = file;
                decorator.Location = new Reference(scalar);
                file.Decorators.Add(decorator);
            }
            else if (scalar.Value == "converter")
            {
                var converter = _converterLoader.Load(parser);
                converter.ModelFile = file;
                converter.Location = new Reference(scalar);
                file.Converters.Add(converter);
            }
            else if (scalar.Value == "class")
            {
                var classe = _classLoader.Load(parser);
                classe.Location = new Reference(scalar);
                file.Classes.Add(classe);
            }
            else if (scalar.Value == "endpoint")
            {
                var endpoint = _endpointLoader.Load(parser);
                endpoint.Location = new Reference(scalar);
                file.Endpoints.Add(endpoint);
            }
            else if (scalar.Value == "alias")
            {
                var alias = new Alias();

                parser.ConsumeMapping(() =>
                {
                    var prop = parser.Consume<Scalar>().Value;
                    parser.TryConsume<Scalar>(out var value);

                    switch (prop)
                    {
                        case "file":
                            alias.File = new Reference(value);
                            break;
                        case "classes":
                            parser.ConsumeSequence(() =>
                            {
                                alias.Classes.Add(new ClassReference(parser.Consume<Scalar>()));
                            });
                            break;
                        case "endpoints":
                            parser.ConsumeSequence(() =>
                            {
                                alias.Endpoints.Add(new Reference(parser.Consume<Scalar>()));
                            });
                            break;
                    }
                });

                alias.ModelFile = file;
                alias.Location = new Reference(scalar);
                file.Aliases.Add(alias);
            }
            else
            {
                throw new ModelException("Type de document inconnu.");
            }

            parser.Consume<MappingEnd>();
            parser.Consume<DocumentEnd>();
        }

        var ns = new Namespace { App = _config.App, Module = file.Module };
        foreach (var classe in file.Classes)
        {
            classe.ModelFile = file;
            classe.Namespace = ns;
        }

        foreach (var endpoint in file.Endpoints)
        {
            endpoint.ModelFile = file;
            endpoint.Namespace = ns;
        }

        if (file.Options.Endpoints.FileName == null)
        {
            var fileSplit = file.Name.Split("/").Last();
            file.Options.Endpoints.FileName = string.Join('_', fileSplit.Split("_").Skip(fileSplit.Contains('_') ? 1 : 0));
        }

        return file;
    }
}