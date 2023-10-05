#nullable disable
using TopModel.Core.FileModel;

namespace TopModel.Core;

public class ClassValue
{
    public string Name { get; set; }

    public Class Class { get; set; }

    public Reference Reference { get; set; }

    public Dictionary<IFieldProperty, string> Value { get; } = new();

    public string ResourceKey => $"{Class.Namespace.ModuleCamel}.{Class.NameCamel}.values.{Name}";

    public string GetLabel(Class classe)
    {
        return classe.DefaultProperty != null
            ? Value[classe.DefaultProperty]
            : Name;
    }
}