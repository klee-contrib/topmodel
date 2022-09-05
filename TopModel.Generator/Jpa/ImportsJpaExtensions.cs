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
        if (ap.Class != null && ap.Class.IsPersistent && ap.Property is AssociationProperty asp)
        {
            if (asp.IsEnum())
            {
                imports.AddRange(asp.Property.GetImports(config));
            }
            else if (ap.Property.Domain.Java!.Imports != null)
            {
                imports.AddRange(ap.Property.Domain.Java!.Imports);
            }
        }

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
                imports.Add("java.util.ArrayList");
                imports.Add("java.util.stream.Collectors");
                imports.Add("javax.persistence.FetchType");
                imports.Add("javax.persistence.CascadeType");
                if (!(ap is JpaAssociationProperty jap && jap.IsReverse) && ap.Association.Namespace.Module.Split('.').First() != ap.Class.Namespace.Module.Split('.').First())
                {
                    imports.Add("javax.persistence.JoinColumn");
                }

                break;
            case AssociationType.ManyToOne:
                imports.Add("javax.persistence.FetchType");
                imports.Add("javax.persistence.JoinColumn");
                break;
            case AssociationType.ManyToMany:
                imports.Add("java.util.List");
                imports.Add("java.util.ArrayList");
                imports.Add("javax.persistence.JoinColumn");
                imports.Add("java.util.stream.Collectors");
                imports.Add("javax.persistence.CascadeType");
                imports.Add("javax.persistence.FetchType");
                if (!(ap is JpaAssociationProperty japo && japo.IsReverse) && ap.Association.Namespace.Module.Split('.').First() != ap.Class.Namespace.Module.Split('.').First())
                {
                    imports.Add("javax.persistence.JoinTable");
                }

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
            imports.AddRange(ap.Property.GetImports(config));
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
            imports.Add("java.util.stream.Collectors");
        }
        else if (cp.DomainKind?.Java?.Imports != null)
        {
            imports.AddRange(cp.DomainKind.Java.Imports);
        }

        return imports;
    }

    public static string GetImport(this Class classe, JpaConfig config)
    {
        var packageRootName = classe.IsPersistent ? config.EntitiesPackageName : config.DtosPackageName;
        var packageName = $"{packageRootName}.{classe.Namespace.Module.ToLower()}";
        return $"{packageName}.{classe.Name}";
    }

    public static List<string> GetImports(this Class classe, List<Class> classes, JpaConfig config)
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

        if (classe.UniqueKeys.Count > 0)
        {
            imports.Add("javax.persistence.UniqueConstraint");
        }

        if (classe.Extends != null)
        {
            imports.Add(classe.GetImport(config));
        }

        imports
        .AddRange(
            classe.FromMappers.SelectMany(fm => fm.Params).Select(fmp => fmp.Class.GetImport(config)));
        imports
        .AddRange(
            classe.ToMappers.Select(fmp => fmp.Class.GetImport(config)));

        var props = classe.GetReverseProperties(classes);

        // Suppression des imports inutiles
        return imports.Distinct().Where(i => string.Join('.', i.Split('.').SkipLast(1)) != string.Join('.', classe.GetImport(config).Split('.').SkipLast(1))).ToList();
    }
}
