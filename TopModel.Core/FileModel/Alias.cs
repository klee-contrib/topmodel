#nullable disable

namespace TopModel.Core.FileModel;

internal class Alias
{
    public Reference File { get; set; }

    public List<ClassReference> Classes { get; set; } = new();

    public List<Reference> Endpoints { get; set; } = new();

    public ModelFile ModelFile { get; set; }

    internal Reference Location { get; set; }
}