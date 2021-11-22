using TopModel.Core;

namespace TopModel.Generator.Jpa;

public static class JpaExtensions
{
    public static string GetAssociationName(this AssociationProperty ap)
    {
        return $"{ap.Association.Name.ToFirstLower()}{ap.Role?.ToFirstUpper() ?? string.Empty}{(ap.Type == AssociationType.OneToMany || ap.Type == AssociationType.ManyToMany ? "List" : string.Empty)}";
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
        if (cp.Composition.Namespace.Module != cp.Class.Namespace.Module)
        {
            imports.Add(cp.Composition.GetImport(config));
        }

        return imports;
    }

    public static List<string> GetImports(this IFieldProperty rp, JpaConfig config)
    {
        var imports = new List<string>();
        if (rp.Class.IsPersistent)
        {
            imports.Add("javax.persistence.Column");
        }

        if (rp.Domain.Java != null)
        {
            if (rp.Domain.Java.Imports != null)
            {
                imports.AddRange(rp.Domain.Java.Imports);
            }
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
                imports.Add("javax.persistence.GeneratedValue");
                imports.Add("javax.persistence.SequenceGenerator");
                imports.Add("javax.persistence.GenerationType");
            }
        }

        if (rp is AliasProperty alpr && alpr.Property is AssociationProperty asop)
        {
            imports.Add($"{asop.Association.GetImport(config)}Code");
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
            if (classe.Properties.Any(p => p is IFieldProperty { Required: true })
                || classe.Properties.Any(p => p is AliasProperty { Required: true }))
            {
                imports.Add("javax.validation.constraints.NotNull");
            }

            imports.Sort();
            return imports;
        }

        if (classe.Reference)
        {
            imports.Add("javax.persistence.Enumerated");
            imports.Add("javax.persistence.EnumType");
            imports.Add("org.hibernate.annotations.Cache");
            imports.Add("org.hibernate.annotations.Cache");
            imports.Add("org.hibernate.annotations.Immutable");
            imports.Add("org.hibernate.annotations.CacheConcurrencyStrategy");
            imports.Add($"{config.DaoPackageName}.references.{classe.Namespace.Module.ToLower()}.{classe.Name}Code");
        }

        if (classe.UniqueKeys?.Count > 0)
        {
            imports.Add("javax.persistence.UniqueConstraint");
        }

        return imports;
    }
}