using MediatR;
using OmniSharp.Extensions.JsonRpc;

namespace TopModel.LanguageServer;

public class MermaidRequest : IJsonRpcRequest, IRequest<Mermaid>
{
    public MermaidRequest(string uri)
    {
        Uri = uri;
    }

    public string Uri { get; }
}