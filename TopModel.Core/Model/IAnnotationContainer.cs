using TopModel.Core.FileModel;

namespace TopModel.Core;

public interface IAnnotationContainer
{
    IList<(Annotation Annotation, StringWithVariables[] Parameters)> Annotations { get; }

    IList<AnnotationReference> AnnotationReferences { get; set; }
}
