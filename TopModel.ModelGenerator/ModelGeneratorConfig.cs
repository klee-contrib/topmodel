using SharpYaml.Serialization;
using TopModel.ModelGenerator.OpenApi;

namespace TopModel.ModelGenerator;

internal class ModelGeneratorConfig
{
    [YamlMember("outputDirectory")]
    public string OutputDirectory { get; set; } = "./";

    [YamlMember("openApi")]
    public OpenApiConfig? OpenApi { get; set; }
}
