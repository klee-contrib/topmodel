using TopModel.Core.FileModel;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Core.Model;

public interface IProperty : IAnnotationContainer
{
    string Name => NameSegments.Name;

    IList<NameSegment> NameSegments { get; }

    IList<NameSegment> PropertyNameSegments { get; }

    string NamePascal => Parent.PreservePropertyCasing ? Name : NameSegments.NamePascal;

    string NameCamel => Parent.PreservePropertyCasing ? Name : NamePascal.ToFirstLower();

    string PropertyNamePascal => Parent.PreservePropertyCasing ? Name : PropertyNameSegments.NamePascal;

    string PropertyNameCamel => Parent.PreservePropertyCasing ? Name : PropertyNamePascal.ToFirstLower();

    string SqlName { get; }

    string? Label { get; }

    bool PrimaryKey { get; }

    bool Required { get; }

    DomainInstance Domain { get; }

    DomainReference? DomainReference { get; }

    string Comment { get; }

    string? DefaultValue { get; }

    bool Readonly { get; }

    LocatedString? Trigram { get; }

    ParamLocation? ParamLocation { get; }

    string? FinalTrigram { get; }

    IDictionary<string, string> CustomProperties { get; }

    IList<string>? Tags { get; }

    Class Class { get; internal set; }

    Endpoint? Endpoint { get; internal set; }

    Decorator? Decorator { get; internal set; }

    IProperty? SourceProperty { get; }

    IPropertyContainer? SourceContainer => SourceProperty?.Parent;

    PropertyMapping? PropertyMapping { get; internal set; }

    IPropertyContainer Parent =>
        Class ?? (IPropertyContainer)Endpoint! ?? (IPropertyContainer)Decorator! ?? PropertyMapping!;

    IProperty ResourceProperty =>
        SourceContainer != null ? SourceContainer.Properties.First(p => p.Name == Name).ResourceProperty
        : this is AliasProperty { OverridesLabel: false, OriginalProperty: IProperty op } ? op.ResourceProperty
        : this;

    string ResourceKey =>
        $"{ResourceProperty.Parent.Namespace.ModuleCamel}.{ResourceProperty.Parent.NameCamel}.{ResourceProperty.PropertyNameCamel}";

    IProperty CommentResourceProperty =>
        SourceContainer != null ? SourceContainer.Properties.First(p => p.Name == Name).CommentResourceProperty
        : this is AliasProperty { OverridesComment: false, OriginalProperty: IProperty op } ? op.CommentResourceProperty
        : this;

    string CommentResourceKey =>
        $"comments.{CommentResourceProperty.Parent.Namespace.ModuleCamel}.{CommentResourceProperty.Parent.NameCamel}.{CommentResourceProperty.PropertyNameCamel}";

    GeneratedValueDefinition? GeneratedValue => this.Association == null ? Domain?.GeneratedValue : null;

    internal ParamLocation? OwnLocation { get; }

    IProperty CloneDefinition();

    IProperty CloneForContainer(IPropertyContainer container);
}
