using YamlDotNet.Core.Events;

namespace TopModel.Core.FileModel;

public class DecoratorReference : Reference
{
    internal DecoratorReference(Scalar scalar)
        : base(scalar) { }

    public IDictionary<ParameterReference, StringWithVariables> ParameterReferences { get; set; } =
        new Dictionary<ParameterReference, StringWithVariables>();
}
