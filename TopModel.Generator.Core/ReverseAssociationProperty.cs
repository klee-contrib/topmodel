using TopModel.Core.Model;

namespace TopModel.Generator;

public class ReverseAssociationProperty : AssociationProperty
{
    public required AssociationProperty ReverseProperty { get; set; }
}
