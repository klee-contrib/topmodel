using YamlDotNet.Core.Events;

namespace TopModel.Core.FileModel;

public class DomainReference : Reference
{
    internal DomainReference(Scalar scalar)
        : base(scalar)
    {
    }

    public Dictionary<ParameterReference, StringWithVariables> ParameterReferences { get; set; } = [];
}