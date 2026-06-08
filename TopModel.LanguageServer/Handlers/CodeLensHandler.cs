using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using TopModel.Core.FileModel;
using TopModel.Core.Utils;

namespace TopModel.LanguageServer.Handlers;

public class CodeLensHandler(LSWorkerStore workerStore) : CodeLensHandlerBase
{
    public override Task<CodeLens> Handle(CodeLens request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request);
    }

    public override async Task<CodeLensContainer?> Handle(CodeLensParams request, CancellationToken cancellationToken)
    {
        await workerStore.WaitForUpdates(cancellationToken);

        var files = workerStore.GetFiles(request.TextDocument);

        var clic = new CodeLensContainer(
            files
                .SelectMany(f =>
                    f.File.Classes.Select(c =>
                        (Location: c.GetLocation()!, References: f.Store.GetClassReferences(c).Select(r => r.Reference))
                    )
                )
                .Concat(
                    files.SelectMany(f =>
                        f.File.Annotations.Select(c =>
                            (
                                Location: c.GetLocation()!,
                                References: f.Store.GetAnnotationReferences(c).Select(r => r.Reference as Reference)
                            )
                        )
                    )
                )
                .Concat(
                    files.SelectMany(f =>
                        f.File.Domains.Select(d =>
                            (
                                Location: d.GetLocation()!,
                                References: f.Store.GetDomainReferences(d).Select(r => r.Reference as Reference)
                            )
                        )
                    )
                )
                .Concat(
                    files.SelectMany(f =>
                        f.File.Decorators.Select(d =>
                            (
                                Location: d.GetLocation()!,
                                References: f.Store.GetDecoratorReferences(d).Select(r => r.Reference as Reference)
                            )
                        )
                    )
                )
                .Concat(
                    files.SelectMany(f =>
                        f.File.DataFlows.Select(d =>
                            (
                                Location: d.GetLocation()!,
                                References: f.Store.GetDataFlowReferences(d).Select(r => r.Reference as Reference)
                            )
                        )
                    )
                )
                .Concat(
                    files.SelectMany(f =>
                        f.File.Endpoints.Select(e =>
                            (
                                Location: e.GetLocation()!,
                                References: f.Store.GetEndpointReferences(e).Select(r => r.Reference as Reference)
                            )
                        )
                    )
                )
                .SelectMany(r => r.References.Select(item => (r.Location, Reference: item)))
                .Distinct()
                .GroupBy(r => r.Location)
                .Select(reference => new CodeLens
                {
                    Range = reference.Key.ToRange()!,
                    Command = new Command()
                    {
                        Title = $"{reference.Count()} references",
                        Name = "topmodel.findRef",
                        Arguments = [reference.Key.Start.Line - 1],
                    },
                })
        );

        return clic;
    }

    protected override CodeLensRegistrationOptions CreateRegistrationOptions(
        CodeLensCapability capability,
        ClientCapabilities clientCapabilities
    )
    {
        return new CodeLensRegistrationOptions { DocumentSelector = TextDocumentSelector.TmdFiles };
    }
}
