using MediatR;
using OmniSharp.Extensions.JsonRpc;

namespace TopModel.LanguageServer.Handlers.Schema;

public class SchemaRequest : IJsonRpcRequest, IRequest<SchemaResponse?>;

