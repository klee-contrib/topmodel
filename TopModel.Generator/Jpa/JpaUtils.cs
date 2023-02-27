using System.Text.RegularExpressions;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Utils;

namespace TopModel.Generator.Jpa;

public static class JpaUtils
{
    public static string GetJavaType(this IProperty prop, bool asList = false)
    {
        return prop switch
        {
            AssociationProperty a => a.GetJavaType(),
            CompositionProperty c => c.GetJavaType(),
            AliasProperty l => l.GetJavaType(),
            RegularProperty r => r.GetJavaType(asList),
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
        if (ap.Type == AssociationType.ManyToMany || ap.Type == AssociationType.OneToMany)
        {
            return $"{ap.NameCamel}";
        }
        else
        {
            return $"{ap.Association.NameCamel}{ap.Role?.ToPascalCase() ?? string.Empty}";
        }
    }

    public static string GetJavaType(this AliasProperty ap)
    {
        if (ap.Class != null && ap.Class.IsPersistent)
        {
            if (ap.Property is AssociationProperty asp)
            {
                if (asp.IsEnum())
                {
                    return asp.Property.GetJavaType(ap.AsList || asp.Type == AssociationType.ManyToMany || asp.Type == AssociationType.OneToMany);
                }
                else
                {
                    return ap.Property.Domain.Java!.Type.ParseTemplate(ap);
                }
            }
            else
            {
                return ap.Property.GetJavaType(ap.AsList);
            }
        }

        if (ap.IsEnum())
        {
            return ap.Property.GetJavaType(ap.AsList);
        }
        else if (ap.Property is AssociationProperty apr)
        {
            return apr.Property.GetJavaType(ap.AsList || apr.Type == AssociationType.ManyToMany || apr.Type == AssociationType.OneToMany);
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
                var javaType = cpo.DomainKind.Java!.Type;
                if (!javaType.Contains("{composition.name}"))
                {
                    javaType += "<{composition.name}>";
                }

                return javaType.ParseTemplate(cpo);
            }
        }

        return ap.Domain.Java!.Type;
    }

    public static string GetJavaType(this RegularProperty rp, bool asList)
    {
        return rp.IsEnum() ? ((asList ? "List<" : string.Empty) + $"{rp.Class.NamePascal}.Values") + (asList ? ">" : string.Empty) : (asList ? rp.Domain.ListDomain! : rp.Domain).Java!.Type.ParseTemplate(rp);
    }

    public static string GetJavaType(this CompositionProperty cp)
    {
        return cp.Kind switch
        {
            "object" => cp.Composition.NamePascal,
            "list" => $"List<{cp.Composition.NamePascal}>",
            "async-list" => $"IAsyncEnumerable<{cp.Composition.NamePascal}>",
            string _ when cp.DomainKind!.Java!.Type.Contains("{composition.name}") => cp.DomainKind.Java.Type.ParseTemplate(cp),
            string _ => $"{cp.DomainKind.Java.Type}<{{composition.name}}>".ParseTemplate(cp)
        };
    }

    public static bool IsEnum(this IFieldProperty rp)
    {
        return rp.Class?.EnumKey == rp
            && rp.Class.Values.All(r => !Regex.IsMatch(r.Value[rp].ToString() ?? string.Empty, "(?<=[^$\\w'\"\\])(?!(abstract|assert|boolean|break|byte|case|catch|char|class|const|continue|default|double|do|else|enum|extends|false|final|finally|float|for|goto|if|implements|import|instanceof|int|interface|long|native|new|null|package|private|protected|public|return|short|static|strictfp|super|switch|synchronized|this|throw|throws|transient|true|try|void|volatile|while|_\\b))([A-Za-z_$][$\\w]*)"));
    }

