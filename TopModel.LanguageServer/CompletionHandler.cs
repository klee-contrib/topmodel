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
        var currentLine = text.ElementAt(request.Position.Line);

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
                var availableClasses = new HashSet<Class>(_modelStore.GetAvailableClasses(file));

                var useIndex = file.Uses.Any()
                    ? file.Uses.Last().ToRange()!.Start.Line + 1
                    : text.First().StartsWith("-")
                        ? 1
                        : 0;

                return Task.FromResult(new CompletionList(
                    _modelStore.Classes
                        .Where(classe => classe.Name.ToLower().ShouldMatch(currentText))
                        .Select(classe => new CompletionItem
                        {
                            Kind = CompletionItemKind.Class,
                            Label = availableClasses.Contains(classe) ? classe.Name : $"{classe.Name} - ({classe.ModelFile.Name})",
                            InsertText = classe.Name,
                            SortText = availableClasses.Contains(classe) ? "0000" + classe.Name : classe.Name,
                            AdditionalTextEdits = !availableClasses.Contains(classe) ?
                                new TextEditContainer(new TextEdit
                                {
                                    NewText = file.Uses.Any() ? $"  - {classe.ModelFile.Name}{Environment.NewLine}" : $"uses:{Environment.NewLine}  - {classe.ModelFile.Name}{Environment.NewLine}",
                                    Range = new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range(useIndex, 0, useIndex, 0)
                                })
                                : null
                        }))); ;
            }
        }
        else if (currentLine.TrimStart().StartsWith("-"))
        {
            var currentText = currentLine.TrimStart()[1..].Trim();

            var file = _modelStore.Files.SingleOrDefault(f => _facade.GetFilePath(f) == request.TextDocument.Uri.GetFileSystemPath());
            if (file != null)
            {
                // Use
                if (
                    text.ElementAt(request.Position.Line - 1) == "uses:"
                    || file.Uses.Select(u => u.Start.Line).Any(l => l == request.Position.Line))
                {
                    return Task.FromResult(new CompletionList(
                        _modelStore.Files.Select(f => f.Name)
                            .Except(file.Uses.Select(u => u.ReferenceName))
                            .Where(name => name != file.Name && name.ToLower().ShouldMatch(currentText))
                            .Select(name => new CompletionItem
                            {
                                Kind = CompletionItemKind.File,
                                Label = name
                            })));
                }
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