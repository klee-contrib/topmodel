using TopModel.Core;

namespace TopModel.Generator.Jpa;

public static class JpaExtensions
{
    public static string GetAssociationName(this AssociationProperty ap)
    {
        return $"{ap.Association.Name.ToFirstLower()}{ap.Role?.ToFirstUpper() ?? string.Empty}{(ap.Type == AssociationType.OneToMany || ap.Type == AssociationType.ManyToMany ? "List" : string.Empty)}";
    }

    public static string GetJavaType(this AssociationProperty ap)
    {
        if (ap.Type == AssociationType.OneToMany || ap.Type == AssociationType.ManyToMany)
        {
            return $"Set<{ap.Association.Name}>";
        }

        return ap.Association.Name;
    }

    public static string GetJavaType(this IProperty prop)
    {
        switch (prop)
        {
            case AssociationProperty a: return a.GetJavaType();
            case CompositionProperty c: return c.GetJavaType();
            case AliasProperty l: return l.GetJavaType();
            case RegularProperty r: return r.GetJavaType();
            default: return string.Empty;
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
        return (ap.Property is RegularProperty rp) && rp.IsEnum();
    }

    public static bool IsAssociatedEnum(this AliasProperty ap)
    {
        return ap.Property is AssociationProperty apr
          && apr.Association.IsPersistent
          && apr.Association.Reference
          && apr.Domain.Name != "DO_ID";
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

    public static List<string> GetImports(this AliasProperty ap, JpaConfig config)
    {
        var imports = new List<string>();
        if (ap.IsEnum() || ap.IsAssociatedEnum())
        {
            var package = $"{config.DaoPackageName}.references.{ap.Property.Class.Namespace.Module.ToLower()}";
            imports.Add(package + "." + ap.GetJavaType());
        }

        if (ap.Domain.Java?.Imports != null)
        {
            imports.AddRange(ap.Domain.Java.Imports);
        }

        return imports;
    }

    public static string GetJavaType(this CompositionProperty cp)
    {
        if (cp.Kind == "object")
        {
            return cp.Composition.Name;
        }
        else if (cp.Kind == "list")
        {
            return $"List<{cp.Composition.Name}>";
        }
        else
        {
            return $"{cp.DomainKind!.Java!.Type}<{cp.Composition.Name}>";
        }
    }

    public static List<string> GetImports(this AssociationProperty ap, JpaConfig config)
    {
        var imports = new List<string>
        {
            $"javax.persistence.{ap.Type}"
        };

        switch (ap.Type)
        {
            case AssociationType.OneToMany:
                imports.Add("java.util.Set");
                imports.Add("java.util.HashSet");
                imports.Add("javax.persistence.FetchType");
                imports.Add("javax.persistence.CascadeType");
                break;
            case AssociationType.ManyToOne:
                imports.Add("javax.persistence.FetchType");
                imports.Add("javax.persistence.JoinColumn");
                break;
            case AssociationType.ManyToMany:
                imports.Add("java.util.Set");
                imports.Add("java.util.HashSet");
                imports.Add("javax.persistence.JoinColumn");
                imports.Add("javax.persistence.JoinTable");
                break;
            case AssociationType.OneToOne:
                imports.Add("javax.persistence.FetchType");
                break;
        }

        if (ap.Association.Namespace.Module != ap.Class.Namespace.Module)
        {
            imports.Add(ap.Association.GetImport(config));
        }

        return imports;
    }

    public static List<string> GetImports(this CompositionProperty cp, JpaConfig config)
    {
        var imports = new List<string>();
        if (cp.Composition.Namespace.Module != cp.Class?.Namespace.Module)
        {
            imports.Add(cp.Composition.GetImport(config));
        }

        if (cp.Kind == "list")
        {
            imports.Add("java.util.List");
        }
        else if (cp.DomainKind?.Java?.Imports != null)
        {
            imports.AddRange(cp.DomainKind.Java.Imports);
        }

        return imports;
    }

    public static List<string> GetImports(this IProperty p, JpaConfig config)
    {
        switch (p)
        {
            case CompositionProperty cp: return cp.GetImports(config);
            case AssociationProperty ap: return ap.GetImports(config);
            case IFieldProperty fp: return fp.GetImports(config);
            default: return new List<string>();
        }
    }

    public static List<string> GetImports(this IFieldProperty rp, JpaConfig config)
    {
        var imports = new List<string>();
        if (rp.Class.IsPersistent)
        {
            imports.Add("javax.persistence.Column");
        }

        if (rp.Domain.Java?.Imports != null)
        {
            imports.AddRange(rp.Domain.Java.Imports);
        }

        if (rp.PrimaryKey && rp.Class.IsPersistent)
        {
            imports.Add("javax.persistence.Id");
            if (
                           rp.Domain.Java!.Type == "Long"
                       || rp.Domain.Java.Type == "long"
                       || rp.Domain.Java.Type == "int"
                       || rp.Domain.Java.Type == "Integer")
            {
                imports.AddRange(new List<string>
                {
                    "javax.persistence.GeneratedValue",
                    "javax.persistence.SequenceGenerator",
                    "javax.persistence.GenerationType"
                });
            }
        }

        if (rp is AliasProperty apo)
        {
            imports.AddRange(apo.GetImports(config));
        }

        return imports;
    }

    public static string GetImport(this Class classe, JpaConfig config)
    {
        var entityDto = classe.IsPersistent ? "entities" : "dtos";
        var packageName = $"{config.DaoPackageName}.{entityDto}.{classe.Namespace.Module.ToLower()}";
        return $"{packageName}.{classe.Name}";
    }

    public static List<string> GetImports(this Class classe, JpaConfig config)
    {
        var imports = new List<string>
            {
                "lombok.NoArgsConstructor",
                "lombok.Builder",
                "lombok.Setter",
                "lombok.ToString",
                "lombok.EqualsAndHashCode",
                "lombok.AllArgsConstructor",
                "java.io.Serializable"
            };

        if (classe.IsPersistent)
        {
            imports.Add("javax.persistence.Entity");
            imports.Add("javax.persistence.Table");
        }
        else
        {
            if (classe.Properties.Any(p =>
                    p is IFieldProperty { Required: true, PrimaryKey: false }
                || p is AliasProperty { Required: true, PrimaryKey: false }))
            {
                imports.Add("javax.validation.constraints.NotNull");
            }

            return imports;
        }

        if (classe.Reference)
        {
            imports.AddRange(new List<string>
            {
                "javax.persistence.Enumerated",
                "javax.persistence.EnumType",
                "org.hibernate.annotations.Cache",
                "org.hibernate.annotations.Cache",
                "org.hibernate.annotations.Immutable",
                "org.hibernate.annotations.CacheConcurrencyStrategy"
            });
            imports.Add($"{config.DaoPackageName}.references.{classe.Namespace.Module.ToLower()}.{classe.Name}Code");
        }

        if (classe.UniqueKeys?.Count > 0)
        {
            imports.Add("javax.persistence.UniqueConstraint");
        }

        return imports;
    }
}