using System.Text.RegularExpressions;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Core.Model;

namespace TopModel.LanguageServer.Handlers;

/// <summary>
/// Calcule les listes de complétion d'un fichier TMD à partir de son texte, de la position du curseur et des fichiers du modèle.
/// N'a aucune dépendance vers le serveur LSP.
/// </summary>
/// <param name="text">Lignes du fichier.</param>
/// <param name="position">Position du curseur.</param>
/// <param name="files">Fichiers (et leur store) du document, le premier étant le fichier courant.</param>
public class CompletionProvider(
    string[] text,
    Position position,
    IReadOnlyList<(ModelFile File, ModelStore Store)> files
)
{
    private static readonly HashSet<string> PropertyKeyWords = ["include", "exclude", "property", "activeProperty"];
    private static readonly HashSet<string> PropertyListKeyWords = ["include", "exclude", "property", "joinProperties"];

    private static readonly HashSet<string> SelfClassPropertyKeyWords =
    [
        "defaultProperty",
        "flagProperty",
        "orderProperty",
        "localeProperty",
        "discriminatorProperty",
        "target",
        "unique",
        "indexes",
    ];

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

    private readonly string currentLine = text.ElementAtOrDefault(position.Line) ?? string.Empty;
    private OmniSharp.Extensions.LanguageServer.Protocol.Models.Range? completeRange;
    private string? searchText;

    private OmniSharp.Extensions.LanguageServer.Protocol.Models.Range CompleteRange =>
        completeRange ??= GetCompleteRange();

    private string SearchText => searchText ??= GetSearchText();

    /// <summary>
    /// Complétion des annotations.
    /// </summary>
    /// <returns>La liste de complétion.</returns>
    public CompletionList CompleteAnnotation()
    {
        return CompleteReference(
            f => f.Store.GetAvailableAnnotations(f.File),
            f => f.Store.Annotations,
            a => a.Name,
            a => a.ModelFile,
            a => $"{a.Description}",
            ordered: true
        );
    }

    /// <summary>
    /// Complétion des paramètres d'une instance d'annotation. Les paramètres déjà renseignés sont exclus.
    /// </summary>
    /// <param name="path">Chemin YAML du curseur (de la forme <c>...annotations[n].NomAnnotation.</c>).</param>
    /// <returns>La liste de complétion.</returns>
    public CompletionList CompleteAnnotationParameter(string path)
    {
        var annotationName = path.TrimEnd('.').Split('.')[^1];
        var referencedAnnotations = files
            .GetInAll(f => f.Store.GetReferencedAnnotations(f.File), f => (f.Key, f.Value.ModelFile.Path))
            .ToDictionary(f => f.Key, f => f.Value);

        return referencedAnnotations.TryGetValue(annotationName, out var annotation)
            ? CompleteTemplateParameters(annotation.TemplateParameters, path)
            : new();
    }

    /// <summary>
    /// Complétion des classes.
    /// </summary>
    /// <returns>La liste de complétion.</returns>
    public CompletionList CompleteClass()
    {
        return CompleteReference(
            f => f.Store.GetAvailableClasses(f.File),
            f => f.Store.Classes,
            c => c.Name,
            c => c.ModelFile,
            c => $"{c.Comment}",
            ordered: false
        );
    }

    /// <summary>
    /// Complétion des flux de données.
    /// </summary>
    /// <returns>La liste de complétion.</returns>
    public CompletionList CompleteDataFlow()
    {
        return CompleteReference(
            f => f.Store.GetAvailableDataFlows(f.File),
            f => f.Store.DataFlows,
            d => d.Name,
            d => d.ModelFile,
            getDescription: null,
            ordered: true
        );
    }

    /// <summary>
    /// Complétion des décorateurs.
    /// </summary>
    /// <returns>La liste de complétion.</returns>
    public CompletionList CompleteDecorator()
    {
        return CompleteReference(
            f => f.Store.GetAvailableDecorators(f.File),
            f => f.Store.Decorators,
            d => d.Name,
            d => d.ModelFile,
            d => $"{d.Description}",
            ordered: true
        );
    }

    /// <summary>
    /// Complétion des paramètres d'une instance de décorateur. Les paramètres déjà renseignés sont exclus.
    /// </summary>
    /// <param name="path">Chemin YAML du curseur (de la forme <c>...decorators[n].NomDecorateur.</c>).</param>
    /// <returns>La liste de complétion.</returns>
    public CompletionList CompleteDecoratorParameter(string path)
    {
        var decoratorName = path.TrimEnd('.').Split('.')[^1];
        var referencedDecorators = files
            .GetInAll(f => f.Store.GetReferencedDecorators(f.File), f => (f.Key, f.Value.ModelFile.Path))
            .ToDictionary(f => f.Key, f => f.Value);

        return referencedDecorators.TryGetValue(decoratorName, out var decorator)
            ? CompleteTemplateParameters(decorator.TemplateParameters, path)
            : new();
    }

    /// <summary>
    /// Complétion des domaines.
    /// </summary>
    /// <returns>La liste de complétion.</returns>
    public CompletionList CompleteDomain()
    {
        return new(
            files
                .GetInAll(
                    f => f.Store.Domains.Where(domain => domain.Key.ToLower().ShouldMatch(SearchText)),
                    f => (f.Key, f.Value.ModelFile.Path)
                )
                .OrderBy(domain => domain.Key)
                .Select(domain => new CompletionItem
                {
                    Kind = CompletionItemKind.EnumMember,
                    Label = domain.Key,
                    TextEdit = new(new TextEdit { NewText = domain.Key, Range = CompleteRange }),
                })
        );
    }

    /// <summary>
    /// Complétion des paramètres d'un domaine (<c>domain: { name: ..., parameters: { ... } }</c>). Les paramètres
    /// déjà renseignés sont exclus.
    /// </summary>
    /// <param name="path">Chemin YAML du curseur (de la forme <c>...domain.parameters.</c>).</param>
    /// <returns>La liste de complétion.</returns>
    public CompletionList CompleteDomainParameter(string path)
    {
        var domainPath = path[..^"parameters.".Length];
        var domainName =
            GetKeyPaths().FirstOrDefault(k => k.Path == domainPath + "name") is { Path: not null } nameKey
                ? text[nameKey.Line][(nameKey.Colon + 1)..].Split('#')[0].Trim()
                : Regex
                    .Match(currentLine, @"name:\s*(?<name>[^\s,{}]+)", RegexOptions.None, TimeSpan.FromMilliseconds(100))
                    .Groups["name"]
                    .Value;

        var domain = files
            .Select(f => f.Store.Domains.TryGetValue(domainName, out var d) ? d : null)
            .FirstOrDefault(d => d != null);

        return domain != null ? CompleteTemplateParameters(domain.TemplateParameters, path) : new();
    }

    /// <summary>
    /// Complétion des endpoints.
    /// </summary>
    /// <returns>La liste de complétion.</returns>
    public CompletionList CompleteEndpoint()
    {
        return CompleteReference(
            f => f.Store.GetAvailableEndpoints(f.File),
            f => f.Store.Endpoints,
            e => e.Name,
            e => e.ModelFile,
            e => $"{e.Description}",
            ordered: false
        );
    }

    /// <summary>
    /// Complétion des fichiers (section <c>uses</c>).
    /// </summary>
    /// <returns>La liste de complétion.</returns>
    public CompletionList CompleteFile()
    {
        return new(
            files
                .GetInAll(
                    f =>
                        f.Store.Files.Select(f => f.Name)
                            .Except(f.File.Uses.Select(u => u.ReferenceName))
                            .Where(name => name != f.File.Name && name.ToLower().ShouldMatch(SearchText)),
                    f => f
                )
                .Select(name => new CompletionItem
                {
                    Kind = CompletionItemKind.File,
                    Label = name,
                    TextEdit = new TextEditOrInsertReplaceEdit(new TextEdit { NewText = name, Range = CompleteRange }),
                })
        );
    }

    /// <summary>
    /// Complétion des propriétés (alias, associations, flux de données, mappings, valeurs...).
    /// </summary>
    /// <returns>La liste de complétion.</returns>
    public CompletionList CompleteProperty()
    {
        // Alias, propriété d'association ou propriété de flux de données
        string? className = null;
        string? endpointName = null;
        string? decoratorName = null;
        var isListElement = currentLine.TrimStart().StartsWith('-');
        var isInlineList = currentLine.Contains(':') && currentLine.Split(':')[1].TrimStart().StartsWith('[');
        var (start, end) = isListElement ? GetParentRange(text, position.Line) : GetObjectRange(text, position.Line);
        var objectLines = text[start..end];
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

        var currentKey = GetCurrentKey(text, position.Line, position.Character);
        var isPropertyKey =
            (isListElement || isInlineList) && PropertyListKeyWords.Contains(currentKey.Key)
            || PropertyKeyWords.Contains(currentKey.Key);

        if (!string.IsNullOrEmpty(className) && isPropertyKey)
        {
            var referencedClasses = files
                .GetInAll(f => f.Store.GetReferencedClasses(f.File), f => (f.Key, f.Value.ModelFile.Path))
                .ToDictionary(f => f.Key, f => f.Value);

            if (referencedClasses.TryGetValue(className, out var referencedClass))
            {
                return CompleteProperty(referencedClass, includeExtends: false);
            }
        }

        if (!string.IsNullOrEmpty(endpointName) && isPropertyKey)
        {
            var referencedEndpoints = files
                .GetInAll(f => f.Store.GetReferencedEndpoints(f.File), f => (f.Key, f.Value.ModelFile.Path))
                .ToDictionary(f => f.Key, f => f.Value);

            if (referencedEndpoints.TryGetValue(endpointName, out var referencedEndpoint))
            {
                return CompleteProperty(referencedEndpoint, includeExtends: false);
            }
        }

        if (!string.IsNullOrEmpty(decoratorName) && isPropertyKey)
        {
            var referencedDecorators = files
                .GetInAll(f => f.Store.GetReferencedDecorators(f.File), f => (f.Key, f.Value.ModelFile.Path))
                .ToDictionary(f => f.Key, f => f.Value);

            if (referencedDecorators.TryGetValue(decoratorName, out var referencedDecorator))
            {
                return CompleteProperty(referencedDecorator, includeExtends: false);
            }
        }

        var rootObject = GetRootObject();
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
            var classe = files[0].File.Classes.SingleOrDefault(c => c.Name == className);
            if (classe != null)
            {
                var includeExtends = false;
                var isSelfClassPropertyKey =
                    SelfClassPropertyKeyWords.Contains(currentKey.Key)
                    || currentKey.Key == "properties"
                        && GetParentKey(text, currentKey.Line, currentKey.End).Key == "indexes";

                var isValues = false;
                var isMappings = false;
                if (!isSelfClassPropertyKey)
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
                        if (!IsCursorOnKey())
                        {
                            var (startP, endP) = GetObjectRange(text, parentKey.Line);
                            var mappedClassLine = text[startP..endP].FirstOrDefault(l => l.Contains("class: "));
                            if (mappedClassLine == null)
                            {
                                return new CompletionList();
                            }

                            className = mappedClassLine.TrimStart().Split(':')[1].Trim();
                        }

                        var referencedClasses = files
                            .GetInAll(f => f.Store.GetReferencedClasses(f.File), f => (f.Key, f.Value.ModelFile.Path))
                            .ToDictionary(f => f.Key, f => f.Value);

                        if (referencedClasses.TryGetValue(className, out var aliasedClass))
                        {
                            classe = aliasedClass;
                        }
                    }
                    else if (isValues && !IsCursorOnKey())
                    {
                        return new CompletionList();
                    }
                }

                if (isValues || isMappings || isSelfClassPropertyKey)
                {
                    return CompleteProperty(classe, includeExtends);
                }
            }
        }

        return new CompletionList();
    }

    /// <summary>
    /// Complétion des tags.
    /// </summary>
    /// <returns>La liste de complétion.</returns>
    public CompletionList CompleteTag()
    {
        var currentTags = files[0].File.Tags;
        return new(
            files
                .SelectMany(f => f.Store.Files.SelectMany(f => f.Tags))
                .Distinct()
                .Where(t => !currentTags.Contains(t) && t.ShouldMatch(SearchText))
                .Select(tag => new CompletionItem
                {
                    Kind = CompletionItemKind.Keyword,
                    Label = tag,
                    TextEdit = new TextEditOrInsertReplaceEdit(new TextEdit { NewText = tag, Range = CompleteRange }),
                })
        );
    }

    private static (string Key, int Line, int End) GetCurrentKey(string[] text, int line, int position)
    {
        var rootLine = text.ElementAtOrDefault(line) ?? string.Empty;
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
                    End: rootLine.Split(':')[0].Length - 1
                );
            }
            else
            {
                return (
                    Key: rootLine[rootLine[..position].LastIndexOf(':')..position].Split(':')[0],
                    Line: line,
                    End: rootLine[..position].LastIndexOf(':') - 1
                );
            }
        }

        if (rootLine[..position].Contains(": "))
        {
            return (Key: rootLine.TrimStart().Split(':')[0], Line: line, End: rootLine.Split(':')[0].Length - 1);
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

        return (Key: rootLine.Split(":")[0].Trim(), Line: requestLine, End: rootLine.Split(":")[0].Length - 1);
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

    private static (string Key, int Line, int End) GetParentKey(string[] text, int line, int position)
    {
        var rootLine = text.ElementAtOrDefault(line) ?? string.Empty;
        var isInLineObject =
            position < rootLine.Length && rootLine[..Math.Min(position, rootLine.Length - 1)].Contains(": {");
        if (isInLineObject)
        {
            return (
                Key: rootLine.TrimStart().Split(':')[0],
                Line: line,
                End: rootLine[..position].LastIndexOf(':') - 1
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

        return (Key: rootLine.Split(":")[0].Trim(), Line: requestLine, End: rootLine.Split(":")[0].Length - 1);
    }

    private static (int Start, int End) GetParentRange(string[] text, int lineNumber)
    {
        var start = lineNumber;
        var indentLevelLine = GetIndentLevel(text, lineNumber);
        while (start > 0 && GetIndentLevel(text, start) >= indentLevelLine)
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

    private CompletionList CompleteProperty(IPropertyContainer container, bool includeExtends)
    {
        var properties = includeExtends && container is Class classe ? classe.ExtendedProperties : container.Properties;
        return new(
            properties
                .Where(f => f.Name.ShouldMatch(SearchText))
                .Select(f => new CompletionItem
                {
                    Kind = CompletionItemKind.Property,
                    Label = f.Name,
                    LabelDetails = new() { Description = $"{f.Comment}" },
                    TextEdit = new TextEditOrInsertReplaceEdit(new TextEdit { NewText = f.Name, Range = CompleteRange }),
                })
        );
    }

    /// <summary>
    /// Complétion d'un élément référencé par son nom (classe, endpoint, décorateur...). Les éléments non disponibles
    /// dans le fichier courant sont suffixés par leur fichier, et leur sélection ajoute ce fichier dans les <c>uses</c>.
    /// </summary>
    private CompletionList CompleteReference<T>(
        Func<(ModelFile File, ModelStore Store), IEnumerable<T>> getAvailable,
        Func<(ModelFile File, ModelStore Store), IEnumerable<T>> getAll,
        Func<T, string> getName,
        Func<T, ModelFile> getModelFile,
        Func<T, string>? getDescription,
        bool ordered
    )
    {
        var available = new HashSet<T>(files.GetInAll(getAvailable, x => (getName(x), getModelFile(x).Path)));
        var items = files.GetInAll(
            f => getAll(f).Where(x => getName(x).ToLower().ShouldMatch(SearchText)),
            x => (getName(x), getModelFile(x).Path)
        );

        if (ordered)
        {
            items = items.OrderBy(getName);
        }

        return new(
            items.Select(x =>
            {
                var name = getName(x);
                var isAvailable = available.Contains(x);
                return new CompletionItem
                {
                    Kind = CompletionItemKind.Class,
                    Label = isAvailable ? name : $"{name} - ({getModelFile(x).Name})",
                    LabelDetails = getDescription == null ? null : new() { Description = getDescription(x) },
                    InsertText = name,
                    SortText = isAvailable ? "0000" + name : name,
                    TextEdit = new(new TextEdit { NewText = name, Range = CompleteRange }),
                    AdditionalTextEdits = isAvailable ? null : GetUsesEdit(getModelFile(x).Name),
                };
            })
        );
    }

    /// <summary>
    /// Complétion des paramètres d'un template (annotation ou décorateur), hors paramètres déjà renseignés.
    /// </summary>
    private CompletionList CompleteTemplateParameters(IEnumerable<TemplateParameter> parameters, string path)
    {
        var existingParameters = GetExistingKeys(path);
        return new(
            parameters
                .Where(p => !existingParameters.Contains(p.Name.Value) && p.Name.Value.ShouldMatch(SearchText))
                .Select(p => new CompletionItem
                {
                    Kind = CompletionItemKind.Property,
                    Label = p.Name.Value,
                    LabelDetails = new() { Description = p.Required ? $"{p.Comment} (requis)" : p.Comment },
                    SortText = (p.Required ? "0" : "1") + p.Name.Value,
                    TextEdit = new TextEditOrInsertReplaceEdit(
                        new TextEdit { NewText = p.Name.Value, Range = CompleteRange }
                    ),
                })
        );
    }

    private OmniSharp.Extensions.LanguageServer.Protocol.Models.Range GetCompleteRange()
    {
        int start,
            end = currentLine.Length;
        if (currentLine.Length > 0 && Array.Exists(Separators, currentLine.Contains))
        {
            var left = currentLine[..position.Character];
            start = left.LastIndexOfAny(Separators) + 1;
            var right = currentLine[position.Character..];
            if (right.IndexOfAny(Separators) >= 0)
            {
                end = position.Character + right.IndexOfAny(Separators);
            }
        }
        else
        {
            start = currentLine.IndexOf(SearchText);
        }

        return new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range(
            position.Line,
            start,
            position.Line,
            end
        );
    }

    /// <summary>
    /// Clés déjà présentes (hors ligne courante) dans l'objet désigné par un chemin de nouvelle clé (point final),
    /// déterminées en calculant le chemin YAML de chaque ligne du document courant.
    /// </summary>
    private HashSet<string> GetExistingKeys(string path)
    {
        var keys = new HashSet<string>();
        foreach (var (_, linePath, _) in GetKeyPaths())
        {
            if (!linePath.StartsWith(path) || linePath.Length == path.Length)
            {
                continue;
            }

            var rest = linePath[path.Length..];
            var key = rest.IndexOfAny(['.', '[']) is >= 0 and var sep ? rest[..sep] : rest;
            if (key.Length > 0)
            {
                keys.Add(key);
            }
        }

        return keys;
    }

    /// <summary>
    /// Chemin YAML de la première clé de chaque ligne du document courant (hors ligne du curseur).
    /// </summary>
    private IEnumerable<(int Line, string Path, int Colon)> GetKeyPaths()
    {
        var docStart = position.Line;
        while (docStart > 0 && !text[docStart - 1].StartsWith("---"))
        {
            docStart--;
        }

        for (var i = docStart; i < text.Length && !text[i].StartsWith("---"); i++)
        {
            // Curseur juste après le ':' de la clé, pour ne pas tomber dans un éventuel commentaire en fin de ligne.
            var colon = text[i].IndexOf(':');
            if (i == position.Line || colon < 0)
            {
                continue;
            }

            if (YamlPathAnalyzer.GetPath(text, i, colon + 1) is { } linePath)
            {
                yield return (i, linePath, colon);
            }
        }
    }

    private (string Object, int Line) GetRootObject()
    {
        var requestLine = position.Line;
        var rootLine = currentLine;
        while (
            !(
                rootLine.StartsWith("class")
                || rootLine.StartsWith("domain")
                || rootLine.StartsWith("decorator")
                || rootLine.StartsWith("converter")
                || rootLine.StartsWith("endpoint")
                || rootLine.StartsWith("dataFlow")
                || rootLine.StartsWith("annotation")
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

    private string GetSearchText()
    {
        int start = 0,
            end = position.Character;
        var left = currentLine[..end];
        if (currentLine.Length > 0 && Array.Exists(Separators, currentLine.Contains))
        {
            start = Math.Min(left.LastIndexOfAny(Separators) + 1, end);
        }

        return currentLine[start..end].Trim();
    }

    /// <summary>
    /// Édition ajoutant le fichier du modèle dans la section <c>uses</c>.
    /// </summary>
    private TextEditContainer GetUsesEdit(string modelFileName)
    {
        var currentFile = files[0].File;
        var useIndex = GetUseIndex(currentFile, text);
        return new(
            new TextEdit
            {
                NewText =
                    currentFile.Uses.Count > 0
                        ? $"  - {modelFileName}{Environment.NewLine}"
                        : $"uses:{Environment.NewLine}  - {modelFileName}{Environment.NewLine}",
                Range = new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range(useIndex, 0, useIndex, 0),
            }
        );
    }

    /// <summary>
    /// Indique si le curseur est sur une clé (pas de ':' avant lui, ou un ',' / '{' après le dernier ':').
    /// </summary>
    private bool IsCursorOnKey()
    {
        var textBefore = currentLine[..position.Character];
        var colon = textBefore.LastIndexOf(':');
        return colon == -1 || colon < Math.Max(textBefore.LastIndexOf(','), textBefore.LastIndexOf('{'));
    }
}
