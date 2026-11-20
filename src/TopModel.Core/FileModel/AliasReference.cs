using YamlDotNet.Core.Events;

namespace TopModel.Core.FileModel;

public class AliasReference
{
    public ClassReference? ClassReference { get; internal set; }

    public EndpointReference? EndpointReference { get; internal set; }

    public DecoratorReference? DecoratorReference { get; internal set; }

    public Reference ContainerReference => ClassReference ?? (EndpointReference as Reference) ?? DecoratorReference!;

    public IList<PropertyReference> IncludeReferences { get; } = [];

    public IList<PropertyReference> ExcludeReferences { get; } = [];

    public void AddExclude(Scalar scalar)
    {
        ExcludeReferences.Add(new PropertyReference(scalar));
    }

    public void AddInclude(Scalar scalar)
    {
        IncludeReferences.Add(new PropertyReference(scalar));
    }
}
