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

            return prop;
        }
        set => _property = value;
    }

#nullable enable

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

    public bool Readonly { get; set; }

    public string? DefaultValue { get; set; }

    public virtual ReverseAssociationDefinition? WithReverse { get; set; }

    public virtual AssociationProperty? ReverseProperty { get; set; }

    public virtual IList<AnnotationInstance> Annotations { get; private set; } = [];

    public virtual IList<AnnotationReference> AnnotationReferences { get; set; } = [];

    public virtual IList<AnnotationInstance> ExcludedAnnotations { get; } = [];

    public virtual IList<AnnotationReference> ExcludedAnnotationReferences { get; } = [];

    public IDictionary<string, string> CustomProperties { get; private set; } = new Dictionary<string, string>();

    public string Name => this.GetAssociationName();

    public string NamePascal =>
        ((IProperty)this).Parent.PreservePropertyCasing ? Name : this.GetAssociationName(pascalCase: true);

    public string NameCamel => ((IProperty)this).Parent.PreservePropertyCasing ? Name : NamePascal.ToFirstLower();

    public string PropertyNamePascal =>
        ((IProperty)this).Parent.PreservePropertyCasing
            ? Name
            : this.GetAssociationName(pascalCase: true, forcePropertyName: true);

    public string PropertyNameCamel =>
        ((IProperty)this).Parent.PreservePropertyCasing ? Name : PropertyNamePascal.ToFirstLower();

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

    public Decorator? SourceDecorator { get; set; }

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

    /// <inheritdoc cref="IProperty.CloneForDecorator" />
    public IProperty CloneForDecorator(Class? classe = null, Endpoint? endpoint = null, Decorator? decorator = null)
    {
        var ap = new AssociationProperty
        {
            SourceDecorator = SourceDecorator ?? Decorator,
            Association = Association,
            Class = classe,
            Comment = Comment,
            Decorator = decorator,
            DefaultValue = DefaultValue,
            Endpoint = endpoint,
            Label = Label,
            Location = Location,
            Required = Required,
            Role = Role,
            Multiple = Multiple,
            Readonly = Readonly,
            PrimaryKey = PrimaryKey,
            WithReverse = WithReverse,
            Trigram = Trigram,
            CustomProperties = CustomProperties,
            Annotations = Annotations,
            ClassName = ClassName,
            DefaultAssociationUseClass = DefaultAssociationUseClass,
            UseLegacyRoleName = UseLegacyRoleName,
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
