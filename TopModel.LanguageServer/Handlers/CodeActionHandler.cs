using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Core.Utils;
using TopModel.Utils;
using Range = OmniSharp.Extensions.LanguageServer.Protocol.Models.Range;

namespace TopModel.LanguageServer.Handlers;

public class CodeActionHandler(LSWorkerStore workerStore, ILanguageServerFacade facade, ModelFileCache modelFileCache)
    : CodeActionHandlerBase
{
    public override Task<CodeAction> Handle(CodeAction request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request);
    }

    public override async Task<CommandOrCodeActionContainer?> Handle(
        CodeActionParams request,
        CancellationToken cancellationToken
    )
    {
        await workerStore.WaitForUpdates(cancellationToken);

        var files = workerStore.GetFiles(request.TextDocument);

        var codeActions = new List<CommandOrCodeAction>();
        if (files.Any())
        {
            var (firstFile, firstStore) = files.First();

            var uselessImports = firstStore.GetUselessImports(firstFile).ToList();
            if (firstFile.Uses.Any() || uselessImports.Any())
            {
                codeActions.Add(GetCodeActionOrganizeImports(request, firstFile, uselessImports));
            }

            foreach (var diagnostic in request.Context.Diagnostics.Where(d => d.Code?.IsString == true))
            {
                if (!Enum.TryParse<ErrorType>(diagnostic.Code!.Value.String, out var modelErrorType))
                {
                    continue;
                }

                if (diagnostic.Severity == DiagnosticSeverity.Warning)
                {
                    codeActions.Add(GetCodeActionIgnoreWarning(request, diagnostic, firstFile));
                }

                switch (modelErrorType)
                {
                    case ErrorType.TMD0002:
                        codeActions.AddRange(GetCodeActionMissingClassImport(request, diagnostic, files));
                        codeActions.AddRange(GetCodeActionAddClass(request, diagnostic, firstFile));
                        break;
                    case ErrorType.TMD0003:
                        codeActions.AddRange(GetCodeActionCreateDomain(request, diagnostic, files));
                        break;
                    case ErrorType.TMD0005:
                        codeActions.AddRange(GetCodeActionMissingDecoratorImport(request, diagnostic, files));
                        break;
                    case ErrorType.TMD0006:
                        codeActions.AddRange(GetCodeActionMissingEndpointImport(request, diagnostic, files));
                        break;
                    case ErrorType.TMD2001:
                        codeActions.AddRange(GetCodeActionMissingAnnotationImport(request, diagnostic, files));
                        codeActions.AddRange(GetCodeActionAddAnnotation(request, diagnostic, firstFile));
                        break;
                    case ErrorType.TMD4002:
                        codeActions.AddRange(GetCodeActionMissingDataFlowImport(request, diagnostic, files));
                        break;
                    case ErrorType.TMD9008:
                        codeActions.AddRange(GetCodeActionMissingWithReverseImport(diagnostic, files));
                        break;
                    default:
                        break;
                }
            }
        }

        return CommandOrCodeActionContainer.From(codeActions);
    }

    protected override CodeActionRegistrationOptions CreateRegistrationOptions(
        CodeActionCapability capability,
        ClientCapabilities clientCapabilities
    )
    {
        return new()
        {
            DocumentSelector = workerStore.TmdFiles,
            ResolveProvider = true,
            CodeActionKinds = new List<CodeActionKind>
            {
                CodeActionKind.SourceOrganizeImports,
                CodeActionKind.QuickFix,
            },
        };
    }

    protected IEnumerable<CommandOrCodeAction> GetCodeActionAddAnnotation(
        CodeActionParams request,
        Diagnostic diagnostic,
        ModelFile firstFile
    )
    {
        var text = modelFileCache.GetFile(request.TextDocument.Uri.GetFileSystemPath());
        var line = text[diagnostic.Range.Start.Line];
        var annotationName = line[
            diagnostic.Range.Start.Character..Math.Min(diagnostic.Range.End.Character, line.Length)
        ];
        return
        [
            new CodeAction
            {
                Title = $"TopModel : Créer l'annotation {annotationName} dans ce fichier",
                Kind = CodeActionKind.QuickFix,
                IsPreferred = true,
                Diagnostics = new List<Diagnostic> { diagnostic },
                Edit = new WorkspaceEdit
                {
                    Changes = new Dictionary<DocumentUri, IEnumerable<TextEdit>>
                    {
                        [new Uri(facade.GetFilePath(firstFile))] =
                        [
                            new()
                            {
                                NewText =
                                    @$"
---
annotation: 
  name: {annotationName}
  description: 
  target:
    - 
",
                                Range = new Range(text.Length, 0, text.Length, 0),
                            },
                        ],
                    },
                },
            },
        ];
    }

    protected IEnumerable<CommandOrCodeAction> GetCodeActionAddClass(
        CodeActionParams request,
        Diagnostic diagnostic,
        ModelFile firstFile
    )
    {
        var text = modelFileCache.GetFile(request.TextDocument.Uri.GetFileSystemPath());
        var line = text[diagnostic.Range.Start.Line];
        var className = line[diagnostic.Range.Start.Character..Math.Min(diagnostic.Range.End.Character, line.Length)];
        return
        [
            new CodeAction
            {
                Title = $"TopModel : Créer la classe {className} dans ce fichier",
                Kind = CodeActionKind.QuickFix,
                IsPreferred = true,
                Diagnostics = new List<Diagnostic> { diagnostic },
                Edit = new WorkspaceEdit
                {
                    Changes = new Dictionary<DocumentUri, IEnumerable<TextEdit>>
                    {
                        [new Uri(facade.GetFilePath(firstFile))] =
                        [
                            new()
                            {
                                NewText =
                                    @$"
---
class: 
  name: {className}
  comment: 
  properties:
    - 
",
                                Range = new Range(text.Length, 0, text.Length, 0),
                            },
                        ],
                    },
                },
            },
        ];
    }

    protected IEnumerable<CommandOrCodeAction> GetCodeActionCreateDomain(
        CodeActionParams request,
        Diagnostic diagnostic,
        IEnumerable<(ModelFile File, ModelStore Store)> files
    )
    {
        var text = modelFileCache.GetFile(request.TextDocument.Uri.GetFileSystemPath());
        var line = text[diagnostic.Range.Start.Line];
        var domainName = line[diagnostic.Range.Start.Character..Math.Min(diagnostic.Range.End.Character, line.Length)];

        var stores = files.Select(f => f.Store);
        return stores
            .SelectMany(store => store.Files.Where(f => f.Domains.Count > 0).Select(file => (store, file)))
            .GroupBy(f => (f.file.Name, f.file.Path))
            .Where(g => g.Count() == stores.Count())
            .Select(g => g.First().file)
            .Select(f =>
            {
                var lastLine = modelFileCache.GetFile(facade.GetFilePath(f)).Length;
                return (CommandOrCodeAction)
                    new CodeAction
                    {
                        Title = $"TopModel : Ajouter le domain au fichier {f.Path}",
                        Kind = CodeActionKind.QuickFix,
                        IsPreferred = true,
                        Diagnostics = new List<Diagnostic> { diagnostic },
                        Edit = new WorkspaceEdit
                        {
                            Changes = new Dictionary<DocumentUri, IEnumerable<TextEdit>>
                            {
                                [new Uri(facade.GetFilePath(f))] =
                                [
                                    new()
                                    {
                                        Range = new Range(new Position(lastLine, 0), new Position(lastLine, 0)),
                                        NewText =
                                            $@"
---
domain:
  name: {domainName}
  label: 
",
                                    },
                                ],
                            },
                        },
                    };
            })
            .ToList();
    }

    protected CommandOrCodeAction GetCodeActionIgnoreWarning(
        CodeActionParams request,
        Diagnostic diagnostic,
        ModelFile firstFile
    )
    {
        var fileText = modelFileCache.GetFile(request.TextDocument.Uri.GetFileSystemPath());
        var line = fileText[diagnostic.Range.Start.Line];

        var warningCode = diagnostic.Code!.Value.String!;

        var ignoreComment = $"# ignore {warningCode}";

        if (!line.Contains('#'))
        {
            line += $" {ignoreComment}";
        }
        else
        {
            var startIndex = line.IndexOf('#');
            if (line[startIndex..].StartsWith("# ignore"))
            {
                line = $"{line[0..startIndex]}{ignoreComment}{line[(startIndex + 8)..]}";
            }
            else
            {
                line = $"{line[0..startIndex]}{ignoreComment}{line[(startIndex + 1)..]}";
            }
        }

        return (CommandOrCodeAction)
            new CodeAction
            {
                Title = $"TopModel : Ignorer cette instance du warning {warningCode}",
                Kind = CodeActionKind.QuickFix,
                IsPreferred = false,
                Diagnostics = new List<Diagnostic> { diagnostic },
                Edit = new WorkspaceEdit
                {
                    Changes = new Dictionary<DocumentUri, IEnumerable<TextEdit>>
                    {
                        [new Uri(facade.GetFilePath(firstFile))] =
                        [
                            new()
                            {
                                Range = new Range(
                                    new Position(diagnostic.Range.Start.Line, 0),
                                    new Position(diagnostic.Range.Start.Line, line.Length)
                                ),
                                NewText = line,
                            },
                        ],
                    },
                },
            };
    }

    protected IEnumerable<CommandOrCodeAction> GetCodeActionMissingAnnotationImport(
        CodeActionParams request,
        Diagnostic diagnostic,
        IEnumerable<(ModelFile File, ModelStore Store)> files
    )
    {
        var firstFile = files.First().File;
        var (annotationName, useIndex) = GetImport(request, diagnostic, firstFile);

        return files
            .GetInAll(f => f.Store.Annotations.Where(c => c.Name == annotationName), f => (f.Name, f.ModelFile.Path))
            .Select(annotation => GetFileImportAction(diagnostic, firstFile, annotation.ModelFile, useIndex));
    }

    protected IEnumerable<CommandOrCodeAction> GetCodeActionMissingClassImport(
        CodeActionParams request,
        Diagnostic diagnostic,
        IEnumerable<(ModelFile File, ModelStore Store)> files
    )
    {
        var firstFile = files.First().File;
        var (className, useIndex) = GetImport(request, diagnostic, firstFile);
        return files
            .GetInAll(f => f.Store.Classes.Where(c => c.Name == className), f => (f.Name, f.ModelFile.Path))
            .Select(classe => GetFileImportAction(diagnostic, firstFile, classe.ModelFile, useIndex));
    }

    protected IEnumerable<CommandOrCodeAction> GetCodeActionMissingDataFlowImport(
        CodeActionParams request,
        Diagnostic diagnostic,
        IEnumerable<(ModelFile File, ModelStore Store)> files
    )
    {
        var firstFile = files.First().File;
        var (dataFlowName, useIndex) = GetImport(request, diagnostic, firstFile);
        return files
            .GetInAll(f => f.Store.DataFlows.Where(c => c.Name == dataFlowName), f => (f.Name, f.ModelFile.Path))
            .Select(dataFlow => GetFileImportAction(diagnostic, firstFile, dataFlow.ModelFile, useIndex));
    }

    protected IEnumerable<CommandOrCodeAction> GetCodeActionMissingDecoratorImport(
        CodeActionParams request,
        Diagnostic diagnostic,
        IEnumerable<(ModelFile File, ModelStore Store)> files
    )
    {
        var firstFile = files.First().File;
        var (decoratorName, useIndex) = GetImport(request, diagnostic, firstFile);
        return files
            .GetInAll(f => f.Store.Decorators.Where(c => c.Name == decoratorName), f => (f.Name, f.ModelFile.Path))
            .Select(decorator => GetFileImportAction(diagnostic, firstFile, decorator.ModelFile, useIndex));
    }

    protected IEnumerable<CommandOrCodeAction> GetCodeActionMissingEndpointImport(
        CodeActionParams request,
        Diagnostic diagnostic,
        IEnumerable<(ModelFile File, ModelStore Store)> files
    )
    {
        var firstFile = files.First().File;
        var (endpointName, useIndex) = GetImport(request, diagnostic, firstFile);
        return files
            .GetInAll(f => f.Store.Endpoints.Where(c => c.Name == endpointName), f => (f.Name, f.ModelFile.Path))
            .Select(endpoint => GetFileImportAction(diagnostic, firstFile, endpoint.ModelFile, useIndex));
    }

    protected IEnumerable<CommandOrCodeAction> GetCodeActionMissingWithReverseImport(
        Diagnostic diagnostic,
        IEnumerable<(ModelFile File, ModelStore Store)> files
    )
    {
        var firstFile = files.First().File;

        var objets = files.Select(f => f.File.GetObjetAtPosition(diagnostic.Range.Start).Objet);

        if (objets.Any(o => o is not Class))
        {
            return [];
        }

        var objet = objets.First()!;
        var targetFile = objet.GetFile();
        var fileText = modelFileCache.GetFile(facade.GetFilePath(targetFile));

        var (className, useIndex) = GetImport(objet.GetName()!, fileText, targetFile);
        return files
            .GetInAll(f => f.Store.Classes.Where(c => c.Name == className), f => (f.Name, f.ModelFile.Path))
            .Select(targetClass =>
                GetFileImportAction(diagnostic, targetClass.ModelFile, firstFile, useIndex, reverse: true)
            );
    }

    protected CodeAction GetCodeActionOrganizeImports(
        CodeActionParams request,
        ModelFile firstFile,
        IList<Reference> uselessImports
    )
    {
        var uses = firstFile.Uses.Except(uselessImports);
        var start = firstFile.Uses[0].ToRange()!.Start;
        var end = firstFile.Uses[^1].ToRange()!.End;
        if (!uses.Any())
        {
            var fileText = modelFileCache.GetFile(request.TextDocument.Uri.GetFileSystemPath()).ToList();
            start = new Position(fileText.FindIndex(line => line.StartsWith("uses")), 0);
            end.Line++;
            end.Character = 0;
            return new CodeAction()
            {
                Title = "Trier les Uses",
                Kind = CodeActionKind.SourceOrganizeImports,
                IsPreferred = true,
                Edit = new WorkspaceEdit
                {
                    Changes = new Dictionary<DocumentUri, IEnumerable<TextEdit>>
                    {
                        [request.TextDocument.Uri] = [new() { NewText = string.Empty, Range = new Range(start, end) }],
                    },
                },
            };
        }
        return new CodeAction()
        {
            Title = "Trier les Uses",
            Kind = CodeActionKind.SourceOrganizeImports,
            IsPreferred = true,
            Edit = new WorkspaceEdit
            {
                Changes = new Dictionary<DocumentUri, IEnumerable<TextEdit>>
                {
                    [request.TextDocument.Uri] =
                    [
                        new()
                        {
                            NewText = string.Join(
                                "\n  - ",
                                uses.DistinctBy(u => u.ReferenceName)
                                    .OrderBy(u => u.ReferenceName)
                                    .Select(u => u.ReferenceName)
                            ),
                            Range = new Range(start, end),
                        },
                    ],
                },
            },
        };
    }

    private static (string ImportName, int UseIndex) GetImport(
        string importName,
        string[] fileText,
        ModelFile modelFile
    )
    {
        var useIndex =
            modelFile!.Uses.Count != 0 ? modelFile.Uses[^1].ToRange()!.Start.Line + 1
            : fileText[0].StartsWith('-') ? 1
            : 0;

        return (importName, useIndex);
    }

    private CommandOrCodeAction GetFileImportAction(
        Diagnostic diagnostic,
        ModelFile targetFile,
        ModelFile sourceFile,
        int useIndex,
        bool reverse = false
    )
    {
        return (CommandOrCodeAction)
            new CodeAction
            {
                Title =
                    $"TopModel : Ajouter l'import {sourceFile.Name}{(reverse ? $" dans le fichier {targetFile.Name}" : string.Empty)}",
                Kind = CodeActionKind.QuickFix,
                IsPreferred = true,
                Diagnostics = new List<Diagnostic> { diagnostic },
                Edit = new WorkspaceEdit
                {
                    Changes = new Dictionary<DocumentUri, IEnumerable<TextEdit>>
                    {
                        [new Uri(facade.GetFilePath(targetFile))] =
                        [
                            new()
                            {
                                NewText =
                                    targetFile.Uses.Count > 0
                                        ? $"  - {sourceFile.Name}{Environment.NewLine}"
                                        : $"uses:{Environment.NewLine}  - {sourceFile.Name}{Environment.NewLine}",
                                Range = new Range(useIndex, 0, useIndex, 0),
                            },
                        ],
                    },
                },
            };
    }

    private (string ImportName, int UseIndex) GetImport(
        CodeActionParams request,
        Diagnostic diagnostic,
        ModelFile modelFile
    )
    {
        var fileText = modelFileCache.GetFile(request.TextDocument.Uri.GetFileSystemPath());
        var line = fileText[diagnostic.Range.Start.Line];
        var importName = line[diagnostic.Range.Start.Character..Math.Min(diagnostic.Range.End.Character, line.Length)];
        return GetImport(importName, fileText, modelFile);
    }
}
