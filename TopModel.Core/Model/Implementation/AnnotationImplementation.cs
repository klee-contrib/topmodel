using TopModel.Core.FileModel;

namespace TopModel.Core.Model.Implementation;

public class AnnotationImplementation
{
    public required StringWithVariables Text { get; set; }

    public IList<AnnotationConstraint> When { get; set; } = [];

    public IList<StringWithVariables> Imports { get; set; } = [];
}
