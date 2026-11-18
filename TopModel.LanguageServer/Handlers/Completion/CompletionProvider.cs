using System.Text.RegularExpressions;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using Range = OmniSharp.Extensions.LanguageServer.Protocol.Models.Range;

namespace TopModel.LanguageServer.Handlers.Completion;

/// <summary>
/// Calcule les listes de complétion d'un fichier TMD à partir de son texte, de la position du curseur et des fichiers du modèle.
/// N'a aucune dépendance vers le serveur LSP.
/// </summary>
/// <param name="text">Lignes du fichier.</param>
/// <param name="position">Position du curseur.</param>
/// <param name="context">Contexte YAML du curseur.</param>
/// <param name="files">Fichiers (et leur store) du document, le premier étant le fichier courant.</param>
public class CompletionProvider(
    string[] text,
    Position position,
    YamlCursorContext context,
    IReadOnlyList<(ModelFile File, ModelStore Store)> files
)
{
    private static readonly Regex MappingValueRegex = new(@"^(?<m>class\.mappers\..+\.)mappings\.(?<k>[^.\[]+)$");

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
    private Range? completeRange;
    private string? searchText;

    private Range CompleteRange => completeRange ??= GetCompleteRange();

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
                    TextEdit = ReplaceWord(domain.Key),
                })
        );
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
                            .Except(f.File.Uses.Select(u => u.Value))
                            .Where(name => name != f.File.Name && name.ToLower().ShouldMatch(SearchText)),
                    f => f
                )
                .Select(name => new CompletionItem
                {
                    Kind = CompletionItemKind.File,
                    Label = name,
                    TextEdit = ReplaceWord(name),
                })
        );
    }

    /// <summary>
    /// Complétion des propriétés de la classe courante (clés de <c>values</c> et de <c>mappings</c>,
    /// <c>defaultProperty</c>, <c>unique</c>, <c>indexes</c>, cible d'un paramètre de mapper...).
    /// </summary>
    /// <param name="path">Chemin YAML du curseur.</param>
    /// <returns>La liste de complétion.</returns>
    public CompletionList CompleteOwnProperty(string path)
    {
        var classe = GetCurrentClass();
        if (classe == null)
        {
            return new();
        }

        // Clé d'une valeur de la classe (la valeur d'une propriété n'est pas complétée).
        if (path.StartsWith("class.values."))
        {
            return IsCursorOnKey() ? CompleteProperty(classe, includeExtends: true) : new();
        }

        // Nouvelle clé d'un mapping de mapper : propriétés héritées incluses.
        return CompleteProperty(classe, includeExtends: path.EndsWith(".mappings."));
    }

    /// <summary>
    /// Complétion des paramètres d'un conteneur de paramètres (instance d'annotation, de décorateur ou de domaine).
    /// Les paramètres déjà renseignés sont exclus.
    /// </summary>
    /// <param name="path">
    /// Chemin YAML du curseur (de la forme <c>...annotations[n].NomAnnotation.</c>,
    /// <c>...decorators[n].NomDecorateur.</c> ou <c>...domain.parameters.</c>).
    /// </param>
    /// <returns>La liste de complétion.</returns>
    public CompletionList CompleteParameter(string path)
    {
        var container = GetParameterContainer(path);
        if (container == null)
        {
            return new();
        }

        var existingParameters = context.GetKeys(path);
        return new(
            container
                .TemplateParameters.Where(p =>
                    !existingParameters.Contains(p.Name.Value) && p.Name.Value.ShouldMatch(SearchText)
                )
                .Select(p => new CompletionItem
                {
                    Kind = CompletionItemKind.Property,
                    Label = p.Name.Value,
                    LabelDetails = new() { Description = p.Required ? $"{p.Comment} (requis)" : p.Comment },
                    SortText = (p.Required ? "0" : "1") + p.Name.Value,
                    TextEdit = ReplaceWord(p.Name.Value),
                })
        );
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
                    TextEdit = ReplaceWord(tag),
                })
        );
    }

    /// <summary>
    /// Complétion des propriétés d'une autre classe, d'un endpoint ou d'un décorateur (alias, association, flux de
    /// données, valeur d'un mapping...).
    /// </summary>
    /// <param name="path">Chemin YAML du curseur.</param>
    /// <returns>La liste de complétion.</returns>
    public CompletionList CompleteTargetProperty(string path)
    {
        // Valeur d'un mapping : propriétés de la classe mappée (à défaut, de la classe courante).
        if (MappingValueRegex.Match(path) is { Success: true } mapping)
        {
            var current = GetCurrentClass();
            if (current == null)
            {
                return new();
            }

            var mappedName = context.GetScalar(mapping.Groups["m"].Value, "class");
            if (mappedName == null)
            {
                return new();
            }

            var target = FindReferenced(f => f.Store.GetReferencedClasses(f.File), c => c.ModelFile, mappedName);
            return CompleteProperty(target ?? current, includeExtends: true);
        }

        var objectPath = path[..(path.LastIndexOf('.') + 1)];
        return ResolveTargetContainer(objectPath) is { } container
            ? CompleteProperty(container, includeExtends: false)
            : new();
    }

    private static int GetUseIndex(ModelFile file, string[] text)
    {
        if (file.Uses.Count > 0)
        {
            return file.Uses[^1].ToRange().Start.Line + 1;
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
                    TextEdit = ReplaceWord(f.Name),
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
                    TextEdit = ReplaceWord(name),
                    AdditionalTextEdits = isAvailable ? null : GetUsesEdit(getModelFile(x).Name),
                };
            })
        );
    }

    /// <summary>
    /// Trouve un élément (classe, endpoint, décorateur) par son nom parmi ceux référencés par les fichiers du document.
    /// </summary>
    private T? FindReferenced<T>(
        Func<(ModelFile File, ModelStore Store), IDictionary<string, T>> getReferenced,
        Func<T, ModelFile> getModelFile,
        string name
    )
        where T : class
    {
        return files
            .GetInAll(
                f => getReferenced(f).TryGetValue(name, out var item) ? [item] : Enumerable.Empty<T>(),
                item => (name, getModelFile(item).Path)
            )
            .FirstOrDefault();
    }

    private Range GetCompleteRange()
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

        return new Range(position.Line, start, position.Line, end);
    }

    /// <summary>
    /// Classe du document courant désignée par <c>class.name</c>.
    /// </summary>
    private Class? GetCurrentClass()
    {
        var name = context.GetScalar("class.", "name");
        return name == null ? null : files[0].File.Classes.SingleOrDefault(c => c.Name == name);
    }

    /// <summary>
    /// Résout le conteneur de paramètres (domaine, décorateur ou annotation) désigné par le chemin YAML du curseur.
    /// </summary>
    /// <param name="path">Chemin YAML du curseur.</param>
    /// <returns>Le conteneur de paramètres, ou null s'il est introuvable.</returns>
    private IParameterContainer? GetParameterContainer(string path)
    {
        if (path.EndsWith(".domain.parameters."))
        {
            var domainPath = path[..^"parameters.".Length];
            var domainName = context.GetScalar(domainPath, "name");
            if (domainName == null)
            {
                return null;
            }

            return FindReferenced(f => f.Store.Domains, d => d.ModelFile, domainName);
        }

        var segments = path.TrimEnd('.').Split('.');
        var name = segments[^1];
        return segments.Length > 1 && segments[^2].StartsWith("decorators[")
            ? FindReferenced(f => f.Store.GetReferencedDecorators(f.File), d => d.ModelFile, name)
            : FindReferenced(f => f.Store.GetReferencedAnnotations(f.File), a => a.ModelFile, name);
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
                Range = new Range(useIndex, 0, useIndex, 0),
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

    /// <summary>
    /// Édition remplaçant le mot sous le curseur. Si ce mot suit directement le ':' séparant une clé et sa valeur,
    /// un espace est ajouté au début du texte inséré.
    /// </summary>
    /// <param name="newText">Texte à insérer.</param>
    private TextEditOrInsertReplaceEdit ReplaceWord(string newText)
    {
        var start = CompleteRange.Start.Character;
        var followsColon = start > 0 && start <= currentLine.Length && currentLine[start - 1] == ':';
        return new TextEditOrInsertReplaceEdit(
            new TextEdit { NewText = followsColon ? $" {newText}" : newText, Range = CompleteRange }
        );
    }

    /// <summary>
    /// Résout le conteneur de propriétés (classe, endpoint ou décorateur) ciblé par l'objet contenant le curseur.
    /// </summary>
    /// <param name="objectPath">Chemin de l'objet, point final inclus.</param>
    private IPropertyContainer? ResolveTargetContainer(string objectPath)
    {
        var className = context.GetScalar(objectPath, "class") ?? context.GetScalar(objectPath, "association");
        if (
            className != null
            && FindReferenced(f => f.Store.GetReferencedClasses(f.File), c => c.ModelFile, className) is { } classe
        )
        {
            return classe;
        }

        var endpointName = context.GetScalar(objectPath, "endpoint");
        if (
            endpointName != null
            && FindReferenced(f => f.Store.GetReferencedEndpoints(f.File), e => e.ModelFile, endpointName)
                is { } endpoint
        )
        {
            return endpoint;
        }

        var decoratorName = context.GetScalar(objectPath, "decorator");
        return decoratorName == null
            ? null
            : FindReferenced(f => f.Store.GetReferencedDecorators(f.File), d => d.ModelFile, decoratorName);
    }
}
