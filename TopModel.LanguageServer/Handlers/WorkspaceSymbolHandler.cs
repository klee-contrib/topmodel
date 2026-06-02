using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using OmniSharp.Extensions.LanguageServer.Protocol.Workspace;
using TopModel.Core;
using TopModel.Core.Utils;

namespace TopModel.LanguageServer.Handlers;

public class WorkspaceSymbolHandler(LSWorkerStore workerStore, ILanguageServerFacade facade)
    : WorkspaceSymbolsHandlerBase
{
    private ModelStore? ModelStore => workerStore.ModelStore;

    /// <inheritdoc cref="MediatR.IRequestHandler{TRequest, TResponse}.Handle" />
    public override async Task<Container<WorkspaceSymbol>?> Handle(
        WorkspaceSymbolParams request,
        CancellationToken cancellationToken
    )
    {
        await ModelStore.WaitForUpdates(cancellationToken);

        return ModelStore
            .Classes.Select(c =>
            {
                return new WorkspaceSymbol
                {
                    Kind = SymbolKind.Class,
                    Name = c.Name,
                    Location = new Location
                    {
                        Range = c.Name.GetLocation().ToRange()!,
                        Uri = facade.GetFilePath(c.ModelFile),
                    },
                };
            })
            .Concat(
                ModelStore
                    .Files.Where(e => e.Endpoints.Count > 0)
                    .SelectMany(f => f.Endpoints)
                    .Select(e =>
                    {
                        return new WorkspaceSymbol
                        {
                            Kind = SymbolKind.Method,
                            Name = e.Name,
                            Location = new Location
                            {
                                Range = e.Name.GetLocation()!.ToRange()!,
                                Uri = facade.GetFilePath(e.ModelFile),
                            },
                        };
                    })
            )
            .Concat(
                ModelStore.Domains.Select(d =>
                {
                    return new WorkspaceSymbol
                    {
                        Kind = SymbolKind.Struct,
                        Name = d.Value.Name,
                        Location = new Location
                        {
                            Range = d.Value.GetLocation().ToRange()!,
                            Uri = facade.GetFilePath(d.Value.GetFile()),
                        },
                    };
                })
            )
            .Concat(
                ModelStore.Annotations.Select(d =>
                {
                    return new WorkspaceSymbol
                    {
                        Kind = SymbolKind.Interface,
                        Name = d.Name,
                        Location = new Location
                        {
                            Range = d.GetLocation().ToRange()!,
                            Uri = facade.GetFilePath(d.GetFile()),
                        },
                    };
                })
            )
            .Concat(
                ModelStore.Decorators.Select(d =>
                {
                    return new WorkspaceSymbol
                    {
                        Kind = SymbolKind.Interface,
                        Name = d.Name,
                        Location = new Location
                        {
                            Range = d.GetLocation().ToRange()!,
                            Uri = facade.GetFilePath(d.GetFile()),
                        },
                    };
                })
            )
            .Concat(
                ModelStore.DataFlows.Select(d =>
                {
                    return new WorkspaceSymbol
                    {
                        Kind = SymbolKind.Operator,
                        Name = d.Name,
                        Location = new Location
                        {
                            Range = d.GetLocation().ToRange()!,
                            Uri = facade.GetFilePath(d.GetFile()),
                        },
                    };
                })
            )
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
