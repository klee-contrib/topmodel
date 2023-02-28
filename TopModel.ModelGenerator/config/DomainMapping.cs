using SharpYaml.Serialization;

namespace TopModel.ModelGenerator;

public class DomainMapping
{

    [YamlMember("name")]
    public string? Name { get; set; }

    [YamlMember("type")]
    public string? Type { get; set; }

    [YamlMember("scale")]
    public string? Scale { get; set; }

    [YamlMember("precision")]
    public string? Precision { get; set; }

    [YamlMember("domain")]
    public string Domain { get; set; } = string.Empty;
}
