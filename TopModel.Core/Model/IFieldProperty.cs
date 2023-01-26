using TopModel.Utils;

namespace TopModel.Core;

public interface IFieldProperty : IProperty
{
    bool Required { get; }

    Domain Domain { get; }

    string? DefaultValue { get; }

    public LocatedString? Trigram { get; set; }

    IFieldProperty ResourceProperty => this is AliasProperty alp && alp.Label == alp.OriginalProperty?.Label
        ? alp.OriginalProperty!.ResourceProperty
        : this;

    string ResourceKey => $"{string.Join('.', ResourceProperty.Class.Namespace.Module.Split('.').Select(e => e.ToFirstLower()))}.{ResourceProperty.Class.Name.ToFirstLower()}.{ResourceProperty.Name.ToFirstLower()}";

    IFieldProperty CommentResourceProperty => this is AliasProperty alp && alp.Comment == alp.OriginalProperty?.Comment
        ? alp.OriginalProperty!.CommentResourceProperty
        : this;

    string CommentResourceKey => $"comments.{string.Join('.', CommentResourceProperty.Class.Namespace.Module.Split('.').Select(e => e.ToFirstLower()))}.{CommentResourceProperty.Class.Name.ToFirstLower()}.{CommentResourceProperty.Name.ToFirstLower()}";

    string SqlName
    {
        get
        {
            var prop = !Class.IsPersistent && this is AliasProperty alp ? alp.Property : this;

            var apPk = prop is AssociationProperty ap
                ? ap.Association switch
                {
                    { PrimaryKey: IFieldProperty pk } => pk,
                    Class classe => classe.Properties.OfType<IFieldProperty>().FirstOrDefault()
                }
                : null;

            var apPkTrigram = apPk?.Trigram ?? apPk?.Class.Trigram;

            var sqlName = prop switch
            {
                AssociationProperty => apPkTrigram != null ? apPk?.SqlName.Replace($"{apPkTrigram}_", string.Empty) : apPk?.SqlName,
                { Class.Extends: not null, PrimaryKey: true } => prop.Name.Replace(prop.Class.Name, string.Empty).ToConstantCase(),
                _ => prop.Name.ToConstantCase()
            };

            string? prefix = prop.Trigram ?? (apPk != null ? apPkTrigram : prop.Class.Trigram);
            prefix = !string.IsNullOrWhiteSpace(prefix) ? $"{prefix}_" : string.Empty;
            var suffix = prop is AssociationProperty { Role: string role }
                ? UseLegacyRoleName
                    ? $"_{role.Replace(" ", "_").ToUpper()}"
                    : $"_{role.ToConstantCase()}"
                : string.Empty;

            return $"{prefix}{sqlName}{suffix}";
        }
    }

    bool UseLegacyRoleName { get; init; }
}