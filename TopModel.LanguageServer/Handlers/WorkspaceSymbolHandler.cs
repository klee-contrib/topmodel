using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using OmniSharp.Extensions.LanguageServer.Protocol.Workspace;
using TopModel.Core.Utils;

namespace TopModel.LanguageServer.Handlers;

public class WorkspaceSymbolHandler(LSWorkerStore workerStore, ILanguageServerFacade facade)
    : WorkspaceSymbolsHandlerBase
{
    /// <inheritdoc cref="MediatR.IRequestHandler{TRequest, TResponse}.Handle" />
    public override async Task<Container<WorkspaceSymbol>?> Handle(
        WorkspaceSymbolParams request,
        CancellationToken cancellationToken
    )
    {
        await workerStore.WaitForUpdates(cancellationToken);

        return workerStore
            .ModelStores.SelectMany(ms => ms.Classes)
            .Select(c => (Kind: SymbolKind.Class, c.Name, Uri: facade.GetFilePath(c.ModelFile)))
            .Distinct()
            .Concat(
                workerStore
                    .ModelStores.SelectMany(ms => ms.Endpoints)
                    .Select(e => (Kind: SymbolKind.Method, e.Name, Uri: facade.GetFilePath(e.ModelFile)))
                    .Distinct()
            )
            .Concat(
                workerStore
                    .ModelStores.SelectMany(ms => ms.Domains.Values)
                    .Select(d => (Kind: SymbolKind.Struct, d.Name, Uri: facade.GetFilePath(d.GetFile())))
                    .Distinct()
            )
            .Concat(
                workerStore
                    .ModelStores.SelectMany(ms => ms.Annotations)
                    .Select(a => (Kind: SymbolKind.Interface, a.Name, Uri: facade.GetFilePath(a.GetFile())))
                    .Distinct()
            )
            .Concat(
                workerStore
                    .ModelStores.SelectMany(ms => ms.Decorators)
                    .Select(d => (Kind: SymbolKind.Interface, d.Name, Uri: facade.GetFilePath(d.GetFile())))
                    .Distinct()
            )
            .Concat(
                workerStore
                    .ModelStores.SelectMany(ms => ms.DataFlows)
                    .Select(d => (Kind: SymbolKind.Operator, d.Name, Uri: facade.GetFilePath(d.GetFile())))
                    .Distinct()
            )
            .Select(c =>
            {
                return new WorkspaceSymbol
                {
                    Kind = c.Kind,
                    Name = c.Name,
                    Location = new Location { Range = c.Name.GetLocation().ToRange()!, Uri = c.Uri },
                };
            })
            .Where(s => s.Name.ShouldMatch(request.Query))
            .ToList();
    }

    protected override WorkspaceSymbolRegistrationOptions CreateRegistrationOptions(
        WorkspaceSymbolCapability capability,
        ClientCapabilities clientCapabilities
    )
    {
        return new();
    }
}
