using YamlDotNet.Core.Events;

namespace TopModel.Core.FileModel;

public abstract class Reference : Location
{
    private protected Reference()
        : base() { }

    private protected Reference(Scalar scalar)
        : base(scalar) { }
}
