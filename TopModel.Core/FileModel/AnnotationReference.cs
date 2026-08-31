using YamlDotNet.Core.Events;

namespace TopModel.Core.FileModel;

public class AnnotationReference : Reference
{
    internal AnnotationReference(Scalar scalar)
        : base(scalar) { }

    public IDictionary<ParameterReference, StringWithVariables> ParameterReferences { get; internal set; } =
        new Dictionary<ParameterReference, StringWithVariables>();
}