    public static bool IsStatic(this Class c)
    {
        return c.PrimaryKey.Single().IsEnum()
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

    public static List<AssociationProperty> GetReverseProperties(this Class classe, List<Class> availableClasses)
    {
        if (classe.Reference)
        {
            return new List<AssociationProperty>();
        }

        return availableClasses
            .SelectMany(c => c.Properties)
            .OfType<AssociationProperty>()
            .Where(p => p is not JpaAssociationProperty)
            .Where(p => p.Type != AssociationType.OneToOne)
            .Where(p => p.Association == classe
                && (p.Type == AssociationType.OneToMany || p.Class.Namespace.RootModule == classe.Namespace.RootModule))
            .ToList();
    }

    public static IList<IProperty> GetProperties(this Class classe, JpaConfig config, List<Class> availableClasses, string tag)
    {
        if (classe.Reference)
        {
            return classe.Properties;
        }

        return classe.Properties.Concat(classe.GetReverseProperties(availableClasses).Select(p => new JpaAssociationProperty()
        {
            Association = p.Class,
            Type = p.Type == AssociationType.OneToMany ? AssociationType.ManyToOne
                : p.Type == AssociationType.ManyToOne ? AssociationType.OneToMany
                : AssociationType.ManyToMany,
            Comment = $"Association réciproque de {{@link {config.GetPackageName(p.Class, tag)}.{p.Class}#{p.GetJavaName()} {p.Class.NamePascal}.{p.GetJavaName()}}}",
            Class = classe,
            ReverseProperty = p,
            Role = p.Role
        })).ToList();
    }

    public static string GetClassFileName(this JpaConfig config, Class classe, string tag)
    {
        return Path.Combine(
            config.OutputDirectory,
            config.ResolveVariables(config.ModelRootPath, tag),
            Path.Combine(config.ResolveVariables(classe.IsPersistent ? config.EntitiesPackageName : config.DtosPackageName, tag).Split(".")),
            classe.Namespace.ModulePath.ToLower(),
            $"{classe.NamePascal}.java");
    }

    public static string GetApiPath(this JpaConfig config, ModelFile file, string tag)
    {
        return Path.Combine(
            config.OutputDirectory,
            Path.Combine(config.ResolveVariables(config.ApiRootPath!, tag).ToLower().Split(".")),
            Path.Combine(config.ResolveVariables(config.ApiPackageName, tag).Split('.')),
            file.Namespace.ModulePath.ToLower());
    }

    public static string GetPackageName(this JpaConfig config, Class classe, string tag, bool? isPersistant = null)
    {
        var packageRoot = config.ResolveVariables(
            isPersistant.HasValue
                ? isPersistant.Value
                    ? config.EntitiesPackageName
                    : config.DtosPackageName
                : classe.IsPersistent
                    ? config.EntitiesPackageName
                    : config.DtosPackageName,
            tag);
        return $"{packageRoot}.{classe.Namespace.Module.ToLower()}";
    }

    public static string GetPackageName(this JpaConfig config, Endpoint endpoint, string tag)
    {
        return $"{config.ResolveVariables(config.ApiPackageName, tag)}.{endpoint.Namespace.Module.ToLower()}";
    }

    public static string GetMapperFilePath(this JpaConfig config, Class classe, bool isPersistant, string tag)
    {
        return Path.Combine(
            config.OutputDirectory,
            config.ResolveVariables(config.ModelRootPath, tag),
            Path.Combine(GetMapperPackage(config, classe, isPersistant, tag).Split('.')),
            $"{GetMapperClassName(classe, isPersistant)}.java");
    }

    public static string GetMapperClassName(this Class classe, FromMapper mapper)
    {
        return GetMapperClassName(classe, IsPersistantMapper(classe, mapper));
    }

    public static string GetMapperClassName(this Class classe, ClassMappings mapper)
    {
        return GetMapperClassName(classe, IsPersistantMapper(classe, mapper));
    }

    public static string GetMapperClassName(this Class classe, bool isPersistant)
    {
        return $@"{classe.Namespace.ModuleFlat}{(isPersistant ? string.Empty : "DTO")}Mappers";
    }

    public static string GetMapperImport(this JpaConfig config, Class classe, FromMapper mapper, string tag)
    {
        return GetMapperImport(config, classe, IsPersistantMapper(classe, mapper), tag);
    }

    public static string GetMapperImport(this JpaConfig config, Class classe, ClassMappings mapper, string tag)
    {
        return GetMapperImport(config, classe, IsPersistantMapper(classe, mapper), tag);
    }

    public static string GetMapperPackage(this JpaConfig config, Class classe, bool isPersistant, string tag)
    {
        return GetPackageName(config, classe, tag, isPersistant);
    }

    private static string GetMapperImport(JpaConfig config, Class classe, bool isPersistant, string tag)
    {
        return $@"{GetMapperPackage(config, classe, isPersistant, tag)}.{GetMapperClassName(classe, isPersistant)}";
    }

    private static bool IsPersistantMapper(Class classe, FromMapper mapper)
    {
        return classe.IsPersistent || mapper.Params.Any(m => m.Class.IsPersistent);
    }

    private static bool IsPersistantMapper(Class classe, ClassMappings mapper)
    {
        return classe.IsPersistent || mapper.Class.IsPersistent;
    }
}
