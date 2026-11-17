using YamlDotNet.Core.Events;

namespace TopModel.Core.FileModel;

public class DeclarationReference : Reference
{
    internal DeclarationReference()
        : base() { }

    internal DeclarationReference(PropertyReference reference)
        : base(reference.Scalar) { }

    internal DeclarationReference(Scalar scalar)
        : base(scalar) { }
}
