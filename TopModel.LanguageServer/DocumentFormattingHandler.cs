using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;
using Range = OmniSharp.Extensions.LanguageServer.Protocol.Models.Range;

class DocumentFormattingHandler : DocumentFormattingHandlerBase
{
    private readonly ModelStore _modelStore;

    private readonly ILanguageServerFacade _facade;
    private readonly ModelFileCache _fileCache;

    public DocumentFormattingHandler(ModelStore modelStore, ILanguageServerFacade facade, ModelFileCache fileCache)
    {
        _modelStore = modelStore;
        _fileCache = fileCache;
        _facade = facade;
    }

    public override Task<TextEditContainer?> Handle(DocumentFormattingParams request, CancellationToken cancellationToken)
    {
        var filePath = request.TextDocument.Uri.GetFileSystemPath();
        var text = _fileCache.GetFile(filePath);
        var file = _modelStore.Files.SingleOrDefault(f => _facade.GetFilePath(f) == request.TextDocument.Uri.GetFileSystemPath());
        if (file == null || !file.Uses.Any())
        {
            return Task.FromResult<TextEditContainer?>(new TextEditContainer());
        }
        var start = file.Uses.First().ToRange()!.Start;
        var end = file.Uses.Last().ToRange()!.End;

        return Task.FromResult<TextEditContainer?>(new TextEditContainer(
            new TextEdit
            {
                NewText = string.Join("\n  - ", file.Uses.OrderBy(u => u.ReferenceName).Select(u => u.ReferenceName)),
                Range = new Range(start, end)
            }
        ));
    }

    protected override DocumentFormattingRegistrationOptions CreateRegistrationOptions(DocumentFormattingCapability capability, ClientCapabilities clientCapabilities)
    {
        return new DocumentFormattingRegistrationOptions
        {
            DocumentSelector = DocumentSelector.ForPattern("**/*.tmd")
        };
    }
}
