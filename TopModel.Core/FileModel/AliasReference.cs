using YamlDotNet.Core.Events;

namespace TopModel.Core.FileModel;

public class AliasReference
{
    public ClassReference? ClassReference { get; set; }

    public EndpointReference? EndpointReference { get; set; }

    public DecoratorReference? DecoratorReference { get; set; }

    public Reference ContainerReference => ClassReference ?? (EndpointReference as Reference) ?? DecoratorReference!;

    public IList<Reference> IncludeReferences { get; } = [];

    public IList<Reference> ExcludeReferences { get; } = [];

    public void AddExclude(Scalar scalar)
    {
        ExcludeReferences.Add(new Reference(scalar));
    }

    public void AddInclude(Scalar scalar)
    {
        IncludeReferences.Add(new Reference(scalar));
    }
}
