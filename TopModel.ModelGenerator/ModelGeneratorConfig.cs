using SharpYaml.Serialization;
using TopModel.ModelGenerator.OpenApi;

namespace TopModel.ModelGenerator;

internal class ModelGeneratorConfig
{

    [YamlMember("modelRoot")]
    public string ModelRoot { get; set; } = "./";

    [YamlMember("openApi")]
    public OpenApiConfig? OpenApi { get; set; }
}
