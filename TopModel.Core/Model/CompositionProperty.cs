using TopModel.Core.FileModel;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Core.Model;

internal class CompositionProperty : IProperty
{
#nullable disable

    public Class Composition { get; set; }

    public string Name { get; set; }

    public string SqlName => CoreUtils.GetSqlTrigram(FinalTrigram) + CoreUtils.GetSqlName(this);

    public Domain Domain { get; set; }

    public IDictionary<string, string> DomainParameters { get; set; } = new Dictionary<string, string>();

    public string Comment { get; set; }

    public bool Readonly
    {
        get => Class?.Readonly == true || field;
        set;
    }

    public Class Class { get; set; }

    public Endpoint Endpoint { get; set; }

    public Decorator Decorator { get; set; }

    public PropertyMapping PropertyMapping { get; set; }

#nullable enable

    public string? Label { get; set; }

    public bool IsMultipart => Composition.Properties.Any(cpp => cpp.Domain?.IsMultipart ?? false);

    public bool PrimaryKey => false;

    public bool Required { get; set; } = true;

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

    /// <inheritdoc cref="IProperty.CloneDefinition" />
    public IProperty CloneDefinition()
    {
        return new CompositionProperty
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
            Trigram = Trigram,
        };
    }

    /// <inheritdoc cref="IProperty.CloneForContainer" />
    public IProperty CloneForContainer(IPropertyContainer container)
    {
        return new CompositionProperty
        {
            SourceProperty = SourceProperty ?? this,
            Class = container as Class,
            Comment = Comment,
            Composition = Composition,
            Decorator = container as Decorator,
            Domain = Domain,
            DomainParameters = DomainParameters,
            Endpoint = container as Endpoint,
            Location = Location,
            Name = Name,
            Required = Required,
            CustomProperties = CustomProperties,
            Readonly = Readonly,
            Trigram = Trigram,
            Annotations = Annotations,
        };
    }

    public override string ToString()
    {
        return Name;
    }
}
