namespace TopModel.Core.Types;

public class JavaDecorator
{
    public string? Extends { get; set; }

    public IList<string> Implements { get; set; } = new List<string>();

    public IList<string> Annotations { get; set; } = new List<string>();

    public IList<string> Imports { get; set; } = new List<string>();
}