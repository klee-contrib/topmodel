using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Jpa;

public static class JpaUtils
{
    public static string GetJavaName(this IProperty prop, bool firstUpper = false)
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
