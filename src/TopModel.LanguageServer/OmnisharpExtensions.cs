using System.Diagnostics.CodeAnalysis;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Core.Utils;
using Location = TopModel.Core.FileModel.Location;
using Range = OmniSharp.Extensions.LanguageServer.Protocol.Models.Range;

namespace TopModel.LanguageServer;

public static class OmnisharpExtensions
{
    /// <summary>
    /// Comparaison de chemins de fichiers, insensible à la casse hors Linux.
    /// </summary>
    public static readonly StringComparer PathComparer = OperatingSystem.IsLinux()
        ? StringComparer.Ordinal
        : StringComparer.OrdinalIgnoreCase;

    private static readonly EnumerationOptions CaseInsensitive = new() { MatchCasing = MatchCasing.CaseInsensitive };

    public static string GetFilePath(this ILanguageServerFacade facade, ModelFile file)
    {
        return facade.GetFilePath(file.Path);
    }

    public static string GetFilePath(this ILanguageServerFacade facade, string path)
    {
        return facade.Workspace.ClientSettings.RootPath + path[1..].Replace('/', Path.DirectorySeparatorChar);
    }

    public static string GetFilePath(this ILanguageServerFacade facade, DocumentUri uri)
    {
        var path = uri.GetFileSystemPath();
        var rootPath = facade.Workspace.ClientSettings.RootPath;

        if (
            OperatingSystem.IsLinux()
            || string.IsNullOrEmpty(rootPath)
            || !path.StartsWith(rootPath, StringComparison.OrdinalIgnoreCase)
        )
        {
            return path;
        }

        var segments = path[rootPath.Length..]
            .Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);
        var current = rootPath;
        for (var i = 0; i < segments.Length; i++)
        {
            var match = Directory.Exists(current)
                ? Directory.EnumerateFileSystemEntries(current, segments[i], CaseInsensitive).FirstOrDefault()
                : null;
            if (match == null)
            {
                return Path.Combine([current, .. segments[i..]]);
            }

            current = match;
        }

