using SharpYaml.Serialization;

namespace TopModel.ModelGenerator.OpenApi;

public class OpenApiConfig
{
    [YamlMember("domains")]
    public IList<OpenApiDomain> Domains { get; set; } = new List<OpenApiDomain>();

    [YamlMember("sources")]
    public Dictionary<string, OpenApiSource> Sources { get; set; } = new();
}
