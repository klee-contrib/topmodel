using MediatR;
using OmniSharp.Extensions.JsonRpc;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Utils.Mermaid;

namespace TopModel.LanguageServer;

public class MermaidHandler(ModelStoreRegistry registry, ILanguageServerFacade facade)
    : IRequestHandler<MermaidRequest, Mermaid>,
        IJsonRpcHandler
{
    public string GetFileName(string uri)
    {
        var entry = FindEntryForUri(uri);
        var file = entry?.Store.Files.SingleOrDefault(f => facade.GetFilePath(f) == uri);
        return file?.Name.Split("/")[^1] ?? string.Empty;
    }

    public string GetModule(string uri)
    {
        var entry = FindEntryForUri(uri);
        var file = entry?.Store.Files.SingleOrDefault(f => facade.GetFilePath(f) == uri);
        return file?.Namespace.Module ?? string.Empty;
    }

    public string GettApp(string uri)
    {
        var entry = FindEntryForUri(uri);
        return entry?.Config.App ?? string.Empty;
    }

    /// <inheritdoc cref="IRequestHandler{TRequest, TResponse}.Handle" />
    public async Task<Mermaid> Handle(MermaidRequest request, CancellationToken cancellationToken)
    {
        await registry.WaitForAllUpdatesAsync(cancellationToken);

        var entry = FindEntryForUri(request.Uri);
        var modelStore = entry?.Store;
        var file = modelStore?.Files.SingleOrDefault(f => facade.GetFilePath(f) == request.Uri);
        var result = MermaidUtils.GetDiagram(modelStore!, file, request.Scope);
        return new Mermaid(
            result,
            GettApp(request.Uri),
            GetModule(request.Uri),
            GetFileName(request.Uri),
            request.Scope
        );
    }

    private ModelStoreEntry? FindEntryForUri(string uri)
    {
        foreach (var entry in registry.All)
        {
            if (entry.Store.Files.Any(f => facade.GetFilePath(f) == uri))
            {
                return entry;
            }
        }

        return null;
    }
}
