using TopModel.Core.FileModel;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Core.Model;

internal class AliasProperty(Declaration declaration) : IProperty
{
    private string? _comment;
    private Class? _composition;
    private IDictionary<string, string> _customProperties = new Dictionary<string, string>();
    private string? _defaultValue;
    private DomainInstance? _domain;
    private string? _label;
    private ParamLocation? _paramLocation;
    private bool? _primaryKey;
    private IProperty _property = null!;
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

    public Class Class { get; set; } = null!;

    public Endpoint? Endpoint { get; set; }

    public Decorator? Decorator { get; set; }

    public PropertyMapping? PropertyMapping { get; set; }

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

    public LocatedString? OwnName { get; set; }

    public IList<NameSegment> NameSegments
    {
        get
        {
            var segments = new List<NameSegment>();

            if (Prefix is LocatedString prefix)
            {
                segments.Add(new NameSegment.Text(prefix));
            }
            else if (Prefix is true && OriginalProperty != null)
            {
                if (OriginalProperty.Class != null)
                {
                    segments.Add(new NameSegment.ClassName(OriginalProperty.Class));
                }
                else
                {
                    segments.Add(new NameSegment.Text(OriginalProperty.Parent.Name));
                }
            }

            if (OwnName != null)
            {
                segments.AddRange(new NameSegment.Text(OwnName));
            }
            else if (_property is AssociationProperty && Composition == null)
            {
                segments.AddRange(this.GetAssociationNameSegments());
            }
            else if (_property != null)
            {
                segments.Add(new NameSegment.PropertyName(_property));
            }

            if (Suffix is LocatedString suffix)
            {
                segments.Add(new NameSegment.Text(suffix));
            }
            else if (Suffix is true && OriginalProperty != null)
            {
                if (OriginalProperty.Class != null)
                {
                    segments.Add(new NameSegment.ClassName(OriginalProperty.Class));
                }
                else
                {
                    segments.Add(new NameSegment.Text(OriginalProperty.Parent.Name));
                }
            }

            return segments;
        }
    }

    public IList<NameSegment> PropertyNameSegments
    {
        get
        {
            var segments = new List<NameSegment>();

            if (Prefix is LocatedString prefix)
            {
                segments.Add(new NameSegment.Text(prefix));
            }
            else if (Prefix is true && OriginalProperty != null)
            {
                if (OriginalProperty.Class != null)
                {
                    segments.Add(new NameSegment.ClassName(OriginalProperty.Class));
                }
                else
                {
                    segments.Add(new NameSegment.Text(OriginalProperty.Parent.Name));
                }
            }

            if (OwnName != null)
            {
                segments.AddRange(new NameSegment.Text(OwnName));
            }
            else if (_property is AssociationProperty && Composition == null)
            {
                segments.AddRange(this.GetAssociationNameSegments(forcePropertyName: true));
            }
            else if (_property != null)
            {
                segments.Add(new NameSegment.PropertyName(_property));
            }

            if (Suffix is LocatedString suffix)
            {
                segments.Add(new NameSegment.Text(suffix));
            }
            else if (Suffix is true && OriginalProperty != null)
            {
                if (OriginalProperty.Class != null)
                {
                    segments.Add(new NameSegment.ClassName(OriginalProperty.Class));
                }
                else
                {
                    segments.Add(new NameSegment.Text(OriginalProperty.Parent.Name));
                }
            }

            return segments;
        }
    }

    public string SqlName => CoreUtils.GetSqlTrigram(FinalTrigram) + CoreUtils.GetSqlName(PersistentProperty ?? this);

    public bool UseClass
    {
        get =>
            Class?.IsPersistent == true
            && (
                (Property as AssociationProperty)?.Property?.Class.Enum == EnumMode.Enum
                || (_useClass ?? (_property as AssociationProperty)?.UseClass ?? false)
            );
        set => _useClass = value;
    }

    public string? Label
    {
        get => _label ?? _property?.Label;
        set => _label = value;
    }

    public bool OverridesLabel => _label != null;

