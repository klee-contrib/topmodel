using TopModel.Core;

namespace TopModel.Generator.Jpa;

public static class ImportsJpaExtensions
{
    public static List<string> GetImports(this IProperty p, JpaConfig config)
    {
        return p switch
        {
            CompositionProperty cp => cp.GetImports(config),
            AssociationProperty ap => ap.GetImports(config),
            IFieldProperty fp => fp.GetImports(config),
            _ => new List<string>(),
        };
    }

    public static List<string> GetImports(this IFieldProperty rp, JpaConfig config)
    {
        var imports = new List<string>();
        if (rp.Class != null && rp.Class.IsPersistent)
        {
            imports.Add("javax.persistence.Column");
        }

        if (rp.Domain.Java?.Imports != null)
        {
            imports.AddRange(rp.Domain.Java.Imports);
        }

        if (rp.Class != null && rp.PrimaryKey && rp.Class.IsPersistent)
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

        if (rp is RegularProperty rpr)
        {
            imports.AddRange(rpr.GetImports(config));
        }

        return imports;
    }

    public static List<string> GetImports(this AliasProperty ap, JpaConfig config)
    {
        var imports = new List<string>();
        if (ap.IsEnum())
        {
            var package = $"{config.DaoPackageName}.references.{ap.Property.Class.Namespace.Module.ToLower()}";
            imports.Add(package + "." + ap.GetJavaType());
        }
        else if (ap.IsAssociatedEnum())
        {
            var package = $"{config.DaoPackageName}.references.{((AssociationProperty)ap.Property).Association.Namespace.Module.ToLower()}";
            imports.Add(package + "." + ap.GetJavaType());
        }

        if (ap.Domain.Java?.Imports != null)
        {
            imports.AddRange(ap.Domain.Java.Imports);
        }

        return imports;
    }

    public static List<string> GetImports(this RegularProperty rp, JpaConfig config)
    {
        var imports = new List<string>();
        if (rp.IsEnum())
        {
            imports.Add($"{config.DaoPackageName}.references.{rp.Class.Namespace.Module.ToLower()}.{rp.GetJavaType()}");
        }

        if (rp.Domain?.Java?.Imports != null)
        {
            imports.AddRange(rp.Domain.Java.Imports);
        }

        return imports;
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
                imports.Add("javax.persistence.JoinColumn");
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
            imports.Add("java.util.ArrayList");
        }
        else if (cp.DomainKind?.Java?.Imports != null)
        {
            imports.AddRange(cp.DomainKind.Java.Imports);
        }

        return imports;
    }

    public static string GetImport(this Class classe, JpaConfig config)
    {
        var entityDto = classe.IsPersistent ? "entities" : "dtos";
        var packageName = $"{config.DaoPackageName}.{entityDto}.{classe.Namespace.Module.ToLower()}";
        return $"{packageName}.{classe.Name}";
    }

    public static List<string> GetImports(this Class classe)
    {
        var imports = new List<string>
            {
                "lombok.NoArgsConstructor",
                "lombok.Builder",
                "lombok.Setter",
                "lombok.ToString",
                "lombok.EqualsAndHashCode",
                "lombok.AllArgsConstructor",
                "java.io.Serializable",
                "javax.annotation.Generated"
            };

        if (classe.IsPersistent)
        {
            imports.Add("javax.persistence.Entity");
            imports.Add("javax.persistence.Table");
        }
        else
        {
            if (classe.Properties.Any(p => p is IFieldProperty { Required: true, PrimaryKey: false }))
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
        }

        if (classe.UniqueKeys?.Count > 0)
        {
            imports.Add("javax.persistence.UniqueConstraint");
        }

        return imports;
    }
}