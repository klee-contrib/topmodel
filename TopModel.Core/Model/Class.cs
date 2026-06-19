#pragma warning disable S1133

using TopModel.Core.FileModel;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Core.Model;

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

    public IList<DecoratorInstance> Decorators { get; } = [];

    public IList<Class> Implements { get; } = [];

    public IList<AnnotationInstance> Annotations { get; } = [];

    public IList<AnnotationInstance> ExcludedAnnotations { get; } = [];

    public IList<AnnotationInstance> PropertyAnnotations { get; } = [];

    public string? Label { get; set; }

    public bool Reference { get; set; }

    public ClassType Type { get; set; }

    [Obsolete("Utiliser `Type` à la place.")]
    public bool Abstract => Type == ClassType.Interface;

    public bool Readonly { get; set; }

    public InheritanceStrategy InheritanceStrategy { get; set; } = InheritanceStrategy.JoinedTables;

    public IProperty? OrderProperty { get; set; }

    public IProperty? DefaultProperty { get; set; }

    public IProperty? FlagProperty { get; set; }

    public IProperty? LocaleProperty { get; set; }

    public IProperty? DiscriminatorProperty { get; set; }

    public string? DiscriminatorValue { get; set; }

    public IList<IProperty> Properties { get; } = [];

    public IList<IProperty> ExtendedProperties =>
        Extends != null ? [.. Extends.ExtendedProperties, .. Properties] : Properties;

    public bool PreservePropertyCasing { get; set; }

    public Namespace Namespace { get; set; }

    public IEnumerable<IProperty> PrimaryKey => Properties.Where(p => p.PrimaryKey);

    public IProperty? ReferenceKey =>
        PrimaryKey.Count() <= 1
            ? PrimaryKey.SingleOrDefault()
                ?? Extends?.PrimaryKey.SingleOrDefault()
                ?? Properties.FirstOrDefault(p => p.Unique)
            : null;

    public IProperty? EnumKey => Enum != null ? ReferenceKey : null;

    public EnumMode? Enum { get; set; }

    public bool Translation { get; set; }

    public IList<ClassValue> Values { get; } = [];

    public IList<IndexDefinition> Indexes { get; } = [];

    [Obsolete("Utiliser `Indexes` à la place.")]
    public IList<IList<IProperty>> UniqueKeys =>
        Indexes.Where(idx => idx.Unique).Select(idx => idx.Properties).ToList();

    public IList<FromMapper> FromMappers { get; } = [];

    public IEnumerable<IProperty> FromMapperProperties =>
        FromMappers.SelectMany(fm => fm.PropertyParams.Select(pp => pp.Property));

    public IList<ClassMappings> ToMappers { get; } = [];

    public IDictionary<string, string> CustomProperties { get; internal set; } = new Dictionary<string, string>();

    public string PluralName
    {
        get => _pluralName ?? (Name.EndsWith("s") ? Name : $"{Name}s");
        set => _pluralName = value;
    }

    public string PluralNameCamel => PluralName.ToCamelCase();

    public string PluralNamePascal => PluralName.ToPascalCase();

    public bool IsPersistent => Properties.Any(p => p.PrimaryKey) || Extends != null && Extends.IsPersistent;

    public ClassReference? ExtendsReference { get; set; }

    public Reference? OrderPropertyReference { get; set; }

    public Reference? DefaultPropertyReference { get; set; }

    public Reference? FlagPropertyReference { get; set; }

    public Reference? LocalePropertyReference { get; set; }

    public Reference? DiscriminatorPropertyReference { get; set; }

    public IList<DecoratorReference> DecoratorReferences { get; internal set; } = [];

    public IList<ClassReference> ImplementReferences { get; internal set; } = [];

    public IList<AnnotationReference> AnnotationReferences { get; internal set; } = [];

    public IList<AnnotationReference> ExcludedAnnotationReferences { get; internal set; } = [];

    public IList<AnnotationReference> PropertyAnnotationReferences { get; internal set; } = [];

    public IDictionary<Reference, IDictionary<Reference, string>> ValueReferences { get; internal set; } =
        new Dictionary<Reference, IDictionary<Reference, string>>();

    public IEnumerable<ClassDependency> ClassDependencies =>
        Properties
            .GetClassDependencies(this)
            .Concat(Extends != null ? [new ClassDependency(Extends, this)] : Array.Empty<ClassDependency>());

    public IList<PropertySource> PropertySourceOrder { get; internal set; } =
    [PropertySource.Properties, PropertySource.Implements, PropertySource.Decorators];

    internal LocatedString? EnumOverride { get; set; }

    internal IList<IProperty> OwnProperties { get; } = [];

    internal IEnumerable<IProperty> FromMapperOwnProperties =>
        FromMappers.SelectMany(fm => fm.OwnPropertyParams.Select(pp => pp.Property));

#nullable disable
    internal Reference Location { get; set; }

    internal IList<string> OwnTags { get; set; } = [];

    public bool Inherit(Class classe) => this == classe || Extends != null && Extends.Inherit(classe);

    public override string ToString()
    {
        return Name;
    }
}
