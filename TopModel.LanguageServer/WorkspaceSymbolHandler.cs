using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using OmniSharp.Extensions.LanguageServer.Protocol.Workspace;
using TopModel.Core;

namespace TopModel.LanguageServer;

public class WorkspaceSymbolHandler(ModelStore modelStore, ILanguageServerFacade facade) : WorkspaceSymbolsHandlerBase
{
    public override async Task<Container<WorkspaceSymbol>?> Handle(WorkspaceSymbolParams request, CancellationToken cancellationToken)
    {
        await modelStore.WaitForUpdates();

        return modelStore.Classes.Select(c =>
        {
            return new WorkspaceSymbol
            {
                Kind = SymbolKind.Class,
                Name = c.Name,
                Location = new Location
                {
                    Range = c.Name.GetLocation().ToRange()!,
                    Uri = facade.GetFilePath(c.ModelFile)
                }
            };
        }).Concat(modelStore.Files.Where(e => e.Endpoints.Count > 0).SelectMany(f => f.Endpoints).Select(e =>
        {
            return new WorkspaceSymbol
            {
                Kind = SymbolKind.Method,
                Name = e.Name,
                Location = new Location
                {
                    Range = e.Name.GetLocation()!.ToRange()!,
                    Uri = facade.GetFilePath(e.ModelFile)
                }
            };
        })).Concat(modelStore.Domains.Select(d =>
        {
            return new WorkspaceSymbol
            {
                Kind = SymbolKind.Struct,
                Name = d.Value.Name,
                Location = new Location
                {
                    Range = d.Value.GetLocation().ToRange()!,
                    Uri = facade.GetFilePath(d.Value.GetFile())
                }
            };
        })).Concat(modelStore.Decorators.Select(d =>
        {
            return new WorkspaceSymbol
            {
                Kind = SymbolKind.Interface,
                Name = d.Name,
                Location = new Location
                {
                    Range = d.GetLocation().ToRange()!,
                    Uri = facade.GetFilePath(d.GetFile())
                }
            };
        })).Concat(modelStore.DataFlows.Select(d =>
        {
            return new WorkspaceSymbol
            {
                Kind = SymbolKind.Operator,
                Name = d.Name,
                Location = new Location
                {
                    Range = d.GetLocation().ToRange()!,
                    Uri = facade.GetFilePath(d.GetFile())
                }
            };
        })).Where(s => s.Name.ShouldMatch(request.Query)).ToList();
    }

    protected override WorkspaceSymbolRegistrationOptions CreateRegistrationOptions(WorkspaceSymbolCapability capability, ClientCapabilities clientCapabilities)
    {
        return new();
    }
}