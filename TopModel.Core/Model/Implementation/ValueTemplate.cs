using TopModel.Core.FileModel;

namespace TopModel.Core.Model.Implementation;

public class ValueTemplate
{
    public const string Default = "$$default$$";

#nullable disable
    public StringWithVariables Value { get; set; }

#nullable enable

    public IList<StringWithVariables> Imports { get; set; } = [];
}
