using TopModel.Core.FileModel;

namespace TopModel.Core;

public class Endpoint
{
    public Namespace Namespace { get; set; }

#nullable disable
    public ModelFile ModelFile { get; set; }

    public LocatedString Name { get; set; }

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

    public IList<IProperty> Params { get; set; } = new List<IProperty>();

    public IEnumerable<ClassDependency> ClassDependencies => Params.Concat(new[] { Returns! }).Where(p => p != null).GetClassDependencies();

    public IEnumerable<Domain> DomainDependencies => Params.Concat(new[] { Returns! }).Where(p => p != null).GetDomainDependencies();

#nullable disable
    internal Reference Location { get; set; }
#nullable enable

    public override string ToString()
    {
        return Name;
    }
}