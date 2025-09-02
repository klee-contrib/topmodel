using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;
using TopModel.Core.FileModel;

namespace TopModel.LanguageServer;

public class RenameHandler(ModelStore modelStore, ILanguageServerFacade facade, ModelConfig config) : RenameHandlerBase
{
    /// <inheritdoc cref="MediatR.IRequestHandler{TRequest, TResponse}.Handle" />
    public override async Task<WorkspaceEdit?> Handle(RenameParams request, CancellationToken cancellationToken)
    {
        await modelStore.WaitForUpdates(cancellationToken);

        var file = modelStore.Files.SingleOrDefault(f =>
            facade.GetFilePath(f) == request.TextDocument.Uri.GetFileSystemPath()
        );
        if (file != null)
        {
            var references = modelStore.GetReferencesForPositionInFile(request.Position, file, includeTransitive: true);
            if (
                references != null
                && references.All(r =>
                    r.Reference.ReferenceName == references.Objet.GetName()
                    || r.Reference is ClassReference
                    || r.Reference is EndpointReference
                )
            )
            {
                return new WorkspaceEdit
                {
                    Changes = references
                        .Where(r => r.Reference.ReferenceName == references.Objet.GetName())
                        .Select(r => new Location
                        {
                            Uri = new Uri(facade.GetFilePath(r.File)),
                            Range = r.Reference.ToRange()!,
                        })
                        .Select(c => new
                        {
                            c.Uri,
                            TextEdit = new TextEdit { NewText = request.NewName, Range = c.Range },
                        })
                        .GroupBy(t => t.Uri, t => t)
                        .ToDictionary(x => x.Key, x => x.Select(y => y.TextEdit)),
                };
            }
        }

        return null;
    }

    protected override RenameRegistrationOptions CreateRegistrationOptions(
        RenameCapability capability,
        ClientCapabilities clientCapabilities
    )
    {
        return new RenameRegistrationOptions { DocumentSelector = config.GetDocumentSelector() };
    }
}
