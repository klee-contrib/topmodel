using TopModel.Core.FileModel;

namespace TopModel.Core.Model;

public class TemplateParameter
{
    public required LocatedString Name { get; init; }

    public required string Comment { get; init; }

    public required bool Required { get; init; }

    public required string DefaultValue { get; init; }

    public Annotation? Annotation { get; internal set; }

    public Decorator? Decorator { get; internal set; }

    public Domain? Domain { get; internal set; }

    public string Description => $"**{Name}** ({Comment})";
}
