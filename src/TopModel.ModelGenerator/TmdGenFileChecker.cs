using TopModel.Utils;

namespace TopModel.ModelGenerator;

public class TmdGenFileChecker() : AbstractFileChecker<ModelGeneratorConfig>("schema.tmdgen.config.json")
{
    public override ModelGeneratorConfig DeserializeConfig(string yaml)
    {
        return Deserialize<ModelGeneratorConfig>(yaml);
    }
}
