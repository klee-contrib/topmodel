using TopModel.Core.FileModel;
using TopModel.Core.Utils;

namespace TopModel.Core.Model;

internal class AssociationProperty(Declaration declaration) : IProperty
{
    private string? _defaultValue;
    private ParamLocation? _paramLocation;
    private IProperty? _property;

    private bool? _useClass;

    public LocatedString? Trigram { get; set; }

    public string? FinalTrigram => Trigram ?? Property.FinalTrigram ?? Class?.Trigram;

    public virtual LocatedString? ClassName { get; set; }

    public virtual Class Association { get; set; } = null!;

    public IProperty Property
    {
        get
        {
            if (_property is not null)
            {
                return _property;
            }

            var prop = _property;
            var ass = Association;

            while (ass is not null && prop is null)
            {
                prop = ass.PrimaryKey.FirstOrDefault();
                ass = ass.Extends;
            }

            return prop!;
        }
        set => _property = value;
    }

    public virtual string? Label { get; set; }

    public virtual string Comment { get; set; } = null!;

    public Class Class { get; set; } = null!;

    public Endpoint? Endpoint { get; set; }

    public Decorator? Decorator { get; set; }

    public PropertyMapping? PropertyMapping { get; set; }

    public virtual LocatedString? Role { get; set; }

    public virtual bool Multiple { get; set; }

    public virtual string As { get; set; } = "list";

    public virtual bool Required
    {
        get => ParamLocation == Model.ParamLocation.Route || ParamLocation == Model.ParamLocation.JsonBody || field;
        set;
    }

    public bool Readonly
    {
        get => Class?.Readonly == true || field;
        set;
    }

    public string? DefaultValue
    {
        get => _defaultValue ?? Domain?.DefaultValue;
        set => _defaultValue = value;
    }

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

            return Endpoint.HasInRoute(this) ? Model.ParamLocation.Route : Model.ParamLocation.Query;
        }
#pragma warning restore S4275
        set => _paramLocation = value;
    }

    public virtual ReverseAssociationDefinition? WithReverse { get; set; }

    public virtual AssociationProperty? ReverseProperty { get; set; }

    public virtual IList<AnnotationInstance> Annotations { get; private set; } = [];

    public virtual IList<AnnotationReference> AnnotationReferences { get; internal set; } = [];

    public virtual IList<AnnotationInstance> ExcludedAnnotations { get; } = [];

    public virtual IList<AnnotationReference> ExcludedAnnotationReferences { get; internal set; } = [];

    public IDictionary<string, string> CustomProperties { get; internal set; } = new Dictionary<string, string>();

    public IList<string>? Tags { get; set; }

    public IList<NameSegment> NameSegments => this.GetAssociationNameSegments();

    public IList<NameSegment> PropertyNameSegments => this.GetAssociationNameSegments(forcePropertyName: true);

    public string SqlName => CoreUtils.GetSqlTrigram(FinalTrigram) + RawSqlName;

    public virtual bool UseClass
    {
        get =>
            Property?.Class.Enum == EnumMode.Enum
            || Class?.IsPersistent == true && (_useClass ?? DefaultAssociationUseClass) && Association.IsPersistent;
        set => _useClass = value;
    }

    public DomainInstance Domain =>
        Multiple && (Property?.Domain?.AsDomains.TryGetValue(As, out var ld) ?? false)
            ? new(ld, Property.Domain.Parameters)
            : Property?.Domain!;

    public bool PrimaryKey { get; set; }

    public IProperty? SourceProperty { get; private set; }

    public PropertyReference? PropertyReference { get; set; }

    public DomainReference? DomainReference => null;

    public ClassReference Reference { get; set; } = null!;

    public IEnumerable<(Reference Reference, object Target)> References =>
        [(Reference, Association), (PropertyReference!, Property)];

    internal Declaration Declaration { get; } = declaration;

    internal string RawSqlName
    {
        get
        {
            var sqlName = CoreUtils.GetSqlName(Property);
            if (!string.IsNullOrWhiteSpace(Role))
            {
                sqlName += $"_{Role.ToConstantCase()}";
            }
            return sqlName;
        }
    }

    internal virtual bool DefaultAssociationUseClass { get; init; }

    ParamLocation? IProperty.OwnLocation => _paramLocation ?? Domain?.ParamLocation;

    /// <inheritdoc cref="IProperty.CloneDefinition" />
    public IProperty CloneDefinition()
    {
        var ap = new AssociationProperty(Declaration)
        {
            AnnotationReferences = AnnotationReferences,
            As = As,
            ClassName = ClassName,
            Comment = Comment,
            CustomProperties = CustomProperties,
            DefaultAssociationUseClass = DefaultAssociationUseClass,
            DefaultValue = _defaultValue,
            ExcludedAnnotationReferences = ExcludedAnnotationReferences,
            Label = Label,
            Multiple = Multiple,
            PrimaryKey = PrimaryKey,
            PropertyReference = PropertyReference,
            Readonly = Readonly,
            Reference = Reference,
            Required = Required,
            Role = Role,
            Tags = Tags,
            Trigram = Trigram,
        };

        if (_useClass.HasValue)
        {
            ap.UseClass = _useClass.Value;
        }

        if (_paramLocation.HasValue)
        {
            ap.ParamLocation = _paramLocation.Value;
        }

        if (WithReverse != null)
        {
            ap.WithReverse = new ReverseAssociationDefinition(WithReverse.Declaration)
            {
                AnnotationReferences = WithReverse.AnnotationReferences,
                ClassName = WithReverse.ClassName,
                Comment = WithReverse.Comment,
                ExcludedAnnotationReferences = WithReverse.ExcludedAnnotationReferences,
                Label = WithReverse.Label,
                Property = ap,
            };
        }

        return ap;
    }

    /// <inheritdoc cref="IProperty.CloneForContainer" />
    public IProperty CloneForContainer(IPropertyContainer container)
    {
        var ap = new AssociationProperty(Declaration)
        {
            SourceProperty = SourceProperty ?? this,
            Annotations = Annotations,
            Association = Association,
            Class = (container as Class)!,
            ClassName = ClassName,
            Comment = Comment,
            CustomProperties = CustomProperties,
            Decorator = container as Decorator,
            DefaultAssociationUseClass = DefaultAssociationUseClass,
            DefaultValue = _defaultValue,
            Endpoint = container as Endpoint,
            Label = Label,
            Multiple = Multiple,
            PrimaryKey = PrimaryKey,
            Readonly = Readonly,
            Required = Required,
            Role = Role,
            Tags = Tags,
            Trigram = Trigram,
            WithReverse = WithReverse,
        };

        if (_useClass.HasValue)
        {
            ap.UseClass = _useClass.Value;
        }

        if (_paramLocation.HasValue)
        {
            ap.ParamLocation = _paramLocation.Value;
        }

        return ap;
    }

    public override string ToString()
    {
        return this.Name;
    }
}
