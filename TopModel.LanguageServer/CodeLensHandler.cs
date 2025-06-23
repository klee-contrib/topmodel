using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;

namespace TopModel.LanguageServer;

public class CodeLensHandler(ModelStore modelStore, ILanguageServerFacade facade, ModelConfig config) : CodeLensHandlerBase
{
    public override Task<CodeLens> Handle(CodeLens request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request);
    }

    public override Task<CodeLensContainer?> Handle(CodeLensParams request, CancellationToken cancellationToken)
    {
        var file = modelStore.Files.SingleOrDefault(f => facade.GetFilePath(f) == request.TextDocument.Uri.GetFileSystemPath());
        if (file != null)
        {
            return Task.FromResult<CodeLensContainer?>(new(file.Classes.Select(clazz =>
                new CodeLens
                {
                    Range = clazz.GetLocation().ToRange()!,
                    Command = new Command()
                    {
                        Title = $"{modelStore.GetClassReferences(clazz).Count()} references",
                        Name = "topmodel.findRef",
                        Arguments =
                        [
                            clazz.GetLocation()!.Start.Line - 1
                        ]
                    }
                })
                .Concat(file.Domains.Select(domain => new CodeLens
                {
                    Range = domain.GetLocation().ToRange()!,
                    Command = new Command()
                    {
                        Title = $"{modelStore.GetDomainReferences(domain).Count()} references",
                        Name = "topmodel.findRef",
                        Arguments =
                        [
                            domain.GetLocation()!.Start.Line - 1
                        ]
                    }
                }))
                .Concat(file.Decorators.Select(decorator => new CodeLens
                {
                    Range = decorator.GetLocation().ToRange()!,
                    Command = new Command()
                    {
                        Title = $"{modelStore.GetDecoratorReferences(decorator).Count()} references",
                        Name = "topmodel.findRef",
                        Arguments =
                        [
                            decorator.GetLocation()!.Start.Line - 1
                        ]
                    }
                })
                .Concat(file.DataFlows.Select(dataFlow => new CodeLens
                {
                    Range = dataFlow.GetLocation().ToRange()!,
                    Command = new Command()
                    {
                        Title = $"{modelStore.GetDataFlowReferences(dataFlow).Count()} references",
                        Name = "topmodel.findRef",
                        Arguments =
                        [
                            dataFlow.GetLocation()!.Start.Line - 1
                        ]
                    }
                })))
                .Concat(file.Endpoints.Select(endpoint => new CodeLens
                {
                    Range = endpoint.GetLocation().ToRange()!,
                    Command = new()
                    {
                        Title = $"{modelStore.GetEndpointReferences(endpoint).Count()} references",
                        Name = "topmodel.findRef",
                        Arguments =
                        [
                            endpoint.GetLocation()!.Start.Line - 1
                        ]
                    }
                }))));
        }

        return Task.FromResult<CodeLensContainer?>(new());
    }

    protected override CodeLensRegistrationOptions CreateRegistrationOptions(CodeLensCapability capability, ClientCapabilities clientCapabilities)
    {
        return new CodeLensRegistrationOptions
        {
            DocumentSelector = config.GetDocumentSelector()
        };
    }
}