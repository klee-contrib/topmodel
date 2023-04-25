using TopModel.Core;
using TopModel.Generator.Core;

namespace TopModel.Generator.Php;

public static class ImportsPhpExtensions
{

    public static List<string> GetTypeImports(this IFieldProperty rp, PhpConfig config, string tag)
    {
        var imports = new List<string>();

        var javaType = config.GetImplementation(rp.Domain);
        if (javaType?.Imports != null)
        {
            imports.AddRange(javaType.Imports.Select(i => i.ParseTemplate(rp)));
        }

        if (rp is AliasProperty apo)
        {
            imports.AddRange(apo.GetTypeImports(config, tag));
        }
        else if (rp is RegularProperty rpr)
        {
            imports.AddRange(rpr.GetTypeImports(config, tag));
        }

        return imports;
    }

    public static List<string> GetTypeImports(this AliasProperty ap, PhpConfig config, string tag)
    {
        var imports = new List<string>();

        if (ap.Property is AssociationProperty apo && apo.Type.IsToMany())
        {
            imports.Add(@"Doctrine\Common\Collections\Collection");
        }

        return imports;
    }

    public static List<string> GetTypeImports(this RegularProperty rp, PhpConfig config, string tag)
    {
        var imports = new List<string>();

        imports.AddRange(config.GetImplementation(rp.Domain)!.Imports.Select(i => i.ParseTemplate(rp)));

        return imports;
    }

    public static List<string> GetTypeImports(this AssociationProperty ap, PhpConfig config, string tag)
    {
        var imports = new List<string>();

        switch (ap.Type)
        {
            case AssociationType.OneToMany:
            case AssociationType.ManyToMany:
                imports.Add(@"Doctrine\Common\Collections\Collection");
                break;
        }

        if (ap.Association.Namespace.Module != ap.Class.Namespace.Module)
        {
            imports.Add(ap.Association.GetImport(config, tag));
        }

        if (ap.Association.Reference)
        {
            imports.AddRange(ap.Property.GetTypeImports(config, tag));
        }

        return imports;
    }

    public static List<string> GetTypeImports(this CompositionProperty cp, PhpConfig config, string tag)
    {
        var imports = new List<string>();
        if (cp.Composition.Namespace.Module != cp.Class?.Namespace.Module)
        {
            imports.Add(cp.Composition.GetImport(config, tag));
        }

        if (cp.Kind == "list")
        {
            imports.Add(@"Doctrine\Common\Collections\Collection");
        }
        else if (cp.DomainKind != null)
        {
            imports.AddRange(config.GetImplementation(cp.DomainKind)!.Imports.Select(i => i.ParseTemplate(cp)));
        }

        return imports;
    }

    public static List<string> GetKindImports(this CompositionProperty cp, PhpConfig config)
    {
        var imports = new List<string>();

        if (cp.Kind == "list")
        {
            imports.Add(@"Doctrine\Common\Collections\Collection");
        }
        else if (cp.DomainKind != null)
        {
            imports.AddRange(config.GetImplementation(cp.DomainKind)!.Imports.Select(i => i.ParseTemplate(cp)));
        }

        return imports;
    }

    public static string GetImport(this Class classe, PhpConfig config, string tag)
    {
        return @$"{config.GetPackageName(classe, tag)}\{classe.NamePascal}";
    }

    public static List<string> GetImports(this Class classe, List<Class> classes, PhpConfig config, string tag)
    {
        var imports = new List<string>();

        if (classe.Extends != null)
        {
            imports.Add(classe.GetImport(config, tag));
        }

        imports
        .AddRange(
            classe.FromMappers.SelectMany(fm => fm.Params).Select(fmp => fmp.Class.GetImport(config, tag)));
        imports
        .AddRange(
            classe.ToMappers.Select(fmp => fmp.Class.GetImport(config, tag)));

        // Suppression des imports inutiles
        return imports;
    }
}
