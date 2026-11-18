using Newtonsoft.Json.Linq;
using OmniSharp.Extensions.JsonRpc;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core.FileModel;
using TopModel.Core.Utils;
using Location = OmniSharp.Extensions.LanguageServer.Protocol.Models.Location;

namespace TopModel.LanguageServer.Handlers;

public class CodeLensHandler(LSWorkerStore workerStore, ILanguageServerFacade facade, ISerializer serializer)
    : CodeLensHandlerBase
{
    public override Task<CodeLens> Handle(CodeLens request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request);
    }

    public override async Task<CodeLensContainer?> Handle(CodeLensParams request, CancellationToken cancellationToken)
    {
        await workerStore.WaitForUpdates(cancellationToken);

        var files = workerStore.GetFiles(request.TextDocument);

        return new CodeLensContainer(
            files
                .SelectMany(f =>
                    f.File.Classes.Select(c =>
                        (
                            Declaration: c.Declaration!,
                            References: f.Store.GetClassReferences(c)
                                .Select(cr => (Reference: cr.Reference as Reference, cr.File))
                        )
                    )
                )
                .Concat(
                    files.SelectMany(f =>
                        f.File.Annotations.Select(c =>
                            (
                                Declaration: c.Declaration!,
                                References: f.Store.GetAnnotationReferences(c)
                                    .Select(r => (Reference: r.Reference as Reference, r.File))
                            )
                        )
                    )
                )
                .Concat(
                    files.SelectMany(f =>
                        f.File.Domains.Select(d =>
                            (
                                Declaration: d.Declaration!,
                                References: f.Store.GetDomainReferences(d)
                                    .Select(r => (Reference: r.Reference as Reference, r.File))
                            )
                        )
                    )
                )
                .Concat(
                    files.SelectMany(f =>
                        f.File.Decorators.Select(d =>
                            (
                                Declaration: d.Declaration!,
                                References: f.Store.GetDecoratorReferences(d)
                                    .Select(r => (Reference: r.Reference as Reference, r.File))
                            )
                        )
                    )
                )
                .Concat(
                    files.SelectMany(f =>
                        f.File.DataFlows.Select(d =>
                            (
                                Declaration: d.Declaration!,
                                References: f.Store.GetDataFlowReferences(d)
                                    .Select(r => (Reference: r.Reference as Reference, r.File))
                            )
                        )
                    )
                )
                .Concat(
                    files.SelectMany(f =>
                        f.File.Endpoints.Select(e =>
                            (
                                Declaration: e.Declaration!,
                                References: f.Store.GetEndpointReferences(e)
                                    .Select(r => (Reference: r.Reference as Reference, r.File))
                            )
                        )
                    )
                )
                .SelectMany(r => r.References.Select(item => (r.Declaration, item.Reference, item.File)))
                .Distinct()
                .GroupBy(r => r.Declaration)
                .Select(group =>
                {
                    var range = group.Key.ToRange();
                    var locations = group
                        .Select(item => new Location
                        {
                            Uri = new Uri(facade.GetFilePath(item.File)),
                            Range = item.Reference.ToRange(),
                        })
                        .ToArray();
                    return new CodeLens
                    {
                        Range = range,
                        Command = new Command()
                        {
                            Title = $"{group.Count()} references",
                            Name = "editor.action.showReferences",
                            Arguments = JArray.FromObject(
                                new object[] { request.TextDocument.Uri, range.Start, locations },
                                serializer.JsonSerializer
                            ),
                        },
                    };
                })
        );
    }

    protected override CodeLensRegistrationOptions CreateRegistrationOptions(
        CodeLensCapability capability,
        ClientCapabilities clientCapabilities
    )
    {
        return new CodeLensRegistrationOptions { DocumentSelector = workerStore.TmdFiles };
    }
}
