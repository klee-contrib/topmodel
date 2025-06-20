namespace TopModel.Core.Model.Implementation;

public class ValueTemplate
{
    public const string Default = "$$default$$";

#nullable disable
    public StringWithParameters Value { get; set; }
#nullable enable

    public List<StringWithParameters> Imports { get; set; } = [];
}
