namespace TopModel.Core.Model.Implementation;

public class ValueTemplate
{
    public const string Default = "$$default$$";

#nullable disable
    public StringWithVariables Value { get; set; }
#nullable enable

    public List<StringWithVariables> Imports { get; set; } = [];
}
