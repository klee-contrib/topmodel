#nullable disable

namespace TopModel.Core.Model.Implementation;

public class DomainType
{
    public string Default { get; set; }

    public string AsTransformed { get; set; }

    public string Enum { get; set; } = "{enum}";

    public string Composition { get; set; } = "{composition.name}";
}
