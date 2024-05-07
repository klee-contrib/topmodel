namespace TopModel.ModelGenerator.Database;

public class ModuleConfig
{
    public required string Name { get; set; }

    public List<string> Classes { get; set; } = [];
}
