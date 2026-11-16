using YamlDotNet.Core.Events;

namespace TopModel.Core.FileModel;

public class DomainReference : ReferenceWithParameters
{
    internal DomainReference(Scalar scalar)
        : base(scalar) { }
}
