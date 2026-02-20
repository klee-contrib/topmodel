using System.Text;
using TopModel.Core.FileModel;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Core.Model;

internal class AliasProperty : IProperty
{
    private string? _comment;
    private Class? _composition;
    private IDictionary<string, string> _customProperties = new Dictionary<string, string>();
    private string? _defaultValue;
    private Domain? _domain;
    private IDictionary<string, string>? _domainParameters;
    private string? _label;
    private string? _name;
    private bool? _primaryKey;
#nullable disable
    private IProperty _property;
    private bool? _readonly;
    private bool? _required;
    private bool? _useClass;

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

    public string? FinalTrigram
    {
        get
        {
            if (PreserveTrigram)
            {
                return OriginalProperty?.FinalTrigram;
            }

            var prop = PersistentProperty ?? this;
            var ap = prop as AssociationProperty ?? (prop as AliasProperty)?.Property as AssociationProperty;
            return prop.Trigram ?? ap?.FinalTrigram ?? prop.Class?.Trigram;
        }
    }

    public string Name
    {
        get
        {
            var name = new StringBuilder();

            if (Prefix != null)
            {
                name.Append(Prefix);
            }

            if (_name != null)
            {
                name.Append(_name);
            }
            else if (_property is AssociationProperty && Composition == null)
            {
                name.Append(this.GetAssociationName());
            }
            else
            {
                name.Append(_property?.Name);
            }

            if (Suffix != null)
            {
                name.Append(Suffix);
            }

            return name.ToString();
        }
        set => _name = value;
    }

    public string NamePascal
    {
        get
        {
            if (((IProperty)this).Parent.PreservePropertyCasing)
            {
                return Name;
            }

            var name = new StringBuilder();

            if (Prefix != null)
            {
                name.Append(Prefix.ToFirstUpper());
            }

            if (_name != null)
            {
                name.Append(_name.ToPascalCase(strictIfUppercase: true));
            }
            else if (_property is AssociationProperty && Composition == null)
            {
                name.Append(this.GetAssociationName(pascalCase: true));
            }
            else
            {
                name.Append(_property?.NamePascal);
            }

            if (Suffix != null)
            {
                name.Append(Suffix);
            }

            return name.ToString();
        }
    }

    public string NameCamel => ((IProperty)this).Parent.PreservePropertyCasing ? Name : NamePascal.ToFirstLower();

    public string PropertyNamePascal
    {
        get
        {
            if (((IProperty)this).Parent.PreservePropertyCasing)
            {
                return Name;
            }

            var name = new StringBuilder();

            if (Prefix != null)
            {
                name.Append(Prefix.ToFirstUpper());
            }

            if (_name != null)
            {
                name.Append(_name.ToPascalCase(strictIfUppercase: true));
            }
            else if (_property is AssociationProperty && Composition == null)
            {
                name.Append(this.GetAssociationName(pascalCase: true, forcePropertyName: true));
            }
            else
            {
                name.Append(_property?.NamePascal);
            }

            if (Suffix != null)
            {
                name.Append(Suffix);
            }

            return name.ToString();
        }
    }

    public string PropertyNameCamel =>
        ((IProperty)this).Parent.PreservePropertyCasing ? Name : PropertyNamePascal.ToFirstLower();

    public string SqlName => CoreUtils.GetSqlTrigram(FinalTrigram) + CoreUtils.GetSqlName(PersistentProperty ?? this);

    public bool UseClass
    {
        get => Class?.IsPersistent == true && (_useClass ?? (_property as AssociationProperty)?.UseClass ?? false);
        set => _useClass = value;
    }

    public string? Label
    {
        get => _label ?? _property?.Label;
        set => _label = value;
    }

    public bool PrimaryKey
    {
        get => _primaryKey ?? (PreservePrimaryKey && (_property?.PrimaryKey ?? false));
        set => _primaryKey = value;
    }

    public bool Required
    {
        get => _required ?? _property?.Required ?? false;
        set => _required = value;
    }

    public bool Readonly
    {
        get => Class?.Readonly == true || (_readonly ?? _property?.Readonly ?? false);
        set => _readonly = value;
    }

    public bool PreservePrimaryKey { get; set; }

    public bool PreserveTrigram { get; set; }

#nullable disable
    public Domain Domain
    {
        get
        {
            var domain = _domain ?? _property?.Domain;
            return As != null
                ? domain != null && domain.AsDomains.TryGetValue(As, out var asDomain)
                    ? asDomain
                    : null
                : domain;
        }
        set => _domain = value;
    }

#nullable enable
    public Domain? DomainOverride => _domain;

    public Class? Composition
    {
        get => _composition ?? _property.Composition;
        set { _composition = value; }
    }

    public ClassReference? CompositionReference { get; set; }

    public IDictionary<string, string> DomainParameters
    {
        get => _domainParameters ?? _property?.DomainParameters ?? new Dictionary<string, string>();
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
        get => _defaultValue ?? (As is null && _property?.DefaultValue != null ? _property?.DefaultValue : null);
        set => _defaultValue = value;
    }

