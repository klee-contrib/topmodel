using TopModel.Core.FileModel;

namespace TopModel.Core.Model.Implementation;

public class AnnotationImplementation
{
    public required StringWithVariables Text { get; init; }

    public IList<AnnotationConstraint> When { get; init; } = [];

    public IList<StringWithVariables> Imports { get; init; } = [];
}
