using TopModel.Core.FileModel;

namespace TopModel.Core.Model.Implementation;

public class DecoratorImplementation
{
    public StringWithVariables? Extends { get; init; }

    public IList<StringWithVariables> Implements { get; init; } = [];

    public IList<StringWithVariables> Imports { get; init; } = [];
}
