using TopModel.Core.FileModel;

namespace TopModel.Core;

public class AliasProperty : IFieldProperty
{
    private string? _comment;
    private string? _label;
    private Domain? _listDomain;
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

    public string? Label
    {
        get => _label ?? _property.Label;
        set => _label = value;
    }

    public bool PrimaryKey => (_property?.PrimaryKey ?? false) && Prefix == null && Suffix == null;

    public bool Required
    {
        get => _required ?? _property.Required;
        set => _required = value;
    }

#nullable disable
    public Domain Domain => _property?.Domain;
#nullable enable

    public string Comment
    {
        get => _comment ?? _property.Comment;
        set => _comment = value;
    }

    public string? DefaultValue => null;

    public Domain? ListDomain
    {
        get => _listDomain ?? (_property as AliasProperty)?.ListDomain;
        set => _listDomain = value;
    }

    public IFieldProperty? OriginalProperty => _property;

    public ClassReference? ClassReference { get; set; }

    public AliasReference? Reference { get; set; }

    public Reference? PropertyReference { get; set; }

    public string? Prefix { get; set; }

    public string? Suffix { get; set; }

#nullable disable
    public bool UseLegacyRoleName { get; init; }

    internal Reference Location { get; set; }

#nullable enable
    internal DomainReference? ListDomainReference { get; set; }

    internal AliasProperty? OriginalAliasProperty { get; private set; }

    public IProperty CloneWithClass(Class classe)
    {
        var alp = new AliasProperty
        {
            Class = classe,
            Comment = _comment!,
            Decorator = Decorator,
            Label = _label,
            Location = Location,
            ListDomain = _listDomain,
            OriginalAliasProperty = OriginalAliasProperty,
            Prefix = Prefix,
            Property = _property,
            Suffix = Suffix,
            UseLegacyRoleName = UseLegacyRoleName
        };

        if (_required.HasValue)
        {
            alp.Required = _required.Value;
        }

        return alp;
    }

    internal AliasProperty Clone(IFieldProperty prop, Reference? includeReference)
    {
        var alp = new AliasProperty
        {
            Property = prop,
            Location = Location,
            ClassReference = Reference,
            PropertyReference = includeReference,
            Class = Class,
            Decorator = Decorator,
            Endpoint = Endpoint,
            Prefix = Prefix,
            Suffix = Suffix,
            Comment = _comment!,
            Label = _label,
            ListDomain = _listDomain,
            ListDomainReference = ListDomainReference,
            OriginalAliasProperty = this,
            UseLegacyRoleName = UseLegacyRoleName
        };

        if (_required.HasValue)
        {
            alp.Required = _required.Value;
        }

        return alp;
    }
}