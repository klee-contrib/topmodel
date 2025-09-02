using TopModel.Core.FileModel;

namespace TopModel.Core.Model.Implementation;

public class DecoratorImplementation
{
    public StringWithVariables? Extends { get; internal set; }

    public IList<StringWithVariables> Implements { get; set; } = [];

    public IList<StringWithVariables> Imports { get; set; } = [];
}
