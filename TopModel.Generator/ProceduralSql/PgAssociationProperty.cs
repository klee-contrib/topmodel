using TopModel.Core;

namespace TopModel.Generator.ProceduralSql;

public class PgAssociationProperty : AssociationProperty
{
#nullable disable
    public bool IsReverse { get; set; } = true;

    public AssociationProperty ReverseProperty { get; set; }
}