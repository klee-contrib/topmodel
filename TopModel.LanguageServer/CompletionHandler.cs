using System.Text.RegularExpressions;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;

namespace TopModel.LanguageServer;

public class CompletionHandler : CompletionHandlerBase
{
    private readonly ModelConfig _config;
    private readonly ILanguageServerFacade _facade;
    private readonly ModelFileCache _fileCache;
    private readonly ModelStore _modelStore;

    public CompletionHandler(ModelStore modelStore, ILanguageServerFacade facade, ModelFileCache fileCache, ModelConfig config)
    {
        _config = config;
        _facade = facade;
        _fileCache = fileCache;
        _modelStore = modelStore;
    }

    public override Task<CompletionItem> Handle(CompletionItem request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request);
    }

    public override Task<CompletionList> Handle(CompletionParams request, CancellationToken cancellationToken)
    {
        var completionList = new CompletionList(isIncomplete: false);

        var text = _fileCache.GetFile(request.TextDocument.Uri.GetFileSystemPath());
        var currentLine = text.ElementAtOrDefault(request.Position.Line);

        if (currentLine == null)
        {
            return Task.FromResult(new CompletionList());
        }

        var file = _modelStore.Files.SingleOrDefault(f => _facade.GetFilePath(f) == request.TextDocument.Uri.GetFileSystemPath());
        if (file != null)
        {
            if (currentLine.Contains("domain: ") || currentLine.Contains("listDomain: ") || currentLine.Contains("kind: ")
                || currentLine.TrimStart().StartsWith("-")
                    && GetRootObject(request) == "converter"
                    && (text.ElementAtOrDefault(request.Position.Line - 1)?.Trim() == "to:"
                        || text.ElementAtOrDefault(request.Position.Line - 1)?.Trim() == "from:"
                        || file.Converters.SelectMany(c => c.DomainsFromReferences).Union(file.Converters.SelectMany(c => c.DomainsToReferences)).Any(dr => dr.Start.Line == request.Position.Line)))
            {
                string searchText;
                if (currentLine.TrimStart().StartsWith("-"))
                {
                    searchText = currentLine.TrimStart()[1..].Trim();
                }
                else
                {
                    searchText = currentLine.Split(":")[1].Trim();
                }

                return Task.FromResult(new CompletionList(
                    _modelStore.Domains
                        .Select(domain => domain.Key)
                        .Concat(currentLine.Contains("kind: ") ? new[] { "object", "list" } : Array.Empty<string>())
                        .OrderBy(domain => domain)
                        .Where(domain => domain.ToLower().ShouldMatch(searchText))
                        .Select(domain => new CompletionItem
                        {
                            Kind = CompletionItemKind.EnumMember,
                            Label = domain,
                            TextEdit = !string.IsNullOrWhiteSpace(searchText)
                                ? new TextEditOrInsertReplaceEdit(new TextEdit
                                {
                                    NewText = domain,
                                    Range = new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range(
                                        request.Position.Line,
                                        currentLine.IndexOf(searchText),
                                        request.Position.Line,
                                        currentLine.IndexOf(searchText) + searchText.Length)
                                })
                                : null,
                        })));
            }

            if (currentLine.Contains("association: ") || currentLine.Contains("composition: ") || currentLine.Contains("    class:") || currentLine.Contains("    - class:") || currentLine.Contains("extends: "))
            {
                var searchText = currentLine.Split(":")[1].Trim();
                var availableClasses = new HashSet<Class>(_modelStore.GetAvailableClasses(file));

                var useIndex = file.Uses.Any()
                    ? file.Uses.Last().ToRange()!.Start.Line + 1
                    : text.First().StartsWith("-")
                        ? 1
                        : 0;

                return Task.FromResult(new CompletionList(
                    _modelStore.Classes
                        .Where(classe => classe.Name.ToLower().ShouldMatch(searchText))
                        .Select(classe => new CompletionItem
                        {
                            Kind = CompletionItemKind.Class,
                            Label = availableClasses.Contains(classe) ? classe.Name : $"{classe.Name} - ({classe.ModelFile.Name})",
                            InsertText = classe.Name,
                            SortText = availableClasses.Contains(classe) ? "0000" + classe.Name : classe.Name,
                            TextEdit = !string.IsNullOrWhiteSpace(searchText)
                                ? new TextEditOrInsertReplaceEdit(new TextEdit
                                {
                                    NewText = classe.Name,
                                    Range = new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range(
                                        request.Position.Line,
                                        currentLine.IndexOf(searchText),
                                        request.Position.Line,
                                        currentLine.IndexOf(searchText) + searchText.Length)
                                })
                                : null,
                            AdditionalTextEdits = !availableClasses.Contains(classe) ?
                                new TextEditContainer(new TextEdit
                                {
                                    NewText = file.Uses.Any() ? $"  - {classe.ModelFile.Name}{Environment.NewLine}" : $"uses:{Environment.NewLine}  - {classe.ModelFile.Name}{Environment.NewLine}",
                                    Range = new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range(useIndex, 0, useIndex, 0)
                                })
                                : null
                        })));
            }

            // Use
            if (currentLine.TrimStart().StartsWith("-") && (
                text.ElementAtOrDefault(request.Position.Line - 1) == "uses:"
                || file.Uses.Select(u => u.Start.Line).Any(l => l == request.Position.Line)))
            {
                var searchText = currentLine.TrimStart()[1..].Trim();

                return Task.FromResult(new CompletionList(
                    _modelStore.Files.Select(f => f.Name)
                        .Except(file.Uses.Select(u => u.ReferenceName))
                        .Where(name => name != file.Name && name.ToLower().ShouldMatch(searchText))
                        .Select(name => new CompletionItem
                        {
                            Kind = CompletionItemKind.File,
                            Label = name,
                            TextEdit = !string.IsNullOrWhiteSpace(searchText)
                                ? new TextEditOrInsertReplaceEdit(new TextEdit
                                {
                                    NewText = name,
                                    Range = new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range(
                                        request.Position.Line,
                                        currentLine.IndexOf(searchText),
                                        request.Position.Line,
                                        currentLine.IndexOf(searchText) + searchText.Length)
                                })
                                : null
                        })));
            }

            // Décorateur
            else if (currentLine.TrimStart().StartsWith("-") && (
                text.ElementAtOrDefault(request.Position.Line - 1)?.TrimStart() == "decorators:"
                || file.Classes.SelectMany(c => c.DecoratorReferences).Any(dr => dr.Start.Line == request.Position.Line)))
            {
                var searchText = currentLine.TrimStart()[1..].Trim();
                var availableDecorators = new HashSet<Decorator>(_modelStore.GetAvailableDecorators(file));

                var useIndex = file.Uses.Any()
                    ? file.Uses.Last().ToRange()!.Start.Line + 1
                    : text.First().StartsWith("-")
                        ? 1
                        : 0;

                return Task.FromResult(new CompletionList(
                    _modelStore.Decorators
                        .OrderBy(decorator => decorator.Name)
                        .Where(decorator => decorator.Name.ToLower().ShouldMatch(searchText))
                        .Select(decorator => new CompletionItem
                        {
                            Kind = CompletionItemKind.Class,
                            Label = availableDecorators.Contains(decorator) ? decorator.Name : $"{decorator.Name} - ({decorator.ModelFile.Name})",
                            InsertText = decorator.Name,
                            SortText = availableDecorators.Contains(decorator) ? "0000" + decorator.Name : decorator.Name,
                            TextEdit = !string.IsNullOrWhiteSpace(searchText)
                                ? new TextEditOrInsertReplaceEdit(new TextEdit
                                {
                                    NewText = decorator.Name,
                                    Range = new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range(
                                        request.Position.Line,
                                        currentLine.IndexOf(searchText),
                                        request.Position.Line,
                                        currentLine.IndexOf(searchText) + searchText.Length)
                                })
                                : null,
                            AdditionalTextEdits = !availableDecorators.Contains(decorator) ?
                                new TextEditContainer(new TextEdit
                                {
                                    NewText = file.Uses.Any() ? $"  - {decorator.ModelFile.Name}{Environment.NewLine}" : $"uses:{Environment.NewLine}  - {decorator.ModelFile.Name}{Environment.NewLine}",
                                    Range = new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range(useIndex, 0, useIndex, 0)
                                })
                                : null
                        })));
            }
            else
            {
                // Alias ou propriété d'association
                string? className = null;
                var classLine = currentLine;
                var requestLine = request.Position.Line;

                while (requestLine > 0 && classLine != null && (classLine.Contains("property:") || classLine.Contains("include:") || classLine.Contains("exclude:") || classLine.TrimStart().StartsWith("-") && !classLine.Contains(':') && !classLine.Contains('[')))
                {
                    requestLine--;
                    classLine = text.ElementAtOrDefault(requestLine);
                }

                if (classLine != null && (classLine.Contains("class:") || classLine.Contains("association:")))
                {
                    className = classLine.Split(':')[1].Trim();
                }

                if (className == null)
                {
                    classLine = text.ElementAtOrDefault(request.Position.Line);

                    while (requestLine < text.Length - 1 && classLine != null && (classLine.Contains("property:") || classLine.Contains("include:") || classLine.Contains("exclude:") || classLine.TrimStart().StartsWith("-") && !classLine.Contains(':') && !classLine.Contains('[')))
                    {
                        requestLine++;
                        classLine = text.ElementAtOrDefault(requestLine);
                    }

                    if (classLine != null && classLine.Contains("class:"))
                    {
                        className = classLine.Split(':')[1].Trim();
                    }
                }

                if (className != null)
                {
                    var searchText = currentLine.Contains(':')
                        ? currentLine.Split(':')[1].Trim()
                        : currentLine.Trim().StartsWith('-')
                        ? currentLine.Trim()[1..].Trim()
                        : string.Empty;

                    var referencedClasses = _modelStore.GetReferencedClasses(file);
                    if (referencedClasses.TryGetValue(className, out var aliasedClass))
                    {
                        return Task.FromResult(new CompletionList(aliasedClass.Properties.OfType<IFieldProperty>()
                            .Where(f => f.Name.ShouldMatch(searchText))
                            .Select(f => new CompletionItem
                            {
                                Kind = CompletionItemKind.Property,
                                Label = f.Name,
                                TextEdit = !string.IsNullOrWhiteSpace(searchText)
                                    ? new TextEditOrInsertReplaceEdit(new TextEdit
                                    {
                                        NewText = f.Name,
                                        Range = new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range(
                                            request.Position.Line,
                                            currentLine.IndexOf(searchText),
                                            request.Position.Line,
                                            currentLine.IndexOf(searchText) + searchText.Length)
                                    })
                                    : null
                            })));
                    }
                }
            }

            // Propriétés de la classe courante
            {
                var requestLine = request.Position.Line;
                var classLine = currentLine;
                while (classLine != "class:")
                {
                    requestLine--;
                    if (requestLine < 0)
                    {
                        break;
                    }

                    classLine = text.ElementAtOrDefault(requestLine);
                }

                if (classLine == "class:")
                {
                    var classe = file.Classes.FirstOrDefault(f => f.GetLocation()?.Start.Line == requestLine + 1);

                    if (classe != null)
                    {
                        string? searchText = null;
                        var includeCompositions = false;

                        if (currentLine.Contains("defaultProperty:") || currentLine.Contains("flagProperty:") || currentLine.Contains("orderProperty:"))
                        {
                            searchText = currentLine.Split(":")[1].Trim();
                        }
                        else
                        {
                            requestLine = request.Position.Line - 1;
                            var ukValuesLine = text.ElementAtOrDefault(requestLine);
                            while (ukValuesLine != null && !Regex.IsMatch(ukValuesLine, "^  \\w") && !ukValuesLine.Contains("mappings:"))
                            {
                                requestLine--;
                                if (requestLine < 0)
                                {
                                    break;
                                }

                                ukValuesLine = text.ElementAtOrDefault(requestLine);
                            }

                            var reqChar = Math.Min(request.Position.Character, currentLine.Length);
                            var isUk = ukValuesLine != null && ukValuesLine.Contains("unique:");
                            var isValues = ukValuesLine != null && ukValuesLine.Contains("values:");
                            var isMappings = ukValuesLine != null && ukValuesLine.Contains("mappings:");
                            var pC = currentLine[..reqChar].LastOrDefault();
                            var pCT = currentLine[..reqChar].TrimEnd().LastOrDefault();
                            if (
                                isUk
                                    && currentLine.Contains('[') && currentLine.IndexOf('[') < reqChar
                                    && (!currentLine.Contains(']') || currentLine.IndexOf(']') >= reqChar)
                                    && (pC != ' ' || pCT == '[' || pCT == ',')
                                || isValues
                                    && currentLine.Contains('{') && currentLine.IndexOf('{') < reqChar
                                    && (!currentLine.Contains('}') || currentLine.IndexOf('}') >= reqChar)
                                    && (pC != ' ' || pCT == '{' || pCT == ',')
                                || isMappings)
                            {
                                searchText = string.Join(
                                    string.Empty,
                                    currentLine[..reqChar].Reverse().TakeWhile(c => c != ',' && c != '[' && c != ' ' && c != '{').Reverse()
                                        .Concat(currentLine[reqChar..].TakeWhile(c => c != ',' && c != ']' && c != ' ' && c != '}' && c != ':')));
                            }

                            if (isMappings && currentLine.Contains(':') && currentLine.IndexOf(':') < reqChar)
                            {
                                string? className = null;
                                classLine = ukValuesLine;

                                while (requestLine > 0 && classLine != null && !classLine.TrimStart().StartsWith('-') && !classLine.Contains("class:"))
                                {
                                    requestLine--;
                                    classLine = text.ElementAtOrDefault(requestLine);
                                }

                                if (classLine != null && classLine.Contains("class:"))
                                {
                                    className = classLine.Split(':')[1].Trim();
                                }

                                if (className == null)
                                {
                                    requestLine = request.Position.Line;
                                    classLine = text.ElementAtOrDefault(requestLine);

                                    while (requestLine < text.Length - 1 && classLine != null && !classLine.Contains("class:"))
                                    {
                                        requestLine++;
                                        classLine = text.ElementAtOrDefault(requestLine);
                                    }

                                    if (classLine != null && classLine.Contains("class:"))
                                    {
                                        className = classLine.Split(':')[1].Trim();
                                    }
                                }

                                if (className != null)
                                {
                                    var referencedClasses = _modelStore.GetReferencedClasses(file);
                                    if (referencedClasses.TryGetValue(className, out var aliasedClass))
                                    {
                                        classe = aliasedClass;
                                    }
                                    else
                                    {
                                        searchText = null;
                                    }
                                }
                            }
                            else if (isMappings)
                            {
                                includeCompositions = true;
                            }
                        }

                        if (searchText != null)
                        {
                            return Task.FromResult(new CompletionList(
                                (includeCompositions
                                    ? classe.Properties
                                    : classe.Properties.Where(p => p is IFieldProperty))
                                .Where(f => f.Name.ShouldMatch(searchText))
                                .Select(f => new CompletionItem
                                {
                                    Kind = CompletionItemKind.Property,
                                    Label = f.Name,
                                    TextEdit = !string.IsNullOrWhiteSpace(searchText)
                                        ? new TextEditOrInsertReplaceEdit(new TextEdit
                                        {
                                            NewText = f.Name,
                                            Range = new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range(
                                                request.Position.Line,
                                                currentLine.IndexOf(searchText),
                                                request.Position.Line,
                                                currentLine.IndexOf(searchText) + searchText.Length)
                                        })
                                        : null
                                })));
                        }
                    }
                }
            }
        }

        return Task.FromResult(new CompletionList());
    }

    protected override CompletionRegistrationOptions CreateRegistrationOptions(CompletionCapability capability, ClientCapabilities clientCapabilities)
    {
        return new CompletionRegistrationOptions
        {
            DocumentSelector = _config.GetDocumentSelector()
        };
    }

    private string GetRootObject(CompletionParams request)
    {
        var text = _fileCache.GetFile(request.TextDocument.Uri.GetFileSystemPath());
        var currentLine = text.ElementAtOrDefault(request.Position.Line);
        var requestLine = request.Position.Line;
        var rootLine = currentLine ?? string.Empty;
        while (
            !(rootLine.StartsWith("class")
            || rootLine.StartsWith("domain")
            || rootLine.StartsWith("decorator")
            || rootLine.StartsWith("converter")
            || rootLine.StartsWith("endpoint")))
        {
            requestLine--;
            if (requestLine < 0 || rootLine.StartsWith("---"))
            {
                break;
            }

            rootLine = text.ElementAtOrDefault(requestLine) ?? string.Empty;
        }

        return rootLine.Split(":")[0];
    }
}