namespace TopModel.ModelGenerator.OpenApi;

public class OpenApiConfig
{
    private string? _outputDirectory;

    public string OutputDirectory { get => _outputDirectory ?? Module; set => _outputDirectory = value; }

    public string Module { get; set; } = "OpenApi";

    public IList<DomainMapping> Domains { get; set; } = new List<DomainMapping>();

#nullable disable
    public string Source { get; set; }
#nullable enable

    public string[]? Include { get; set; }

    public IList<string> EndpointTags { get; set; } = new List<string>();

    public IList<string> ModelTags { get; set; } = new List<string>();

    public string ModelFileName { get; set; } = "Model";

    public bool PreservePropertyCasing { get; set; } = true;

    public string? ClassPrefix { get; set; }
}
