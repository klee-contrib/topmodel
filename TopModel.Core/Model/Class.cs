using TopModel.Core.FileModel;
using TopModel.Utils;

namespace TopModel.Core;

public class Class : IPropertyContainer
{
    private string? _pluralName;

    public LocatedString? Trigram { get; set; }

#nullable disable
    public LocatedString Name { get; set; }

    public string NamePascal => Name.Value.ToPascalCase(strictIfUppercase: true);

    public string NameCamel => Name.Value.ToCamelCase(strictIfUppercase: true);

    public string SqlName { get; set; }

    public string Comment { get; set; }

    public ModelFile ModelFile { get; set; }

#nullable enable
    public Class? Extends { get; set; }

    public List<(Decorator Decorator, string[] Parameters)> Decorators { get; } = new();

    public string? Label { get; set; }

    public bool Reference { get; set; }

    public bool Abstract { get; set; }

    public IFieldProperty? OrderProperty { get; set; }

    public IFieldProperty? DefaultProperty { get; set; }

    public IFieldProperty? FlagProperty { get; set; }

    public IList<IProperty> Properties { get; } = new List<IProperty>();

    public bool PreservePropertyCasing { get; set; }

    public Namespace Namespace { get; set; }

    public IEnumerable<IFieldProperty> PrimaryKey => Properties.OfType<IFieldProperty>().Where(p => p.PrimaryKey);

    public IFieldProperty? ReferenceKey =>
        PrimaryKey.Count() <= 1
            ? PrimaryKey.SingleOrDefault() ?? Properties.OfType<IFieldProperty>().FirstOrDefault()
            : null;

    public IFieldProperty? EnumKey => Enum ? ReferenceKey : null;

    public bool Enum { get; set; }

    public List<ClassValue> Values { get; } = new();

    public List<List<IFieldProperty>> UniqueKeys { get; } = new();

    public List<FromMapper> FromMappers { get; } = new();

    public List<ClassMappings> ToMappers { get; } = new();

    public string PluralName
    {
        get => _pluralName ?? (Name.EndsWith("s") ? Name : $"{Name}s");
        set => _pluralName = value;
    }

    public string PluralNameCamel => PluralName.ToCamelCase();

    public string PluralNamePascal => PluralName.ToPascalCase();

    public bool IsPersistent => Properties.Any(p => p is not AliasProperty && p.PrimaryKey);

    public ClassReference? ExtendsReference { get; set; }

    public Reference? OrderPropertyReference { get; set; }

    public Reference? DefaultPropertyReference { get; set; }

    public Reference? FlagPropertyReference { get; set; }

    public List<DecoratorReference> DecoratorReferences { get; } = new();

    public List<List<Reference>> UniqueKeyReferences { get; } = new();

    public Dictionary<Reference, Dictionary<Reference, string>> ValueReferences { get; } = new();

    public IEnumerable<ClassDependency> ClassDependencies => Properties.GetClassDependencies(this)
        .Concat(Extends != null ? new[] { new ClassDependency(Extends, this) } : Array.Empty<ClassDependency>());

    internal LocatedString? EnumOverride { get; set; }

#nullable disable
    internal Reference Location { get; set; }

    public bool Inherit(Class classe) => this == classe || this.Extends != null && this.Extends.Inherit(classe);

    public override string ToString()
    {
        return Name;
    }
}