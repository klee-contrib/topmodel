using TopModel.Core.FileModel;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Core.Model;

internal class RegularProperty : IProperty
{
    private ParamLocation? _paramLocation;

#nullable disable

    public string Name { get; set; }

#nullable enable

    public string SqlName => CoreUtils.GetSqlTrigram(FinalTrigram) + CoreUtils.GetSqlName(this);

    public string? Label { get; set; }

    public bool PrimaryKey { get; set; }

    public bool Required
    {
        get => ParamLocation == Model.ParamLocation.Route || ParamLocation == Model.ParamLocation.JsonBody || field;
        set;
    }

    public bool Readonly
    {
        get => Class?.Readonly == true || field;
        set;
    }

    public LocatedString? Trigram { get; set; }

    public string? FinalTrigram => Trigram ?? Class?.Trigram;

#nullable disable

    public Domain Domain { get; set; }

    public IDictionary<string, string> DomainParameters { get; set; } = new Dictionary<string, string>();

    public string Comment { get; set; }

#nullable enable

    public IList<AnnotationInstance> Annotations { get; private set; } = [];

    public IList<AnnotationReference> AnnotationReferences { get; internal set; } = [];

    public IList<AnnotationInstance> ExcludedAnnotations { get; } = [];

    public IList<AnnotationReference> ExcludedAnnotationReferences { get; internal set; } = [];

    public IDictionary<string, string> CustomProperties { get; internal set; } = new Dictionary<string, string>();

    public IList<string>? Tags { get; set; }

#nullable disable

    public Class Class { get; set; }

    public Endpoint Endpoint { get; set; }

    public Decorator Decorator { get; set; }

    public PropertyMapping PropertyMapping { get; set; }

    public DomainReference DomainReference { get; set; }

#nullable enable

    public string? DefaultValue { get; set; }

    public IProperty? SourceProperty { get; private set; }

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

#nullable disable

    internal Reference Location { get; set; }

#nullable enable

    string IProperty.TrueNamePascal => Name.ToPascalCase(strictIfUppercase: true);

    string IProperty.TruePropertyNamePascal => Name.ToPascalCase(strictIfUppercase: true);

    ParamLocation? IProperty.OwnLocation => _paramLocation ?? Domain?.ParamLocation;

    /// <inheritdoc cref="IProperty.CloneDefinition" />
    public IProperty CloneDefinition()
    {
        var rp = new RegularProperty
        {
            AnnotationReferences = AnnotationReferences,
            Comment = Comment,
            CustomProperties = CustomProperties,
            DefaultValue = DefaultValue,
            DomainReference = DomainReference,
            ExcludedAnnotationReferences = ExcludedAnnotationReferences,
            Label = Label,
            Location = Location,
            Name = Name,
            PrimaryKey = PrimaryKey,
            Readonly = Readonly,
            Required = Required,
            Tags = Tags,
            Trigram = Trigram,
        };

        if (_paramLocation.HasValue)
        {
            rp.ParamLocation = _paramLocation.Value;
        }

        return rp;
    }

    /// <inheritdoc cref="IProperty.CloneForContainer" />
    public IProperty CloneForContainer(IPropertyContainer container)
    {
        var rp = new RegularProperty
        {
            SourceProperty = SourceProperty ?? this,
            Annotations = Annotations,
            Class = container as Class,
            Comment = Comment,
            CustomProperties = CustomProperties,
            Decorator = container as Decorator,
            DefaultValue = DefaultValue,
            Domain = Domain,
            DomainParameters = DomainParameters,
            Endpoint = container as Endpoint,
            Label = Label,
            Location = Location,
            Name = Name,
            PrimaryKey = PrimaryKey,
            Required = Required,
            Readonly = Readonly,
            Tags = Tags,
            Trigram = Trigram,
        };

        if (_paramLocation.HasValue)
        {
            rp.ParamLocation = _paramLocation.Value;
        }

        return rp;
    }

    public override string ToString()
    {
        return Name;
    }
}
