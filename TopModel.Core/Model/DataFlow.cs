using TopModel.Core.FileModel;

namespace TopModel.Core.Model;

public class DataFlow(DeclarationReference declarationReference)
{
    public required ModelFile ModelFile { get; init; }

    public LocatedString Name { get; internal set; } = null!;

    public string Target { get; internal set; } = null!;

    public Class Class { get; internal set; } = null!;

    public ClassReference ClassReference { get; internal set; } = null!;

    public DataFlowType Type { get; internal set; }

    public IList<DataFlow> DependsOn { get; } = [];

    public IList<DataFlowReference> DependsOnReference { get; internal set; } = [];

    public IList<FlowHook> Hooks { get; internal set; } = [];

    public IProperty? ActiveProperty { get; internal set; }

    public PropertyReference? ActivePropertyReference { get; internal set; }

    public IList<DataFlowSource> Sources { get; } = [];

    internal DeclarationReference DeclarationReference { get; } = declarationReference;

    public override string ToString()
    {
        return Name;
    }
}
