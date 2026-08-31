using TopModel.Core.FileModel;

namespace TopModel.Core.Model;

internal class ReverseAssociationProperty() : AssociationProperty(null!)
{
#pragma warning disable CS8765
    public override required AssociationProperty ReverseProperty { get; set; }
#pragma warning restore CS8765

    public override Class Association => ReverseProperty.Class;

    public override bool Multiple => !ReverseProperty.Multiple && !ReverseProperty.Unique;

    public override string As => ReverseProperty.As;

    public override bool Required => !ReverseProperty.Unique && ReverseProperty.Required;

    public override string? ClassName => ReverseProperty.WithReverse?.ClassName;

    public override string? Role => ReverseProperty.Role;

    public override string? Label => ReverseProperty.WithReverse?.Label;

    public override string Comment =>
        ReverseProperty.WithReverse?.Comment
        ?? $"Association réciproque de {ReverseProperty.Class.NamePascal}.{ReverseProperty.Name}";

    public override ReverseAssociationDefinition WithReverse => new(ReverseProperty.Location) { Property = this };

    public override IList<AnnotationInstance> Annotations => ReverseProperty.WithReverse?.Annotations ?? [];

    public override IList<AnnotationReference> AnnotationReferences =>
        ReverseProperty.WithReverse?.AnnotationReferences ?? [];

    public override IList<AnnotationInstance> ExcludedAnnotations =>
        ReverseProperty.WithReverse?.ExcludedAnnotations ?? [];

    public override IList<AnnotationReference> ExcludedAnnotationReferences =>
        ReverseProperty.WithReverse?.ExcludedAnnotationReferences ?? [];

    public override bool UseClass => ReverseProperty.UseClass;

    internal override bool UseLegacyRoleName => ReverseProperty.UseLegacyRoleName;

    internal override bool DefaultAssociationUseClass => ReverseProperty.DefaultAssociationUseClass;
}
