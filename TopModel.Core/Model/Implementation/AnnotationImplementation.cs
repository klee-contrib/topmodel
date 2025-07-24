namespace TopModel.Core.Model.Implementation;

public class AnnotationImplementation
{
    public required StringWithVariables Text { get; set; }

    public List<AnnotationConstraint> When { get; set; } = [];

    public List<StringWithVariables> Imports { get; set; } = [];
}