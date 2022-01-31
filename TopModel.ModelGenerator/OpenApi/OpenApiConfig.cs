using SharpYaml.Serialization;

namespace TopModel.ModelGenerator.OpenApi;

public class OpenApiConfig
{
    [YamlMember("domains")]
    public Dictionary<string, string> Domains { get; set; } = new();

    [YamlMember("sources")]
    public List<OpenApiSource> Sources { get; set; } = new();
}

public class OpenApiSource
{
    [YamlMember("url")]
    public string? Url { get; set; }

    [YamlMember("login")]
    public string? Login { get; set; }
}