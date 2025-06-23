using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Utils;
using Range = OmniSharp.Extensions.LanguageServer.Protocol.Models.Range;

namespace TopModel.LanguageServer;

public class CodeActionHandler(ModelStore modelStore, ILanguageServerFacade facade, ModelFileCache modelFileCache, ModelConfig config) : CodeActionHandlerBase
{
    public override Task<CodeAction> Handle(CodeAction request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request);
    }

    public override Task<CommandOrCodeActionContainer?> Handle(CodeActionParams request, CancellationToken cancellationToken)
    {
        var modelFile = modelStore.Files.SingleOrDefault(f => facade.GetFilePath(f) == request.TextDocument.Uri.GetFileSystemPath());
        var codeActions = new List<CommandOrCodeAction>();
        if (modelFile != null)
        {
            if (modelFile.Uses.Except(modelFile.UselessImports).Any())
            {
                codeActions.Add(GetCodeActionOrganizeImports(request, modelFile));
            }

            foreach (var diagnostic in request.Context.Diagnostics.Where(d => !string.IsNullOrEmpty(d.Code)))
            {
                var modelErrorType = Enum.Parse<ModelErrorType>(diagnostic.Code!);
                switch (modelErrorType)
                {
                    case ModelErrorType.TMD1005:
                        codeActions.AddRange(GetCodeActionCreateDomain(request, diagnostic));
                        break;
                    case ModelErrorType.TMD1002:
                        codeActions.AddRange(GetCodeActionMissingClassImport(request, diagnostic, modelFile));
                        codeActions.AddRange(GetCodeActionAddClass(request, diagnostic, modelFile));
                        break;
                    case ModelErrorType.TMD1006:
                        codeActions.AddRange(GetCodeActionMissingEndpointImport(request, diagnostic, modelFile));
                        break;
                    case ModelErrorType.TMD1008:
                        codeActions.AddRange(GetCodeActionMissingDecoratorImport(request, diagnostic, modelFile));
                        break;
                    case ModelErrorType.TMD2000:
                        codeActions.AddRange(GetCodeActionMissingDataFlowImport(request, diagnostic, modelFile));
                        break;
                    default:
                        break;
                }
            }
        }

        return Task.FromResult<CommandOrCodeActionContainer?>(CommandOrCodeActionContainer.From(codeActions));
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
                        [request.TextDocument.Uri] =
                        [
                            new()
                            {
                                NewText = string.Join(
                                    "\n  - ",
                                    modelFile.Uses
                                        .Except(uselessImports)
                                        .DistinctBy(u => u.ReferenceName)
                                        .OrderBy(u => u.ReferenceName)
                                        .Select(u => u.ReferenceName)),
                                Range = new Range(start, end)
                            }
                        ]
                    }
            }
        };
    }

    protected override CodeActionRegistrationOptions CreateRegistrationOptions(CodeActionCapability capability, ClientCapabilities clientCapabilities)
    {
        return new()
        {
            DocumentSelector = config.GetDocumentSelector(),
            ResolveProvider = true,
            CodeActionKinds = new List<CodeActionKind>
            {
                CodeActionKind.SourceOrganizeImports,
                CodeActionKind.QuickFix
            }
        };
    }

    protected IEnumerable<CommandOrCodeAction> GetCodeActionAddClass(CodeActionParams request, Diagnostic diagnostic, ModelFile modelFile)
    {
        var text = modelFileCache.GetFile(request.TextDocument.Uri.GetFileSystemPath());
        var line = text.ElementAt(diagnostic.Range.Start.Line);
        var className = line[diagnostic.Range.Start.Character..Math.Min(diagnostic.Range.End.Character, line.Length)];
        return
        [
            new CodeAction
            {
                Title = $"TopModel : Créer la classe {className} dans ce fichier",
                Kind = CodeActionKind.QuickFix,
                IsPreferred = true,
                Diagnostics = new List<Diagnostic>
                {
                    diagnostic
                },
                Edit = new WorkspaceEdit
                {
                    Changes =
                        new Dictionary<DocumentUri, IEnumerable<TextEdit>>
                        {
                            [new Uri(facade.GetFilePath(modelFile))] =
                            [
                                new()
                                {
                                    NewText = @$"
---
class: 
  name: {className}
  comment: 
  properties:
    - 
",
                                    Range = new Range(text.Length, 0, text.Length, 0)
                                }
                            ]
                        }
                }
            }
        ];
    }

    protected IEnumerable<CommandOrCodeAction> GetCodeActionCreateDomain(CodeActionParams request, Diagnostic diagnostic)
    {
        var fs = request.TextDocument.Uri.GetFileSystemPath();
        var text = modelFileCache.GetFile(request.TextDocument.Uri.GetFileSystemPath());
        var line = text.ElementAt(diagnostic.Range.Start.Line);
        var domainName = line[diagnostic.Range.Start.Character..Math.Min(diagnostic.Range.End.Character, line.Length)];

        return modelStore.Files.Where(f => f.Domains.Count > 0).Select(f =>
        {
            var lastLine = File.ReadAllLines(facade.GetFilePath(f)).Length;
            return (CommandOrCodeAction)new CodeAction
            {
                Title = $"TopModel : Ajouter le domain au fichier {f.Path}",
                Kind = CodeActionKind.QuickFix,
                IsPreferred = true,
                Diagnostics = new List<Diagnostic>
                {
                    diagnostic
                },
                Edit = new WorkspaceEdit
                {
                    Changes =
                        new Dictionary<DocumentUri, IEnumerable<TextEdit>>
                        {
                            [new Uri(facade.GetFilePath(f))] =
                            [
                                new()
                                {
                                    Range = new Range(new Position(lastLine, 0), new Position(lastLine, 0)),
                                    NewText = $@"
---
domain:
  name: {domainName}
  label: 
"
                                }
                            ]
                        }
                }
            };
        }).ToList();
    }

    protected IEnumerable<CommandOrCodeAction> GetCodeActionMissingClassImport(CodeActionParams request, Diagnostic diagnostic, ModelFile modelFile)
    {
        var (className, useIndex) = GetImport(request, diagnostic, modelFile);
        return modelStore.Classes.Where(c => c.Name == className)
            .Select(classToImport => GetFileImportAction(diagnostic, modelFile, classToImport.ModelFile, useIndex));
    }

    protected IEnumerable<CommandOrCodeAction> GetCodeActionMissingDataFlowImport(CodeActionParams request, Diagnostic diagnostic, ModelFile modelFile)
    {
        var (dataFlowName, useIndex) = GetImport(request, diagnostic, modelFile);
        return modelStore.DataFlows.Where(c => c.Name == dataFlowName)
            .Select(decoratorToImport => GetFileImportAction(diagnostic, modelFile, decoratorToImport.ModelFile, useIndex));
    }

    protected IEnumerable<CommandOrCodeAction> GetCodeActionMissingDecoratorImport(CodeActionParams request, Diagnostic diagnostic, ModelFile modelFile)
    {
        var (decoratorName, useIndex) = GetImport(request, diagnostic, modelFile);
        return modelStore.Decorators.Where(c => c.Name == decoratorName)
            .Select(decoratorToImport => GetFileImportAction(diagnostic, modelFile, decoratorToImport.ModelFile, useIndex));
    }

    protected IEnumerable<CommandOrCodeAction> GetCodeActionMissingEndpointImport(CodeActionParams request, Diagnostic diagnostic, ModelFile modelFile)
    {
        var (endpointName, useIndex) = GetImport(request, diagnostic, modelFile);
        return modelStore.Endpoints.Where(c => c.Name == endpointName)
            .Select(endpointToImport => GetFileImportAction(diagnostic, modelFile, endpointToImport.ModelFile, useIndex));
    }

    private CommandOrCodeAction GetFileImportAction(Diagnostic diagnostic, ModelFile targetFile, ModelFile sourceFile, int useIndex)
    {
        return (CommandOrCodeAction)new CodeAction
        {
            Title = $"TopModel : Ajouter l'import {sourceFile.Name}",
            Kind = CodeActionKind.QuickFix,
            IsPreferred = true,
            Diagnostics = new List<Diagnostic> { diagnostic },
            Edit = new WorkspaceEdit
            {
                Changes =
                    new Dictionary<DocumentUri, IEnumerable<TextEdit>>
                    {
                        [new Uri(facade.GetFilePath(targetFile))] =
                        [
                            new()
                            {
                                NewText = targetFile.Uses.Count > 0 ? $"  - {sourceFile.Name}{Environment.NewLine}" : $"uses:{Environment.NewLine}  - {sourceFile.Name}{Environment.NewLine}",
                                Range = new Range(useIndex, 0, useIndex, 0)
                            }
                        ]
                    }
            }
        };
    }

    private (string ImportName, int UseIndex) GetImport(CodeActionParams request, Diagnostic diagnostic, ModelFile modelFile)
    {
        var text = modelFileCache.GetFile(request.TextDocument.Uri.GetFileSystemPath());
        var line = text.ElementAt(diagnostic.Range.Start.Line);
        var importName = line[diagnostic.Range.Start.Character..Math.Min(diagnostic.Range.End.Character, line.Length)];
        var useIndex = modelFile!.Uses.Count != 0
            ? modelFile.Uses.Last().ToRange()!.Start.Line + 1
            : text.First().StartsWith('-')
                ? 1
                : 0;

        return (importName, useIndex);
    }
}