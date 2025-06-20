using YamlDotNet.Core.Events;

namespace TopModel.Core.FileModel;

public class ParameterReference : Reference
{
    internal ParameterReference()
        : base()
    {
    }

    internal ParameterReference(Scalar scalar)
        : base(scalar)
    {
    }
}