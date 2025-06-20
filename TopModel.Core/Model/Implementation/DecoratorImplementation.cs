namespace TopModel.Core.Model.Implementation;

public class DecoratorImplementation
{
    public StringWithParameters? Extends { get; set; }

    public IList<StringWithParameters> Implements { get; set; } = [];

    public IList<StringWithParameters> Annotations { get; set; } = [];

    public IList<StringWithParameters> Imports { get; set; } = [];
}