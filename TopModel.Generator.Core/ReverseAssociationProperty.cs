using TopModel.Core;

namespace TopModel.Generator;

public class ReverseAssociationProperty : AssociationProperty
{
#nullable disable
    public bool IsReverse { get; set; } = true;

    public AssociationProperty ReverseProperty { get; set; }
}