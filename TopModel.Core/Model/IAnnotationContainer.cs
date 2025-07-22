using TopModel.Core.FileModel;

namespace TopModel.Core;

public interface IAnnotationContainer
{
    IList<AnnotationInstance> Annotations { get; }

    IList<AnnotationReference> AnnotationReferences { get; }
}
