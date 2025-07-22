using YamlDotNet.Core.Events;

namespace TopModel.Core.FileModel;

public class AnnotationReference : Reference
{
    internal AnnotationReference(Scalar scalar)
        : base(scalar)
    {
    }

    public Dictionary<ParameterReference, StringWithVariables> ParameterReferences { get; set; } = [];
}