    public bool PrimaryKey
    {
        get => _primaryKey ?? (PreservePrimaryKey && (_property?.PrimaryKey ?? false));
        set => _primaryKey = value;
    }

    public bool Required
    {
        get =>
            ParamLocation == Model.ParamLocation.Route
            || ParamLocation == Model.ParamLocation.JsonBody
            || (_required ?? _property?.Required ?? false);
        set => _required = value;
    }

    public bool Readonly
    {
        get => Class?.Readonly == true || _readonly == true;
        set => _readonly = value;
    }

    public bool PreservePrimaryKey { get; set; }

    public bool PreserveTrigram { get; set; }

    public bool DiscardAssociations { get; set; }

    public DomainInstance Domain
    {
        get
        {
            var domain = _domain ?? _property?.Domain;
            return As != null
                ? domain != null && domain.AsDomains.TryGetValue(As, out var asDomain)
                    ? new(asDomain, domain.Parameters)
                    : null!
                : domain!;
        }
        set => _domain = value;
    }

    public DomainInstance? DomainOverride => _domain;

    public Class? Composition
    {
        get => _composition ?? _property.Composition;
        set { _composition = value; }
    }

    public ClassReference? CompositionReference { get; set; }

    public DomainReference? DomainReference { get; set; }

    public string Comment
    {
        get => _comment ?? _property.Comment;
        set => _comment = value;
    }

    public bool OverridesComment => _comment != null;

    public string? DefaultValue
    {
        get =>
            _defaultValue
            ?? (As is null && _property?.DefaultValue != null ? _property?.DefaultValue : null)
            ?? Domain?.DefaultValue;
        set => _defaultValue = value;
    }

    public string? As { get; set; }

