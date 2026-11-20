using YamlDotNet.Core.Events;

namespace TopModel.Core.FileModel;

public class Declaration : Location
{
    internal Declaration()
        : base() { }

    internal Declaration(PropertyReference reference)
        : base(reference.Scalar) { }

    internal Declaration(Scalar scalar)
        : base(scalar) { }
}
