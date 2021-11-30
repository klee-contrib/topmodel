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

#nullable enable
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

    public Domain Domain => _property.Domain;

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

    internal string? Prefix { get; set; }

    internal string? Suffix { get; set; }

    internal AliasReference? Reference { get; set; }

    internal DomainReference? ListDomainReference { get; set; }

    internal AliasProperty Clone(IFieldProperty prop)
    {
        var alp = new AliasProperty { Property = prop, Class = Class, Endpoint = Endpoint, Prefix = Prefix, Suffix = Suffix };
        alp.Comment = _comment!;
        alp.Label = _label;
        alp.ListDomain = _listDomain;

        if (_required.HasValue)
        {
            alp.Required = _required.Value;
        }

        return alp;
    }
}