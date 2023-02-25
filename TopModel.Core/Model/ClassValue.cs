#nullable disable
namespace TopModel.Core;

public class ClassValue
{
    public string Name { get; set; }

    public Class Class { get; set; }

    public Dictionary<IFieldProperty, string> Value { get; } = new();

    public string ResourceKey => $"{Class.Namespace.ModuleFirstLower}.{Class.Name.ToFirstLower()}.values.{Name}";
}