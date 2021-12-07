using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;

class CompletionHandler : CompletionHandlerBase
{
    private readonly ModelStore _modelStore;
    private readonly ILanguageServerFacade _facade;
    private readonly ModelFileCache _fileCache;

    public CompletionHandler(ModelStore modelStore, ILanguageServerFacade facade, ModelFileCache fileCache)
    {
        _modelStore = modelStore;
        _facade = facade;
        _fileCache = fileCache;
    }

    public override Task<CompletionItem> Handle(CompletionItem request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request);
    }

    public override Task<CompletionList> Handle(CompletionParams request, CancellationToken cancellationToken)
    {
        var completionList = new CompletionList(isIncomplete: false);

        var text = _fileCache.GetFile(request.TextDocument.Uri.GetFileSystemPath());
        var currentLine = text.Split(Environment.NewLine).ElementAt(request.Position.Line);

        if (currentLine.Contains("domain: "))
        {
            var currentText = currentLine.Split(":")[1].Trim();
            return Task.FromResult(new CompletionList(
                _modelStore.Domains
                    .OrderBy(domain => domain.Key)
                    .Where(domain => domain.Key.ToLower().ShouldMatch(currentText))
                    .Select(domain => new CompletionItem
                    {
                        Kind = CompletionItemKind.EnumMember,
                        Label = domain.Key
                    })));
        }
        else if (currentLine.Contains("association: ") || currentLine.Contains("composition: ") || currentLine.Contains("    class:"))
        {
            var file = _modelStore.Files.SingleOrDefault(f => _facade.GetFilePath(f) == request.TextDocument.Uri.GetFileSystemPath());
            if (file != null)
            {
                var currentText = currentLine.Split(":")[1].Trim();
                return Task.FromResult(new CompletionList(
                    _modelStore.GetAvailableClasses(file)
                        .OrderBy(domain => domain.Name)
                        .Where(domain => domain.Name.ToLower().ShouldMatch(currentText))
                        .Select(domain => new CompletionItem
                        {
                            Kind = CompletionItemKind.Class,
                            Label = domain.Name
                        })));
            }
        }

        return Task.FromResult(new CompletionList());
    }

    protected override CompletionRegistrationOptions CreateRegistrationOptions(CompletionCapability capability, ClientCapabilities clientCapabilities)
    {
        return new CompletionRegistrationOptions
        {
            DocumentSelector = DocumentSelector.ForLanguage("yaml")
        };
    }
}