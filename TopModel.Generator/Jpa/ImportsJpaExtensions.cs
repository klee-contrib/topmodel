using TopModel.Core;

namespace TopModel.Generator.Jpa;

public static class ImportsJpaExtensions
{
    public static List<string> GetPersistenceImports(this IProperty p, JpaConfig config)
    {
        return p switch
        {
            AssociationProperty ap => ap.GetPersistenceImports(config),
            IFieldProperty fp => fp.GetPersistenceImports(config),
            _ => new List<string>(),
        };
    }

    public static List<string> GetTypeImports(this IProperty p, JpaConfig config)
    {
        return p switch
        {
            CompositionProperty cp => cp.GetTypeImports(config),
            AssociationProperty ap => ap.GetTypeImports(config),
            IFieldProperty fp => fp.GetTypeImports(config),
            _ => new List<string>(),
        };
    }

    public static List<string> GetPersistenceImports(this IFieldProperty rp, JpaConfig config)
    {
        var imports = new List<string>();
        if (rp.Class != null && rp.Class.IsPersistent)
        {
            imports.Add(config.PersistenceMode.ToString().ToLower() + ".persistence.Column");
        }

        var javaType = rp.Domain.Java;

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

        return imports;
    }

    public static List<string> GetImports(this AliasProperty ap, JpaConfig config)
    {
        var imports = new List<string>();
        if (ap.Class != null && ap.Class.IsPersistent && ap.Property is AssociationProperty asp)
        {
            if (asp.IsEnum())
            {
                imports.AddRange(asp.Property.GetPersistenceImports(config));
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

        imports.AddRange(rp.Domain.Java!.Imports.Select(i => i.ParseTemplate(rp)));

        return imports;
    }

    public static List<string> GetPersistenceImports(this AssociationProperty ap, JpaConfig config)
    {
        var persistenceRoot = $"{config.PersistenceMode.ToString().ToLower()}.persistence.";
        var imports = new List<string>
        {
            $"{persistenceRoot}{ap.Type}"
        };

        switch (ap.Type)
        {
            case AssociationType.OneToMany:
                imports.Add($"{persistenceRoot}FetchType");
                imports.Add($"{persistenceRoot}CascadeType");
                if (!(ap is JpaAssociationProperty jap && jap.IsReverse) && ap.Association.Namespace.Module.Split('.').First() != ap.Class.Namespace.Module.Split('.').First())
                {
                    imports.Add($"{persistenceRoot}JoinColumn");
                }

                break;
            case AssociationType.ManyToOne:
                imports.Add($"{persistenceRoot}FetchType");
                imports.Add($"{persistenceRoot}JoinColumn");
                break;
            case AssociationType.ManyToMany:
                imports.Add("java.util.List");
                imports.Add($"{persistenceRoot}JoinColumn");
                imports.Add($"{persistenceRoot}CascadeType");
                imports.Add($"{persistenceRoot}FetchType");
                if (!(ap is JpaAssociationProperty japo && japo.IsReverse))
                {
                    imports.Add($"{persistenceRoot}JoinTable");
                }

                break;
            case AssociationType.OneToOne:
                imports.Add($"{persistenceRoot}FetchType");
                imports.Add($"{persistenceRoot}JoinColumn");
                imports.Add($"{persistenceRoot}CascadeType");
                break;
        }

        return imports;
    }

    public static List<string> GetTypeImports(this IFieldProperty rp, JpaConfig config)
    {
        var imports = new List<string>();

        var javaType = rp.Domain.Java;
        if (javaType?.Imports != null)
        {
            imports.AddRange(javaType.Imports.Select(i => i.ParseTemplate(rp)));
        }

        if (rp is AliasProperty apo)
        {
            imports.AddRange(apo.GetTypeImports(config));
        }
        else if (rp is RegularProperty rpr)
        {
            imports.AddRange(rpr.GetTypeImports(config));
        }

        return imports;
    }

    public static List<string> GetTypeImports(this AliasProperty ap, JpaConfig config)
    {
        var imports = new List<string>();
        if (ap.Class != null && ap.Class.IsPersistent && ap.Property is AssociationProperty asp)
        {
            if (asp.IsEnum())
            {
                imports.AddRange(asp.Property.GetTypeImports(config));
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

        if (ap.Property is AssociationProperty apo && (apo.Type == AssociationType.ManyToMany || apo.Type == AssociationType.OneToMany))
        {
            imports.Add("java.util.List");
        }

        return imports;
    }

    public static List<string> GetTypeImports(this RegularProperty rp, JpaConfig config)
    {
        var imports = new List<string>();
        if (rp.IsEnum())
        {
            imports.Add($"{rp.Class.GetImport(config)}");
        }

        imports.AddRange(rp.Domain.Java!.Imports.Select(i => i.ParseTemplate(rp)));

        return imports;
    }

    public static List<string> GetTypeImports(this AssociationProperty ap, JpaConfig config)
    {
        var persistenceRoot = $"{config.PersistenceMode.ToString().ToLower()}.persistence.";
        var imports = new List<string>();

        switch (ap.Type)
        {
            case AssociationType.OneToMany:
            case AssociationType.ManyToMany:
                imports.Add("java.util.List");
                break;
        }

        if (ap.Association.Namespace.Module != ap.Class.Namespace.Module)
        {
            imports.Add(ap.Association.GetImport(config));
        }

        if (ap.Association.Reference)
        {
            imports.AddRange(ap.Property.GetTypeImports(config));
        }

        return imports;
    }

    public static List<string> GetTypeImports(this CompositionProperty cp, JpaConfig config)
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
        else if (cp.DomainKind != null)
        {
            imports.AddRange(cp.DomainKind.Java!.Imports.Select(i => i.ParseTemplate(cp)));
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
        var javaOrJakarta = config.PersistenceMode.ToString().ToLower();
        var imports = new List<string>
            {
                $"{javaOrJakarta}.annotation.Generated",
            };

        if (classe.IsPersistent)
        {
            imports.Add($"{javaOrJakarta}.persistence.Entity");
            imports.Add($"{javaOrJakarta}.persistence.Table");
        }
        else
        {
            imports.Add("java.io.Serializable");
            if (classe.Properties.Any(p => p is IFieldProperty { Required: true, PrimaryKey: false }))
            {
                imports.Add($"{javaOrJakarta}.validation.constraints.NotNull");
            }
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
        return imports;
    }
}
