using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using TopModel.Core.FileModel;

namespace TopModel.LanguageServer;

public class RenameHandler(ModelStoreRegistry registry) : RenameHandlerBase
{
    /// <inheritdoc cref="MediatR.IRequestHandler{TRequest, TResponse}.Handle" />
    public override async Task<WorkspaceEdit?> Handle(RenameParams request, CancellationToken cancellationToken)
    {
        var filePath = request.TextDocument.Uri.GetFileSystemPath();
        var entry = registry.GetPrimaryForFile(filePath);
        if (entry == null)
        {
            return null;
        }

        var modelStore = entry.Store;
        await modelStore.WaitForUpdates(cancellationToken);

        var file = modelStore.Files.SingleOrDefault(f => f.GetFilePath() == filePath);
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
                            Uri = new Uri(r.File.GetFilePath()),
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
        return new RenameRegistrationOptions { DocumentSelector = registry.GetCombinedDocumentSelector() };
    }
}
