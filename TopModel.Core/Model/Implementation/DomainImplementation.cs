using TopModel.Core.FileModel;

namespace TopModel.Core.Model.Implementation;

public class DomainImplementation
{
    public StringWithVariables? Type { get; internal set; }

    public StringWithVariables? GenericType { get; internal set; }

    public IList<StringWithVariables> Imports { get; internal set; } = [];

    public string? Collector { get; internal set; }

    public IDictionary<string, ValueTemplate> ValueTemplates { get; } = new Dictionary<string, ValueTemplate>();

    public ValueTemplate? GetValueTemplate(string value)
    {
        return ValueTemplates.TryGetValue(value, out var v) ? v
            : ValueTemplates.TryGetValue(ValueTemplate.Default, out var v2) ? v2
            : null;
    }
}
