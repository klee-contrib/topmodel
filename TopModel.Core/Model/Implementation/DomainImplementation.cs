namespace TopModel.Core.Model.Implementation;

public class DomainImplementation
{
    public string? Type { get; set; }

    public string? GenericType { get; set; }

    public List<string> Imports { get; set; } = new List<string>();

    public List<TargetedText> Annotations { get; set; } = new List<TargetedText>();
}