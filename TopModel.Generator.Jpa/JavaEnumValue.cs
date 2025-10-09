using TopModel.Utils;

namespace TopModel.Generator.Jpa;

public class JavaEnumValue(string name)
{
    public string Name { get; } = name;

    public IList<string> Parameters { get; } = [];

    public override string ToString()
    {
        return $"{Name.ToConstantCase()}({string.Join(", ", Parameters)})";
    }
}
