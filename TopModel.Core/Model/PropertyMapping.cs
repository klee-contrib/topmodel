#nullable disable
using TopModel.Core.FileModel;

namespace TopModel.Core;

public class PropertyMapping : IPropertyContainer
{
    public IProperty Property { get; set; }

    public IProperty TargetProperty { get; set; }

    public Reference TargetPropertyReference { get; set; }

    public FromMapper FromMapper { get; set; }

    public ModelFile ModelFile => throw new NotImplementedException();

    public LocatedString Name => throw new NotImplementedException();

    public string NamePascal => Property.NamePascal;

    public string NameCamel => Property.NameCamel;

    public Namespace Namespace => throw new NotImplementedException();

    public IList<IProperty> Properties => [Property];

    public bool PreservePropertyCasing => false;
}
