using TopModel.Core.FileModel;

namespace TopModel.Core;

public class DataFlow
{
#nullable disable
    public LocatedString Name { get; set; }

    public string Target { get; set; }

    public Class Class { get; set; }

    public ClassReference ClassReference { get; set; }

    public DataFlowType Type { get; set; }
#nullable enable

    public List<DataFlow> DependsOn { get; set; } = new();

    public List<Reference> DependsOnReference { get; set; } = new();

    public bool PostQuery { get; set; }

    public bool PreQuery { get; set; }

    public IFieldProperty? ActiveProperty { get; set; }

    public Reference? ActivePropertyReference { get; set; }

    public List<DataFlowSource> Sources { get; set; } = new();
}
