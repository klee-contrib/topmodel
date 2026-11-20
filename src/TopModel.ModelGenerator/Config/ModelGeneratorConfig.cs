using TopModel.ModelGenerator.Database;
using TopModel.ModelGenerator.OpenApi;
using TopModel.Utils;

namespace TopModel.ModelGenerator;

public class ModelGeneratorConfig : ConfigBase
{
    public IList<OpenApiConfig> OpenApi { get; set; } = [];

    public IList<DatabaseConfig> Database { get; set; } = [];

    public override ModelGeneratorConfig Init(string rootDir)
    {
        ConfigRoot = rootDir;
        ModelRoot ??= "./";
        LockFileName ??= "tmdgen.lock";
        ModelUtils.CombinePath(rootDir, this, c => c.ModelRoot);
        return this;
    }
}