        return current;
    }

    public static IEnumerable<T> GetInAll<T>(
        this IEnumerable<(ModelFile File, ModelStore Store)> files,
        Func<(ModelFile File, ModelStore Store), IEnumerable<T>> selector,
        Func<T, object> getName
    )
    {
        return files
            .SelectMany(selector)
            .GroupBy(getName)
            .Where(g => g.Count() == files.Count())
            .Select(g => g.First());
    }

    public static Locations GetLocationsForObjetAtPosition(
        this ModelStore modelStore,
        Position position,
        ModelFile file,
        bool includeTransitive = false
    )
    {
        var (_, referencedObject) = file.GetObjetAtPosition(position);

        var definedObjects = file
            .Classes.Where(c =>
                c.Name.Declaration!.Start.Line - 1 == position.Line || c.Declaration!.Start.Line - 1 == position.Line
            )
            .Cast<object>()
            .Concat(
                file.Annotations.Where(d =>
                        d.Name.Declaration!.Start.Line - 1 == position.Line
                        || d.Declaration!.Start.Line - 1 == position.Line
                    )
                    .Cast<object>()
            )
            .Concat(
                file.Domains.Where(d =>
                        d.Name.Declaration!.Start.Line - 1 == position.Line
                        || d.Declaration!.Start.Line - 1 == position.Line
                    )
                    .Cast<object>()
            )
            .Concat(
                file.Decorators.Where(d =>
                        d.Name.Declaration!.Start.Line - 1 == position.Line
                        || d.Declaration!.Start.Line - 1 == position.Line
                    )
                    .Cast<object>()
            )
            .Concat(
                file.DataFlows.Where(d =>
                        d.Name.Declaration!.Start.Line - 1 == position.Line
                        || d.Declaration!.Start.Line - 1 == position.Line
                    )
                    .Cast<object>()
            )
            .Concat(
                file.Endpoints.Where(d =>
                        d.Name.Declaration!.Start.Line - 1 == position.Line
                        || d.Declaration!.Start.Line - 1 == position.Line
                    )
                    .Cast<object>()
            )
            .Concat(file.Parameters.Where(d => d.Name.Declaration!.Start.Line - 1 == position.Line).Cast<object>())
            .Concat(file.Properties.Where(p => p.Declaration!.Start.Line - 1 == position.Line))
            .Concat(file.ReverseProperties.Where(p => p.Declaration!.Start.Line - 1 == position.Line));

        var definedObject = definedObjects.Count() == 1 ? definedObjects.Single() : null;

        return new Locations(
            definedObject ?? referencedObject,
            new[] { definedObject!, referencedObject! }
                .Where(o => o != null)
                .SelectMany<object, (Location Reference, ModelFile File)>(objet =>
                    objet switch
                    {
                        Class classe =>
                        [
                            (Reference: classe.Name.Declaration!, File: classe.ModelFile),
                            .. modelStore.GetClassReferences(classe, includeTransitive),
                        ],
                        Domain domain =>
                        [
                            (Reference: domain.Name.Declaration!, File: domain.ModelFile),
                            .. modelStore.GetDomainReferences(domain),
                        ],
                        Annotation annotation =>
                        [
                            (Reference: annotation.Name.Declaration!, File: annotation.ModelFile),
                            .. modelStore.GetAnnotationReferences(annotation),
                        ],
                        Decorator decorator =>
                        [
                            (Reference: decorator.Name.Declaration!, File: decorator.ModelFile),
                            .. modelStore.GetDecoratorReferences(decorator),
                        ],
                        DataFlow dataFlow =>
                        [
                            (Reference: dataFlow.Name.Declaration!, File: dataFlow.ModelFile),
                            .. modelStore.GetDataFlowReferences(dataFlow),
                        ],
                        Endpoint endpoint =>
                        [
                            (Reference: endpoint.Name.Declaration!, File: endpoint.ModelFile),
                            .. modelStore.GetEndpointReferences(endpoint),
                        ],
                        IProperty property =>
                        [
                            (Reference: property.Declaration!, File: property.ModelFile),
                            .. modelStore.GetPropertyReferences(property, includeTransitive),
                        ],
                        TemplateParameter parameter =>
                        [
                            (Reference: parameter.Declaration!, File: parameter.ModelFile),
                            .. modelStore.GetParameterReferences(parameter),
                        ],
                        _ => [],
                    }
                )
                .Distinct()
        );
    }

    public static string? GetName(this object objet)
    {
        return objet switch
        {
            Class classe => classe.Name,
            Domain domain => domain.Name,
            Decorator decorator => decorator.Name,
            Annotation annotation => annotation.Name,
            DataFlow dataFlow => dataFlow.Name,
            Endpoint endpoint => endpoint.Name,
            IProperty property => property.OriginalProperty?.Name ?? property.Name,
            TemplateParameter templateParameter => templateParameter.Name,
            _ => null,
        };
    }

    public static (Reference? Reference, object? Objet) GetObjetAtPosition(this ModelFile file, Position position)
    {
        var references = file.References;

        var matchedReference = references
            .Keys.Where(reference =>
                reference.Start.Line - 1 <= position.Line
                && position.Line <= reference.End.Line - 1
                && reference.Start.Column - 1 <= position.Character
                && position.Character <= reference.End.Column - 1
            )
            .OrderBy(r => r.End.Index - r.Start.Index)
            .FirstOrDefault();

        return (matchedReference, matchedReference != null ? references[matchedReference] : null);
    }

    public static bool ShouldMatch(this string word, string otherword)
    {
        if (string.IsNullOrWhiteSpace(otherword))
        {
            return true;
        }

        var currentIndex = 0;

        foreach (var character in otherword.ToLower())
        {
            if ((currentIndex = word.ToLower().IndexOf(character, currentIndex)) == -1)
            {
                return false;
            }
        }

        return true;
    }

    [return: NotNullIfNotNull(nameof(loc))]
    public static Range? ToRange(this Location? loc)
    {
        return loc == null
            ? null
            : new Range(
                (int)loc.Start.Line - 1,
                (int)loc.Start.Column - 1,
                (int)loc.End.Line - 1,
                (int)loc.End.Column - 1
            );
    }

    /// <summary>
    /// Construit le WorkspaceEdit remplaçant chaque référence donnée par le texte associé.
    /// </summary>
    public static WorkspaceEdit ToWorkspaceEdit(
        this ILanguageServerFacade facade,
        IEnumerable<(ModelFile File, Location Reference, string NewText)> edits
    )
    {
        return new()
        {
            Changes = edits
                .Select(e =>
                    (
                        Uri: facade.GetFilePath(e.File),
                        Edit: new TextEdit { Range = e.Reference.ToRange(), NewText = e.NewText }
                    )
                )
                .Distinct()
                .GroupBy(e => e.Uri, e => e.Edit)
                .ToDictionary(g => DocumentUri.From(g.Key), g => g.AsEnumerable()),
        };
    }
}
