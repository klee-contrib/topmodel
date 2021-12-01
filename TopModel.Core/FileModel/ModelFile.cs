#nullable disable
namespace TopModel.Core.FileModel;

public class ModelFile
{
    public string Module { get; set; }

    public IList<string> Tags { get; set; } = new List<string>();

    public IList<string> Uses { get; set; } = new List<string>();

    public string Name { get; set; }

    public string Path { get; set; }

    public IList<Class> Classes { get; set; }

    public IList<Domain> Domains { get; set; }

    public IList<Endpoint> Endpoints { get; set; }

    public IList<IProperty> Properties => Classes.Where(c => !ResolvedAliases.Contains(c)).SelectMany(c => c.Properties)
        .Concat(Endpoints.Where(e => !ResolvedAliases.Contains(e)).SelectMany(e => e.Params))
        .Concat(Endpoints.Where(e => !ResolvedAliases.Contains(e)).Select(e => e.Returns))
        .Where(p => p != null)
        .ToList();

    internal IList<Alias> Aliases { get; set; }

    internal List<object> ResolvedAliases { get; set; } = new();

    public override string ToString()
    {
        return Name;
    }
}