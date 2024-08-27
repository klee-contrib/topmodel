﻿using TopModel.Core.FileModel;

namespace TopModel.Core;

public class ClassMappings
{
#nullable disable
    public bool To { get; set; }

    public LocatedString Name { get; set; }

    public Class Class { get; set; }

    public ClassReference ClassReference { get; set; }

    public bool Required { get; set; } = true;

#nullable enable
    public string? Comment { get; set; }

    public Dictionary<IProperty, IProperty> Mappings { get; } = [];

    public Dictionary<Reference, Reference> MappingReferences { get; } = [];

    public IEnumerable<IProperty> MissingRequiredProperties => Class.Properties
        .Where(p =>
            p.Required
            && (p is CompositionProperty or AliasProperty { Property: CompositionProperty } || p.DefaultValue == null)
            && !(p.Class.IsPersistent && p.PrimaryKey && p.Class.PrimaryKey.Count() == 1 && p.Domain.AutoGeneratedValue))
        .Except(Mappings.Select(m => m.Value));
}
