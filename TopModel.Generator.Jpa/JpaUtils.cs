using System.Text.RegularExpressions;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Utils;
using TopModel.Generator.Core;

namespace TopModel.Generator.Jpa;

public static class JpaUtils
{
    public static string GetJavaType(this JpaConfig config, IProperty prop, bool asList = false)
    {
        return prop switch
        {
            AssociationProperty a => a.GetJavaType(),
            CompositionProperty c => config.GetJavaType(c),
            AliasProperty l => config.GetJavaType(l),
            RegularProperty r => config.GetJavaType(r, asList),
            _ => string.Empty,
        };
    }

    public static string GetJavaName(this IProperty prop, bool firstUpper = false)
    {
        string propertyName = prop.NameCamel;
        if (prop is AssociationProperty ap)
        {
            propertyName = ap.GetAssociationName();
        }

        return firstUpper ? propertyName.ToFirstUpper() : propertyName;
    }

    public static string GetJavaType(this AssociationProperty ap)
    {
        var isList = ap.Type == AssociationType.OneToMany || ap.Type == AssociationType.ManyToMany;
        if (isList)
        {
            return $"List<{ap.Association.NamePascal}>";
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

    public static string GetJavaType(this JpaConfig config, AliasProperty ap)
    {
        if (ap.Class != null && ap.Class.IsPersistent)
        {
            if (ap.Property is AssociationProperty asp)
            {
                if (asp.IsEnum())
                {
                    return config.GetJavaType(asp.Property, ap.AsList || asp.Type == AssociationType.ManyToMany || asp.Type == AssociationType.OneToMany);
                }
                else
                {
                    return config.GetImplementation(ap.Property.Domain)!.Type.ParseTemplate(ap);
                }
            }
            else
            {
                return config.GetJavaType(ap.Property, ap.AsList);
            }
        }

        if (ap.IsEnum())
        {
            return config.GetJavaType(ap.Property, ap.AsList);
        }
        else if (ap.Property is AssociationProperty apr)
        {
            return config.GetJavaType(apr.Property, ap.AsList || apr.Type == AssociationType.ManyToMany || apr.Type == AssociationType.OneToMany);
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
                var javaType = config.GetImplementation(cpo.DomainKind)!.Type;
                if (!javaType.Contains("{composition.name}"))
                {
                    javaType += "<{composition.name}>";
                }

                return javaType.ParseTemplate(cpo);
            }
        }

        return config.GetImplementation(ap.Domain)!.Type;
    }

    public static string GetJavaType(this JpaConfig config, RegularProperty rp, bool asList)
    {
        return rp.IsEnum() && rp.Class.IsStatic() ? ((asList ? "List<" : string.Empty) + $"{rp.Class.NamePascal}.Values") + (asList ? ">" : string.Empty) : config.GetImplementation(asList ? rp.Domain.ListDomain! : rp.Domain)!.Type.ParseTemplate(rp);
    }

    public static string GetJavaType(this JpaConfig config, CompositionProperty cp)
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

    public static bool IsEnum(this IFieldProperty rp)
    {
        return rp.Class?.EnumKey == rp
            && rp.Class.Values.All(r => !Regex.IsMatch(r.Value[rp].ToString() ?? string.Empty, "(?<=[^$\\w'\"\\])(?!(abstract|assert|boolean|break|byte|case|catch|char|class|const|continue|default|double|do|else|enum|extends|false|final|finally|float|for|goto|if|implements|import|instanceof|int|interface|long|native|new|null|package|private|protected|public|return|short|static|strictfp|super|switch|synchronized|this|throw|throws|transient|true|try|void|volatile|while|_\\b))([A-Za-z_$][$\\w]*)"));
    }

    public static bool IsStatic(this Class c)
    {
        return c.Enum
            && !c.Properties.OfType<AssociationProperty>().Any(a => a.Association != c && !a.Association.IsStatic());
    }

    public static bool IsEnum(this AliasProperty ap)
    {
        return ap.Property is RegularProperty rp && rp.IsEnum();
    }

    public static bool IsAssociatedEnum(this AliasProperty ap)
    {
        return ap.Property is AssociationProperty apr && apr.IsEnum();
    }

    public static bool IsEnum(this AssociationProperty apr)
    {
        return apr.Property != null && apr.Property.IsEnum();
    }

    public static string GetClassFileName(this JpaConfig config, Class classe, string tag)
    {
        return Path.Combine(
            config.OutputDirectory,
            config.ResolveVariables(classe.IsPersistent ? config.EntitiesPath : config.DtosPath, tag, module: classe.Namespace.Module).ToFilePath(),
            $"{classe.NamePascal}.java");
    }

    public static string GetApiPath(this JpaConfig config, ModelFile file, string tag)
    {
        return Path.Combine(
            config.OutputDirectory,
            config.ResolveVariables(config.ApiPath!, tag, module: file.Namespace.Module).ToFilePath());
    }

    public static string GetPackageName(this JpaConfig config, Class classe, string tag, bool? isPersistent = null)
    {
        return config.GetPackageName(
            classe.Namespace,
            isPersistent.HasValue ? isPersistent.Value ? config.EntitiesPath : config.DtosPath : classe.IsPersistent ? config.EntitiesPath : config.DtosPath,
            tag);
    }

    public static string GetPackageName(this JpaConfig config, Endpoint endpoint, string tag)
    {
        return config.GetPackageName(endpoint.Namespace, config.ApiPath, tag);
    }

    public static string GetPackageName(this JpaConfig config, Namespace ns, string modelPath, string tag)
    {
        return config.ResolveVariables(modelPath, tag, module: ns.Module).ToPackageName();
    }

    public static string GetMapperFilePath(this JpaConfig config, (Class Classe, FromMapper Mapper) mapper, string tag)
    {
        var (ns, modelPath) = config.GetMapperLocation(mapper);
        return Path.Combine(
            config.OutputDirectory,
            config.ResolveVariables(
                modelPath,
                tag: tag,
                module: ns.Module).ToFilePath(),
            $"{config.GetMapperName(ns, modelPath)}.java");
    }

    public static string GetMapperFilePath(this JpaConfig config, (Class Classe, ClassMappings Mapper) mapper, string tag)
    {
        var (ns, modelPath) = config.GetMapperLocation(mapper);
        return Path.Combine(
            config.OutputDirectory,
            config.ResolveVariables(
                modelPath,
                tag: tag,
                module: ns.Module).ToFilePath(),
            $"{config.GetMapperName(ns, modelPath)}.java");
    }

    public static (Namespace Namespace, string ModelPath) GetMapperLocation(this JpaConfig config, (Class Classe, FromMapper Mapper) mapper)
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

    public static (Namespace Namespace, string ModelPath) GetMapperLocation(this JpaConfig config, (Class Classe, ClassMappings Mapper) mapper)
    {
        if (mapper.Classe.IsPersistent)
        {
            return (mapper.Classe.Namespace, config.EntitiesPath);
        }

        if (mapper.Mapper.Class.IsPersistent)
        {
            return (mapper.Mapper.Class.Namespace, config.EntitiesPath);
        }

        return (mapper.Classe.Namespace, config.DtosPath);
    }

    public static string GetMapperName(this JpaConfig config, Namespace ns, string modelPath)
    {
        return $"{ns.ModuleFlat}{(modelPath == config.EntitiesPath ? string.Empty : "DTO")}Mappers";
    }

    public static string GetMapperImport(this JpaConfig config, Namespace ns, string modelPath, string tag)
    {
        return $@"{config.GetPackageName(ns, modelPath, tag)}.{config.GetMapperName(ns, modelPath)}";
    }

    public static string ToFilePath(this string path)
    {
        return path.ToLower().Replace(':', '.').Replace('.', Path.DirectorySeparatorChar);
    }

    public static string ToPackageName(this string path)
    {
        return path.Split(':').Last().ToLower().Replace('/', '.').Replace('\\', '.');
    }
}
