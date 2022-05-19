#nullable disable
namespace TopModel.Core;

public class ReferenceValue
{
    public string Name { get; set; }

    public Dictionary<IFieldProperty, string> Value { get; } = new();
}