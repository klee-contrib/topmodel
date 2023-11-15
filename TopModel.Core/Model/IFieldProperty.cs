using TopModel.Utils;

namespace TopModel.Core;

public interface IFieldProperty : IProperty
{
    bool Required { get; }

    string? DefaultValue { get; }

    LocatedString? Trigram { get; set; }

    IFieldProperty ResourceProperty => Decorator != null && Parent != Decorator
        ? Decorator.Properties.OfType<IFieldProperty>().First(p => p.Name == Name).ResourceProperty
        : this is AliasProperty alp && alp.Label == alp.OriginalProperty?.Label
        ? alp.OriginalProperty!.ResourceProperty
        : this;

    string ResourceKey => $"{ResourceProperty.Parent.Namespace.ModuleCamel}.{ResourceProperty.Parent.NameCamel}.{ResourceProperty.NameCamel}";

    IFieldProperty CommentResourceProperty => Decorator != null && Parent != Decorator
        ? Decorator.Properties.OfType<IFieldProperty>().First(p => p.Name == Name).CommentResourceProperty
        : this is AliasProperty alp && alp.Comment == alp.OriginalProperty?.Comment
        ? alp.OriginalProperty!.CommentResourceProperty
        : this;

    string CommentResourceKey => $"comments.{CommentResourceProperty.Parent.Namespace.ModuleCamel}.{CommentResourceProperty.Parent.NameCamel}.{CommentResourceProperty.NameCamel}";

    string SqlName
    {
        get
        {
            var prop = (this as AliasProperty)?.PersistentProperty ?? this;

            var ap = (prop as AssociationProperty) ?? ((prop as AliasProperty)?.Property as AssociationProperty);

            var apPk = ap switch
            {
                { Property: IFieldProperty p } => p,
                { Association: Class classe } => classe.Properties.OfType<IFieldProperty>().FirstOrDefault(),
                _ => null
            };

            var apPkTrigram = apPk?.Trigram ?? apPk?.Class.Trigram;

            var sqlName = prop switch
            {
                AssociationProperty or AliasProperty { Property: AssociationProperty } => apPkTrigram != null ? apPk?.SqlName.Replace($"{apPkTrigram}_", string.Empty) : apPk?.SqlName,
                { Class.Extends: not null, PrimaryKey: true } when Parent.PreservePropertyCasing => prop.Name.Replace(prop.Class.Name, string.Empty),
                { Class.Extends: not null, PrimaryKey: true } => prop.Name.Replace(prop.Class.Name, string.Empty).ToConstantCase(),
                _ when Parent.PreservePropertyCasing => prop.Name,
                _ => prop.Name.ToConstantCase()
            };

            string? prefix = prop.Trigram ?? (apPk != null ? apPkTrigram : prop.Class.Trigram);
            prefix = !string.IsNullOrWhiteSpace(prefix) ? $"{prefix}_" : string.Empty;
            var suffix = ap?.Role != null
                ? UseLegacyRoleName
                    ? $"_{ap.Role.Replace(" ", "_").ToUpper()}"
                    : $"_{ap.Role.ToConstantCase()}"
                : string.Empty;

            return $"{prefix}{sqlName}{suffix}";
        }
    }

    bool UseLegacyRoleName { get; init; }
}