using TopModel.Core.FileModel;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Core.Model;

public class Endpoint : IPropertyContainer
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

    public bool IsMultipart =>
        Params.Any(p =>
            !p.IsRouteParam() && (p.Domain?.IsMultipart ?? false) || p is CompositionProperty cp && cp.IsMultipart
        );

    public IList<IProperty> Properties => Params.Concat([Returns!]).Where(p => p != null).ToList();

    public bool PreservePropertyCasing { get; set; }

    public IDictionary<string, string> CustomProperties { get; internal set; } = new Dictionary<string, string>();

    public IList<DecoratorInstance> Decorators { get; } = [];

    public IList<AnnotationInstance> Annotations { get; } = [];

    public IList<AnnotationInstance> ExcludedAnnotations { get; } = [];

    public IList<AnnotationInstance> PropertyAnnotations { get; } = [];

    [Obsolete("Utiliser `Config.GetClassDependencies(endpoint)`.")]
    public IEnumerable<ClassDependency> ClassDependencies => Properties.GetClassDependencies();

    public IList<DecoratorReference> DecoratorReferences { get; internal set; } = [];

    public IList<AnnotationReference> AnnotationReferences { get; internal set; } = [];

    public IList<AnnotationReference> ExcludedAnnotationReferences { get; internal set; } = [];

    public IList<AnnotationReference> PropertyAnnotationReferences { get; internal set; } = [];

    public IList<PropertySource> PropertySourceOrder { get; internal set; } =
    [PropertySource.Params, PropertySource.Decorators];

    internal IProperty? OwnReturns { get; set; }

    internal IList<IProperty> OwnParams { get; set; } = [];

#nullable disable

    internal Reference Location { get; set; }

    internal IList<string> OwnTags { get; set; } = [];

#nullable enable

    public override string ToString()
    {
        return Name;
    }
}
