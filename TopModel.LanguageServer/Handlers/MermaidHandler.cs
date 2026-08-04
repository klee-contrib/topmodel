using MediatR;
using OmniSharp.Extensions.JsonRpc;
using TopModel.LanguageServer.Handlers.Mermaid;
using TopModel.Utils.Mermaid;

namespace TopModel.LanguageServer.Handlers;

public class MermaidHandler(LSWorkerStore workerStore)
    : IRequestHandler<MermaidRequest, MermaidResponse?>,
        IJsonRpcHandler
{
    /// <inheritdoc cref="IRequestHandler{TRequest, TResponse}.Handle" />
    public async Task<MermaidResponse?> Handle(MermaidRequest request, CancellationToken cancellationToken)
    {
        await workerStore.WaitForUpdates(cancellationToken);

        // Un seul language server pouvant désormais servir plusieurs apps, on résout d'abord à
        // quelle app (et donc quel modèle) appartient le fichier demandé.
        var (file, store, app) = workerStore.GetFileWithApp(request.Uri);
        if (store == null)
        {
            return null;
        }

        var result = MermaidUtils.GetDiagram(store, file, request.Scope);
        return new MermaidResponse(
            result,
            file?.Namespace.Module ?? string.Empty,
            file?.Name.Split("/")[^1] ?? string.Empty,
            app ?? string.Empty,
            request.Scope
        );
    }
}
