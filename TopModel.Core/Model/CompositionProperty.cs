#nullable disable
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
}