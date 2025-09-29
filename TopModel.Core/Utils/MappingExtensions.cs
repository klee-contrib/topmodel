using OneOf;
using TopModel.Core.Model;

namespace TopModel.Core.Utils;

public static class MappingExtensions
{
    public static string GetComment(this OneOf<ClassMappings, PropertyMapping> mapping)
    {
        return mapping.Match(c => c.Comment ?? $"Instance de '{c.Class.NamePascal}'", p => p.Property.Comment);
    }

    public static Converter? GetConverter(this Domain? fromDomain, Domain? toDomain)
    {
        if (fromDomain != null && toDomain != null && fromDomain != toDomain)
        {
            return fromDomain.ConvertersFrom.FirstOrDefault(c =>
                c.From.Contains(fromDomain) && c.To.Contains(toDomain)
            );
        }

        return null;
    }

    public static string GetName(this OneOf<ClassMappings, PropertyMapping> mapping)
    {
        return mapping.Match(c => c.Name.ToString(), p => p.Property.Name);
    }

    public static string GetNameCamel(this OneOf<ClassMappings, PropertyMapping> mapping)
    {
        return mapping.Match(c => c.Name.ToCamelCase(), p => p.Property.NameCamel);
    }

    public static bool GetRequired(this OneOf<ClassMappings, PropertyMapping> mapping)
    {
        return mapping.Match(
            c => c.Required,
            p =>
                p.Property.Required
                && p.Property
                    is (
                            CompositionProperty
                            or AliasProperty { Property: CompositionProperty }
                            or { DefaultValue: null }
                        )
                        and not AssociationProperty { Type: AssociationType.OneToMany or AssociationType.ManyToMany }
        );
    }
}
