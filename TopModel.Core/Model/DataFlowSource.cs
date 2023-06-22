using TopModel.Core.FileModel;

namespace TopModel.Core;

#nullable disable

public class DataFlowSource
{
    public string Source { get; set; }

    public Class Class { get; set; }

    public ClassReference ClassReference { get; set; }

    public DataFlowSourceMode Mode { get; set; }

    public List<IFieldProperty> JoinProperties { get; set; } = new();

    public List<Reference> JoinPropertyReferences { get; set; } = new();

    public bool InnerJoin { get; set; }
}