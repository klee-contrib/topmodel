using MediatR;
using OmniSharp.Extensions.JsonRpc;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;
using TopModel.Utils.Mermaid;

namespace TopModel.LanguageServer;

public class MermaidHandler(ModelStore modelStore, ILanguageServerFacade facade)
    : IRequestHandler<MermaidRequest, Mermaid>,
        IJsonRpcHandler
{
    public string GetFileName(string uri)
    {
        var file = modelStore.Files.SingleOrDefault(f => facade.GetFilePath(f) == uri);
        if (file is null)
        {
            return string.Empty;
        }

        return file!.Name.Split("/")[^1];
    }

    public string GetModule(string uri)
    {
        var file = modelStore.Files.SingleOrDefault(f => facade.GetFilePath(f) == uri);
        if (file is null)
        {
            return string.Empty;
        }

        return file!.Namespace.Module;
    }

    /// <inheritdoc cref="IRequestHandler{TRequest, TResponse}.Handle" />
    public async Task<Mermaid> Handle(MermaidRequest request, CancellationToken cancellationToken)
    {
        await modelStore.WaitForUpdates(cancellationToken);
        var file = modelStore.Files.SingleOrDefault(f => facade.GetFilePath(f) == request.Uri);
        var result = MermaidUtils.GetDiagram(modelStore, file, request.Scope);
        return new Mermaid(result, GetModule(request.Uri), GetFileName(request.Uri), request.Scope);
    }
}