    public string? As { get; set; }

    public bool AutoGeneratedValue => !PreservePrimaryKey && (Domain?.AutoGeneratedValue ?? false);

    public IList<AnnotationInstance> Annotations =>
        [
            .. OriginalProperty?.Annotations.Where(ann =>
                !OriginalProperty.ExcludedAnnotations.Any(ann2 => ann.Annotation == ann2.Annotation)
                && !OwnAnnotations.Any(ann2 => ann.Annotation == ann2.Annotation)
            ) ?? [],
            .. OwnAnnotations,
        ];

    public IList<AnnotationInstance> OwnAnnotations { get; private set; } = [];

    public IList<AnnotationReference> AnnotationReferences { get; set; } = [];

    public IList<AnnotationInstance> ExcludedAnnotations { get; set; } = [];

    public IList<AnnotationReference> ExcludedAnnotationReferences { get; set; } = [];

    public IDictionary<string, string> CustomProperties
    {
        get
        {
            var customProperties = new Dictionary<string, string>(OriginalProperty!.CustomProperties);
            foreach (var cp in _customProperties)
            {
                customProperties[cp.Key] = cp.Value;
            }

            return customProperties;
        }
        set => _customProperties = value;
    }

    public Decorator? SourceDecorator { get; set; }

    public IProperty? OriginalProperty => _property;

    public IProperty? PersistentProperty =>
        Class?.IsPersistent ?? false ? this
        : OriginalProperty is AliasProperty op ? op.PersistentProperty
        : OriginalProperty?.Class?.IsPersistent ?? false ? OriginalProperty
        : null;

    public AliasReference? Reference { get; set; }

    public Reference? PropertyReference { get; set; }

    public string? Prefix { get; set; }

    public string? Suffix { get; set; }

#nullable disable
    internal Reference Location { get; set; }

#nullable enable

    internal AliasProperty? OriginalAliasProperty { get; private set; }

    /// <inheritdoc cref="IProperty.CloneForDecorator" />
    public IProperty CloneForDecorator(Class? classe = null, Endpoint? endpoint = null, Decorator? decorator = null)
    {
        var alp = new AliasProperty
        {
            SourceDecorator = SourceDecorator ?? Decorator,
            Class = classe,
            Comment = _comment!,
            Decorator = decorator,
            DefaultValue = _defaultValue,
            Endpoint = endpoint,
            Label = _label,
            Location = Location,
            As = As,
            OriginalAliasProperty = OriginalAliasProperty,
            Prefix = Prefix,
            Property = _property,
            Suffix = Suffix,
            Name = _name!,
            Trigram = Trigram,
            PreservePrimaryKey = PreservePrimaryKey,
            PreserveTrigram = PreserveTrigram,
            DomainParameters = _domainParameters!,
            CustomProperties = _customProperties,
            OwnAnnotations = OwnAnnotations,
            ExcludedAnnotations = ExcludedAnnotations,
        };

        if (_domain != null)
        {
            alp.Domain = _domain;
        }

        if (_composition != null)
        {
            alp.Composition = _composition;
        }

        if (_primaryKey != null)
        {
            alp.PrimaryKey = _primaryKey.Value;
        }

        if (_required.HasValue)
        {
            alp.Required = _required.Value;
        }

        if (_readonly.HasValue)
        {
            alp.Readonly = _readonly.Value;
        }

        if (_useClass.HasValue)
        {
            alp.UseClass = _useClass.Value;
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
            SourceDecorator = SourceDecorator,
            DomainReference = DomainReference,
            CompositionReference = CompositionReference,
            Endpoint = Endpoint,
            Prefix = Prefix,
            Suffix = Suffix,
            Comment = _comment!,
            Name = _name!,
            Trigram = Trigram,
            DefaultValue = _defaultValue,
            Label = _label,
            As = As,
            PreservePrimaryKey = PreservePrimaryKey,
            PreserveTrigram = PreserveTrigram,
            OriginalAliasProperty = this,
            DomainParameters = _domainParameters!,
            CustomProperties = _customProperties,
            OwnAnnotations = OwnAnnotations,
            ExcludedAnnotations = ExcludedAnnotations,
            AnnotationReferences = AnnotationReferences,
            ExcludedAnnotationReferences = ExcludedAnnotationReferences,
        };

        if (_domain != null)
        {
            alp.Domain = _domain;
        }

        if (_composition != null)
        {
            alp.Composition = _composition;
        }

        if (_primaryKey != null)
        {
            alp.PrimaryKey = _primaryKey.Value;
        }

        if (_required.HasValue)
        {
            alp.Required = _required.Value;
        }

        if (_readonly.HasValue)
        {
            alp.Readonly = _readonly.Value;
        }

        if (_useClass.HasValue)
        {
            alp.UseClass = _useClass.Value;
        }

        return alp;
    }
}
