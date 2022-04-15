namespace TopModel.Core.Types;

public class JavaDecorator
{
    public string? Extends { get; set; }

    public string? Implements { get; set; }

    public IList<string> Annotations { get; set; } = new List<string>();

    public IList<string> Imports { get; set; } = new List<string>();
}