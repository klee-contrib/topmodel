using MediatR;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server.Capabilities;
using TopModel.Core;

class TextDocumentSyncHandler : TextDocumentSyncHandlerBase
{
    private readonly ModelFileCache _fileCache;
    private readonly ModelStore _modelStore;
    private readonly ModelConfig _config;
    public TextDocumentSyncHandler(ModelStore modelStore, ModelFileCache fileCache, ModelConfig config)
    {
        _fileCache = fileCache;
        _modelStore = modelStore;
        _config = config;
    }

    public override TextDocumentAttributes GetTextDocumentAttributes(DocumentUri uri)
    {
        return new TextDocumentAttributes(uri, "yaml");
    }

    public override Task<Unit> Handle(DidOpenTextDocumentParams request, CancellationToken cancellationToken)
    {
        _fileCache.UpdateFile(request.TextDocument.Uri.GetFileSystemPath(), request.TextDocument.Text);
        _modelStore.TryApplyUpdates();
        return Unit.Task;
    }

    public override Task<Unit> Handle(DidChangeTextDocumentParams request, CancellationToken cancellationToken)
    {
        var filePath = request.TextDocument.Uri.GetFileSystemPath();
        var content = request.ContentChanges.Single().Text;
        _fileCache.UpdateFile(filePath, content);
        _modelStore.OnModelFileChange(filePath, content);
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
            DocumentSelector = _config.GetDocumentSelector(),
            Change = TextDocumentSyncKind.Full,
            Save = new SaveOptions { IncludeText = true }
        };
    }
}