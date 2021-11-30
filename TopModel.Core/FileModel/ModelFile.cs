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

    public IList<IProperty> Properties => Classes.SelectMany(c => c.Properties)
        .Concat(Endpoints.SelectMany(e => e.Params))
        .Concat(Endpoints.Select(e => e.Returns))
        .Where(p => p != null)
        .ToList();

    internal IList<Alias> Aliases { get; set; }

    public override string ToString()
    {
        return Name;
    }
}