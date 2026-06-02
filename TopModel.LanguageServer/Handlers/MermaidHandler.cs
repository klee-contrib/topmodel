using MediatR;
using OmniSharp.Extensions.JsonRpc;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;
using TopModel.LanguageServer.Handlers.Mermaid;
using TopModel.Utils.Mermaid;

namespace TopModel.LanguageServer.Handlers;

public class MermaidHandler(LSWorkerStore workerStore, ILanguageServerFacade facade)
    : IRequestHandler<MermaidRequest, MermaidResponse?>,
        IJsonRpcHandler
{
    private ModelStore? ModelStore => workerStore.ModelStore;

    /// <inheritdoc cref="IRequestHandler{TRequest, TResponse}.Handle" />
    public async Task<MermaidResponse?> Handle(MermaidRequest request, CancellationToken cancellationToken)
    {
        if (ModelStore == null)
        {
            return null;
        }

        await workerStore.WaitForUpdates(cancellationToken);
        var file = ModelStore.Files.SingleOrDefault(f => facade.GetFilePath(f) == request.Uri);
        var result = MermaidUtils.GetDiagram(ModelStore, file, request.Scope);
        return new MermaidResponse(
            result,
            file?.Namespace.Module ?? string.Empty,
            file?.Name.Split("/")[^1] ?? string.Empty,
            request.Scope
        );
    }
}
