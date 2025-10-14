#nullable disable
using TopModel.Core.FileModel;

namespace TopModel.Core.Model;

public class ClassValue
{
    public string Name { get; set; }

    public Class Class { get; set; }

    public Reference Reference { get; set; }

    public IDictionary<IProperty, string> Value { get; } = new Dictionary<IProperty, string>();

    public string ResourceKey => $"{Class.Namespace.ModuleCamel}.{Class.NameCamel}.values.{Name}";

    public string GetLabel(Class classe)
    {
        return classe.DefaultProperty != null ? Value[classe.DefaultProperty] : Name;
    }
}
