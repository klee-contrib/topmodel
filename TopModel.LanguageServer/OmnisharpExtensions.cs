using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Core.Utils;

namespace TopModel.LanguageServer;

public static class OmnisharpExtensions
{
    public static string GetFilePath(this ModelFile file) =>
        Path.GetFullPath(file.Path.Replace('/', Path.DirectorySeparatorChar));

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

        var matchedReference = references.Keys.SingleOrDefault(reference =>
            reference.Start.Line - 1 <= position.Line
            && position.Line <= reference.End.Line - 1
            && reference.Start.Column - 1 <= position.Character
            && position.Character <= reference.End.Column - 1
        );

        return (matchedReference, matchedReference != null ? references[matchedReference] : null);
    }

    public static References? GetReferencesForPositionInFile(
        this ModelStore modelStore,
        Position position,
        ModelFile file,
        bool includeTransitive = false
    )
    {
        var (_, referencedObject) = file.GetObjetAtPosition(position);

        var definedObjects = file
            .Classes.Where(c =>
                c.Name.GetLocation()!.Start.Line - 1 == position.Line
                || c.GetLocation()!.Start.Line - 1 == position.Line
            )
            .Cast<object>()
            .Concat(
                file.Annotations.Where(d =>
                        d.Name.GetLocation()!.Start.Line - 1 == position.Line
                        || d.GetLocation()!.Start.Line - 1 == position.Line
                    )
                    .Cast<object>()
            )
            .Concat(
                file.Domains.Where(d =>
                        d.Name.GetLocation()!.Start.Line - 1 == position.Line
                        || d.GetLocation()!.Start.Line - 1 == position.Line
                    )
                    .Cast<object>()
            )
            .Concat(
                file.Decorators.Where(d =>
                        d.Name.GetLocation()!.Start.Line - 1 == position.Line
                        || d.GetLocation()!.Start.Line - 1 == position.Line
                    )
                    .Cast<object>()
            )
            .Concat(
                file.DataFlows.Where(d =>
                        d.Name.GetLocation()!.Start.Line - 1 == position.Line
                        || d.GetLocation()!.Start.Line - 1 == position.Line
                    )
                    .Cast<object>()
            )
            .Concat(
                file.Endpoints.Where(d =>
                        d.Name.GetLocation()!.Start.Line - 1 == position.Line
                        || d.GetLocation()!.Start.Line - 1 == position.Line
                    )
                    .Cast<object>()
            )
            .Concat(file.Parameters.Where(d => d.Name.GetLocation()!.Start.Line - 1 == position.Line).Cast<object>())
            .Concat(file.Properties.Where(p => p.GetLocation()!.Start.Line - 1 == position.Line))
            .Concat(file.ReverseProperties.Where(p => p.GetLocation()!.Start.Line - 1 == position.Line));

        var definedObject = definedObjects.Count() == 1 ? definedObjects.Single() : null;

        return new References(
            definedObject ?? referencedObject,
            new[] { definedObject!, referencedObject! }
                .Where(o => o != null)
                .SelectMany<object, (Reference Reference, ModelFile File)>(objet =>
                    objet switch
                    {
                        Class classe =>
                        [
                            (Reference: classe.Name.GetLocation()!, File: classe.GetFile()!),
                            .. modelStore.GetClassReferences(classe),
                        ],
                        Domain domain =>
                        [
                            (Reference: domain.Name.GetLocation()!, File: domain.GetFile()!),
                            .. modelStore.GetDomainReferences(domain),
                        ],
                        Annotation annotation =>
                        [
                            (Reference: annotation.Name.GetLocation()!, File: annotation.GetFile()!),
                            .. modelStore.GetAnnotationReferences(annotation),
                        ],
                        Decorator decorator =>
                        [
                            (Reference: decorator.Name.GetLocation()!, File: decorator.GetFile()!),
                            .. modelStore.GetDecoratorReferences(decorator),
                        ],
                        DataFlow dataFlow =>
                        [
                            (Reference: dataFlow.Name.GetLocation()!, File: dataFlow.GetFile()!),
                            .. modelStore.GetDataFlowReferences(dataFlow),
                        ],
                        Endpoint endpoint =>
                        [
                            (Reference: endpoint.Name.GetLocation()!, File: endpoint.GetFile()!),
                            .. modelStore.GetEndpointReferences(endpoint),
                        ],
                        IProperty property =>
                        [
                            (Reference: property.GetLocation()!, File: property.GetFile()!),
                            .. modelStore.GetPropertyReferences(property, includeTransitive),
                        ],
                        TemplateParameter parameter =>
                        [
                            (Reference: parameter.GetLocation()!, File: parameter.GetFile()!),
                            .. modelStore.GetParameterReferences(parameter),
                        ],
                        _ => null!,
                    }
                )
                .Distinct()
        );
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

    public static OmniSharp.Extensions.LanguageServer.Protocol.Models.Range? ToRange(this Reference? loc)
    {
        return loc == null
            ? null
            : new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range(
                (int)loc.Start.Line - 1,
                (int)loc.Start.Column - 1,
                (int)loc.End.Line - 1,
                (int)loc.End.Column - 1
            );
    }
}
