using TopModel.ModelGenerator.Database;
using TopModel.ModelGenerator.OpenApi;

namespace TopModel.ModelGenerator;

internal class ModelGeneratorConfig
{
    public string ModelRoot { get; set; } = "./";

    public string LockFileName { get; set; } = "tmdgen.lock";

    public List<OpenApiConfig> OpenApi { get; set; } = new();

    public List<DatabaseConfig> Database { get; set; } = new();
}
