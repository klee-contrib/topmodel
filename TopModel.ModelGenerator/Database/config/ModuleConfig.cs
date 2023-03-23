#nullable disable

namespace TopModel.ModelGenerator.Database;

public class ModuleConfig
{
    public string Name { get; set; }

    public List<string> Classes { get; set; } = new();
}
