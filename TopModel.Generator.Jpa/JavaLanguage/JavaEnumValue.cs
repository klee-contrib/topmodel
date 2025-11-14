namespace TopModel.Generator.Jpa;

public class JavaEnumValue(string name)
{
    public string Name { get; } = name;
    public string Comment { get; set; } = string.Empty;

    public IList<string> Imports { get; set; } = [];

    public IList<string> Parameters { get; } = [];

    public override string ToString()
    {
        return $"{Name}{(Parameters.Count > 0 ? $"({string.Join(", ", Parameters)})" : string.Empty)}";
    }
}
