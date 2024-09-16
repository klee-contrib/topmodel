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

    public IEnumerable<string> Tags => ModelFile.Tags.Concat(OwnTags).Distinct();

#nullable enable
    public Class? Extends { get; set; }

    public List<(Decorator Decorator, string[] Parameters)> Decorators { get; } = [];

    public string? Label { get; set; }

    public bool Reference { get; set; }

    public bool Abstract { get; set; }

    public IProperty? OrderProperty { get; set; }

    public IProperty? DefaultProperty { get; set; }

    public IProperty? FlagProperty { get; set; }

    public IList<IProperty> Properties { get; } = [];

    public IList<IProperty> ExtendedProperties => Extends != null ? Properties.Concat(Extends.ExtendedProperties).ToList() : Properties;

    public bool PreservePropertyCasing { get; set; }

    public Namespace Namespace { get; set; }

    public IEnumerable<IProperty> PrimaryKey => Properties.Where(p => p.PrimaryKey);

    public IProperty? ReferenceKey =>
        PrimaryKey.Count() <= 1
            ? PrimaryKey.SingleOrDefault() ?? Extends?.PrimaryKey.SingleOrDefault() ?? Properties.FirstOrDefault()
            : null;

    public IProperty? EnumKey => Enum ? ReferenceKey : null;

    public bool Enum { get; set; }

    public List<ClassValue> Values { get; } = [];

    public List<List<IProperty>> UniqueKeys { get; } = [];

    public List<FromMapper> FromMappers { get; } = [];

    public IEnumerable<IProperty> FromMapperProperties => FromMappers.SelectMany(fm => fm.PropertyParams.Select(pp => pp.Property));

    public List<ClassMappings> ToMappers { get; } = [];

    public Dictionary<string, string> CustomProperties { get; } = [];

    public string PluralName
    {
        get => _pluralName ?? (Name.EndsWith("s") ? Name : $"{Name}s");
        set => _pluralName = value;
    }

    public string PluralNameCamel => PluralName.ToCamelCase();

    public string PluralNamePascal => PluralName.ToPascalCase();

    public bool IsPersistent => Properties.Any(p => p.PrimaryKey) || (Extends != null && Extends.IsPersistent);

    public ClassReference? ExtendsReference { get; set; }

    public Reference? OrderPropertyReference { get; set; }

    public Reference? DefaultPropertyReference { get; set; }

    public Reference? FlagPropertyReference { get; set; }

    public List<DecoratorReference> DecoratorReferences { get; } = [];

    public List<List<Reference>> UniqueKeyReferences { get; } = [];

    public Dictionary<Reference, Dictionary<Reference, string>> ValueReferences { get; } = [];

    public IEnumerable<ClassDependency> ClassDependencies => Properties.GetClassDependencies(this)
        .Concat(Extends != null ? [new ClassDependency(Extends, this)] : Array.Empty<ClassDependency>());

    internal LocatedString? EnumOverride { get; set; }

#nullable disable
    internal Reference Location { get; set; }

    internal List<string> OwnTags { get; set; } = [];

    public bool Inherit(Class classe) => this == classe || this.Extends != null && this.Extends.Inherit(classe);

    public override string ToString()
    {
        return Name;
    }
}