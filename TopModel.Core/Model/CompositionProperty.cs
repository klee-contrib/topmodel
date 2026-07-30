using TopModel.Core.FileModel;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Core.Model;

internal class CompositionProperty : IProperty
{
    private ParamLocation? _paramLocation;

#nullable disable

    public Class Composition { get; set; }

    public string Name { get; set; }

    public string SqlName => CoreUtils.GetSqlTrigram(FinalTrigram) + CoreUtils.GetSqlName(this);

    public Domain Domain { get; set; }

    public IDictionary<string, string> DomainParameters { get; set; } = new Dictionary<string, string>();

#nullable enable

    public IList<string>? Tags { get; set; }

#nullable disable

    public string Comment { get; set; }

    public bool Readonly
    {
        get => Class?.Readonly == true || field;
        set;
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

            if (
                (Composition?.Properties.Any(cpp => cpp.ParamLocation == Model.ParamLocation.FormData) ?? false)
                || Endpoint.Params.Any(p => p != this && p.ParamLocation == Model.ParamLocation.FormData)
            )
            {
                return Model.ParamLocation.FormData;
            }

            return Model.ParamLocation.JsonBody;
        }
#pragma warning restore S4275
        set => _paramLocation = value;
    }

    public Class Class { get; set; }

    public Endpoint Endpoint { get; set; }

    public Decorator Decorator { get; set; }

    public PropertyMapping PropertyMapping { get; set; }

#nullable enable

    public string? Label { get; set; }

    [Obsolete("Utiliser ParamLocation == ParamLocation.FormData")]
    public bool IsMultipart => ParamLocation == Model.ParamLocation.FormData;

    public bool PrimaryKey => false;

    public bool Required
    {
        get => ParamLocation == Model.ParamLocation.JsonBody || field;
        set;
    } = true;

    public string? DefaultValue => null;

    public LocatedString? Trigram { get; set; }

    public string? FinalTrigram => Trigram ?? Class?.Trigram;

    public IList<AnnotationInstance> Annotations { get; private set; } = [];

    public IList<AnnotationReference> AnnotationReferences { get; internal set; } = [];

    public IList<AnnotationInstance> ExcludedAnnotations { get; } = [];

    public IList<AnnotationReference> ExcludedAnnotationReferences { get; internal set; } = [];

    public IDictionary<string, string> CustomProperties { get; internal set; } = new Dictionary<string, string>();

    public IProperty? SourceProperty { get; private set; }

    public DomainReference? DomainReference { get; set; }

#nullable disable

    public ClassReference Reference { get; set; }

#nullable enable

    internal required Reference Location { get; set; }

    string IProperty.TrueNamePascal => Name.ToPascalCase(strictIfUppercase: true);

    string IProperty.TruePropertyNamePascal => Name.ToPascalCase(strictIfUppercase: true);

    ParamLocation? IProperty.OwnLocation => _paramLocation ?? Domain?.ParamLocation;

    /// <inheritdoc cref="IProperty.CloneDefinition" />
    public IProperty CloneDefinition()
    {
        var cp = new CompositionProperty
        {
            AnnotationReferences = AnnotationReferences,
            Comment = Comment,
            CustomProperties = CustomProperties,
            DomainReference = DomainReference,
            ExcludedAnnotationReferences = ExcludedAnnotationReferences,
            Label = Label,
            Location = Location,
            Name = Name,
            Readonly = Readonly,
            Reference = Reference,
            Required = Required,
            Tags = Tags,
            Trigram = Trigram,
        };

        if (_paramLocation.HasValue)
        {
            cp.ParamLocation = _paramLocation.Value;
        }

        return cp;
    }

    /// <inheritdoc cref="IProperty.CloneForContainer" />
    public IProperty CloneForContainer(IPropertyContainer container)
    {
        var cp = new CompositionProperty
        {
            SourceProperty = SourceProperty ?? this,
            Annotations = Annotations,
            Class = container as Class,
            Comment = Comment,
            Composition = Composition,
            CustomProperties = CustomProperties,
            Decorator = container as Decorator,
            Domain = Domain,
            DomainParameters = DomainParameters,
            Endpoint = container as Endpoint,
            Location = Location,
            Name = Name,
            Required = Required,
            Readonly = Readonly,
            Tags = Tags,
            Trigram = Trigram,
        };

        if (_paramLocation.HasValue)
        {
            cp.ParamLocation = _paramLocation.Value;
        }

        return cp;
    }

    public override string ToString()
    {
        return Name;
    }
}
