#nullable disable

namespace TopModel.Core.FileModel;

internal class Alias
{
    public string File { get; set; }

    public string[] Classes { get; set; } = Array.Empty<string>();

    public string[] Endpoints { get; set; } = Array.Empty<string>();

    public ModelFile ModelFile { get; set; }

    internal Reference Location { get; set; }
}