using SharpYaml.Serialization;

namespace TopModel.ModelGenerator.OpenApi;

public class OpenApiDomain
{

    [YamlMember("name")]
    public string? name { get; set; }

    [YamlMember("type")]
    public string? type { get; set; }

    [YamlMember("domain")]
    public string domain { get; set; } = string.Empty;
}
