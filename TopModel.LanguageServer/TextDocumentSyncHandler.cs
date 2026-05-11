using MediatR;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server.Capabilities;

namespace TopModel.LanguageServer;

public class TextDocumentSyncHandler(ModelStoreRegistry registry, ModelFileCache fileCache)
    : TextDocumentSyncHandlerBase
{
    /// <inheritdoc cref="ITextDocumentIdentifier.GetTextDocumentAttributes" />
    public override TextDocumentAttributes GetTextDocumentAttributes(DocumentUri uri)
    {
        return new TextDocumentAttributes(uri, "yaml");
    }

    public override Task<Unit> Handle(DidOpenTextDocumentParams request, CancellationToken cancellationToken)
    {
        fileCache.UpdateFile(request.TextDocument.Uri.GetFileSystemPath(), request.TextDocument.Text);
        return Unit.Task;
    }

    public override async Task<Unit> Handle(DidChangeTextDocumentParams request, CancellationToken cancellationToken)
    {
        var filePath = request.TextDocument.Uri.GetFileSystemPath();
        var content = request.ContentChanges.Single().Text;
        fileCache.UpdateFile(filePath, content);

        // Notifie tous les stores qui couvrent ce fichier (racines imbriquées ou identiques).
        foreach (var entry in registry.GetAllForFile(filePath))
        {
            await entry.Store.OnModelFileChange(filePath, content, cancellationToken);
        }

        return Unit.Value;
    }

    public override Task<Unit> Handle(DidSaveTextDocumentParams request, CancellationToken cancellationToken)
    {
        return Unit.Task;
    }

    public override Task<Unit> Handle(DidCloseTextDocumentParams request, CancellationToken cancellationToken)
    {
        return Unit.Task;
    }

    protected override TextDocumentSyncRegistrationOptions CreateRegistrationOptions(
        TextSynchronizationCapability capability,
        ClientCapabilities clientCapabilities
    )
    {
        return new TextDocumentSyncRegistrationOptions
        {
            DocumentSelector = registry.GetCombinedDocumentSelector(),
            Change = TextDocumentSyncKind.Full,
            Save = new SaveOptions { IncludeText = true },
        };
    }
}
