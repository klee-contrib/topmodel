namespace TopModel.Core.Model.Implementation;

public class DomainImplementation
{
    public StringWithVariables? Type { get; set; }

    public StringWithVariables? GenericType { get; set; }

    public List<StringWithVariables> Imports { get; set; } = [];

    public IDictionary<string, ValueTemplate> ValueTemplates { get; set; } = new Dictionary<string, ValueTemplate>();

    public ValueTemplate? GetValueTemplate(string value)
    {
        return ValueTemplates.TryGetValue(value, out var v)
           ? v
           : ValueTemplates.TryGetValue(ValueTemplate.Default, out var v2)
               ? v2
               : null;
    }
}