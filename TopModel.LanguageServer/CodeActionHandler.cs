﻿using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;
using TopModel.Core.FileModel;
using Range = OmniSharp.Extensions.LanguageServer.Protocol.Models.Range;

class CodeActionHandler : CodeActionHandlerBase
{
    private readonly ILanguageServerFacade _facade;
    private readonly ModelStore _modelStore;
    private readonly ModelFileCache _fileCache;
    public CodeActionHandler(ModelStore modelStore, ILanguageServerFacade facade, ModelFileCache modelFileCache)
    {
        _modelStore = modelStore;
        _facade = facade;
        _fileCache = modelFileCache;
    }

    public override Task<CodeAction> Handle(CodeAction request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request);
    }

    public override Task<CommandOrCodeActionContainer> Handle(CodeActionParams request, CancellationToken cancellationToken)
    {
        var modelFile = _modelStore.Files.SingleOrDefault(f => _facade.GetFilePath(f) == request.TextDocument.Uri.GetFileSystemPath())!;
        var codeActions = new List<CommandOrCodeAction>();
        if (modelFile.Uses.Any())
        {
            codeActions.Add(GetCodeActionOrganizeImports(request, modelFile));
        }
        foreach (var diagnostic in request.Context.Diagnostics)
        {
            var modelErrorType = GetTypeFromCode(diagnostic.Code!);
            switch (modelErrorType)
            {
                case ModelErrorType.TMD_1005:
                    codeActions.AddRange(GetCodeActionCreateDomain(request, diagnostic, modelFile));
                    break;
                default:
                    break;
            }
        }
        return Task.FromResult(CommandOrCodeActionContainer.From(codeActions));
    }

    protected static CodeAction GetCodeActionOrganizeImports(CodeActionParams request, ModelFile modelFile)
    {
        var start = modelFile.Uses.First().ToRange()!.Start;
        var end = modelFile.Uses.Last().ToRange()!.End;
        var uselessImports = modelFile.UselessImports;
        return new CodeAction()
        {
            Title = "Trier les Uses",
            Kind = CodeActionKind.SourceOrganizeImports,
            IsPreferred = true,
            Edit = new WorkspaceEdit
            {
                Changes =
                    new Dictionary<DocumentUri, IEnumerable<TextEdit>>
                    {
                        [request.TextDocument.Uri] = new List<TextEdit>(){
                            new TextEdit()
                        {
                            NewText = string.Join("\n  - ",
                            modelFile.Uses
                                .Except(uselessImports)
                                .DistinctBy(u => u.ReferenceName)
                                .OrderBy(u => u.ReferenceName)
                                .Select(u => u.ReferenceName)),
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
                CodeActionKind.SourceOrganizeImports,
                CodeActionKind.QuickFix
            },

        };
    }

    protected IEnumerable<CommandOrCodeAction> GetCodeActionCreateDomain(CodeActionParams request, Diagnostic diagnostic, ModelFile modelFile)
    {
        var file = _modelStore.Files.SingleOrDefault(f => _facade.GetFilePath(f) == request.TextDocument.Uri.GetFileSystemPath());

        var fs = request.TextDocument.Uri.GetFileSystemPath();
        var text = _fileCache.GetFile(request.TextDocument.Uri.GetFileSystemPath());
        var line = text.ElementAt(diagnostic.Range.Start.Line);
        var domainName = line.Substring(diagnostic.Range.Start.Character, diagnostic.Range.End.Character - diagnostic.Range.Start.Character);

        return _modelStore.Files.Where(f => f.Domains.Any()).Select(f =>
        {
            var lastLine = File.ReadAllLines(_facade.GetFilePath(f)).Count();
            return (CommandOrCodeAction)new CodeAction()
            {
                Title = $"TopModel : Ajouter le domain au fichier {f.Path}",
                Kind = CodeActionKind.QuickFix,
                IsPreferred = true,
                Diagnostics = new List<Diagnostic>{
                diagnostic
            },
                Edit = new WorkspaceEdit
                {
                    Changes =
                        new Dictionary<DocumentUri, IEnumerable<TextEdit>>
                        {
                            [new Uri(_facade.GetFilePath(f))] = new List<TextEdit>()
                            {
                                new TextEdit()
                                {
                                    Range = new Range(new Position(lastLine, 0), new Position(lastLine, 0)),
                                    NewText = $@"
---
domain:
  name: {domainName}
  label: 
"
                                }
                            }
                        }
                }
            };
        }).ToList();
    }

    protected ModelErrorType GetTypeFromCode(string code)
    {
        return Enum.Parse<ModelErrorType>("TMD_" + code);
    }
}