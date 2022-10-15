using TopModel.Utils;

namespace TopModel.Core;

public interface IFieldProperty : IProperty
{
    bool Required { get; }

    Domain Domain { get; }

    string? DefaultValue { get; }

    public LocatedString? Trigram { get; set; }

    IFieldProperty ResourceProperty => this is AliasProperty alp && alp.Label == alp.OriginalProperty!.Label
        ? alp.OriginalProperty!.ResourceProperty
        : this;

    string ResourceKey => $"{string.Join('.', ResourceProperty.Class.Namespace.Module.Split('.').Select(e => e.ToFirstLower()))}.{ResourceProperty.Class.Name.ToFirstLower()}.{ResourceProperty.Name.ToFirstLower()}";

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
                { Class.Extends: not null, PrimaryKey: true } => prop.Name.Replace(prop.Class.Name, string.Empty).ToSnakeCase(),
                _ => prop.Name.ToSnakeCase()
            };

            string? prefix = prop.Trigram ?? (apPk != null ? apPkTrigram : prop.Class.Trigram);
            prefix = !string.IsNullOrWhiteSpace(prefix) ? $"{prefix}_" : string.Empty;
            var suffix = prop is AssociationProperty { Role: string role } ? $"_{role.Replace(" ", "_").ToUpper()}" : string.Empty;

            return $"{prefix}{sqlName}{suffix}";
        }
    }

    string JavaName => this is AssociationProperty ap
        ? (ap.Association.Trigram?.ToLower().ToFirstUpper() ?? string.Empty) + ap.Property.Name + (ap.Role?.Replace(" ", string.Empty) ?? string.Empty)
        : (Class.Trigram?.ToLower().ToFirstUpper() ?? string.Empty) + Name;
}