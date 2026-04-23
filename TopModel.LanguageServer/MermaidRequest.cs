using MediatR;
using OmniSharp.Extensions.JsonRpc;
using TopModel.Utils.Mermaid;

namespace TopModel.LanguageServer;

public class MermaidRequest(string uri, MermaidScope? scope) : IJsonRpcRequest, IRequest<Mermaid>
{
    public string Uri { get; } = uri;

    public MermaidScope Scope { get; } = scope ?? MermaidScope.File;
}
