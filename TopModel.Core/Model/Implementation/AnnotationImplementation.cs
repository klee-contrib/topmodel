namespace TopModel.Core.Model.Implementation;

public class AnnotationImplementation
{
    public required StringWithVariables Text { get; set; }

    public Target Target { get; set; } = Target.Persisted_Dto;

    public List<StringWithVariables> Imports { get; set; } = [];
}