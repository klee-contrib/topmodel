using TopModel.Core.FileModel;

namespace TopModel.Core.Model;

internal class ReverseAssociationDefinition(Reference location) : IAnnotationContainer
{
    public required AssociationProperty Property { get; set; }

    public string? ClassName { get; set; }

    public string? Label { get; set; }

    public string? Comment { get; set; }

    public IList<AnnotationInstance> Annotations { get; } = [];

    public IList<AnnotationReference> AnnotationReferences { get; internal set; } = [];

    public IList<AnnotationInstance> ExcludedAnnotations { get; } = [];

    public IList<AnnotationReference> ExcludedAnnotationReferences { get; internal set; } = [];

    internal Reference Location { get; } = location;

    public override string ToString()
    {
        return $"Reverse{Property?.Name}";
    }
}
