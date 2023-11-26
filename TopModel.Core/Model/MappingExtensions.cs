using OneOf;

namespace TopModel.Core;

public static class MappingExtensions
{
    public static string GetName(this OneOf<ClassMappings, PropertyMapping> mapping)
    {
        return mapping.Match(c => c.Name.ToString(), p => p.Property.Name);
    }
}
