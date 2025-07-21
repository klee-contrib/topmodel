using YamlDotNet.Core.Events;

namespace TopModel.Core.FileModel;

public class AnnotationReference : Reference
{
    internal AnnotationReference(Scalar scalar)
        : base(scalar)
    {
    }

    public IList<ParameterReference> ParameterReferences { get; set; } = [];
}