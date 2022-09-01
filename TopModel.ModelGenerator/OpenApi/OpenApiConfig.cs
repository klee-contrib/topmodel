using SharpYaml.Serialization;

namespace TopModel.ModelGenerator.OpenApi;

public class OpenApiConfig
{
    [YamlMember("domains")]
    public Dictionary<string, string> Domains { get; set; } = new();

    [YamlMember("sources")]
    public Dictionary<string, OpenApiSource> Sources { get; set; } = new();
}

public class OpenApiSource
{
    [YamlMember("url")]
    public string? Url { get; set; }

    [YamlMember("path")]
    public string? Path { get; set; }

    [YamlMember("login")]
    public string? Login { get; set; }

    [YamlMember("include")]
    public string[]? Include { get; set; }

    [YamlMember("tags")]
    public string[] Tags { get; set; } = new[] { "OpenApi" };

    [YamlMember("modelFileName")]
    public string ModelFileName { get; set; } = "Model";
}