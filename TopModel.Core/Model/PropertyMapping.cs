#nullable disable
using TopModel.Core.FileModel;

namespace TopModel.Core;

public class PropertyMapping
{
    public IProperty Property { get; set; }

    public IProperty TargetProperty { get; set; }

    public Reference TargetPropertyReference { get; set; }

    public FromMapper FromMapper { get; set; }
}
