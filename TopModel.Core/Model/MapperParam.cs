namespace TopModel.Core.Model;

public readonly union MapperParam(ClassMappings, PropertyMapping)
{
    public string Comment =>
        this switch
        {
            ClassMappings c => c.Comment ?? $"Instance de '{c.Class.NamePascal}'",
            PropertyMapping p => p.Property.Comment,
        };

    public string Name =>
        this switch
        {
            ClassMappings c => c.Name.ToString(),
            PropertyMapping p => p.Property.Name,
        };

    public string NameCamel =>
        this switch
        {
            ClassMappings c => c.Name.ToCamelCase(),
            PropertyMapping p => p.Property.NameCamel,
        };

    public bool Required =>
        this switch
        {
            ClassMappings c => c.Required,
            PropertyMapping p => p.Property.Required
                && p.Property
                    is ({ Composition: not null } or { DefaultValue: null })
                        and { AssociationMultiple: false },
        };
}
