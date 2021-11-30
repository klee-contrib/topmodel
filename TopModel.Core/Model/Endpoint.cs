using TopModel.Core.FileModel;

namespace TopModel.Core;

public class Endpoint
{
    public Namespace Namespace { get; set; }

#nullable disable
    public ModelFile ModelFile { get; set; }

    public string Name { get; set; }

    public string Method { get; set; }

    public string Route { get; set; }

    public string Description { get; set; }
#nullable enable

    public IProperty? Returns { get; set; }

    public IList<IProperty> Params { get; set; } = new List<IProperty>();

#nullable disable
    internal Reference Location { get; set; }
#nullable enable
}