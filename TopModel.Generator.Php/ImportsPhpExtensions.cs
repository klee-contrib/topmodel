using TopModel.Core;

namespace TopModel.Generator.Php;

public static class ImportsPhpExtensions
{
    public static string GetImport(this Class classe, PhpConfig config, string tag)
    {
        return @$"{config.GetPackageName(classe, tag)}\{classe.NamePascal}";
    }

    public static List<string> GetTypeImports(this IFieldProperty rp, PhpConfig config, string tag)
    {
        var imports = new List<string>();

        imports.AddRange(config.GetDomainImports(rp, tag));

        if (rp is AliasProperty apo)
        {
            imports.AddRange(apo.GetTypeImports());
        }
        else if (rp is RegularProperty rpr)
        {
            imports.AddRange(rpr.GetTypeImports(config, tag));
        }

        return imports;
    }

    private static List<string> GetTypeImports(this AliasProperty ap)
    {
        var imports = new List<string>();

        if (ap.Property is AssociationProperty apo && apo.Type.IsToMany())
        {
            imports.Add(@"Doctrine\Common\Collections\Collection");
        }

        return imports;
    }
}
