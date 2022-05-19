using TopModel.Core.FileModel;

namespace TopModel.Core;

public class Class
{
    private string? _pluralName;

    public LocatedString? Trigram { get; set; }

#nullable disable
    public LocatedString Name { get; set; }

    public string SqlName { get; set; }

    public string Comment { get; set; }

    public ModelFile ModelFile { get; set; }

#nullable enable
    public Class? Extends { get; set; }

    public List<Decorator> Decorators { get; } = new();

    public string? Label { get; set; }

    public bool Reference { get; set; }

    public string? OrderProperty { get; set; }

    public string? DefaultProperty { get; set; }

    public string? FlagProperty { get; set; }

    public IList<IProperty> Properties { get; } = new List<IProperty>();

    public Namespace Namespace { get; set; }

    public IFieldProperty? PrimaryKey => Properties.OfType<IFieldProperty>().SingleOrDefault(p => p.PrimaryKey);

    public IFieldProperty? LabelProperty => Properties.OfType<IFieldProperty>().SingleOrDefault(p => p.Name == (DefaultProperty ?? "Libelle"));

    public List<ReferenceValue> ReferenceValues { get; } = new();

    public List<List<IFieldProperty>> UniqueKeys { get; } = new();

    public string PluralName
    {
        get => _pluralName ?? (Name.EndsWith("s") ? Name : $"{Name}s");
        set => _pluralName = value;
    }

    public bool IsPersistent => Properties.Any(p => p is not AliasProperty && p.PrimaryKey) || Properties.All(p => p is AssociationProperty);

    public ClassReference? ExtendsReference { get; set; }

    public List<DecoratorReference> DecoratorReferences { get; } = new();

    public List<List<Reference>> UniqueKeyReferences { get; } = new();

    public Dictionary<Reference, Dictionary<Reference, string>> ReferenceValueReferences { get; } = new();

#nullable disable
    internal Reference Location { get; set; }

    public override string ToString()
    {
        return Name;
    }
}