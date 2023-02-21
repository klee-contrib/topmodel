using System.Text.RegularExpressions;
using TopModel.Core;
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

    public static string GetJavaName(this IProperty prop)
    {
        string propertyName = prop.Name.ToFirstLower();
        if (prop is AssociationProperty ap)
        {
            propertyName = ap.GetAssociationName();
        }

        return propertyName;
    }

    public static string GetJavaType(this AssociationProperty ap)
    {
        var isList = ap.Type == AssociationType.OneToMany || ap.Type == AssociationType.ManyToMany;
        if (isList)
        {
            return $"List<{ap.Association.Name}>";
        }

        return ap.Association.Name;
    }

    public static string GetAssociationName(this AssociationProperty ap)
    {
        if (ap.Type == AssociationType.ManyToMany || ap.Type == AssociationType.OneToMany)
        {
            return $"{ap.Name.ToFirstLower()}";
        }
        else
        {
            return $"{ap.Association.Name.ToFirstLower()}{ap.Role?.ToFirstUpper() ?? string.Empty}";
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
                return $"List<{cpo.Composition.Name}>";
            }
            else if (cpo.Kind == "object")
            {
                return cpo.Composition.Name;
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
        return rp.IsEnum() ? ((asList ? "List<" : string.Empty) + $"{rp.Class.Name.ToFirstUpper()}.Values") + (asList ? ">" : string.Empty) : (asList ? rp.Domain.ListDomain! : rp.Domain).Java!.Type.ParseTemplate(rp);
    }

    public static string GetJavaType(this CompositionProperty cp)
    {
        return cp.Kind switch
        {
            "object" => cp.Composition.Name,
            "list" => $"List<{cp.Composition.Name}>",
            "async-list" => $"IAsyncEnumerable<{cp.Composition.Name}>",
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
                && (p.Type == AssociationType.OneToMany || p.Class.Namespace.Module.Split('.').First() == classe.Namespace.Module.Split('.').First()))
            .ToList();
    }

    public static IList<IProperty> GetProperties(this Class classe, JpaConfig config, List<Class> availableClasses)
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
            Comment = $"Association réciproque de {{@link {p.Class.GetPackageName(config)}.{p.Class}#{p.GetJavaName()} {p.Class.Name}.{p.GetJavaName()}}}",
            Class = classe,
            ReverseProperty = p,
            Role = p.Role
        })).ToList();
    }

    public static string GetPackageName(this Class classe, JpaConfig config)
    {
        var packageRoot = classe.IsPersistent ? config.EntitiesPackageName : config.DtosPackageName;
        return $"{packageRoot}.{classe.Namespace.Module.ToLower()}";
    }

    public static string? GetMapperFilePath(this JpaConfig config, Class? sampleClass)
    {
        if (sampleClass == null)
        {
            return null;
        }

        return GetMapperFilePath(config, sampleClass.ModelFile.Module, sampleClass.IsPersistent);
    }

    public static string? GetMapperFilePath(this JpaConfig config, string module, bool isPersistent)
    {
        var directory = Path.Combine(config.OutputDirectory, config.ModelRootPath, string.Join('/', (isPersistent ? config.EntitiesPackageName : config.DtosPackageName).Split('.')), module.Split('.').First().ToLower());
        Directory.CreateDirectory(directory);

        return Path.Combine(directory, GetMapperClassFileName(config, module, isPersistent)!);
    }

    public static string? GetMapperClassName(this JpaConfig config, string module, bool isPersistant)
    {
        if (module == null)
        {
            return null;
        }

        return $@"{module.Split('.').First()}{(isPersistant ? string.Empty : "DTO")}Mappers";
    }

    public static string? GetMapperClassName(this JpaConfig config, Class classe, FromMapper? mapper)
    {
        if (mapper == null)
        {
            return null;
        }

        var isPersistant = classe.IsPersistent || mapper.Params.Any(m => m.Class.IsPersistent);
        return GetMapperClassName(config, classe.ModelFile.Module, isPersistant);
    }

    public static string? GetMapperClassName(this JpaConfig config, Class classe, ClassMappings? mapper)
    {
        if (mapper == null)
        {
            return null;
        }

        var isPersistant = classe.IsPersistent || mapper.Class.IsPersistent;
        return GetMapperClassName(config, classe.ModelFile.Module, isPersistant);
    }

    public static string? GetMapperClassFileName(this JpaConfig config, Class? sampleClass)
    {
        if (sampleClass == null)
        {
            return null;
        }

        return $@"{GetMapperClassName(config, sampleClass.ModelFile.Module, sampleClass.IsPersistent)}.java";
    }

    public static string? GetMapperClassFileName(this JpaConfig config, string module, bool isPersistant)
    {
        return $@"{GetMapperClassName(config, module, isPersistant)}.java";
    }

    public static bool IsPersistantMapper(Class classe, ClassMappings mapper)
    {
        return classe.IsPersistent || mapper.Class.IsPersistent;
    }

    public static string? GetMapperImport(this JpaConfig config, Class? classe, FromMapper mapper)
    {
        if (classe == null)
        {
            return null;
        }

        var isPersistant = IsPersistantMapper(classe, mapper);

        return $@"{(isPersistant ? config.EntitiesPackageName : config.DtosPackageName)}.{classe.ModelFile.Module.Split('.').First().ToLower()}.{GetMapperClassName(config, classe.ModelFile.Module, isPersistant)}";
    }

    public static string? GetMapperImport(this JpaConfig config, Class? classe, ClassMappings mapper)
    {
        if (classe == null)
        {
            return null;
        }

        var isPersistant = IsPersistantMapper(classe, mapper);

        return $@"{(isPersistant ? config.EntitiesPackageName : config.DtosPackageName)}.{classe.ModelFile.Module.Split('.').First().ToLower()}.{GetMapperClassName(config, classe.ModelFile.Module, isPersistant)}";
    }

    private static bool IsPersistantMapper(Class classe, FromMapper mapper)
    {
        return classe.IsPersistent || mapper.Params.Any(m => m.Class.IsPersistent);
    }
}
