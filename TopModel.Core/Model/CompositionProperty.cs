#nullable disable
using TopModel.Core.FileModel;

namespace TopModel.Core;

public class CompositionProperty : IProperty
{
    public Class Composition { get; set; }

    public string Name { get; set; }

    public string Kind { get; set; }

#nullable enable
    public Domain? DomainKind { get; set; }
#nullable disable

    public string Comment { get; set; }

    public Class Class { get; set; }

    public Endpoint Endpoint { get; set; }

    public string Label => Name;

    public bool PrimaryKey => false;

    public ClassReference Reference { get; set; }

    internal Reference Location { get; set; }

#nullable enable
    internal DomainReference? DomainKindReference { get; set; }
}