using TopModel.Core;
using TopModel.Utils;

namespace TopModel.Generator.Jpa;

public static class GetJavaTypeJpaExtensions
{
    public static string GetJavaType(this IProperty prop)
    {
        return prop switch
        {
            AssociationProperty a => a.GetJavaType(),
            CompositionProperty c => c.GetJavaType(),
            AliasProperty l => l.GetJavaType(),
            RegularProperty r => r.GetJavaType(),
            _ => string.Empty,
        };
    }

    public static string GetJavaType(this AssociationProperty ap)
    {
        if (ap.Type == AssociationType.OneToMany || ap.Type == AssociationType.ManyToMany)
        {
            return $"Set<{ap.Association.Name}>";
        }

        return ap.Association.Name;
    }

    public static string GetAssociationName(this AssociationProperty ap)
    {
        return $"{ap.Association.Name.ToFirstLower()}{ap.Role?.ToFirstUpper() ?? string.Empty}{(ap.Type == AssociationType.OneToMany || ap.Type == AssociationType.ManyToMany ? "List" : string.Empty)}";
    }

    public static string GetJavaType(this AliasProperty ap)
    {
        if (ap.IsEnum())
        {
            return ap.Property.GetJavaType();
        }
        else if (ap.Property is AssociationProperty apr && ap.IsAssociatedEnum())
        {
            return apr.Association.PrimaryKey!.GetJavaType();
        }

        return ap.Domain.Java!.Type;
    }

    public static string GetJavaType(this RegularProperty rp)
    {
        return rp.IsEnum() ? $"{rp.Class.Name.ToFirstUpper()}Code" : rp.Domain.Java!.Type;
    }

    public static string GetJavaType(this CompositionProperty cp)
    {
        if (cp.Kind == "object")
        {
            return cp.Composition.Name;
        }
        else if (cp.Kind == "list")
        {
            return $"Set<{cp.Composition.Name}>";
        }
        else
        {
            return $"{cp.DomainKind!.Java!.Type}<{cp.Composition.Name}>";
        }
    }

    public static bool IsEnum(this RegularProperty rp)
    {
        return rp.Class != null
                && rp.Class.IsPersistent
                && rp.Class.Reference
                && rp.PrimaryKey
                && rp.Domain.Name != "DO_ID";
    }

    public static bool IsEnum(this AliasProperty ap)
    {
        return ap.Property is RegularProperty rp && rp.IsEnum();
    }

    public static bool IsAssociatedEnum(this AliasProperty ap)
    {
        return ap.Property is AssociationProperty apr
          && apr.Association.IsPersistent
          && apr.Association.Reference
          && apr.Domain.Name != "DO_ID";
    }
}