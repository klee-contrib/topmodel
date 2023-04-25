using TopModel.Core;

namespace TopModel.Generator;

public class ReverseAssociationProperty : AssociationProperty
{
#nullable disable
    public AssociationProperty ReverseProperty { get; set; }
}