using TopModel.Core;

namespace TopModel.Generator.Jpa;

public static class ImportsJpaExtensions
{
    public static string GetImport(this Class classe, JpaConfig config, string tag)
    {
        return $"{config.GetPackageName(classe, tag)}.{classe.NamePascal}";
    }

    public static List<string> GetImports(this Class classe, JpaConfig config, string tag)
    {
        var javaOrJakarta = config.PersistenceMode.ToString().ToLower();
        var imports = new List<string>();

        if (classe.Extends != null)
        {
            imports.Add(classe.GetImport(config, tag));
        }

        imports
            .AddRange(classe.FromMappers.SelectMany(fm => fm.Params).Select(fmp => fmp.Class.GetImport(config, tag)));
        imports
            .AddRange(classe.ToMappers.Select(fmp => fmp.Class.GetImport(config, tag)));

        // Suppression des imports inutiles
        return imports;
    }

    public static List<string> GetKindImports(this CompositionProperty cp, JpaConfig config, string tag)
    {
        return config.GetDomainImports(cp, tag).ToList();
    }

    public static List<string> GetTypeImports(this IProperty p, JpaConfig config, string tag, bool noAnnotations = false)
    {
        return p switch
        {
            CompositionProperty cp => cp.GetTypeImports(config, tag),
            AssociationProperty ap => ap.GetTypeImports(config, tag),
            IFieldProperty fp => fp.GetTypeImports(config, tag, noAnnotations),
            _ => new List<string>(),
        };
    }

    private static List<string> GetTypeImports(this AliasProperty ap, JpaConfig config, string tag)
    {
        var imports = new List<string>();
        if (ap.Class != null && ap.Class.IsPersistent && ap.Property is AssociationProperty asp)
        {
            if (config.CanClassUseEnums(asp.Association))
            {
                imports.AddRange(asp.Property.GetTypeImports(config, tag));
            }
        }

        if (config.CanClassUseEnums(ap.Property.Class))
        {
            imports.Add(ap.Property.Class.GetImport(config, tag));
        }
        else if (ap.Property is AssociationProperty apr && config.CanClassUseEnums(apr.Property.Class))
        {
            imports.Add(apr.Association.GetImport(config, tag));
        }

        if (ap.Property is AssociationProperty apo && apo.Type.IsToMany())
        {
            imports.Add("java.util.List");
        }

        return imports;
    }

    private static List<string> GetTypeImports(this AssociationProperty ap, JpaConfig config, string tag)
    {
        var imports = new List<string>();

        if (ap.Type.IsToMany())
        {
            imports.Add("java.util.List");
        }

        imports.Add(ap.Association.GetImport(config, tag));

        if (ap.Association.Reference)
        {
            imports.AddRange(ap.Property.GetTypeImports(config, tag));
        }

        return imports;
    }

    private static List<string> GetTypeImports(this CompositionProperty cp, JpaConfig config, string tag)
    {
        var imports = new List<string>();
        if (cp.Composition.Namespace.Module != cp.Class?.Namespace.Module)
        {
            imports.Add(cp.Composition.GetImport(config, tag));
        }

        imports.AddRange(config.GetDomainImports(cp, tag));

        return imports;
    }

    private static List<string> GetTypeImports(this IFieldProperty rp, JpaConfig config, string tag, bool noAnnotations = false)
    {
        var imports = new List<string>();

        imports.AddRange(config.GetDomainImports(rp, tag, noAnnotations));

        if (rp is AliasProperty apo)
        {
            imports.AddRange(apo.GetTypeImports(config, tag));
        }
        else if (rp is RegularProperty rpr)
        {
            imports.AddRange(rpr.GetTypeImports(config, tag, noAnnotations));
        }

        return imports;
    }

    private static List<string> GetTypeImports(this RegularProperty rp, JpaConfig config, string tag, bool noAnnotations = false)
    {
        var imports = new List<string>();
        if (rp.Class != null && config.CanClassUseEnums(rp.Class))
        {
            imports.Add($"{rp.Class.GetImport(config, tag)}");
        }

        imports.AddRange(config.GetDomainImports(rp, tag, noAnnotations));

        return imports;
    }
}