    public IList<AnnotationInstance> Annotations =>
        [
            .. OriginalProperty?.Annotations.Where(ann =>
                !OriginalProperty.ExcludedAnnotations.Any(ann2 => ann.Annotation == ann2.Annotation)
                && !OwnAnnotations.Any(ann2 => ann.Annotation == ann2.Annotation)
            )
                ?? [],
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

    public IList<string>? Tags { get; set; }

    public IProperty? SourceProperty { get; private set; }

    public IProperty? OriginalProperty => _property;

    public IProperty? PersistentProperty =>
        Class?.IsPersistent ?? false ? this
        : OriginalProperty is AliasProperty op ? op.PersistentProperty
        : OriginalProperty?.Class?.IsPersistent ?? false ? OriginalProperty
        : null;

    public AliasReference? Reference { get; set; }

    public PropertyReference? PropertyReference { get; set; }

    public Affix? Prefix { get; set; }

    public Affix? Suffix { get; set; }

    public ParamLocation? ParamLocation
    {
#pragma warning disable S4275
        get
        {
            if (Endpoint == null || !Endpoint.Params.Contains(this))
            {
                return null;
            }

            var ownLocation = ((IProperty)this).OwnLocation;
            if (ownLocation != null)
            {
                return ownLocation;
            }

            if (
                Composition != null
                && (
                    Composition.Properties.Any(cpp => cpp.ParamLocation == Model.ParamLocation.FormData)
                    || Endpoint.Params.Any(p => p != this && p.ParamLocation == Model.ParamLocation.FormData)
                )
            )
            {
                return Model.ParamLocation.FormData;
            }

            return Endpoint.HasInRoute(this) ? Model.ParamLocation.Route
                : Composition != null ? Model.ParamLocation.JsonBody
                : Model.ParamLocation.Query;
        }
#pragma warning restore S4275
        set => _paramLocation = value;
    }

    public IEnumerable<(Reference Reference, object Target)> References =>
        [
            (Reference?.ClassReference!, OriginalProperty?.Class!),
            (Reference?.EndpointReference!, OriginalProperty?.Endpoint!),
            (Reference?.DecoratorReference!, OriginalProperty?.Decorator!),
            (PropertyReference!, OriginalProperty!),
            (DomainReference!, Domain?.Domain!),
            (CompositionReference!, Composition!),
            .. Reference?.ExcludeReferences?.Select(er =>
                (
                    er as Reference,
                    OriginalProperty?.Class?.Properties?.FirstOrDefault(p => p?.Name == er?.Value)! as object
                )
            )
                ?? [],
            .. DomainReference?.ParameterReferences.Keys.Select(
                (pr, i) => (pr as Reference, Domain?.TemplateParameters.ElementAtOrDefault(i)! as object)
            )
                ?? [],
        ];

    internal Declaration Declaration { get; } = declaration;

    internal AliasProperty? OriginalAliasProperty { get; private set; }

    ParamLocation? IProperty.OwnLocation => _paramLocation ?? Domain?.ParamLocation;

    /// <inheritdoc cref="IProperty.CloneDefinition" />
    public IProperty CloneDefinition()
    {
        var alp = new AliasProperty(Declaration)
        {
            AnnotationReferences = AnnotationReferences,
            As = As,
            Comment = _comment!,
            CompositionReference = CompositionReference,
            CustomProperties = _customProperties,
            DefaultValue = _defaultValue,
            DiscardAssociations = DiscardAssociations,
            DomainReference = DomainReference,
            ExcludedAnnotationReferences = ExcludedAnnotationReferences,
            Label = _label,
            OwnName = OwnName,
            Prefix = Prefix,
            PreservePrimaryKey = PreservePrimaryKey,
            PreserveTrigram = PreserveTrigram,
            Reference = Reference,
            Suffix = Suffix,
            Tags = Tags,
            Trigram = Trigram,
        };

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

        if (_paramLocation.HasValue)
        {
            alp.ParamLocation = _paramLocation.Value;
        }

        return alp;
    }

    /// <inheritdoc cref="IProperty.CloneForContainer" />
    public IProperty CloneForContainer(IPropertyContainer container)
    {
        var alp = new AliasProperty(Declaration)
        {
            SourceProperty = SourceProperty ?? this,
            As = As,
            Class = (container as Class)!,
            Comment = _comment!,
            CustomProperties = _customProperties,
            Decorator = container as Decorator,
            DefaultValue = _defaultValue,
            DiscardAssociations = DiscardAssociations,
            Endpoint = container as Endpoint,
            ExcludedAnnotations = ExcludedAnnotations,
            Label = _label,
            OwnName = OwnName,
            OriginalAliasProperty = OriginalAliasProperty,
            OwnAnnotations = OwnAnnotations,
            Prefix = Prefix,
            PreservePrimaryKey = PreservePrimaryKey,
            PreserveTrigram = PreserveTrigram,
            Property = _property,
            Suffix = Suffix,
            Tags = Tags,
            Trigram = Trigram,
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

        if (_paramLocation.HasValue)
        {
            alp.ParamLocation = _paramLocation.Value;
        }

        return alp;
    }

    public override string ToString()
    {
        return this.Name;
    }

    internal AliasProperty Clone(IProperty prop, PropertyReference? includeReference)
    {
        var alp = new AliasProperty(includeReference != null ? new Declaration(includeReference) : Declaration)
        {
            AnnotationReferences = AnnotationReferences,
            As = As,
            Class = Class,
            Comment = _comment!,
            CompositionReference = CompositionReference,
            CustomProperties = _customProperties,
            Decorator = Decorator,
            DefaultValue = _defaultValue,
            DiscardAssociations = DiscardAssociations,
            DomainReference = DomainReference,
            Endpoint = Endpoint,
            ExcludedAnnotations = ExcludedAnnotations,
            ExcludedAnnotationReferences = ExcludedAnnotationReferences,
            Label = _label,
            OwnName = OwnName,
            OriginalAliasProperty = this,
            OwnAnnotations = OwnAnnotations,
            Prefix = Prefix,
            PreservePrimaryKey = PreservePrimaryKey,
            PreserveTrigram = PreserveTrigram,
            Property = prop,
            PropertyReference = includeReference,
            Reference = Reference,
            SourceProperty = SourceProperty,
            Suffix = Suffix,
            Tags = Tags,
            Trigram = Trigram,
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

        if (_paramLocation.HasValue)
        {
            alp.ParamLocation = _paramLocation.Value;
        }

        return alp;
    }
}
