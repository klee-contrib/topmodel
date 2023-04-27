using TopModel.Core;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Php;

public static class PhpUtils
{
    public static string GetPhpName(this IProperty prop, bool firstUpper = false)
    {
        string propertyName = prop.NameCamel;
        if (prop is AssociationProperty ap)
        {
            propertyName = ap.GetAssociationName();
        }

        return firstUpper ? propertyName.ToFirstUpper() : propertyName;
    }

    public static string GetAssociationName(this AssociationProperty ap)
    {
        if (ap.Type.IsToMany())
        {
            return $"{ap.NameCamel}";
        }
        else
        {
            return $"{ap.Association.NameCamel}{ap.Role?.ToPascalCase() ?? string.Empty}";
        }
    }

    public static string GetPackageName(this PhpConfig config, Class classe, string tag, bool? isPersistent = null)
    {
        return config.GetPackageName(
            classe.Namespace,
            isPersistent.HasValue ? isPersistent.Value ? config.EntitiesPath : config.DtosPath : classe.IsPersistent ? config.EntitiesPath : config.DtosPath,
            tag);
    }

    public static string GetPackageName(this PhpConfig config, Namespace ns, string modelPath, string tag)
    {
        return config.ResolveVariables(modelPath, tag, module: ns.Module).ToPackageName();
    }

    public static string ToPackageName(this string path)
    {
        return @"App\" + path.Split(':').Last().Replace('/', '\\').Replace('.', '\\');
    }

    public static string ToFilePath(this string path)
    {
        return path.Replace(':', '.').Replace('.', Path.DirectorySeparatorChar);
    }

    public static string GetClassFileName(this PhpConfig config, Class classe, string tag)
    {
        return Path.Combine(
            config.OutputDirectory,
            config.ResolveVariables(classe.IsPersistent ? config.EntitiesPath : config.DtosPath, tag, module: classe.Namespace.Module).ToFilePath(),
            $"{classe.NamePascal}.php");
    }
}
