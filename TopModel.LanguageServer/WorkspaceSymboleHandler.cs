using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using OmniSharp.Extensions.LanguageServer.Protocol.Workspace;
using TopModel.Core;

class WorkspaceSymbolHandler : WorkspaceSymbolsHandlerBase
{
    private readonly ModelStore _modelStore;
    private readonly ILanguageServerFacade _facade;

    public WorkspaceSymbolHandler(ModelStore modelStore, ILanguageServerFacade facade)
    {
        _modelStore = modelStore;
        _facade = facade;
    }

    public override Task<Container<SymbolInformation>?> Handle(WorkspaceSymbolParams request, CancellationToken cancellationToken)
    {
        return Task.FromResult<Container<SymbolInformation>?>(_modelStore.Classes.Select(c =>
        {
            return new SymbolInformation
            {
                Deprecated = false,
                Kind = SymbolKind.Class,
                Name = c.Name,
                Location = new Location
                {
                    Range = c.GetLocation().ToRange()!,
                    Uri = _facade.GetFilePath(c.ModelFile)
                }
            };
        }).Concat(_modelStore.Files.Where(e => e.Endpoints.Count > 0).SelectMany(f => f.Endpoints).Select(e =>
        {
            return new SymbolInformation
            {
                Deprecated = false,
                Kind = SymbolKind.Method,
                Name = e.Name,
                Location = new Location
                {
                    Range = e.Name.GetLocation()!.ToRange()!,
                    Uri = _facade.GetFilePath(e.ModelFile)
                }
            };
        })).Concat(_modelStore.Domains.Select(d =>
        {
            return new SymbolInformation
            {
                Deprecated = false,
                Kind = SymbolKind.Struct,
                Name = d.Value.Name,
                Location = new Location
                {
                    Range = d.Value.GetLocation().ToRange()!,
                    Uri = _facade.GetFilePath(d.Value.GetFile())
                }
            };
        })).Where(s => s.Name.ShouldMatch(request.Query)).ToList());
    }

    protected override WorkspaceSymbolRegistrationOptions CreateRegistrationOptions(WorkspaceSymbolCapability capability, ClientCapabilities clientCapabilities)
    {
        return new WorkspaceSymbolRegistrationOptions();
    }
}