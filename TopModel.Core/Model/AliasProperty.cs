using TopModel.Core.FileModel;
using TopModel.Utils;

namespace TopModel.Core;

public class AliasProperty : IProperty
{
    private string? _comment;
    private string? _defaultValue;
    private Domain? _domain;
    private string[]? _domainParameters;
    private string? _label;
    private string? _name;

#nullable disable
    private IProperty _property;
    private bool? _readonly;
    private bool? _required;

    public IProperty Property
    {
        get
        {
            var prop = _property;
            while (prop is AliasProperty alp)
            {
                prop = alp.Property;
            }

            return prop;
        }

        set => _property = value;
    }

    public Class Class { get; set; }

    public Endpoint Endpoint { get; set; }

    public Decorator Decorator { get; set; }

    public PropertyMapping PropertyMapping { get; set; }

#nullable enable

    public LocatedString? Trigram { get; set; }

    public string Name
    {
        get =>
            (Prefix ?? string.Empty)
            + (_name ?? _property?.Name)
            + (Suffix ?? string.Empty);
        set => _name = value;
    }

    public string NamePascal => ((IProperty)this).Parent.PreservePropertyCasing
        ? Name
        : (Prefix?.ToFirstUpper() ?? string.Empty)
            + (_name?.ToPascalCase() ?? _property?.NamePascal)
            + (Suffix ?? string.Empty);

    public string NameCamel => ((IProperty)this).Parent.PreservePropertyCasing
        ? Name
        : (Prefix?.ToFirstLower() ?? string.Empty)
            + (string.IsNullOrWhiteSpace(Prefix)
                ? (_name?.ToCamelCase() ?? _property?.NameCamel)
                : (_name?.ToPascalCase() ?? _property?.NamePascal))
            + (Suffix ?? string.Empty);

    public string NameByClassPascal => Class.IsPersistent ? (Prefix?.ToFirstUpper() ?? string.Empty)
            + (_name?.ToPascalCase() ?? _property?.NameByClassPascal)
            + (Suffix ?? string.Empty) : NamePascal;

    public string NameByClassCamel => Class.IsPersistent ? (Prefix?.ToFirstLower() ?? string.Empty)
            + (string.IsNullOrWhiteSpace(Prefix)
                ? (_name?.ToCamelCase() ?? _property?.NameByClassCamel)
                : (_name?.ToPascalCase() ?? _property?.NameByClassPascal))
            + (Suffix ?? string.Empty) : NameCamel;

    public string? Label
    {
        get => _label ?? _property?.Label;
        set => _label = value;
    }

    public bool PrimaryKey { get; set; }

    public bool Required
    {
        get => _required ?? _property?.Required ?? false;
        set => _required = value;
    }

    public bool Readonly
    {
        get => _readonly ?? _property?.Readonly ?? false;
        set => _readonly = value;
    }

#nullable disable
    public Domain Domain
    {
        get
        {
            var domain = _domain ?? _property?.Domain;
            return As != null ? (domain != null && domain.AsDomains.TryGetValue(As, out var asDomain) ? asDomain : null) : domain;
        }

        set => _domain = value;
    }
#nullable enable

    public string[] DomainParameters
    {
        get => _domainParameters ?? _property?.DomainParameters ?? [];
        set => _domainParameters = value;
    }

    public DomainReference? DomainReference { get; set; }

    public string Comment
    {
        get => _comment ?? _property.Comment;
        set => _comment = value;
    }

    public string? DefaultValue
    {
        get => _defaultValue ?? _property?.DefaultValue;
        set => _defaultValue = value;
    }

    public string? As { get; set; }

    public IProperty? OriginalProperty => _property;

    public IProperty? PersistentProperty => (Class?.IsPersistent ?? false)
        ? this
        : OriginalProperty is AliasProperty op
            ? op.PersistentProperty
            : (OriginalProperty?.Class?.IsPersistent ?? false)
                ? OriginalProperty
                : null;

    public bool AliasedPrimaryKey => (OriginalProperty is AliasProperty op
        ? op.PrimaryKey || op.AliasedPrimaryKey
        : (OriginalProperty?.PrimaryKey ?? false))
        && Prefix == null && Suffix == null;

    public AliasReference? Reference { get; set; }

    public Reference? PropertyReference { get; set; }

    public string? Prefix { get; set; }

    public string? Suffix { get; set; }

#nullable disable
    public bool UseLegacyRoleName { get; init; }

    internal Reference Location { get; set; }

#nullable enable

    internal AliasProperty? OriginalAliasProperty { get; private set; }

    /// <inheritdoc cref="IProperty.CloneWithClassOrEndpoint" />
    public IProperty CloneWithClassOrEndpoint(Class? classe = null, Endpoint? endpoint = null)
    {
        var alp = new AliasProperty
        {
            Class = classe,
            Comment = _comment!,
            Decorator = Decorator,
            DefaultValue = _defaultValue,
            Endpoint = endpoint,
            Label = _label,
            Location = Location,
            As = As,
            PrimaryKey = PrimaryKey,
            OriginalAliasProperty = OriginalAliasProperty,
            Prefix = Prefix,
            Property = _property,
            Suffix = Suffix,
            Name = _name!,
            Trigram = Trigram,
            UseLegacyRoleName = UseLegacyRoleName,
            DomainParameters = _domainParameters!
        };

        if (_domain != null)
        {
            alp.Domain = _domain;
        }

        if (_required.HasValue)
        {
            alp.Required = _required.Value;
        }

        if (_readonly.HasValue)
        {
            alp.Readonly = _readonly.Value;
        }

        return alp;
    }

    public override string ToString()
    {
        return Name;
    }

    internal AliasProperty Clone(IProperty prop, Reference? includeReference)
    {
        var alp = new AliasProperty
        {
            Property = prop,
            Location = Location,
            Reference = Reference,
            PropertyReference = includeReference,
            Class = Class,
            Decorator = Decorator,
            DomainReference = DomainReference,
            Endpoint = Endpoint,
            PrimaryKey = PrimaryKey,
            Prefix = Prefix,
            Suffix = Suffix,
            Comment = _comment!,
            Name = _name!,
            Trigram = Trigram,
            DefaultValue = _defaultValue,
            Label = _label,
            As = As,
            OriginalAliasProperty = this,
            UseLegacyRoleName = UseLegacyRoleName,
            DomainParameters = _domainParameters!
        };

        if (_domain != null)
        {
            alp.Domain = _domain;
        }

        if (_required.HasValue)
        {
            alp.Required = _required.Value;
        }

        if (_readonly.HasValue)
        {
            alp.Readonly = _readonly.Value;
        }

        return alp;
    }
}