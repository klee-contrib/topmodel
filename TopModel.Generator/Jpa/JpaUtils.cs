using TopModel.Core;
using TopModel.Utils;

namespace TopModel.Generator.Jpa;

public static class JpaUtils
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
        var isList = ap.Type == AssociationType.OneToMany || ap.Type == AssociationType.ManyToMany;
        if (ap.Association.Reference)
        {
            if (isList)
            {
                return $"List<{ap.Association.PrimaryKey!.GetJavaType()}>";
            }
            else
            {
                return ap.Association.PrimaryKey!.GetJavaType();
            }
        }

        if (isList)
        {
            return $"List<{ap.Association.Name}>";
        }

        return ap.Association.Name;
    }

    public static string GetAssociationName(this AssociationProperty ap)
    {
        if (ap.Type == AssociationType.ManyToMany || ap.Type == AssociationType.OneToMany || ap.Association.Reference)
        {
            return $"{ap.Name.ToFirstLower()}";
        }
        else
        {
            return $"{ap.Association.Name.ToFirstLower()}{ap.Role?.ToFirstUpper() ?? string.Empty}";
        }
    }

    public static string GetJavaType(this AliasProperty ap)
    {
        if (ap.IsEnum())
        {
            return ap.Property.GetJavaType();
        }
        else if (ap.Property is AssociationProperty apr && ap.IsAssociatedEnum())
        {
            if (apr.Type == AssociationType.ManyToMany || apr.Type == AssociationType.OneToMany)
            {
                return $"List<{apr.Association.PrimaryKey!.GetJavaType()}>";
            }

            return apr.Association.PrimaryKey!.GetJavaType();
        }
        else if (ap.Property is AssociationProperty oapr)
        {
            if (oapr.Type == AssociationType.ManyToMany || oapr.Type == AssociationType.OneToMany)
            {
                return $"List<{oapr.Association.PrimaryKey!.GetJavaType()}>";
            }
        }

        return ap.Domain.Java!.Type;
    }

    public static string GetJavaType(this RegularProperty rp)
    {
        return rp.IsEnum() ? $"{rp.Class.Name.ToFirstUpper()}.Values" : rp.Domain.Java!.Type;
    }

    public static string GetJavaType(this CompositionProperty cp)
    {
        return cp.Kind switch
        {
            "object" => cp.Composition.Name,
            "list" => $"List<{cp.Composition.Name}>",
            "async-list" => $"IAsyncEnumerable<{cp.Composition.Name}>",
            string _ when cp.DomainKind!.Java!.Type.Contains("{class}") => cp.DomainKind.Java.Type.Replace("{class}", cp.Composition.Name),
            string _ => $"{cp.DomainKind.Java.Type}<{cp.Composition.Name}>"
        };
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