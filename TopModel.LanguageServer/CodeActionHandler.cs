using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;
using Range = OmniSharp.Extensions.LanguageServer.Protocol.Models.Range;

class CodeActionHandler : CodeActionHandlerBase
{
    private readonly ModelStore _modelStore;

    private readonly ILanguageServerFacade _facade;
    private readonly ModelFileCache _fileCache;

    public CodeActionHandler(ModelStore modelStore, ILanguageServerFacade facade, ModelFileCache fileCache)
    {
        _modelStore = modelStore;
        _fileCache = fileCache;
        _facade = facade;
    }

    public override Task<CodeAction> Handle(CodeAction request, CancellationToken cancellationToken)
    {

        return Task.FromResult<CodeAction>(request);
    }

    public override Task<CommandOrCodeActionContainer> Handle(CodeActionParams request, CancellationToken cancellationToken)
    {
        var codeActions = new List<CommandOrCodeAction>();
        var unsortedImportDiagnostics = request.Context.Diagnostics.Where(d => d.Code == ModelErrorType.UNSORTED_IMPORT.ToString() || d.Code == ModelErrorType.USESLESS_IMPORT.ToString());
        if (unsortedImportDiagnostics.Any())
        {
            codeActions.Add(getCodeActionSortImports(request, unsortedImportDiagnostics.First()));
        }

        return Task.FromResult<CommandOrCodeActionContainer>(CommandOrCodeActionContainer.From(codeActions));
    }
    protected CodeAction getCodeActionSortImports(CodeActionParams request, Diagnostic diagnostic)
    {
        var file = _modelStore.Files.SingleOrDefault(f => _facade.GetFilePath(f) == request.TextDocument.Uri.GetFileSystemPath());
        var start = file.Uses.First().ToRange()!.Start;
        var end = file.Uses.Last().ToRange()!.End;
        var uselessImports = file.UselessImports;
        return new CodeAction()
        {
            Title = "Trier les Uses",
            Kind = CodeActionKind.SourceOrganizeImports,
            IsPreferred = true,
            Diagnostics = new List<Diagnostic>(){
                    diagnostic
                },
            Edit = new WorkspaceEdit
            {
                Changes =
                    new Dictionary<DocumentUri, IEnumerable<TextEdit>>
                    {
                        [request.TextDocument.Uri] = new List<TextEdit>(){
                            new TextEdit()
                        {
                            NewText = string.Join("\n  - ", file.Uses.Except(uselessImports).OrderBy(u => u.ReferenceName).Select(u => u.ReferenceName)),
                            Range = new Range(start, end)
                        }
                    }
                    }
            }
        };
    }
    protected override CodeActionRegistrationOptions CreateRegistrationOptions(CodeActionCapability capability, ClientCapabilities clientCapabilities)
    {
        return new()
        {
            DocumentSelector = DocumentSelector.ForPattern("**/*.tmd"),
            ResolveProvider = true,
            CodeActionKinds = new List<CodeActionKind>(){
                CodeActionKind.SourceOrganizeImports
            },

        };
    }
}