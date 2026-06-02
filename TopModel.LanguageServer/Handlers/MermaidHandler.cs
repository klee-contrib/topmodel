using MediatR;
using OmniSharp.Extensions.JsonRpc;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;
using TopModel.LanguageServer.Handlers.Mermaid;
using TopModel.Utils.Mermaid;

namespace TopModel.LanguageServer.Handlers;

public class MermaidHandler(LSWorkerStore workerStore, ILanguageServerFacade facade)
    : IRequestHandler<MermaidRequest, MermaidResponse>,
        IJsonRpcHandler
{
    private ModelStore? ModelStore => workerStore.ModelStore;

    public string GetFileName(string uri)
    {
        var file = ModelStore.Files.SingleOrDefault(f => facade.GetFilePath(f) == uri);
        if (file is null)
        {
            return string.Empty;
        }

        return file!.Name.Split("/")[^1];
    }

    public string GetModule(string uri)
    {
        var file = ModelStore.Files.SingleOrDefault(f => facade.GetFilePath(f) == uri);
        if (file is null)
        {
            return string.Empty;
        }

        return file!.Namespace.Module;
    }

    /// <inheritdoc cref="IRequestHandler{TRequest, TResponse}.Handle" />
    public async Task<MermaidResponse> Handle(MermaidRequest request, CancellationToken cancellationToken)
    {
        await ModelStore.WaitForUpdates(cancellationToken);
        var file = ModelStore.Files.SingleOrDefault(f => facade.GetFilePath(f) == request.Uri);
        var result = MermaidUtils.GetDiagram(ModelStore, file, request.Scope);
        return new MermaidResponse(result, GetModule(request.Uri), GetFileName(request.Uri), request.Scope);
    }
}
