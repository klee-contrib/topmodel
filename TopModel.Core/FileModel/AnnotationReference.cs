using YamlDotNet.Core.Events;

namespace TopModel.Core.FileModel;

public class AnnotationReference : ReferenceWithParameters
{
    internal AnnotationReference(Scalar scalar)
        : base(scalar) { }
}
