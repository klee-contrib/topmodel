using TopModel.Core.Model;

namespace TopModel.Generator;

public class ReverseAssociationProperty : AssociationProperty
{
    public required AssociationProperty ReverseProperty { get; set; }

    public override Class Association => ReverseProperty.Class;

    public override AssociationType Type =>
        ReverseProperty.Type == AssociationType.OneToMany ? AssociationType.ManyToOne
        : ReverseProperty.Type == AssociationType.ManyToOne ? AssociationType.OneToMany
        : ReverseProperty.Type == AssociationType.OneToOne ? AssociationType.OneToOne
        : AssociationType.ManyToMany;

    public override string? ClassName => ReverseProperty.ReverseClassName;

    public override string? Role => ReverseProperty.Role;

    public override string Comment =>
        $"Association réciproque de {ReverseProperty.Class.NamePascal}.{ReverseProperty.Name}";

    public override bool HasReverse => true;
}
