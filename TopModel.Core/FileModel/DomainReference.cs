using YamlDotNet.Core.Events;

namespace TopModel.Core.FileModel;

internal class DomainReference : Reference
{
    public DomainReference(Scalar scalar)
        : base(scalar)
    {
    }
}