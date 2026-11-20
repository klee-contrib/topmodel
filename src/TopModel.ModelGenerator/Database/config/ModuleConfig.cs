namespace TopModel.ModelGenerator.Database;

public class ModuleConfig
{
    public required string Name { get; set; }

    public IList<string> Classes { get; set; } = [];

    public IList<string> Tags { get; set; } = [];
}
