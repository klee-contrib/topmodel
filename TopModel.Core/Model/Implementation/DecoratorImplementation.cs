namespace TopModel.Core.Model.Implementation;

public class DecoratorImplementation
{
    public StringWithVariables? Extends { get; set; }

    public IList<StringWithVariables> Implements { get; set; } = [];

    public IList<StringWithVariables> Annotations { get; set; } = [];

    public IList<StringWithVariables> Imports { get; set; } = [];
}