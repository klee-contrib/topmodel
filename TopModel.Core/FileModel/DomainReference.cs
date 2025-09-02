using YamlDotNet.Core.Events;

namespace TopModel.Core.FileModel;

public class DomainReference : Reference
{
    internal DomainReference(Scalar scalar)
        : base(scalar) { }

    public IDictionary<ParameterReference, StringWithVariables> ParameterReferences { get; set; } =
        new Dictionary<ParameterReference, StringWithVariables>();
}
