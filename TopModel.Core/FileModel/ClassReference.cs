using YamlDotNet.Core.Events;

namespace TopModel.Core.FileModel;

public class ClassReference : Reference
{
    internal ClassReference()
    {
    }

    internal ClassReference(Scalar scalar)
        : base(scalar)
    {
    }
}