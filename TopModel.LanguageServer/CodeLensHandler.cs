using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using TopModel.Core.Utils;

namespace TopModel.LanguageServer;

public class CodeLensHandler(ModelStoreRegistry registry) : CodeLensHandlerBase
{
    public override Task<CodeLens> Handle(CodeLens request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request);
    }

    public override async Task<CodeLensContainer?> Handle(CodeLensParams request, CancellationToken cancellationToken)
    {
        var filePath = request.TextDocument.Uri.GetFileSystemPath();
        var entry = registry.GetPrimaryForFile(filePath);
        if (entry == null)
        {
            return new();
        }

        var modelStore = entry.Store;
        await modelStore.WaitForUpdates(cancellationToken);
        var file = modelStore.Files.SingleOrDefault(f => f.GetFilePath() == filePath);
        if (file != null)
        {
            return new(
                file.Classes.Select(clazz => new CodeLens
                    {
                        Range = clazz.GetLocation().ToRange()!,
                        Command = new Command()
                        {
                            Title = $"{modelStore.GetClassReferences(clazz).Count()} references",
                            Name = "topmodel.findRef",
                            Arguments = [clazz.GetLocation()!.Start.Line - 1],
                        },
                    })
                    .Concat(
                        file.Annotations.Select(annotation => new CodeLens
                        {
                            Range = annotation.GetLocation().ToRange()!,
                            Command = new Command()
                            {
                                Title = $"{modelStore.GetAnnotationReferences(annotation).Count()} references",
                                Name = "topmodel.findRef",
                                Arguments = [annotation.GetLocation()!.Start.Line - 1],
                            },
                        })
                    )
                    .Concat(
                        file.Domains.Select(domain => new CodeLens
                        {
                            Range = domain.GetLocation().ToRange()!,
                            Command = new Command()
                            {
                                Title = $"{modelStore.GetDomainReferences(domain).Count()} references",
                                Name = "topmodel.findRef",
                                Arguments = [domain.GetLocation()!.Start.Line - 1],
                            },
                        })
                    )
                    .Concat(
                        file.Decorators.Select(decorator => new CodeLens
                            {
                                Range = decorator.GetLocation().ToRange()!,
                                Command = new Command()
                                {
                                    Title = $"{modelStore.GetDecoratorReferences(decorator).Count()} references",
                                    Name = "topmodel.findRef",
                                    Arguments = [decorator.GetLocation()!.Start.Line - 1],
                                },
                            })
                            .Concat(
                                file.DataFlows.Select(dataFlow => new CodeLens
                                {
                                    Range = dataFlow.GetLocation().ToRange()!,
                                    Command = new Command()
                                    {
                                        Title = $"{modelStore.GetDataFlowReferences(dataFlow).Count()} references",
                                        Name = "topmodel.findRef",
                                        Arguments = [dataFlow.GetLocation()!.Start.Line - 1],
                                    },
                                })
                            )
                    )
                    .Concat(
                        file.Endpoints.Select(endpoint => new CodeLens
                        {
                            Range = endpoint.GetLocation().ToRange()!,
                            Command = new()
                            {
                                Title = $"{modelStore.GetEndpointReferences(endpoint).Count()} references",
                                Name = "topmodel.findRef",
                                Arguments = [endpoint.GetLocation()!.Start.Line - 1],
                            },
                        })
                    )
            );
        }

        return new();
    }

    protected override CodeLensRegistrationOptions CreateRegistrationOptions(
        CodeLensCapability capability,
        ClientCapabilities clientCapabilities
    )
    {
        return new CodeLensRegistrationOptions { DocumentSelector = registry.GetCombinedDocumentSelector() };
    }
}
