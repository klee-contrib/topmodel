using TopModel.Core.FileModel;
using TopModel.Utils;

namespace TopModel.Core;

public class Endpoint : IPropertyContainer, IAnnotationContainer
{
    public Namespace Namespace { get; set; }

#nullable disable
    public ModelFile ModelFile { get; set; }

    public IEnumerable<string> Tags => ModelFile.Tags.Concat(OwnTags).Distinct();

    public LocatedString Name { get; set; }

    public string NamePascal => Name.Value.ToPascalCase();

    public string NameCamel => Name.Value.ToCamelCase();

    public string Method { get; set; }

    public StringWithVariables Route { get; set; }

    public string FullRoute
    {
        get
        {
            var endpointPrefix = (ModelFile.Options?.Endpoints?.Prefix ?? string.Empty).Trim('/');
            var endpointRoute = Route.Trim('/');
            return $"{endpointPrefix}{(string.IsNullOrEmpty(endpointPrefix) || string.IsNullOrEmpty(endpointRoute) ? string.Empty : "/")}{endpointRoute}";
        }
    }

    public string Description { get; set; }
#nullable enable

    public IProperty? Returns { get; set; }

    public IList<IProperty> Params { get; set; } = [];

    public bool IsMultipart => Params.Any(p => !p.IsRouteParam() && (p.Domain?.IsMultipart ?? false) || p is CompositionProperty cp && cp.IsMultipart);

    public IList<IProperty> Properties => Params.Concat([Returns!]).Where(p => p != null).ToList();

    public bool PreservePropertyCasing { get; set; }

    public Dictionary<string, string> CustomProperties { get; } = [];

    public IList<DecoratorInstance> Decorators { get; } = [];

    public IList<AnnotationInstance> Annotations { get; } = [];

    public IEnumerable<ClassDependency> ClassDependencies => Properties.GetClassDependencies();

    public IList<DecoratorReference> DecoratorReferences { get; } = [];

    public IList<AnnotationReference> AnnotationReferences { get; set; } = [];

#nullable disable

    internal Reference Location { get; set; }

    internal IList<string> OwnTags { get; set; } = [];

#nullable enable

    public override string ToString()
    {
        return Name;
    }
}