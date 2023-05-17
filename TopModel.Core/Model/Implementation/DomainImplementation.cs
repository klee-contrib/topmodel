namespace TopModel.Core.Model.Implementation;

public class DomainImplementation
{
#nullable disable
    public DomainType Type { get; set; }

    public List<string> Imports { get; set; } = new List<string>();

    public List<TargetedText> Annotations { get; set; } = new List<TargetedText>();
}