using YamlDotNet.Serialization;

namespace TopModel.Core.Model.Implementation;

public class DecoratorImplementation
{
    [YamlIgnore]
    public string? Extends => ExtendsWithVariables;

    public IList<StringWithVariables> Implements { get; set; } = [];

    public IList<StringWithVariables> Annotations { get; set; } = [];

    public IList<StringWithVariables> Imports { get; set; } = [];

    [YamlMember(Alias = "extends")]
    public StringWithVariables? ExtendsWithVariables { get; internal set; }
}