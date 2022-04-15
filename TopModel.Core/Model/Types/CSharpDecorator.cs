namespace TopModel.Core.Types;

public class CSharpDecorator
{
    public string? Extends { get; set; }

    public string? Implements { get; set; }

    public IList<string> Annotations { get; set; } = new List<string>();

    public IList<string> Usings { get; set; } = new List<string>();
}