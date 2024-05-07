using TopModel.Core;

namespace TopModel.Generator;

public class ReverseAssociationProperty : AssociationProperty
{
    public required AssociationProperty ReverseProperty { get; set; }
}