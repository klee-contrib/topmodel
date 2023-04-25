using TopModel.Core;
using TopModel.Utils;
using TopModel.Generator.Core;

namespace TopModel.Generator.Php;

public static class PhpUtils
{
    public static string GetPhpType(this PhpConfig config, IProperty prop, bool asList = false)
    {
        return prop switch
        {
            AssociationProperty a => a.GetPhpType(),
            CompositionProperty c => config.GetPhpType(c),
            AliasProperty l => config.GetPhpType(l),
            RegularProperty r => config.GetPhpType(r, asList),
            _ => string.Empty,
        };
    }

    public static string GetPhpName(this IProperty prop, bool firstUpper = false)
    {
        string propertyName = prop.NameCamel;
        if (prop is AssociationProperty ap)
        {
            propertyName = ap.GetAssociationName();
        }

        return (firstUpper ? propertyName.ToFirstUpper() : propertyName);
    }

    public static string GetPhpType(this AssociationProperty ap)
    {
        var isList = ap.Type == AssociationType.OneToMany || ap.Type == AssociationType.ManyToMany;
        if (isList)
        {
            return $"Collection";
        }

        return ap.Association.NamePascal;
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

    public static string GetPhpType(this PhpConfig config, AliasProperty ap)
    {
        if (ap.Class != null && ap.Class.IsPersistent)
        {
            if (ap.Property is AssociationProperty asp)
            {
                return config.GetImplementation(ap.Property.Domain)!.Type.ParseTemplate(ap);
            }
            else
            {
                return config.GetPhpType(ap.Property, ap.AsList);
            }
        }

        if (ap.Property is AssociationProperty apr)
        {
            return config.GetPhpType(apr.Property, ap.AsList || apr.Type == AssociationType.ManyToMany || apr.Type == AssociationType.OneToMany);
        }
        else if (ap.Property is CompositionProperty cpo)
        {
            if (cpo.Kind == "list")
            {
                return $"List<{cpo.Composition.NamePascal}>";
            }
            else if (cpo.Kind == "object")
            {
                return cpo.Composition.NamePascal;
            }
            else if (cpo.DomainKind != null)
            {
                var phpType = config.GetImplementation(cpo.DomainKind)!.Type;
                if (!phpType.Contains("{composition.name}"))
                {
                    phpType += "<{composition.name}>";
                }

                return phpType.ParseTemplate(cpo);
            }
        }

        return config.GetImplementation(ap.Domain)!.Type;
    }

    public static string GetPhpType(this PhpConfig config, RegularProperty rp, bool asList)
    {
        return config.GetImplementation(asList ? rp.Domain.ListDomain! : rp.Domain)!.Type.ParseTemplate(rp);
    }

    public static string GetPhpType(this PhpConfig config, CompositionProperty cp)
    {
        return cp.Kind switch
        {
            "object" => cp.Composition.NamePascal,
            "list" => $"List<{cp.Composition.NamePascal}>",
            "async-list" => $"IAsyncEnumerable<{cp.Composition.NamePascal}>",
            string _ when config.GetImplementation(cp.DomainKind)!.Type.Contains("{composition.name}") => config.GetImplementation(cp.DomainKind)!.Type.ParseTemplate(cp),
            string _ => $"{config.GetImplementation(cp.DomainKind)!.Type}<{{composition.name}}>".ParseTemplate(cp)
        };
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

    public static (Namespace Namespace, string ModelPath) GetMapperLocation(this PhpConfig config, (Class Classe, FromMapper Mapper) mapper)
    {
        if (mapper.Classe.IsPersistent)
        {
            return (mapper.Classe.Namespace, config.EntitiesPath);
        }

        var persistentParam = mapper.Mapper.Params.FirstOrDefault(p => p.Class.IsPersistent);
        if (persistentParam != null)
        {
            return (persistentParam.Class.Namespace, config.EntitiesPath);
        }

        return (mapper.Classe.Namespace, config.DtosPath);
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
