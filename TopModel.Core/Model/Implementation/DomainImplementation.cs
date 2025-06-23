namespace TopModel.Core.Model.Implementation;

public class DomainImplementation
{
    public string? Type => TypeWithVariables;

    public string? GenericType => GenericTypeWithVariables;

    public List<StringWithVariables> Imports { get; set; } = [];

    public List<TargetedText> Annotations { get; set; } = [];

    public IDictionary<string, ValueTemplate> ValueTemplates { get; set; } = new Dictionary<string, ValueTemplate>();

    internal StringWithVariables? TypeWithVariables { get; set; }

    internal StringWithVariables? GenericTypeWithVariables { get; set; }

    public ValueTemplate? GetValueTemplate(string value)
    {
        return ValueTemplates.TryGetValue(value, out var v)
           ? v
           : ValueTemplates.TryGetValue(ValueTemplate.Default, out var v2)
               ? v2
               : null;
    }
}