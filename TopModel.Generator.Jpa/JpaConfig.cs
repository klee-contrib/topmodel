using System.Text.RegularExpressions;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Core.Model.Implementation;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Jpa;

public class JpaConfig : GeneratorConfigBase
{
    /// <summary>
    /// Localisation des classes persistées du modèle, relative au répertoire de génération. Par défaut, 'javagen/{app}/entities/{module}'.
    /// </summary>
    public string EntitiesPath { get; set; } = "javagen:{app}/entities/{module}";

    /// <summary>
    /// Localisation des DAOs, relative au répertoire de génération.
    /// </summary>
    public string? DaosPath { get; set; }

    /// <summary>
    /// Localisation des classses non persistées du modèle, relative au répertoire de génération. Par défaut, 'javagen/{app}/dtos/{module}'.
    /// </summary>
    public string DtosPath { get; set; } = "javagen:{app}/dtos/{module}";

    /// <summary>
    /// Localisation du l'API générée (client ou serveur), relative au répertoire de génération. Par défaut, 'javagen/{app}/api/{module}'.
    /// </summary>
    public string ApiPath { get; set; } = "javagen:{app}/api/{module}";

    /// <summary>
    /// Mode de génération de l'API ("Client" ou "Server").
    /// </summary>
    public string? ApiGeneration { get; set; }

    /// <summary>
    /// Localisation des ressources, relative au répertoire de génération.
    /// </summary>
    public string? ResourcesPath { get; set; }

    /// <summary>
    /// Option pour générer des getters et setters vers l'enum des références plutôt que sur la table
    /// </summary>
    public bool EnumShortcutMode { get; set; }

    /// <summary>
    /// Option pour générer une enum des champs des classes persistées
    /// </summary>
    public Target FieldsEnum { get; set; } = Target.None;

    /// <summary>
    /// Précise l'interface des fields enum générés.
    /// </summary>
    public string? FieldsEnumInterface { get; set; }

    /// <summary>
    /// Précise le nom du package dans lequel générer les controllers.
    /// </summary>
    public PersistenceMode PersistenceMode { get; set; } = PersistenceMode.Javax;

    /// <summary>
    /// Mode de génération des séquences.
    /// </summary>
    public IdentityConfig Identity { get; set; } = new() { Mode = IdentityMode.IDENTITY };

    public override string[] PropertiesWithLangVariableSupport => new[]
    {
        nameof(ResourcesPath)
    };

    public override string[] PropertiesWithTagVariableSupport => new[]
    {
        nameof(EntitiesPath),
        nameof(DaosPath),
        nameof(DtosPath),
        nameof(ApiPath),
        nameof(ApiGeneration),
        nameof(ResourcesPath)
    };

    public override string[] PropertiesWithModuleVariableSupport => new[]
    {
        nameof(EntitiesPath),
        nameof(DaosPath),
        nameof(DtosPath),
        nameof(ApiPath)
    };

    public override bool CanClassUseEnums(Class classe, IEnumerable<Class>? availableClasses = null, IFieldProperty? prop = null)
    {
        return base.CanClassUseEnums(classe, availableClasses, prop)
            && !classe.Properties.OfType<AssociationProperty>().Any(a => a.Association != classe && !CanClassUseEnums(a.Association, availableClasses));
    }

    public string GetApiPath(ModelFile file, string tag)
    {
        return Path.Combine(
            OutputDirectory,
            ResolveVariables(ApiPath!, tag, module: file.Namespace.Module).ToFilePath());
    }

    public string GetClassFileName(Class classe, string tag)
    {
        return Path.Combine(
            OutputDirectory,
            ResolveVariables(classe.IsPersistent ? EntitiesPath : DtosPath, tag, module: classe.Namespace.Module).ToFilePath(),
            $"{classe.NamePascal}.java");
    }

    public string GetMapperFilePath((Class Classe, FromMapper Mapper) mapper, string tag)
    {
        var (ns, modelPath) = GetMapperLocation(mapper);
        return Path.Combine(
            OutputDirectory,
            ResolveVariables(modelPath, tag: tag, module: ns.Module).ToFilePath(),
            $"{GetMapperName(ns, modelPath)}.java");
    }

    public string GetMapperFilePath((Class Classe, ClassMappings Mapper) mapper, string tag)
    {
        var (ns, modelPath) = GetMapperLocation(mapper);
        return Path.Combine(
            OutputDirectory,
            ResolveVariables(modelPath, tag: tag, module: ns.Module).ToFilePath(),
            $"{GetMapperName(ns, modelPath)}.java");
    }

    public string GetMapperImport(Namespace ns, string modelPath, string tag)
    {
        return $@"{GetPackageName(ns, modelPath, tag)}.{GetMapperName(ns, modelPath)}";
    }

    public (Namespace Namespace, string ModelPath) GetMapperLocation((Class Classe, FromMapper Mapper) mapper)
    {
        if (mapper.Classe.IsPersistent)
        {
            return (mapper.Classe.Namespace, EntitiesPath);
        }

        var persistentParam = mapper.Mapper.Params.FirstOrDefault(p => p.Class.IsPersistent);
        if (persistentParam != null)
        {
            return (persistentParam.Class.Namespace, EntitiesPath);
        }

        return (mapper.Classe.Namespace, DtosPath);
    }

    public (Namespace Namespace, string ModelPath) GetMapperLocation((Class Classe, ClassMappings Mapper) mapper)
    {
        if (mapper.Classe.IsPersistent)
        {
            return (mapper.Classe.Namespace, EntitiesPath);
        }

        if (mapper.Mapper.Class.IsPersistent)
        {
            return (mapper.Mapper.Class.Namespace, EntitiesPath);
        }

        return (mapper.Classe.Namespace, DtosPath);
    }

    public string GetMapperName(Namespace ns, string modelPath)
    {
        return $"{ns.ModuleFlat}{(modelPath == EntitiesPath ? string.Empty : "DTO")}Mappers";
    }

    public string GetPackageName(Endpoint endpoint, string tag)
    {
        return GetPackageName(endpoint.Namespace, ApiPath, tag);
    }

    public string GetPackageName(Class classe, string tag, bool? isPersistent = null)
    {
        return GetPackageName(
            classe.Namespace,
            isPersistent.HasValue ? isPersistent.Value ? EntitiesPath : DtosPath : classe.IsPersistent ? EntitiesPath : DtosPath,
            tag);
    }

    public string GetPackageName(Namespace ns, string modelPath, string tag)
    {
        return ResolveVariables(modelPath, tag, module: ns.Module).ToPackageName();
    }

    protected override string GetConstEnumName(string className, string refName)
    {
        return $"{className.ToPascalCase()}.Values.{refName}";
    }

    protected override string GetEnumType(string className, string propName, bool asList = false, bool isPrimaryKeyDef = false)
    {
        var type = $"{className.ToPascalCase()}.Values";
        return asList ? GetListType(type) : type;
    }

    protected override string GetListType(string name, bool useIterable = true)
    {
        return $"List<{name}>";
    }

    protected override bool IsEnumNameValid(string name)
    {
        return base.IsEnumNameValid(name) && !Regex.IsMatch(name ?? string.Empty, "(?<=[^$\\w'\"\\])(?!(abstract|assert|boolean|break|byte|case|catch|char|class|const|continue|default|double|do|else|enum|extends|false|final|finally|float|for|goto|if|implements|import|instanceof|int|interface|long|native|new|null|package|private|protected|public|return|short|static|strictfp|super|switch|synchronized|this|throw|throws|transient|true|try|void|volatile|while|_\\b))([A-Za-z_$][$\\w]*)");
    }
}