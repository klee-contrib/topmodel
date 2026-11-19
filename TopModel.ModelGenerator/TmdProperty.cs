using Microsoft.OpenApi;

namespace TopModel.ModelGenerator;

public class TmdProperty
{
    public string Name { get; set; } = null!;

    public IOpenApiSchema? CompositionReference { get; set; }

    public TmdClass? Class { get; set; }

    public string? Comment { get; set; }

    public string? Domain { get; set; }

    public bool Required { get; set; }

    public string SqlName { get; set; } = string.Empty;
}
