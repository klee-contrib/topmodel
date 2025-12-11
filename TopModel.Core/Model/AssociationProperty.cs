using System.Text;
using TopModel.Core.FileModel;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Core.Model;

public class AssociationProperty : IProperty
{
    private IProperty? _property;

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

    public virtual AssociationType Type { get; set; }

    public Reference? ExplicitType { get; set; }

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

    public string Name
    {
        get
        {
            if (Association == null)
            {
                return string.Empty;
            }

            var name = new StringBuilder();

            if (ClassName != null)
            {
                name.Append(ClassName);
            }
            else if (Type == AssociationType.OneToMany || Type == AssociationType.ManyToMany)
            {
                name.Append(Association.PluralName);
            }
            else if (Association.Extends == null || !Association.PrimaryKey.Any())
            {
                name.Append(Association.Name);
            }

            if (Type == AssociationType.ManyToOne || Type == AssociationType.OneToOne)
            {
                name.Append(Property?.Name);
            }

            if (!string.IsNullOrWhiteSpace(Role))
            {
                name.Append(Role?.Replace(" ", string.Empty));
            }

            return name.ToString();
        }
    }

    public string NameCamel
    {
        get
        {
            if (((IProperty)this).Parent.PreservePropertyCasing)
            {
                return Name;
            }

            if (Association == null)
            {
                return string.Empty;
            }

            var name = new StringBuilder();

            if (ClassName != null)
            {
                name.Append(ClassName.ToCamelCase(strictIfUppercase: true));
            }
            else if (Type == AssociationType.OneToMany || Type == AssociationType.ManyToMany)
            {
                name.Append(Association.PluralNameCamel);
            }
            else if (Association.Extends == null || !Association.PrimaryKey.Any())
            {
                name.Append(Association.NameCamel);
            }

            if (Type == AssociationType.ManyToOne || Type == AssociationType.OneToOne)
            {
                if (name.Length != 0)
                {
                    name.Append(Property?.NameCamel.ToFirstUpper());
                }
                else
                {
                    name.Append(Property?.NameCamel);
                }
            }

            if (!string.IsNullOrWhiteSpace(Role))
            {
                name.Append(Role?.Replace(" ", string.Empty).ToPascalCase(strictIfUppercase: true));
            }

            return name.ToString();
        }
    }

    public string NamePascal => ((IProperty)this).Parent.PreservePropertyCasing ? Name : NameCamel.ToFirstUpper();

    public string NameByClassPascal =>
        Type.ToMany
            ? $"{NamePascal}"
            : $"{ClassName?.ToPascalCase(strictIfUppercase: true) ?? Association.NamePascal}{Role?.ToPascalCase() ?? string.Empty}";

    public string NameByClassCamel =>
        Type.ToMany
            ? $"{NameCamel}"
            : $"{ClassName?.ToCamelCase(strictIfUppercase: true) ?? Association.NameCamel}{Role?.ToPascalCase() ?? string.Empty}";

    public string SqlName => CoreUtils.GetSqlTrigram(FinalTrigram) + RawSqlName;

    public Domain Domain =>
        Type.ToMany && (Property?.Domain?.AsDomains.TryGetValue(As, out var ld) ?? false) ? ld : Property?.Domain!;

    public IDictionary<string, string> DomainParameters =>
        Property?.DomainParameters ?? new Dictionary<string, string>();

    public bool PrimaryKey { get; set; }

    public Decorator? SourceDecorator { get; set; }

    public Reference? PropertyReference { get; set; }

    public DomainReference? DomainReference => null;

#nullable disable
    public ClassReference Reference { get; set; }

    public bool UseLegacyRoleName { get; init; }

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

    /// <inheritdoc cref="IProperty.CloneForDecorator" />
    public IProperty CloneForDecorator(Class? classe = null, Endpoint? endpoint = null, Decorator? decorator = null)
    {
        return new AssociationProperty
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
            Type = Type,
            Readonly = Readonly,
            PrimaryKey = PrimaryKey,
            WithReverse = WithReverse,
            Trigram = Trigram,
            UseLegacyRoleName = UseLegacyRoleName,
            CustomProperties = CustomProperties,
            Annotations = Annotations,
            ClassName = ClassName,
        };
    }

    public override string ToString()
    {
        return Name;
    }
}
