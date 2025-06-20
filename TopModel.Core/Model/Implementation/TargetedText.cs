namespace TopModel.Core.Model.Implementation;

public class TargetedText
{
    public required StringWithParameters Text { get; set; }

    public Target Target { get; set; } = Target.Persisted_Dto;

    public List<StringWithParameters> Imports { get; set; } = [];
}