using OmniSharp.Extensions.JsonRpc;
using MediatR;
namespace TopModel.LanguageServer;

public class MermaidRequest : IJsonRpcRequest, IRequest<Mermaid>
{
    public string Uri { get; init; }
    public MermaidRequest(string uri)
    {
        this.Uri = uri;
    }
}