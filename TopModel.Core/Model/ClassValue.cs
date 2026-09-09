using TopModel.Core.FileModel;

namespace TopModel.Core.Model;

public class ClassValue
{
    public const string Null = "null";
    public const string Undefined = "undefined";

    public required string Name { get; init; }

    public required Class Class { get; init; }

    public required Reference Reference { get; init; }

    public IDictionary<IProperty, string> Value { get; } = new Dictionary<IProperty, string>();

    public string ResourceKey => $"{Class.Namespace.ModuleCamel}.{Class.NameCamel}.values.{Name}";

    public string GetLabel(Class classe)
    {
        return classe.DefaultProperty != null ? Value[classe.DefaultProperty] : Name;
    }
}
