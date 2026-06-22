using TopModel.Core.FileModel;

namespace TopModel.Core.Model;

public class GeneratedValueDefinition
{
    public StringWithVariables? SequenceName { get; set; }

    public int Start { get; set; } = 1;

    public int Increment { get; set; } = 1;
}
