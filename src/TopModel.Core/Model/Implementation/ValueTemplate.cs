using TopModel.Core.FileModel;

namespace TopModel.Core.Model.Implementation;

public class ValueTemplate
{
    public const string Default = "$$default$$";

    public StringWithVariables Value { get; internal set; } = null!;

    public IList<StringWithVariables> Imports { get; } = [];
}
