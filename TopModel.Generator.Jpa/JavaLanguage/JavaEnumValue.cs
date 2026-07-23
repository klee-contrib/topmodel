namespace TopModel.Generator.Jpa;

public class JavaEnumValue(string name)
{
    public string Name { get; } = name;
    public string Comment { get; set; } = string.Empty;

    public IReadOnlyList<string> Imports { get; init; } = [];

    public IList<string> Parameters { get; } = [];
}
