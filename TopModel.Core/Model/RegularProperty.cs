using TopModel.Core.FileModel;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Core.Model;

internal class RegularProperty : IProperty
{
#nullable disable
    public string Name { get; set; }

    public string NamePascal =>
        ((IProperty)this).Parent.PreservePropertyCasing ? Name : Name.ToPascalCase(strictIfUppercase: true);

    public string NameCamel =>
        ((IProperty)this).Parent.PreservePropertyCasing ? Name : Name.ToCamelCase(strictIfUppercase: true);

    public string PropertyNamePascal => NamePascal;

    public string PropertyNameCamel => NameCamel;

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

    public IList<AnnotationReference> AnnotationReferences { get; set; } = [];

    public IList<AnnotationInstance> ExcludedAnnotations { get; } = [];

    public IList<AnnotationReference> ExcludedAnnotationReferences { get; } = [];

    public IDictionary<string, string> CustomProperties { get; private set; } = new Dictionary<string, string>();

    public Class Class { get; set; }

    public Endpoint Endpoint { get; set; }

    public Decorator Decorator { get; set; }

    public PropertyMapping PropertyMapping { get; set; }

    public DomainReference DomainReference { get; set; }

#nullable enable

    public string? DefaultValue { get; set; }

    public Decorator? SourceDecorator { get; set; }

#nullable disable
    internal Reference Location { get; set; }
#nullable enable
#pragma warning disable KTA1600
    /// <inheritdoc cref="IProperty.CloneForDecorator" />
    public IProperty CloneForDecorator(Class? classe = null, Endpoint? endpoint = null, Decorator? decorator = null)
    {
        return new RegularProperty
        {
            SourceDecorator = SourceDecorator ?? Decorator,
            Class = classe,
            Comment = Comment,
            Decorator = decorator,
            DefaultValue = DefaultValue,
            Domain = Domain,
            DomainParameters = DomainParameters,
            Endpoint = endpoint,
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
