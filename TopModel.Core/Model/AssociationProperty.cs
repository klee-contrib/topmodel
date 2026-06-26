using TopModel.Core.FileModel;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Core.Model;

internal class AssociationProperty : IProperty
{
    private IProperty? _property;

    private bool? _useClass;

    public LocatedString? Trigram { get; set; }

    public string? FinalTrigram => Trigram ?? Property.FinalTrigram ?? Class?.Trigram;

    public virtual string? ClassName { get; set; }

#nullable disable

    public virtual Class Association { get; set; }

#nullable enable

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

#nullable disable

    public virtual string Comment { get; set; }

    public Class Class { get; set; }

    public Endpoint Endpoint { get; set; }

    public Decorator Decorator { get; set; }

    public PropertyMapping PropertyMapping { get; set; }

#nullable enable

    public virtual string? Role { get; set; }

    public virtual bool Multiple { get; set; }

    public virtual string As { get; set; } = "list";

    public virtual bool Required { get; set; }

    public bool Readonly
    {
        get => Class?.Readonly == true || field;
        set;
    }

    public string? DefaultValue { get; set; }

    public virtual ReverseAssociationDefinition? WithReverse { get; set; }

    public virtual AssociationProperty? ReverseProperty { get; set; }

    public virtual IList<AnnotationInstance> Annotations { get; private set; } = [];

    public virtual IList<AnnotationReference> AnnotationReferences { get; internal set; } = [];

    public virtual IList<AnnotationInstance> ExcludedAnnotations { get; } = [];

    public virtual IList<AnnotationReference> ExcludedAnnotationReferences { get; internal set; } = [];

    public IDictionary<string, string> CustomProperties { get; internal set; } = new Dictionary<string, string>();

    public IList<string>? Tags { get; set; }

    public string Name => this.GetAssociationName();

    public string SqlName => CoreUtils.GetSqlTrigram(FinalTrigram) + RawSqlName;

    public virtual bool UseClass
    {
        get =>
            Property?.Class.Enum == EnumMode.Enum
            || Class?.IsPersistent == true && (_useClass ?? DefaultAssociationUseClass);
        set => _useClass = value;
    }

    public Domain Domain =>
        Multiple && (Property?.Domain?.AsDomains.TryGetValue(As, out var ld) ?? false) ? ld : Property?.Domain!;

    public IDictionary<string, string> DomainParameters =>
        Property?.DomainParameters ?? new Dictionary<string, string>();

    public bool PrimaryKey { get; set; }

    public IProperty? SourceProperty { get; private set; }

    public Reference? PropertyReference { get; set; }

    public DomainReference? DomainReference => null;

#nullable disable

    public ClassReference Reference { get; set; }

    internal Reference Location { get; set; }

#nullable enable

    internal string RawSqlName
    {
        get
        {
            var sqlName = CoreUtils.GetSqlName(Property);
            if (!string.IsNullOrWhiteSpace(Role))
            {
                sqlName += UseLegacyRoleName ? $"_{Role.Replace(' ', '_').ToUpper()}" : $"_{Role.ToConstantCase()}";
            }
            return sqlName;
        }
    }

    internal virtual bool DefaultAssociationUseClass { get; init; }

    internal virtual bool UseLegacyRoleName { get; init; }

    string IProperty.TrueNamePascal => this.GetAssociationName(pascalCase: true);

    string IProperty.TruePropertyNamePascal => this.GetAssociationName(pascalCase: true, forcePropertyName: true);

    /// <inheritdoc cref="IProperty.CloneDefinition" />
    public IProperty CloneDefinition()
    {
        var ap = new AssociationProperty
        {
            AnnotationReferences = AnnotationReferences,
            As = As,
            ClassName = ClassName,
            Comment = Comment,
            CustomProperties = CustomProperties,
            DefaultAssociationUseClass = DefaultAssociationUseClass,
            DefaultValue = DefaultValue,
            ExcludedAnnotationReferences = ExcludedAnnotationReferences,
            Label = Label,
            Location = Location,
            Multiple = Multiple,
            PrimaryKey = PrimaryKey,
            PropertyReference = PropertyReference,
            Readonly = Readonly,
            Reference = Reference,
            Required = Required,
            Role = Role,
            Tags = Tags,
            Trigram = Trigram,
            UseLegacyRoleName = UseLegacyRoleName,
        };

        if (_useClass.HasValue)
        {
            ap.UseClass = _useClass.Value;
        }

        if (WithReverse != null)
        {
            ap.WithReverse = new ReverseAssociationDefinition
            {
                AnnotationReferences = WithReverse.AnnotationReferences,
                ClassName = WithReverse.ClassName,
                Comment = WithReverse.Comment,
                ExcludedAnnotationReferences = WithReverse.ExcludedAnnotationReferences,
                Label = WithReverse.Label,
                Location = WithReverse.Location,
                Property = ap,
            };
        }

        return ap;
    }

    /// <inheritdoc cref="IProperty.CloneForContainer" />
    public IProperty CloneForContainer(IPropertyContainer container)
    {
        var ap = new AssociationProperty
        {
            SourceProperty = SourceProperty ?? this,
            Annotations = Annotations,
            Association = Association,
            Class = container as Class,
            ClassName = ClassName,
            Comment = Comment,
            CustomProperties = CustomProperties,
            Decorator = container as Decorator,
            DefaultAssociationUseClass = DefaultAssociationUseClass,
            DefaultValue = DefaultValue,
            Endpoint = container as Endpoint,
            Label = Label,
            Location = Location,
            Multiple = Multiple,
            PrimaryKey = PrimaryKey,
            Readonly = Readonly,
            Required = Required,
            Role = Role,
            Tags = Tags,
            Trigram = Trigram,
            UseLegacyRoleName = UseLegacyRoleName,
            WithReverse = WithReverse,
        };

        if (_useClass.HasValue)
        {
            ap.UseClass = _useClass.Value;
        }

        return ap;
    }

    public override string ToString()
    {
        return Name;
    }
}
