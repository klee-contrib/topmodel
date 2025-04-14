namespace TopModel.Core;

public class TemplateParameter
{
    public required string Name { get; set; }

    public required string Comment { get; set; }

    public string DefaultValue { get; set; } = string.Empty;
}
