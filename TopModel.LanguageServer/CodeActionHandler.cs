using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Core.Utils;
using TopModel.Utils;
using Range = OmniSharp.Extensions.LanguageServer.Protocol.Models.Range;

namespace TopModel.LanguageServer;

public class CodeActionHandler(ModelStoreRegistry registry, ModelFileCache modelFileCache) : CodeActionHandlerBase
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
        var filePath = request.TextDocument.Uri.GetFileSystemPath();
        var entry = registry.GetPrimaryForFile(filePath);
        if (entry == null)
        {
            return CommandOrCodeActionContainer.From(Array.Empty<CommandOrCodeAction>());
        }

        var modelStore = entry.Store;
        await modelStore.WaitForUpdates(cancellationToken);

        var modelFile = modelStore.Files.SingleOrDefault(f => f.GetFilePath() == filePath);
        var codeActions = new List<CommandOrCodeAction>();
        if (modelFile != null)
        {
            if (
                modelFile.Uses.Except(modelStore.GetUselessImports(modelFile)).Any()
                || modelStore.GetUselessImports(modelFile).Any()
            )
            {
                codeActions.Add(GetCodeActionOrganizeImports(request, modelFile));
            }

            foreach (var diagnostic in request.Context.Diagnostics.Where(d => !string.IsNullOrEmpty(d.Code)))
            {
                if (diagnostic.Severity == DiagnosticSeverity.Warning)
                {
                    codeActions.Add(GetCodeActionIgnoreWarning(request, diagnostic, modelFile));
                }

                var modelErrorType = Enum.Parse<ErrorType>(diagnostic.Code!);
                switch (modelErrorType)
                {
                    case ErrorType.TMD0002:
                        codeActions.AddRange(
                            GetCodeActionMissingClassImport(request, diagnostic, modelFile, modelStore)
                        );
                        codeActions.AddRange(GetCodeActionAddClass(request, diagnostic, modelFile));
                        break;
                    case ErrorType.TMD0003:
                        codeActions.AddRange(GetCodeActionCreateDomain(request, diagnostic, modelStore));
                        break;
                    case ErrorType.TMD0005:
                        codeActions.AddRange(
                            GetCodeActionMissingDecoratorImport(request, diagnostic, modelFile, modelStore)
                        );
                        break;
                    case ErrorType.TMD0006:
                        codeActions.AddRange(
                            GetCodeActionMissingEndpointImport(request, diagnostic, modelFile, modelStore)
                        );
                        break;
                    case ErrorType.TMD2001:
                        codeActions.AddRange(
                            GetCodeActionMissingAnnotationImport(request, diagnostic, modelFile, modelStore)
                        );
                        codeActions.AddRange(GetCodeActionAddAnnotation(request, diagnostic, modelFile));
                        break;
                    case ErrorType.TMD4002:
                        codeActions.AddRange(
                            GetCodeActionMissingDataFlowImport(request, diagnostic, modelFile, modelStore)
                        );
                        break;
                    case ErrorType.TMD9008:
                        codeActions.AddRange(
                            GetCodeActionMissingWithReverseImport(request, diagnostic, modelFile, modelStore)
                        );
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
            DocumentSelector = registry.GetCombinedDocumentSelector(),
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
        ModelFile modelFile
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
                        [new Uri(modelFile.GetFilePath())] =
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
        ModelFile modelFile
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
                        [new Uri(modelFile.GetFilePath())] =
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
        ModelStore modelStore
    )
    {
        var text = modelFileCache.GetFile(request.TextDocument.Uri.GetFileSystemPath());
        var line = text[diagnostic.Range.Start.Line];
        var domainName = line[diagnostic.Range.Start.Character..Math.Min(diagnostic.Range.End.Character, line.Length)];

        return modelStore
            .Files.Where(f => f.Domains.Count > 0)
            .Select(f =>
            {
                var lastLine = File.ReadAllLines(f.GetFilePath()).Length;
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
                                [new Uri(f.GetFilePath())] =
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
        ModelFile modelFile
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
                        [new Uri(modelFile.GetFilePath())] =
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
        ModelFile modelFile,
        ModelStore modelStore
    )
    {
        var (decoratorName, useIndex) = GetImport(request, diagnostic, modelFile);
        return modelStore
            .Annotations.Where(c => c.Name == decoratorName)
            .Select(annotationToImport =>
                GetFileImportAction(diagnostic, modelFile, annotationToImport.ModelFile, useIndex)
            );
    }

    protected IEnumerable<CommandOrCodeAction> GetCodeActionMissingClassImport(
        CodeActionParams request,
        Diagnostic diagnostic,
        ModelFile modelFile,
        ModelStore modelStore
    )
    {
        var (className, useIndex) = GetImport(request, diagnostic, modelFile);
        return modelStore
            .Classes.Where(c => c.Name == className)
            .Select(classToImport => GetFileImportAction(diagnostic, modelFile, classToImport.ModelFile, useIndex));
    }

    protected IEnumerable<CommandOrCodeAction> GetCodeActionMissingDataFlowImport(
        CodeActionParams request,
        Diagnostic diagnostic,
        ModelFile modelFile,
        ModelStore modelStore
    )
    {
        var (dataFlowName, useIndex) = GetImport(request, diagnostic, modelFile);
        return modelStore
            .DataFlows.Where(c => c.Name == dataFlowName)
            .Select(decoratorToImport =>
                GetFileImportAction(diagnostic, modelFile, decoratorToImport.ModelFile, useIndex)
            );
    }

    protected IEnumerable<CommandOrCodeAction> GetCodeActionMissingDecoratorImport(
        CodeActionParams request,
        Diagnostic diagnostic,
        ModelFile modelFile,
        ModelStore modelStore
    )
    {
        var (decoratorName, useIndex) = GetImport(request, diagnostic, modelFile);
        return modelStore
            .Decorators.Where(c => c.Name == decoratorName)
            .Select(decoratorToImport =>
                GetFileImportAction(diagnostic, modelFile, decoratorToImport.ModelFile, useIndex)
            );
    }

    protected IEnumerable<CommandOrCodeAction> GetCodeActionMissingEndpointImport(
        CodeActionParams request,
        Diagnostic diagnostic,
        ModelFile modelFile,
        ModelStore modelStore
    )
    {
        var (endpointName, useIndex) = GetImport(request, diagnostic, modelFile);
        return modelStore
            .Endpoints.Where(c => c.Name == endpointName)
            .Select(endpointToImport =>
                GetFileImportAction(diagnostic, modelFile, endpointToImport.ModelFile, useIndex)
            );
    }

    protected IEnumerable<CommandOrCodeAction> GetCodeActionMissingWithReverseImport(
        CodeActionParams request,
        Diagnostic diagnostic,
        ModelFile modelFile,
        ModelStore modelStore
    )
    {
        var (_, objet) = modelFile.GetObjetAtPosition(diagnostic.Range.Start);

        if (objet is not Class)
        {
            return [];
        }

        var targetFile = objet.GetFile();
        var fileText = File.ReadAllLines(targetFile.GetFilePath());

        var (className, useIndex) = GetImport(objet.GetName()!, fileText, targetFile);
        return modelStore
            .Classes.Where(c => c.Name == className)
            .Select(targetClass =>
                GetFileImportAction(diagnostic, targetClass.ModelFile, modelFile, useIndex, reverse: true)
            );
    }

    protected CodeAction GetCodeActionOrganizeImports(CodeActionParams request, ModelFile modelFile)
    {
        var entry = registry.GetPrimaryForFile(request.TextDocument.Uri.GetFileSystemPath())!;
        var modelStore = entry.Store;
        var uses = modelFile.Uses.Except(modelStore.GetUselessImports(modelFile));
        var start = modelFile.Uses[0].ToRange()!.Start;
        var end = modelFile.Uses[^1].ToRange()!.End;
        if (!uses.Any())
        {
            var fileText = modelFileCache.GetFile(request.TextDocument.Uri.GetFileSystemPath()).ToList();
            start = new Position(fileText.FindIndex(line => line.StartsWith("uses")), 0);
            end.Line = end.Line + 1;
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
                        [new Uri(targetFile.GetFilePath())] =
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
