using TopModel.Core.FileModel;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Core.Model;

public class Endpoint(Reference location) : IPropertyContainer
{
    public required Namespace Namespace { get; init; }

    public required ModelFile ModelFile { get; init; }

    public IEnumerable<string> Tags => ModelFile.Tags.Concat(OwnTags).Distinct();

    public LocatedString Name { get; internal set; } = null!;

    public string NamePascal => Name.Value.ToPascalCase();

    public string NameCamel => Name.Value.ToCamelCase();

    public string Method { get; internal set; } = null!;

    public StringWithVariables Route { get; internal set; } = null!;

    public string FullRoute
    {
        get
        {
            var endpointPrefix = (ModelFile.Options?.Endpoints?.Prefix ?? string.Empty).Trim('/');
            var endpointRoute = Route.Trim('/');
            return $"{endpointPrefix}{(string.IsNullOrEmpty(endpointPrefix) || string.IsNullOrEmpty(endpointRoute) ? string.Empty : "/")}{endpointRoute}";
        }
    }

    public string Description { get; internal set; } = null!;

    public IProperty? Returns { get; internal set; }

    public IList<IProperty> Params { get; } = [];

    public bool IsMultipart => Params.Any(p => p.ParamLocation == ParamLocation.FormData);

    public IList<IProperty> Properties => Params.Concat([Returns!]).Where(p => p != null).ToList();

    public bool PreservePropertyCasing { get; internal set; }

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

    internal Reference Location { get; } = location;

    internal IList<string> OwnTags { get; set; } = [];

    public override string ToString()
    {
        return Name;
    }
}
