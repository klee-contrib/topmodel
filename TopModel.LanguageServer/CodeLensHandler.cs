using Newtonsoft.Json.Linq;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;

namespace TopModel.LanguageServer;

public class CodeLensHandler : CodeLensHandlerBase
{
    private readonly ModelConfig _config;
    private readonly ILanguageServerFacade _facade;
    private readonly ModelStore _modelStore;

    public CodeLensHandler(ModelStore modelStore, ILanguageServerFacade facade, ModelConfig config)
    {
        _config = config;
        _facade = facade;
        _modelStore = modelStore;
    }

    public override Task<CodeLens> Handle(CodeLens request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request);
    }

    public override Task<CodeLensContainer> Handle(CodeLensParams request, CancellationToken cancellationToken)
    {
        var file = _modelStore.Files.SingleOrDefault(f => _facade.GetFilePath(f) == request.TextDocument.Uri.GetFileSystemPath());
        if (file != null)
        {
            return Task.FromResult(new CodeLensContainer(file.Classes.Where(c => !file.ResolvedAliases.Contains(c)).Select(clazz =>
                new CodeLens
                {
                    Range = clazz.GetLocation().ToRange()!,
                    Command = new Command()
                    {
                        Title = $"{_modelStore.GetClassReferences(clazz).Count()} references",
                        Name = "topmodel.findRef",
                        Arguments = new JArray
                        {
                            clazz.GetLocation()!.Start.Line - 1
                        }
                    }
                })
                .Concat(file.Domains.Select(domain => new CodeLens
                {
                    Range = domain.GetLocation().ToRange()!,
                    Command = new Command()
                    {
                        Title = $"{_modelStore.GetDomainReferences(domain).Count()} references",
                        Name = "topmodel.findRef",
                        Arguments = new JArray
                        {
                            domain.GetLocation()!.Start.Line - 1
                        }
                    }
                }))
                .Concat(file.Decorators.Select(decorator => new CodeLens
                {
                    Range = decorator.GetLocation().ToRange()!,
                    Command = new Command()
                    {
                        Title = $"{_modelStore.GetDecoratorReferences(decorator).Count()} references",
                        Name = "topmodel.findRef",
                        Arguments = new JArray
                        {
                            decorator.GetLocation()!.Start.Line - 1
                        }
                    }
                })
                .Concat(file.DataFlows.Select(dataFlow => new CodeLens
                {
                    Range = dataFlow.GetLocation().ToRange()!,
                    Command = new Command()
                    {
                        Title = $"{_modelStore.GetDataFlowReferences(dataFlow).Count()} references",
                        Name = "topmodel.findRef",
                        Arguments = new JArray
                        {
                            dataFlow.GetLocation()!.Start.Line - 1
                        }
                    }
                })))));
        }

        return Task.FromResult<CodeLensContainer>(new());
    }

    protected override CodeLensRegistrationOptions CreateRegistrationOptions(CodeLensCapability capability, ClientCapabilities clientCapabilities)
    {
        return new CodeLensRegistrationOptions
        {
            DocumentSelector = _config.GetDocumentSelector()
        };
    }
}