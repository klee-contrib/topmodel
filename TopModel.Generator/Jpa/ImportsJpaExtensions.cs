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
        else if (rp is RegularProperty rpr)
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
            imports.Add(ap.Property.Class.GetImport(config));
        }
        else if (ap.Property is AssociationProperty apr && ap.IsAssociatedEnum())
        {
            imports.Add(apr.Association.GetImport(config));
        }

        if (ap.Domain.Java?.Imports != null)
        {
            imports.AddRange(ap.Domain.Java.Imports);
        }

        if (ap.Property is AssociationProperty apo && (apo.Type == AssociationType.ManyToMany || apo.Type == AssociationType.OneToMany))
        {
            imports.Add("java.util.List");
        }

        return imports;
    }

    public static List<string> GetImports(this RegularProperty rp, JpaConfig config)
    {
        var imports = new List<string>();
        if (rp.IsEnum())
        {
            imports.Add($"{rp.Class.GetImport(config)}");
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
                imports.Add("java.util.List");
                imports.Add("java.util.Collections");
                imports.Add("javax.persistence.FetchType");
                imports.Add("javax.persistence.CascadeType");
                imports.Add("javax.persistence.JoinColumn");
                break;
            case AssociationType.ManyToOne:
                imports.Add("javax.persistence.FetchType");
                imports.Add("javax.persistence.JoinColumn");
                break;
            case AssociationType.ManyToMany:
                imports.Add("java.util.List");
                imports.Add("java.util.Collections");
                imports.Add("javax.persistence.JoinColumn");
                imports.Add("javax.persistence.FetchType");
                imports.Add("javax.persistence.JoinTable");
                break;
            case AssociationType.OneToOne:
                imports.Add("javax.persistence.FetchType");
                imports.Add("javax.persistence.JoinColumn");
                imports.Add("javax.persistence.CascadeType");
                break;
        }

        if (ap.Association.Namespace.Module != ap.Class.Namespace.Module)
        {
            imports.Add(ap.Association.GetImport(config));
        }

        if (ap.Association.Reference)
        {
            imports.AddRange(ap.Association.PrimaryKey!.GetImports(config));
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
                "javax.annotation.Generated",
            };

        if (classe.IsPersistent)
        {
            imports.Add("javax.persistence.Entity");
            imports.Add("javax.persistence.Table");
        }
        else
        {
            imports.Add("java.io.Serializable");
            if (classe.Properties.Any(p => p is IFieldProperty { Required: true, PrimaryKey: false }))
            {
                imports.Add("javax.validation.constraints.NotNull");
            }
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

        if (classe.Extends != null)
        {
            imports.Add(classe.GetImport(config));
        }

        imports.AddRange(GetAliasClass(classe)
            .Select(c => c.Class.GetImport(config)));
        imports
        .AddRange(
            classe.Properties
            .OfType<AliasProperty>()
            .Select(ap => ap.Property)
            .OfType<AssociationProperty>()
            .Where(a => (a.Type == AssociationType.OneToMany || a.Type == AssociationType.ManyToMany))
            .Select(a => a.Association.GetImport(config)));

        // Suppression des imports inutiles
        return imports.Where(i => string.Join('.', i.Split('.').SkipLast(1)) != string.Join('.', classe.GetImport(config).Split('.').SkipLast(1))).ToList();
    }

    public static IList<AliasClass> GetAliasClass(Class classe)
    {
        var classes = classe
            .Properties.OfType<AliasProperty>()
            .Select(p => new AliasClass()
            {
                Prefix = p.Prefix,
                Suffix = p.Suffix,
                Class = p.OriginalProperty?.Class
            })
            .DistinctBy(c => c.Name)
            .ToList();

        if (classe.Extends is null)
        {
            return classes;
        }
        else
        {
            return GetAliasClass(classe.Extends).Concat(classes).DistinctBy(c => c.Name).ToList();
        }
    }
}
