namespace TopModel.Core.Model.Implementation;

public class TargetedText
{
    public required string Text { get; set; }

    public Target Target { get; set; } = Target.Persisted_Dto;

    public List<string> Imports { get; set; } = [];
}