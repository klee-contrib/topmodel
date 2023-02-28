using SharpYaml.Serialization;

namespace TopModel.ModelGenerator.Database;

public class ModuleConfig
{
    #nullable disable
    [YamlMember("name")]
    public string Name { get; set; }

    [YamlMember("classes")]
    public List<string> Classes { get; set; } = new();
}
