using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Workspace;
using TopModel.Core.Utils;

namespace TopModel.LanguageServer;

public class WorkspaceSymbolHandler(ModelStoreRegistry registry) : WorkspaceSymbolsHandlerBase
{
    /// <inheritdoc cref="MediatR.IRequestHandler{TRequest, TResponse}.Handle" />
    public override async Task<Container<WorkspaceSymbol>?> Handle(
        WorkspaceSymbolParams request,
        CancellationToken cancellationToken
    )
    {
        await registry.WaitForAllUpdatesAsync(cancellationToken);

        return registry
            .All.Select(entry => entry.Store)
            .SelectMany(modelStore =>
                modelStore
                    .Classes.Select(c => new WorkspaceSymbol
                    {
                        Kind = SymbolKind.Class,
                        Name = c.Name,
                        Location = new Location
                        {
                            Range = c.Name.GetLocation().ToRange()!,
                            Uri = c.ModelFile.GetFilePath(),
                        },
                    })
                    .Concat(
                        modelStore
                            .Files.Where(e => e.Endpoints.Count > 0)
                            .SelectMany(f => f.Endpoints)
                            .Select(e => new WorkspaceSymbol
                            {
                                Kind = SymbolKind.Method,
                                Name = e.Name,
                                Location = new Location
                                {
                                    Range = e.Name.GetLocation()!.ToRange()!,
                                    Uri = e.ModelFile.GetFilePath(),
                                },
                            })
                    )
                    .Concat(
                        modelStore.Domains.Select(d => new WorkspaceSymbol
                        {
                            Kind = SymbolKind.Struct,
                            Name = d.Value.Name,
                            Location = new Location
                            {
                                Range = d.Value.GetLocation().ToRange()!,
                                Uri = d.Value.GetFile().GetFilePath(),
                            },
                        })
                    )
                    .Concat(
                        modelStore.Annotations.Select(d => new WorkspaceSymbol
                        {
                            Kind = SymbolKind.Interface,
                            Name = d.Name,
                            Location = new Location
                            {
                                Range = d.GetLocation().ToRange()!,
                                Uri = d.GetFile().GetFilePath(),
                            },
                        })
                    )
                    .Concat(
                        modelStore.Decorators.Select(d => new WorkspaceSymbol
                        {
                            Kind = SymbolKind.Interface,
                            Name = d.Name,
                            Location = new Location
                            {
                                Range = d.GetLocation().ToRange()!,
                                Uri = d.GetFile().GetFilePath(),
                            },
                        })
                    )
                    .Concat(
                        modelStore.DataFlows.Select(d => new WorkspaceSymbol
                        {
                            Kind = SymbolKind.Operator,
                            Name = d.Name,
                            Location = new Location
                            {
                                Range = d.GetLocation().ToRange()!,
                                Uri = d.GetFile().GetFilePath(),
                            },
                        })
                    )
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
