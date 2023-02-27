using TopModel.Core.FileModel;
using TopModel.Utils;

namespace TopModel.Core;

public class AliasProperty : IFieldProperty
{
    private string? _comment;
    private string? _defaultValue;
    private Domain? _domain;
    private string? _label;
    private bool? _readonly;
    private bool? _required;

#nullable disable
    private IFieldProperty _property;

    public IFieldProperty Property
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

#nullable enable

    public LocatedString? Trigram { get; set; }

    public string Name => (Prefix ?? string.Empty) + _property?.Name + (Suffix ?? string.Empty);

    public string NamePascal => Name.ToPascalCase();

    public string NameCamel => Name.ToCamelCase();

    public string? Label
    {
        get => _label ?? _property?.Label;
        set => _label = value;
    }

    public bool PrimaryKey => (_property?.PrimaryKey ?? false) && Prefix == null && Suffix == null;

    public bool Required
    {
        get => _required ?? _property.Required;
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
            return AsList ? domain?.ListDomain : domain;
        }

        set => _domain = value;
    }
#nullable enable

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

    public bool AsList { get; set; }

    public IFieldProperty? OriginalProperty => _property;

    public AliasReference? Reference { get; set; }

    public Reference? PropertyReference { get; set; }

    public string? Prefix { get; set; }

    public string? Suffix { get; set; }

#nullable disable
    public bool UseLegacyRoleName { get; init; }

    internal Reference Location { get; set; }

#nullable enable

    internal AliasProperty? OriginalAliasProperty { get; private set; }

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
            AsList = AsList,
            OriginalAliasProperty = OriginalAliasProperty,
            Prefix = Prefix,
            Property = _property,
            Suffix = Suffix,
            UseLegacyRoleName = UseLegacyRoleName
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

    internal AliasProperty Clone(IFieldProperty prop, Reference? includeReference)
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
            Prefix = Prefix,
            Suffix = Suffix,
            Comment = _comment!,
            DefaultValue = _defaultValue,
            Label = _label,
            AsList = AsList,
            OriginalAliasProperty = this,
            UseLegacyRoleName = UseLegacyRoleName
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