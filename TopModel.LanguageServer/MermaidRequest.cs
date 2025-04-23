using MediatR;
using OmniSharp.Extensions.JsonRpc;

namespace TopModel.LanguageServer;

public class MermaidRequest : IJsonRpcRequest, IRequest<Mermaid>
{
    public MermaidRequest(string uri, MermaidScope? scope)
    {
        Uri = uri;
        Scope = scope ?? MermaidScope.File;
    }

    public string Uri { get; }

    public MermaidScope Scope { get; }
}