namespace TopModel.Core;

public class TemplateParameter
{
    public required LocatedString Name { get; set; }

    public required string Comment { get; set; }

    public bool Required { get; set; }

    public string DefaultValue { get; set; } = string.Empty;

    public Decorator? Decorator { get; set; }

    public Domain? Domain { get; set; }
}
