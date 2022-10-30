namespace TopModel.Core.Types;

public class TargetedText
{
#nullable disable
    public string Text { get; set; }

    public Target Target { get; set; } = Target.Persisted_Dto;

    public List<string> Imports { get; set; } = new List<string>();

    public List<string> Usings { get; set; } = new List<string>();
}