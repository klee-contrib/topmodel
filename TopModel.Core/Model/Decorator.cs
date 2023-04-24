using TopModel.Core.FileModel;
using TopModel.Core.Model.Implementation;
using TopModel.Utils;

namespace TopModel.Core;

public class Decorator : IPropertyContainer
{
#nullable disable
    public LocatedString Name { get; set; }

    public string NamePascal => Name.Value.ToPascalCase();

    public string NameCamel => Name.Value.ToCamelCase();

    public string Description { get; set; }
#nullable enable

    public Dictionary<string, DecoratorImplementation> Implementations { get; set; } = new();

#nullable disable
    public ModelFile ModelFile { get; set; }

    public Namespace Namespace { get; set; }

    public IList<IProperty> Properties { get; } = new List<IProperty>();

    public bool PreservePropertyCasing { get; set; }

    internal Reference Location { get; set; }

    public override string ToString()
    {
        return Name;
    }
}
