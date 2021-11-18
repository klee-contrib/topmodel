using YamlDotNet.Core.Events;

namespace TopModel.Core.FileModel;

internal class DomainRelation : Relation
{
    public DomainRelation(Scalar scalar)
        : base(scalar)
    {
    }
}