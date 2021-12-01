using MediatR;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server.Capabilities;
using TopModel.Core;

class TextDocumentSyncHandler : TextDocumentSyncHandlerBase
{
    private readonly ModelStore _modelStore;

    public TextDocumentSyncHandler(ModelStore modelStore)
    {
        _modelStore = modelStore;
    }

    public override TextDocumentAttributes GetTextDocumentAttributes(DocumentUri uri)
    {
        return new TextDocumentAttributes(uri, "yaml");
    }

    public override Task<Unit> Handle(DidOpenTextDocumentParams request, CancellationToken cancellationToken)
    {
        _modelStore.TryApplyUpdates();
        return Unit.Task;
    }

    public override Task<Unit> Handle(DidChangeTextDocumentParams request, CancellationToken cancellationToken)
    {
        _modelStore.OnModelFileChange(request.TextDocument.Uri.GetFileSystemPath(), request.ContentChanges.Single().Text);
        return Unit.Task;
    }

    public override Task<Unit> Handle(DidSaveTextDocumentParams request, CancellationToken cancellationToken)
    {
        _modelStore.OnModelFileChange(request.TextDocument.Uri.GetFileSystemPath());
        return Unit.Task;
    }

    public override Task<Unit> Handle(DidCloseTextDocumentParams request, CancellationToken cancellationToken)
    {
        return Unit.Task;
    }

    protected override TextDocumentSyncRegistrationOptions CreateRegistrationOptions(SynchronizationCapability capability, ClientCapabilities clientCapabilities)
    {
        return new TextDocumentSyncRegistrationOptions
        {
            DocumentSelector = DocumentSelector.ForLanguage("yaml"),
            Change = TextDocumentSyncKind.Full,
            Save = new SaveOptions { IncludeText = true }
        };
    }
}