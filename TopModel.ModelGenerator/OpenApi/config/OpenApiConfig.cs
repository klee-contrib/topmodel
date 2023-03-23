using SharpYaml.Serialization;

namespace TopModel.ModelGenerator.OpenApi;

public class OpenApiConfig
{
    private string? _outputDirectory;

    [YamlMember("outputDirectory")]
    public string OutputDirectory { get => _outputDirectory ?? Module; set => _outputDirectory = value; }

    [YamlMember("module")]
    public string Module { get; set; } = "OpenApi";

    [YamlMember("domains")]
    public IList<DomainMapping> Domains { get; set; } = new List<DomainMapping>();

#nullable disable
    [YamlMember("source")]
    public string Source { get; set; }
#nullable enable

    [YamlMember("include")]
    public string[]? Include { get; set; }

    [YamlMember("endpointTags")]

    public IList<string> EndpointTags { get; set; } = new List<string>();

    [YamlMember("modelTags")]
    public IList<string> ModelTags { get; set; } = new List<string>();

    [YamlMember("modelFileName")]
    public string ModelFileName { get; set; } = "Model";

    [YamlMember("preservePropertyCasing")]
    public bool PreservePropertyCasing { get; set; } = true;
}
