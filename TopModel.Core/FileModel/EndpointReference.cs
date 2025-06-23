using YamlDotNet.Core.Events;

namespace TopModel.Core.FileModel;

public class EndpointReference : Reference
{
    internal EndpointReference()
    {
    }

    internal EndpointReference(Scalar scalar)
        : base(scalar)
    {
    }
}