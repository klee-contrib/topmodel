using NJsonSchema;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Core.Model;

namespace TopModel.LanguageServer;

public class CompletionHandler(
    ModelStore modelStore,
    ILanguageServerFacade facade,
    ModelFileCache fileCache,
    ModelConfig config
) : CompletionHandlerBase
{
    private static readonly char[] Separators =
    [
        ':',
        ',',
        '{',
        '}',
        '[',
        ']',
        ' ',
        '(',
        ')',
        '-',
        '=',
        '\'',
        '#',
        '\n',
        '.',
        '"',
    ];

    public override Task<CompletionItem> Handle(CompletionItem request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request);
    }

    public override async Task<CompletionList> Handle(CompletionParams request, CancellationToken cancellationToken)
    {
        await modelStore.WaitForUpdates(cancellationToken);

        var text = fileCache.GetFile(request.TextDocument.Uri.GetFileSystemPath());
        var currentLine = text.ElementAtOrDefault(request.Position.Line);

        if (currentLine == null)
        {
            return new();
        }

        var file = modelStore.Files.SingleOrDefault(f =>
            facade.GetFilePath(f) == request.TextDocument.Uri.GetFileSystemPath()
        );
        if (file == null || currentLine == string.Empty)
        {
            return new();
        }

        var reqChar = Math.Min(request.Position.Character, currentLine.Length);
        var rootObject = GetRootObject(request).Object;
        if (rootObject.StartsWith("--"))
        {
            return new();
        }

        var currentKey = GetCurrentKey(request).Key;
        var parentKey = GetParentKey(request).Key;
        var useIndex = GetUseIndex(file, text);
        if (
            parentKey == "asDomains" && currentLine[..reqChar].Contains(':')
            || currentKey == "domain"
            || parentKey == "domain" && currentLine[..reqChar].Contains("name: ")
            || rootObject == "converter" && (currentKey == "to" || currentKey == "from")
        )
        {
            return CompleteDomain(request);
        }

        List<string> classCompleteKeys = ["association", "composition", "class", "extends"];

        if (classCompleteKeys.Contains(currentKey) && parentKey != currentKey)
        {
            return CompleteClass(request, file, useIndex);
        }

        if (currentKey == "endpoint")
        {
            return CompleteEndpoint(request, file, useIndex);
        }

        // Tags
        if (currentKey == "tags")
        {
            return CompleteTag(request, file);
        }

        // Use
        if (currentKey == "uses")
        {
            return CompleteFile(request, file);
        }
        // Décorateur
        else if (currentKey.Contains("decorator"))
        {
            return CompleteDecorator(request, file, useIndex);
        }
        // Annotation
        else if (currentKey.ToLowerInvariant().Contains("annotation"))
        {
            return CompleteAnnotation(request, file, useIndex);
        }
        // DataFlow
        else if (currentKey == "dependsOn")
        {
            return CompleteDataFlow(request, file, useIndex);
        }
        else
        {
            return CompleteProperty(request, text, currentLine, file);
        }
    }

    protected override CompletionRegistrationOptions CreateRegistrationOptions(
        CompletionCapability capability,
        ClientCapabilities clientCapabilities
    )
    {
        return new CompletionRegistrationOptions { DocumentSelector = config.GetDocumentSelector() };
    }

    private static (string Key, int Line, int End, bool IsKey) GetCurrentKey(string[] text, int line, int position)
    {
        var currentLine = text.ElementAtOrDefault(line);
        var rootLine = currentLine ?? string.Empty;
        if (rootLine.Trim().StartsWith('-'))
        {
            rootLine = rootLine.Replace('-', ' ');
        }

        var isInLineObject = rootLine[..position].Contains(": {");
        if (isInLineObject)
        {
            var isKey =
                rootLine[..position].LastIndexOf(':')
                < Math.Max(rootLine[..position].LastIndexOf(','), rootLine[..position].LastIndexOf('{'));
            if (isKey)
            {
                return (
                    Key: rootLine.TrimStart().Split(':')[0],
                    Line: line,
                    End: rootLine.Split(':')[0].Length - 1,
                    IsKey: true
                );
            }
            else
            {
                return (
                    Key: rootLine[rootLine[..position].LastIndexOf(':')..position].Split(':')[0],
                    Line: line,
                    End: rootLine[..position].LastIndexOf(':') - 1,
                    IsKey: false
                );
            }
        }

        if (rootLine[..position].Contains(": "))
        {
            return (
                Key: rootLine.TrimStart().Split(':')[0],
                Line: line,
                End: rootLine.Split(':')[0].Length - 1,
                IsKey: false
            );
        }

        var requestLine = line;
        var currentIndent = GetIndentLevel(text, line);
        var rootIndent = currentIndent;

        while (rootIndent >= currentIndent && requestLine > 0 && !rootLine.StartsWith("---"))
        {
            requestLine--;

            rootLine = text.ElementAtOrDefault(requestLine) ?? string.Empty;
            while (
                (
                    string.IsNullOrWhiteSpace(rootLine)
                    || rootLine.Contains('#') && string.IsNullOrWhiteSpace(rootLine[..rootLine.IndexOf('#')].Trim())
                )
                && requestLine > 0
            )
            {
                requestLine--;
                rootLine = text.ElementAtOrDefault(requestLine) ?? string.Empty;
            }

            rootIndent = GetIndentLevel(text, requestLine);
            if (rootLine.Trim().StartsWith('-'))
            {
                rootLine = rootLine.Replace('-', ' ');
            }
        }

        return (
            Key: rootLine.Split(":")[0].Trim(),
            Line: requestLine,
            End: rootLine.Split(":")[0].Length - 1,
            IsKey: false
        );
    }

    private static int GetIndentLevel(string[] text, int lineNumber)
    {
        var line = text[lineNumber];
        return GetIndentLevel(line);
    }

    private static int GetIndentLevel(string line)
    {
        if (line.StartsWith("---"))
        {
            return 0;
        }

        var isList = line.TrimStart().StartsWith('-');
        if (isList)
        {
            line = line.Replace('-', ' ');
        }

        return line.Length - line.TrimStart().Length;
    }

    private static (int Start, int End) GetObjectRange(string[] text, int lineNumber)
    {
        var start = lineNumber;
        var end = lineNumber;
        var indentLevelLine = GetIndentLevel(text, lineNumber);
        while (start > 0 && !text[start].StartsWith("---") && GetIndentLevel(text, start) >= indentLevelLine)
        {
            start--;
            if (GetIndentLevel(text, start) == indentLevelLine && text[start].TrimStart().StartsWith('-'))
            {
                start--;
                break;
            }
        }

        while (
            end < text.Length
            && !text[end].StartsWith("---")
            && GetIndentLevel(text, end) >= indentLevelLine
            && !(GetIndentLevel(text, end) == indentLevelLine && text[end].TrimStart().StartsWith('-'))
        )
        {
            end++;
        }

        return (Math.Min(start + 1, lineNumber), Math.Max(end, lineNumber));
    }

    private static (string Key, int Line, int End, bool IsKey) GetParentKey(string[] text, int line, int position)
    {
        var currentLine = text.ElementAtOrDefault(line);
        var rootLine = currentLine ?? string.Empty;
        var isInLineObject =
            position < rootLine.Length && rootLine[..Math.Min(position, rootLine.Length - 1)].Contains(": {");
        if (isInLineObject)
        {
            return (
                Key: rootLine.TrimStart().Split(':')[0],
                Line: line,
                End: rootLine[..position].LastIndexOf(':') - 1,
                IsKey: false
            );
        }

        var requestLine = line;
        var currentIndent = GetIndentLevel(text, line);
        var rootIndent = currentIndent;

        while (rootIndent >= currentIndent && requestLine > 0 && !rootLine.StartsWith("---"))
        {
            requestLine--;
            rootLine = text.ElementAtOrDefault(requestLine) ?? string.Empty;
            while (
                (
                    string.IsNullOrWhiteSpace(rootLine)
                    || rootLine.Contains('#') && string.IsNullOrWhiteSpace(rootLine[..rootLine.IndexOf('#')].Trim())
                )
                && requestLine > 0
            )
            {
                requestLine--;
                rootLine = text.ElementAtOrDefault(requestLine) ?? string.Empty;
            }

            rootLine = text.ElementAtOrDefault(requestLine) ?? string.Empty;
            rootIndent = GetIndentLevel(text, requestLine);
        }

        return (
            Key: rootLine.Split(":")[0].Trim(),
            Line: requestLine,
            End: rootLine.Split(":")[0].Length - 1,
            IsKey: false
        );
    }

    private static (int Start, int End) GetParentRange(string[] text, int lineNumber)
    {
        var start = lineNumber;
        var indentLevelLine = GetIndentLevel(text, lineNumber);
        while (GetIndentLevel(text, start) >= indentLevelLine)
        {
            start--;
        }

        return GetObjectRange(text, start);
    }

    private static int GetUseIndex(ModelFile file, string[] text)
    {
        if (file.Uses.Count > 0)
        {
            return file.Uses[^1].ToRange()!.Start.Line + 1;
        }
        else if (text[0].StartsWith('-'))
        {
            return 1;
        }

        return 0;
    }

    private CompletionList CompleteAnnotation(CompletionParams request, ModelFile file, int useIndex)
    {
        var searchText = GetSearchText(request);
        var availableAnnotations = new HashSet<Annotation>(modelStore.GetAvailableAnnotations(file));

        return new(
            modelStore
                .Annotations.Where(annotation => annotation.Name.ToLower().ShouldMatch(searchText))
                .OrderBy(annotation => annotation.Name)
                .Select(annotation => new CompletionItem
                {
                    Kind = CompletionItemKind.Class,
                    Label = availableAnnotations.Contains(annotation)
                        ? annotation.Name
                        : $"{annotation.Name} - ({annotation.ModelFile.Name})",
                    LabelDetails = new() { Description = $"{annotation.Description}" },
                    InsertText = annotation.Name,
                    SortText = availableAnnotations.Contains(annotation) ? "0000" + annotation.Name : annotation.Name,
                    TextEdit = new(
                        new TextEdit { NewText = annotation.Name, Range = GetCompleteRange(searchText, request) }
                    ),
                    AdditionalTextEdits = !availableAnnotations.Contains(annotation)
                        ? new(
                            new TextEdit
                            {
                                NewText =
                                    file.Uses.Count > 0
                                        ? $"  - {annotation.ModelFile.Name}{Environment.NewLine}"
                                        : $"uses:{Environment.NewLine}  - {annotation.ModelFile.Name}{Environment.NewLine}",
                                Range = new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range(
                                    useIndex,
                                    0,
                                    useIndex,
                                    0
                                ),
                            }
                        )
                        : null,
                })
        );
    }

    private CompletionList CompleteClass(CompletionParams request, ModelFile file, int useIndex)
    {
        var searchText = GetSearchText(request);
        var availableClasses = new HashSet<Class>(modelStore.GetAvailableClasses(file));

        return new(
            modelStore
                .Classes.Where(classe => classe.Name.ToLower().ShouldMatch(searchText))
                .Select(classe => new CompletionItem
                {
                    Kind = CompletionItemKind.Class,
                    Label = availableClasses.Contains(classe)
                        ? classe.Name
                        : $"{classe.Name} - ({classe.ModelFile.Name})",
                    LabelDetails = new() { Description = $"{classe.Comment}" },
                    InsertText = classe.Name,
                    SortText = availableClasses.Contains(classe) ? "0000" + classe.Name : classe.Name,
                    TextEdit = new(
                        new TextEdit { NewText = classe.Name, Range = GetCompleteRange(searchText, request) }
                    ),
                    AdditionalTextEdits = !availableClasses.Contains(classe)
                        ? new(
                            new TextEdit
                            {
                                NewText =
                                    file.Uses.Count > 0
                                        ? $"  - {classe.ModelFile.Name}{Environment.NewLine}"
                                        : $"uses:{Environment.NewLine}  - {classe.ModelFile.Name}{Environment.NewLine}",
                                Range = new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range(
                                    useIndex,
                                    0,
                                    useIndex,
                                    0
                                ),
                            }
                        )
                        : null,
                })
        );
    }

    private CompletionList CompleteDataFlow(CompletionParams request, ModelFile file, int useIndex)
    {
        var searchText = GetSearchText(request);
        var availableDataFlows = new HashSet<DataFlow>(modelStore.GetAvailableDataFlows(file));

        return new(
            modelStore
                .DataFlows.Where(dataFlow => dataFlow.Name.ToLower().ShouldMatch(searchText))
                .OrderBy(dataFlow => dataFlow.Name)
                .Select(dataFlow => new CompletionItem
                {
                    Kind = CompletionItemKind.Class,
                    Label = availableDataFlows.Contains(dataFlow)
                        ? dataFlow.Name
                        : $"{dataFlow.Name} - ({dataFlow.ModelFile.Name})",
                    InsertText = dataFlow.Name,
                    SortText = availableDataFlows.Contains(dataFlow) ? "0000" + dataFlow.Name : dataFlow.Name,
                    TextEdit = new(
                        new TextEdit { NewText = dataFlow.Name, Range = GetCompleteRange(searchText, request) }
                    ),
                    AdditionalTextEdits = !availableDataFlows.Contains(dataFlow)
                        ? new(
                            new TextEdit
                            {
                                NewText =
                                    file.Uses.Count > 0
                                        ? $"  - {dataFlow.ModelFile.Name}{Environment.NewLine}"
                                        : $"uses:{Environment.NewLine}  - {dataFlow.ModelFile.Name}{Environment.NewLine}",
                                Range = new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range(
                                    useIndex,
                                    0,
                                    useIndex,
                                    0
                                ),
                            }
                        )
                        : null,
                })
        );
    }

    private CompletionList CompleteDecorator(CompletionParams request, ModelFile file, int useIndex)
    {
        var searchText = GetSearchText(request);
        var availableDecorators = new HashSet<Decorator>(modelStore.GetAvailableDecorators(file));

        return new(
            modelStore
                .Decorators.Where(decorator => decorator.Name.ToLower().ShouldMatch(searchText))
                .OrderBy(decorator => decorator.Name)
                .Select(decorator => new CompletionItem
                {
                    Kind = CompletionItemKind.Class,
                    Label = availableDecorators.Contains(decorator)
                        ? decorator.Name
                        : $"{decorator.Name} - ({decorator.ModelFile.Name})",
                    LabelDetails = new() { Description = $"{decorator.Description}" },
                    InsertText = decorator.Name,
                    SortText = availableDecorators.Contains(decorator) ? "0000" + decorator.Name : decorator.Name,
                    TextEdit = new(
                        new TextEdit { NewText = decorator.Name, Range = GetCompleteRange(searchText, request) }
                    ),
                    AdditionalTextEdits = !availableDecorators.Contains(decorator)
                        ? new(
                            new TextEdit
                            {
                                NewText =
                                    file.Uses.Count > 0
                                        ? $"  - {decorator.ModelFile.Name}{Environment.NewLine}"
                                        : $"uses:{Environment.NewLine}  - {decorator.ModelFile.Name}{Environment.NewLine}",
                                Range = new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range(
                                    useIndex,
                                    0,
                                    useIndex,
                                    0
                                ),
                            }
                        )
                        : null,
                })
        );
    }

    private CompletionList CompleteDomain(CompletionParams request)
    {
        var searchText = GetSearchText(request);
        return new(
            modelStore
                .Domains.Where(domain => domain.Key.ToLower().ShouldMatch(searchText))
                .OrderBy(domain => domain.Key)
                .Select(domain => new CompletionItem
                {
                    Kind = CompletionItemKind.EnumMember,
                    Label = domain.Key,
                    TextEdit = new(
                        new TextEdit { NewText = domain.Key, Range = GetCompleteRange(searchText, request) }
                    ),
                })
        );
    }

    private CompletionList CompleteEndpoint(CompletionParams request, ModelFile file, int useIndex)
    {
        var searchText = GetSearchText(request);
        var availableEndpoints = new HashSet<Endpoint>(modelStore.GetAvailableEndpoints(file));

        return new(
            modelStore
                .Endpoints.Where(endpoint => endpoint.Name.ToLower().ShouldMatch(searchText))
                .Select(endpoint => new CompletionItem
                {
                    Kind = CompletionItemKind.Class,
                    Label = availableEndpoints.Contains(endpoint)
                        ? endpoint.Name
                        : $"{endpoint.Name} - ({endpoint.ModelFile.Name})",
                    LabelDetails = new() { Description = $"{endpoint.Description}" },
                    InsertText = endpoint.Name,
                    SortText = availableEndpoints.Contains(endpoint) ? "0000" + endpoint.Name : endpoint.Name,
                    TextEdit = new(
                        new TextEdit { NewText = endpoint.Name, Range = GetCompleteRange(searchText, request) }
                    ),
                    AdditionalTextEdits = !availableEndpoints.Contains(endpoint)
                        ? new(
                            new TextEdit
                            {
                                NewText =
                                    file.Uses.Count > 0
                                        ? $"  - {endpoint.ModelFile.Name}{Environment.NewLine}"
                                        : $"uses:{Environment.NewLine}  - {endpoint.ModelFile.Name}{Environment.NewLine}",
                                Range = new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range(
                                    useIndex,
                                    0,
                                    useIndex,
                                    0
                                ),
                            }
                        )
                        : null,
                })
        );
    }

    private CompletionList CompleteFile(CompletionParams request, ModelFile file)
    {
        var searchText = GetSearchText(request);
        return new(
            modelStore
                .Files.Select(f => f.Name)
                .Except(file.Uses.Select(u => u.ReferenceName))
                .Where(name => name != file.Name && name.ToLower().ShouldMatch(searchText))
                .Select(name => new CompletionItem
                {
                    Kind = CompletionItemKind.File,
                    Label = name,
                    TextEdit = new TextEditOrInsertReplaceEdit(
                        new TextEdit { NewText = name, Range = GetCompleteRange(searchText, request) }
                    ),
                })
        );
    }

    private CompletionList CompleteProperty(CompletionParams request, string[] text, string currentLine, ModelFile file)
    {
        // Alias, propriété d'association ou propriété de flux de données
        string? className = null;
        string? endpointName = null;
        string? decoratorName = null;
        var requestLine = request.Position.Line;
        var isListElement = currentLine.TrimStart().StartsWith('-');
        var isInlineList = currentLine.Contains(':') && currentLine.Split(':')[1].TrimStart().StartsWith('[');
        var (start, end) = isListElement ? GetParentRange(text, requestLine) : GetObjectRange(text, requestLine);
        var objectLines = text[start..end];
        var searchText = GetSearchText(request);
        var cl = objectLines.FirstOrDefault(o => o.Contains("class: ") || o.Contains("association: "));
        if (cl != null)
        {
            className = cl.Split(": ")[1].Trim();
        }

        var ep = objectLines.FirstOrDefault(o => o.Contains("endpoint: "));
        if (ep != null)
        {
            endpointName = ep.Split(": ")[1].Trim();
        }

        var dc = objectLines.FirstOrDefault(o => o.Contains("decorator: "));
        if (dc != null)
        {
            decoratorName = dc.Split(": ")[1].Trim();
        }

        var currentKey = GetCurrentKey(request);
        var propertyListKeyWords = new List<string>() { "include", "exclude", "property", "joinProperties" };
        var propertyKeyWords = new List<string>() { "include", "exclude", "property", "activeProperty" };

        if (
            !string.IsNullOrEmpty(className)
            && (
                (isListElement || isInlineList) && propertyListKeyWords.Contains(currentKey.Key)
                || propertyKeyWords.Contains(currentKey.Key)
            )
        )
        {
            var referencedClasses = modelStore.GetReferencedClasses(file);
            if (referencedClasses.TryGetValue(className, out var referencedClass))
            {
                return CompleteProperty(request, referencedClass, includeExtends: false);
            }
        }

        if (
            !string.IsNullOrEmpty(endpointName)
            && (
                (isListElement || isInlineList) && propertyListKeyWords.Contains(currentKey.Key)
                || propertyKeyWords.Contains(currentKey.Key)
            )
        )
        {
            var referencedEndpoints = modelStore.GetReferencedEndpoints(file);
            if (referencedEndpoints.TryGetValue(endpointName, out var referencedEndpoint))
            {
                return CompleteProperty(request, referencedEndpoint, includeExtends: false);
            }
        }

        if (
            !string.IsNullOrEmpty(decoratorName)
            && (
                (isListElement || isInlineList) && propertyListKeyWords.Contains(currentKey.Key)
                || propertyKeyWords.Contains(currentKey.Key)
            )
        )
        {
            var referencedDecorators = modelStore.GetReferencedDecorators(file);
            if (referencedDecorators.TryGetValue(decoratorName, out var referencedDecorator))
            {
                return CompleteProperty(request, referencedDecorator, includeExtends: false);
            }
        }

        var rootObject = GetRootObject(request);
        if (rootObject.Object == "class")
        {
            var (Start, End) = GetObjectRange(text, rootObject.Line);
            var classLines = text[Start..End];
            var nameLine = classLines.OrderBy(GetIndentLevel).FirstOrDefault(l => l.Trim().StartsWith("name: "));
            if (nameLine == null)
            {
                return new CompletionList();
            }

            className = nameLine.Split(':')[1].Trim();
            var classe = file.Classes.SingleOrDefault(c => c.Name == className);
            if (classe != null)
            {
                var includeExtends = false;

                var selfClassPropertyKeyWords = new List<string>()
                {
                    "defaultProperty",
                    "flagProperty",
                    "orderProperty",
                    "target",
                    "unique",
                    "indexes",
                };

                if (GetParentKey(text, currentKey.Line, currentKey.End).Key == "indexes")
                {
                    selfClassPropertyKeyWords.Add("properties");
                }

                var isValues = false;
                var isMappings = false;
                if (!selfClassPropertyKeyWords.Contains(currentKey.Key))
                {
                    var parentKey = currentKey;
                    while (
                        parentKey.Key != "class"
                        && parentKey.Key != "mappings"
                        && parentKey.Key != "values"
                        && parentKey.Line > 0
                    )
                    {
                        parentKey = GetParentKey(text, parentKey.Line, parentKey.End);
                    }

                    isValues = parentKey.Key == "values";
                    isMappings = parentKey.Key == "mappings";
                    includeExtends = isMappings || isValues;

                    if (isMappings)
                    {
                        var requestLineText = text[request.Position.Line];
                        var textBefore = requestLineText[..request.Position.Character];
                        var isKey =
                            textBefore.LastIndexOf(':') == -1
                            || textBefore.LastIndexOf(':')
                                < Math.Max(textBefore.LastIndexOf(','), textBefore.LastIndexOf('{'));
                        if (!isKey)
                        {
                            var (startP, endP) = GetObjectRange(text, parentKey.Line);
                            var parentObjectLines = text[startP..endP];
                            className = parentObjectLines
                                .First(l => l.Contains("class: "))
                                .TrimStart()
                                .Split(':')[1]
                                .Trim();
                        }

                        var referencedClasses = modelStore.GetReferencedClasses(file);
                        if (referencedClasses.TryGetValue(className, out var aliasedClass))
                        {
                            classe = aliasedClass;
                        }
                    }
                    else if (isValues)
                    {
                        var requestLineText = text[request.Position.Line];
                        var textBefore = requestLineText[..request.Position.Character];
                        var isKey =
                            textBefore.LastIndexOf(':') == -1
                            || textBefore.LastIndexOf(':')
                                < Math.Max(textBefore.LastIndexOf(','), textBefore.LastIndexOf('{'));
                        if (!isKey)
                        {
                            return new CompletionList();
                        }
                    }
                }

                if (
                    searchText != null
                    && (isValues || isMappings || selfClassPropertyKeyWords.Contains(currentKey.Key))
                )
                {
                    return CompleteProperty(request, classe, includeExtends);
                }
            }
        }

        return new CompletionList();
    }

    private CompletionList CompleteProperty(CompletionParams request, IPropertyContainer container, bool includeExtends)
    {
        var properties = includeExtends && container is Class classe ? classe.ExtendedProperties : container.Properties;
        var searchText = GetSearchText(request);
        return new(
            properties
                .Where(f => f.Name.ShouldMatch(searchText))
                .Select(f => new CompletionItem
                {
                    Kind = CompletionItemKind.Property,
                    Label = f.Name,
                    LabelDetails = new() { Description = $"{f.Comment}" },
                    TextEdit = new TextEditOrInsertReplaceEdit(
                        new TextEdit { NewText = f.Name, Range = GetCompleteRange(searchText, request) }
                    ),
                })
        );
    }

    private CompletionList CompleteTag(CompletionParams request, ModelFile file)
    {
        var searchText = GetSearchText(request);
        return new(
            modelStore
                .Files.SelectMany(f => f.Tags)
                .Distinct()
                .Where(t => !file.Tags.Contains(t) && t.ShouldMatch(searchText))
                .Select(tag => new CompletionItem
                {
                    Kind = CompletionItemKind.Keyword,
                    Label = tag,
                    TextEdit = new TextEditOrInsertReplaceEdit(
                        new TextEdit { NewText = tag, Range = GetCompleteRange(searchText, request) }
                    ),
                })
        );
    }

    private OmniSharp.Extensions.LanguageServer.Protocol.Models.Range GetCompleteRange(
        string searchText,
        CompletionParams request
    )
    {
        var text = fileCache.GetFile(request.TextDocument.Uri.GetFileSystemPath());
        var currentLine = text.ElementAtOrDefault(request.Position.Line)!;
        int start,
            end = currentLine.Length;
        if (currentLine.Length > 0 && Array.Exists(Separators, currentLine.Contains))
        {
            var left = currentLine[..request.Position.Character];
            start = left.LastIndexOfAny(Separators) + 1;
            var right = currentLine[request.Position.Character..];
            if (right.IndexOfAny(Separators) >= 0)
            {
                end = request.Position.Character + right.IndexOfAny(Separators);
            }
        }
        else
        {
            start = currentLine.IndexOf(searchText);
        }

        return new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range(
            request.Position.Line,
            start,
            request.Position.Line,
            end
        );
    }

    private (string Key, int Line, int End, bool IsKey) GetCurrentKey(CompletionParams request)
    {
        var text = fileCache.GetFile(request.TextDocument.Uri.GetFileSystemPath());
        return GetCurrentKey(text, request.Position.Line, request.Position.Character);
    }

    private (string Key, int Line, int End, bool IsKey) GetParentKey(CompletionParams request)
    {
        var text = fileCache.GetFile(request.TextDocument.Uri.GetFileSystemPath());
        return GetParentKey(text, request.Position.Line, request.Position.Character);
    }

    private (string Object, int Line) GetRootObject(CompletionParams request)
    {
        var text = fileCache.GetFile(request.TextDocument.Uri.GetFileSystemPath());
        var currentLine = text.ElementAtOrDefault(request.Position.Line);
        var requestLine = request.Position.Line;
        var rootLine = currentLine ?? string.Empty;
        while (
            !(
                rootLine.StartsWith("class")
                || rootLine.StartsWith("domain")
                || rootLine.StartsWith("decorator")
                || rootLine.StartsWith("converter")
                || rootLine.StartsWith("endpoint")
                || rootLine.StartsWith("dataFlow")
            )
        )
        {
            requestLine--;
            if (requestLine < 0 || rootLine.StartsWith("---"))
            {
                break;
            }

            rootLine = text.ElementAtOrDefault(requestLine) ?? string.Empty;
        }

        return (Object: rootLine.Split(":")[0], Line: requestLine);
    }

    private string GetSearchText(CompletionParams request)
    {
        var text = fileCache.GetFile(request.TextDocument.Uri.GetFileSystemPath());
        var currentLine = text.ElementAtOrDefault(request.Position.Line)!;
        int start = 0,
            end = request.Position.Character;
        var left = currentLine[..end];
        if (currentLine.Length > 0 && Array.Exists(Separators, currentLine.Contains))
        {
            start = Math.Min(left.LastIndexOfAny(Separators) + 1, end);
        }

        return currentLine[start..end].Trim();
    }
}
