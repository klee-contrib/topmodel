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
            imports.Add(config.PersistenceMode.ToString().ToLower() + ".persistence.Column");
        }

        var javaType = rp.Domain.Java;
        if (javaType?.Imports != null)
        {
            imports.AddRange(javaType.Imports.Select(i => i.ParseTemplate(rp)));
        }

        if (rp.Class != null && rp.PrimaryKey && rp.Class.IsPersistent)
        {
            imports.Add(config.PersistenceMode.ToString().ToLower() + ".persistence.Id");
            if (
                javaType!.Type == "Long"
                || javaType.Type == "long"
                || javaType.Type == "int"
                || javaType.Type == "Integer")
            {
                var javaOrJakarta = config.PersistenceMode.ToString().ToLower();
                imports.AddRange(new List<string>
                {
                    $"{javaOrJakarta}.persistence.GeneratedValue",
                    $"{javaOrJakarta}.persistence.GenerationType"
                });
                if (config.Identity.Mode == IdentityMode.SEQUENCE)
                {
                    imports.Add($"{javaOrJakarta}.persistence.SequenceGenerator");
                }
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
                imports.AddRange(ap.Property.Domain.Java!.Imports.Select(i => i.ParseTemplate(ap)));
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
            imports.AddRange(ap.Domain.Java.Imports.Select(i => i.ParseTemplate(ap)));
        }

        if (ap.Property is AssociationProperty apo && (apo.Type == AssociationType.ManyToMany || apo.Type == AssociationType.OneToMany))
        {
            imports.Add("java.util.List");
            imports.Add("java.util.stream.Collectors");
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
            imports.AddRange(rp.Domain.Java.Imports.Select(i => i.ParseTemplate(rp)));
        }

        return imports;
    }

    public static List<string> GetImports(this AssociationProperty ap, JpaConfig config)
    {
        var imports = new List<string>
        {
            config.PersistenceMode.ToString().ToLower() + $".persistence.{ap.Type}"
        };

        switch (ap.Type)
        {
            case AssociationType.OneToMany:
                imports.Add("java.util.List");
                imports.Add("java.util.ArrayList");
                imports.Add("java.util.stream.Collectors");
                imports.Add(config.PersistenceMode.ToString().ToLower() + ".persistence.FetchType");
                imports.Add(config.PersistenceMode.ToString().ToLower() + ".persistence.CascadeType");
                if (!(ap is JpaAssociationProperty jap && jap.IsReverse) && ap.Association.Namespace.Module.Split('.').First() != ap.Class.Namespace.Module.Split('.').First())
                {
                    imports.Add(config.PersistenceMode.ToString().ToLower() + ".persistence.JoinColumn");
                }

                break;
            case AssociationType.ManyToOne:
                imports.Add(config.PersistenceMode.ToString().ToLower() + ".persistence.FetchType");
                imports.Add(config.PersistenceMode.ToString().ToLower() + ".persistence.JoinColumn");
                break;
            case AssociationType.ManyToMany:
                imports.Add("java.util.List");
                imports.Add("java.util.ArrayList");
                imports.Add(config.PersistenceMode.ToString().ToLower() + ".persistence.JoinColumn");
                imports.Add("java.util.stream.Collectors");
                imports.Add(config.PersistenceMode.ToString().ToLower() + ".persistence.CascadeType");
                imports.Add(config.PersistenceMode.ToString().ToLower() + ".persistence.FetchType");
                if (!(ap is JpaAssociationProperty japo && japo.IsReverse))
                {
                    imports.Add(config.PersistenceMode.ToString().ToLower() + ".persistence.JoinTable");
                }

                break;
            case AssociationType.OneToOne:
                imports.Add(config.PersistenceMode.ToString().ToLower() + ".persistence.FetchType");
                imports.Add(config.PersistenceMode.ToString().ToLower() + ".persistence.JoinColumn");
                imports.Add(config.PersistenceMode.ToString().ToLower() + ".persistence.CascadeType");
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
                config.PersistenceMode.ToString().ToLower() + ".annotation.Generated",
            };

        if (classe.IsPersistent)
        {
            imports.Add(config.PersistenceMode.ToString().ToLower() + ".persistence.Entity");
            imports.Add(config.PersistenceMode.ToString().ToLower() + ".persistence.Table");
        }
        else
        {
            imports.Add("java.io.Serializable");
            if (classe.Properties.Any(p => p is IFieldProperty { Required: true, PrimaryKey: false }))
            {
                imports.Add(config.PersistenceMode.ToString().ToLower() + ".validation.constraints.NotNull");
            }
        }

        if (classe.Reference)
        {
            imports.AddRange(new List<string>
            {
                "org.hibernate.annotations.Cache",
                "org.hibernate.annotations.CacheConcurrencyStrategy"
            });
            if (classe.ReferenceValues.Any())
            {
                imports.AddRange(new List<string>
            {
                config.PersistenceMode.ToString().ToLower() + ".persistence.Enumerated",
                config.PersistenceMode.ToString().ToLower() + ".persistence.EnumType",
            });
            }

            if (classe.IsStatic())
            {
                imports.Add("org.hibernate.annotations.Immutable");
            }
        }

        if (classe.UniqueKeys.Any())
        {
            imports.Add(config.PersistenceMode.ToString().ToLower() + ".persistence.UniqueConstraint");
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
