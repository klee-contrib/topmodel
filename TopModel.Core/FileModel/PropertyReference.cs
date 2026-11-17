using YamlDotNet.Core.Events;

namespace TopModel.Core.FileModel;

public class PropertyReference : Reference
{
    internal PropertyReference(Scalar scalar)
        : base(scalar) { }
}
