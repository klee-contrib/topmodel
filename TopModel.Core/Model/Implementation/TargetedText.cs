namespace TopModel.Core.Model.Implementation;

public class TargetedText
{
    public required StringWithVariables Text { get; set; }

    public Target Target { get; set; } = Target.Persisted_Dto;

    public List<StringWithVariables> Imports { get; set; } = [];
}