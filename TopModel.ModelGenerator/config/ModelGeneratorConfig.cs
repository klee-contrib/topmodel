using SharpYaml.Serialization;
using TopModel.ModelGenerator.OpenApi;
using TopModel.ModelGenerator.Database;

namespace TopModel.ModelGenerator;

internal class ModelGeneratorConfig
{

    [YamlMember("modelRoot")]
    public string ModelRoot { get; set; } = "./";

    [YamlMember("openApi")]
    public List<OpenApiConfig> OpenApi { get; set; } = new();

    [YamlMember("database")]
    public List<DatabaseConfig> Database { get; set; } = new();
}
