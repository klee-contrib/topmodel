using TopModel.Core.FileModel;

namespace TopModel.Core.Model;

public interface IAnnotationContainer
{
    IList<AnnotationInstance> Annotations { get; }

    IList<AnnotationReference> AnnotationReferences { get; }

    IList<AnnotationInstance> ExcludedAnnotations { get; }

    IList<AnnotationReference> ExcludedAnnotationReferences { get; }
}
