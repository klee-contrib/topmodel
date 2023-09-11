namespace TopModel.Core.Model.Implementation;

public class ValueTemplate
{
    public const string Default = "$$default$$";

#nullable disable
    public string Value { get; set; }
#nullable enable

    public List<string> Imports { get; set; } = new List<string>();
}
