using TopModel.Core.FileModel;

namespace TopModel.Core.Model;

public class GeneratedValueDefinition
{
    public StringWithVariables? SequenceName { get; init; }

    public int Start { get; init; } = 1;

    public int Increment { get; init; } = 1;
}
