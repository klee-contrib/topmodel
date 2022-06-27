using TopModel.Core.FileModel;

namespace TopModel.Core;

public class ClassMappings
{
#nullable disable
    public LocatedString Name { get; set; }

    public Class Class { get; set; }

    public ClassReference ClassReference { get; set; }

#nullable enable

    public Dictionary<IFieldProperty, IFieldProperty> Mappings { get; } = new();

    public Dictionary<Reference, Reference> MappingReferences { get; } = new();

    public ClassMappings? ParentMapping { get; set; }
}
