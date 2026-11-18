using YamlDotNet.Core.Events;

namespace TopModel.Core.FileModel;

public class PropertyReference : Reference
{
    internal PropertyReference()
        : base() { }

    internal PropertyReference(Scalar scalar)
        : base(scalar) { }
}
