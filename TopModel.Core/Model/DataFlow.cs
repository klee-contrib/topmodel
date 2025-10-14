using TopModel.Core.FileModel;

namespace TopModel.Core.Model;

public class DataFlow
{
#nullable disable
    public ModelFile ModelFile { get; set; }

    public Reference Location { get; set; }

    public LocatedString Name { get; set; }

    public string Target { get; set; }

    public Class Class { get; set; }

    public ClassReference ClassReference { get; set; }

    public DataFlowType Type { get; set; }

#nullable enable

    public IList<DataFlow> DependsOn { get; set; } = [];

    public IList<DataFlowReference> DependsOnReference { get; set; } = [];

    public IList<FlowHook> Hooks { get; set; } = [];

    public IProperty? ActiveProperty { get; set; }

    public Reference? ActivePropertyReference { get; set; }

    public IList<DataFlowSource> Sources { get; set; } = [];

    public override string ToString()
    {
        return Name;
    }
}
