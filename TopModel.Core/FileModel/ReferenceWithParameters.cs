using YamlDotNet.Core.Events;

namespace TopModel.Core.FileModel;

public abstract class ReferenceWithParameters : Reference
{
    private protected ReferenceWithParameters(Scalar scalar)
        : base(scalar) { }

    public IDictionary<ParameterReference, StringWithVariables> ParameterReferences { get; internal set; } =
        new Dictionary<ParameterReference, StringWithVariables>();
}
