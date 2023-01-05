using SharpYaml.Serialization;

namespace TopModel.ModelGenerator.OpenApi;

public class OpenApiSource
{
    [YamlMember("url")]
    public string? Url { get; set; }

    [YamlMember("path")]
    public string? Path { get; set; }

    [YamlMember("include")]
    public string[]? Include { get; set; }

    [YamlMember("endpointTags")]
    public IList<string> EndpointTags { get; set; } = new List<string>();

    [YamlMember("modelTags")]
    public IList<string> ModelTags { get; set; } = new List<string>();

    [YamlMember("modelFileName")]
    public string ModelFileName { get; set; } = "Model";
}