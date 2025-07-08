#nullable disable

using Microsoft.OpenApi.Models.Interfaces;

namespace TopModel.ModelGenerator;

public class TmdProperty
{
    public const string DefaultComment = "Non documenté";

    public string Name { get; set; }

    public IOpenApiSchema CompositionReference { get; set; }

    public TmdClass Class { get; set; }

    public string Comment { get; set; } = DefaultComment;

    public string Domain { get; set; }

    public bool Required { get; set; }
}
