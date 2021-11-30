using YamlDotNet.Core.Events;

namespace TopModel.Core.FileModel;

internal class ClassReference : Reference
{
    public ClassReference()
    {
    }

    public ClassReference(Scalar scalar)
        : base(scalar)
    {
    }
}