using TopModel.Core.FileModel;

namespace TopModel.Core.Model;

public class ReverseAssociationDefinition : IAnnotationContainer
{
    public required AssociationProperty Property { get; set; }

    public string? ClassName { get; set; }

    public string? Label { get; set; }

    public string? Comment { get; set; }

    public IList<AnnotationInstance> Annotations { get; } = [];

    public IList<AnnotationReference> AnnotationReferences { get; } = [];

    public IList<AnnotationInstance> ExcludedAnnotations { get; } = [];

    public IList<AnnotationReference> ExcludedAnnotationReferences { get; } = [];

    public override string ToString()
    {
        return $"Reverse{Property?.Name}";
    }
}
