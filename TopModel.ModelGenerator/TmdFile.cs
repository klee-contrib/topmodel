namespace TopModel.ModelGenerator;

public class TmdFile
{
#nullable disable
    public string Name { get; set; }

    public List<string> Tags { get; set; } = new();

    public List<TmdClass> Classes { get; set; } = new();

    public List<TmdEndpoint> Endpoints { get; set; } = new();

#nullable enable
    public string? Module { get; set; }

    public List<TmdFile> Uses => Classes.SelectMany(c => c.Dependencies).Concat(Endpoints.SelectMany(e => e.Dependencies)).Where(c => c.File != null).Select(f => f!.File!).Distinct().OrderBy(u => u.Name).ToList();
}