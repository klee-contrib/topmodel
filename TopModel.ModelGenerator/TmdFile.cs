namespace TopModel.ModelGenerator;

public class TmdFile
{
    public required string Name { get; set; }

    public IList<string> Tags { get; set; } = [];

    public IList<TmdClass> Classes { get; set; } = [];

    public IList<TmdEndpoint> Endpoints { get; set; } = [];

    public string? Module { get; set; }

    public string? Path { get; set; }

    public IList<TmdFile> Uses =>
        Classes
            .SelectMany(c => c.Dependencies)
            .Where(d => d.File != this)
            .Concat(Endpoints.SelectMany(e => e.Dependencies))
            .Where(c => c.File != null)
            .Select(f => f!.File!)
            .Distinct()
            .OrderBy(u => u.Name)
            .ToList();

    public IList<TmdFile> ExtendedUses => Uses.Concat(Uses.SelectMany(u => u.ExtendedUses)).Distinct().ToList();
}
