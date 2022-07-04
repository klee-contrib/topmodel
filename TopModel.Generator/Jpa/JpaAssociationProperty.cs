using TopModel.Core;

namespace TopModel.Generator.Jpa;

public class JpaAssociationProperty : AssociationProperty
{
#nullable disable
    public bool IsReverse { get; set; } = true;

    public AssociationProperty ReverseProperty { get; set; }
}