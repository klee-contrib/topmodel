using YamlDotNet.Core.Events;

namespace TopModel.Core.FileModel;

public class DecoratorReference : Reference
{
    internal DecoratorReference(Scalar scalar)
        : base(scalar)
    {
    }
}