using TopModel.Core.FileModel;
using TopModel.Utils;

namespace TopModel.Core;

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

    public string Route { get; set; }

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

    public bool IsMultipart => Params.Any(p => p.Domain?.IsMultipart ?? false || p is CompositionProperty cp && cp.IsMultipart);

    public IList<IProperty> Properties => Params.Concat([Returns!]).Where(p => p != null).ToList();

    public bool PreservePropertyCasing { get; set; }

    public Dictionary<string, string> CustomProperties { get; } = [];

    public List<(Decorator Decorator, string[] Parameters)> Decorators { get; } = [];

    public IEnumerable<ClassDependency> ClassDependencies => Properties.GetClassDependencies();

    public List<DecoratorReference> DecoratorReferences { get; } = [];

#nullable disable
    internal Reference Location { get; set; }

    internal List<string> OwnTags { get; set; } = [];

#nullable enable

    public override string ToString()
    {
        return Name;
    }
}