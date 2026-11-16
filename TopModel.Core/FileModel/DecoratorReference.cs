using YamlDotNet.Core.Events;

namespace TopModel.Core.FileModel;

public class DecoratorReference : ReferenceWithParameters
{
    internal DecoratorReference(Scalar scalar)
        : base(scalar) { }
}
