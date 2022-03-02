﻿using TopModel.Core.FileModel;
using TopModel.Utils;

namespace TopModel.Core;

public class Class
{
    private string? _sqlName;

    private string? _pluralName;

    public LocatedString? Trigram { get; set; }

#nullable disable
    public LocatedString Name { get; set; }

    public string Comment { get; set; }

    public ModelFile ModelFile { get; set; }

#nullable enable
    public Class? Extends { get; set; }

    public string? Label { get; set; }

    public bool Reference { get; set; }

    public string? OrderProperty { get; set; }

    public string? DefaultProperty { get; set; }

    public string? FlagProperty { get; set; }

    public IList<IProperty> Properties { get; } = new List<IProperty>();

    public Namespace Namespace { get; set; }

    public IFieldProperty? PrimaryKey => Properties.OfType<IFieldProperty>().SingleOrDefault(p => p.PrimaryKey);

    public IFieldProperty? LabelProperty => Properties.OfType<IFieldProperty>().SingleOrDefault(p => p.Name == (DefaultProperty ?? "Libelle"));

    public IList<ReferenceValue>? ReferenceValues { get; set; }

    public IList<IList<IFieldProperty>>? UniqueKeys { get; set; }

    public string SqlName
    {
        get => _sqlName ?? ModelUtils.ConvertCsharp2Bdd(Name);
        set => _sqlName = value;
    }

    public string PluralName
    {
        get => _pluralName ?? (Name.EndsWith("s") ? Name : $"{Name}s");
        set => _pluralName = value;
    }

    public bool IsPersistent => Properties.Any(p => p is not AliasProperty && p.PrimaryKey) || Properties.All(p => p is AssociationProperty);

    public ClassReference? ExtendsReference { get; set; }

#nullable disable
    internal Reference Location { get; set; }

    public override string ToString()
    {
        return Name;
    }
}