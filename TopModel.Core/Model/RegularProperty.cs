using TopModel.Core.FileModel;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Core.Model;

internal class RegularProperty : IProperty
{
#nullable disable

    public string Name { get; set; }

#nullable enable

    public string SqlName => CoreUtils.GetSqlTrigram(FinalTrigram) + CoreUtils.GetSqlName(this);

    public string? Label { get; set; }

    public bool PrimaryKey { get; set; }

    public bool Required { get; set; }

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

    public IList<AnnotationInstance> Annotations { get; private set; } = [];

    public IList<AnnotationReference> AnnotationReferences { get; internal set; } = [];

    public IList<AnnotationInstance> ExcludedAnnotations { get; } = [];

    public IList<AnnotationReference> ExcludedAnnotationReferences { get; internal set; } = [];

    public IDictionary<string, string> CustomProperties { get; internal set; } = new Dictionary<string, string>();

    public Class Class { get; set; }

    public Endpoint Endpoint { get; set; }

    public Decorator Decorator { get; set; }

    public PropertyMapping PropertyMapping { get; set; }

    public DomainReference DomainReference { get; set; }

#nullable enable

    public string? DefaultValue { get; set; }

    public IProperty? SourceProperty { get; private set; }

#nullable disable

    internal Reference Location { get; set; }

#nullable enable

    string IProperty.TrueNamePascal => Name.ToPascalCase(strictIfUppercase: true);

    string IProperty.TruePropertyNamePascal => Name.ToPascalCase(strictIfUppercase: true);

    /// <inheritdoc cref="IProperty.CloneDefinition" />
    public IProperty CloneDefinition()
    {
        return new RegularProperty
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
            Trigram = Trigram,
        };
    }

    /// <inheritdoc cref="IProperty.CloneForContainer" />
    public IProperty CloneForContainer(IPropertyContainer container)
    {
        return new RegularProperty
        {
            SourceProperty = SourceProperty ?? this,
            Class = container as Class,
            Comment = Comment,
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
            Trigram = Trigram,
            CustomProperties = CustomProperties,
            Annotations = Annotations,
        };
    }

    public override string ToString()
    {
        return Name;
    }
}
