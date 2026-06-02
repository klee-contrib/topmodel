using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;
using TopModel.Core.Utils;

namespace TopModel.LanguageServer.Handlers;

public class CodeLensHandler(LSWorkerStore workerStore, ILanguageServerFacade facade) : CodeLensHandlerBase
{
    private ModelStore? ModelStore => workerStore.ModelStore;

    public override Task<CodeLens> Handle(CodeLens request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request);
    }

    public override async Task<CodeLensContainer?> Handle(CodeLensParams request, CancellationToken cancellationToken)
    {
        await workerStore.WaitForUpdates(cancellationToken);

        var file = ModelStore?.Files.SingleOrDefault(f =>
            facade.GetFilePath(f) == request.TextDocument.Uri.GetFileSystemPath()
        );
        if (file != null)
        {
            return new(
                file.Classes.Select(clazz => new CodeLens
                    {
                        Range = clazz.GetLocation().ToRange()!,
                        Command = new Command()
                        {
                            Title = $"{ModelStore!.GetClassReferences(clazz).Count()} references",
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
                                Title = $"{ModelStore!.GetAnnotationReferences(annotation).Count()} references",
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
                                Title = $"{ModelStore!.GetDomainReferences(domain).Count()} references",
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
                                    Title = $"{ModelStore!.GetDecoratorReferences(decorator).Count()} references",
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
                                        Title = $"{ModelStore!.GetDataFlowReferences(dataFlow).Count()} references",
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
                                Title = $"{ModelStore!.GetEndpointReferences(endpoint).Count()} references",
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
        return new CodeLensRegistrationOptions { DocumentSelector = TextDocumentSelector.TmdFiles };
    }
}
