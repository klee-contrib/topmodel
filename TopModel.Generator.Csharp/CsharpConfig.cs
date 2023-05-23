using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Generator.Core;
using TopModel.Utils;
using YamlDotNet.Serialization;

namespace TopModel.Generator.Csharp;

/// <summary>
/// Paramètres pour la génération du C#.
/// </summary>
public class CsharpConfig : GeneratorConfigBase
{
    private string? _referencesModelPath;

    /// <summary>
    /// Localisation du modèle persisté, relative au répertoire de génération. Par défaut : {app}.{module}.Models.
    /// </summary>
    public string PersistentModelPath { get; set; } = "{app}.{module}.Models";

    /// <summary>
    /// Localisation des classes de références persistées, relative au répertoire de génération. Par défaut égal à "PersistentModelPath".
    /// </summary>
    public string PersistentReferencesModelPath { get => _referencesModelPath ?? PersistentModelPath; set => _referencesModelPath = value; }

    /// <summary>
    /// Localisation du modèle non persisté, relative au répertoire de génération. Par défaut : {app}.{module}.Models/Dto.
    /// </summary>
    public string NonPersistentModelPath { get; set; } = "{app}.{module}.Models/Dto";

    /// <summary>
    /// Localisation du l'API générée (client ou serveur), relative au répertoire de génération. Par défaut : "{app}.Web".
    /// </summary>
    public string ApiRootPath { get; set; } = "{app}.Web";

    /// <summary>
    /// Chemin vers lequel sont créés les fichiers d'endpoints générés, relative à la racine de l'API. Par défaut : "{module}".
    /// </summary>
    public string ApiFilePath { get; set; } = "{module}";

    /// <summary>
    /// Mode de génération de l'API ("Client" ou "Server").
    /// </summary>
    public string? ApiGeneration { get; set; }

    /// <summary>
    /// Génère des contrôleurs d'API synchrones.
    /// </summary>
    public bool NoAsyncControllers { get; set; }

    /// <summary>
    /// Localisation du DbContext, relative au répertoire de génération.
    /// </summary>
    public string? DbContextPath { get; set; }

    /// <summary>
    /// Nom du DbContext. Par défaut : {app}DbContext.
    /// </summary>
    public string DbContextName { get; set; } = "{app}DbContext";

#nullable disable

    /// <summary>
    /// Chemin vers lequel générer les interfaces d'accesseurs de référence. Par défaut : {DbContextPath}/Reference.
    /// </summary>
    public string ReferenceAccessorsInterfacePath { get; set; }

    /// <summary>
    /// Chemin vers lequel générer les implémentation d'accesseurs de référence. Par défaut : {DbContextPath}/Reference.
    /// </summary>
    public string ReferenceAccessorsImplementationPath { get; set; }
#nullable enable

    /// <summary>
    /// Nom des accesseurs de référence (préfixé par 'I' pour l'interface). Par défaut : {module}ReferenceAccessors.
    /// </summary>
    public string ReferenceAccessorsName { get; set; } = "{module}ReferenceAccessors";

    /// <summary>
    /// Utilise les migrations EF pour créer/mettre à jour la base de données.
    /// </summary>
    public bool UseEFMigrations { get; set; }

    /// <summary>
    /// Utilise des noms de tables et de colonnes en lowercase.
    /// </summary>
    public bool UseLowerCaseSqlNames { get; set; }

    /// <summary>
    /// Le nom du schéma de base de données à cibler (si non renseigné, EF utilise 'dbo'/"public').
    /// </summary>
    public string? DbSchema { get; set; }

    /// <summary>
    /// Utilise les features C# 10 dans la génération.
    /// </summary>
    public bool UseLatestCSharp { get; set; } = true;

    /// <summary>
    /// Si on génère avec Kinetix.
    /// </summary>
    public bool Kinetix { get; set; } = true;

    /// <summary>
    /// Namespace de l'enum de domaine pour Kinetix. Par défaut : '{app}.Common'.
    /// </summary>
    public string DomainNamespace { get; set; } = "{app}.Common";

    /// <summary>
    /// Retire les attributs de colonnes sur les alias.
    /// </summary>
    public bool NoColumnOnAlias { get; set; }

    /// <summary>
    /// Considère tous les classes comme étant non-Persistentes (= pas d'attribut SQL).
    /// </summary>
    [YamlMember(Alias = "noPersistence")]
    public string? NoPersistenceParam { get; set; }

    /// <summary>
    /// Si un mapper contient au moins une classe de ces tags, alors il sera généré avec les tags de cette classe (au lieu du comportement par défaut qui priorise les tags de la classe persistée puis de celle qui définit le mapper).
    /// </summary>
    public string[] MapperTagsOverrides { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Utilise des enums au lieu de strings pour les PKs de listes de référence statiques.
    /// </summary>
    public bool EnumsForStaticReferences { get; set; }

    /// <summary>
    /// Annote les tables et les colonnes générées par EF avec les commentaires du modèle (nécessite `UseEFMigrations`).
    /// </summary>
    public bool UseEFComments { get; set; }

    public override string[] PropertiesWithModuleVariableSupport => new[]
    {
        nameof(PersistentModelPath),
        nameof(PersistentReferencesModelPath),
        nameof(NonPersistentModelPath),
        nameof(ApiFilePath),
        nameof(DbSchema),
        nameof(ReferenceAccessorsName),
        nameof(ReferenceAccessorsInterfacePath),
        nameof(ReferenceAccessorsImplementationPath)
    };

    public override string[] PropertiesWithTagVariableSupport => new[]
    {
        nameof(PersistentModelPath),
        nameof(PersistentReferencesModelPath),
        nameof(NonPersistentModelPath),
        nameof(NoPersistenceParam),
        nameof(DbContextPath),
        nameof(DbContextName),
        nameof(DbSchema),
        nameof(ReferenceAccessorsName),
        nameof(ReferenceAccessorsInterfacePath),
        nameof(ReferenceAccessorsImplementationPath),
        nameof(ApiGeneration),
        nameof(ApiRootPath),
        nameof(ApiFilePath)
    };

    public override bool CanClassUseEnums(Class classe, IEnumerable<Class>? availableClasses, IFieldProperty? prop = null)
    {
        return EnumsForStaticReferences && base.CanClassUseEnums(classe, availableClasses, prop);
    }

    public string GetApiPath(ModelFile file, string tag, bool withControllers = false)
    {
        return Path.Combine(
            OutputDirectory,
            ResolveVariables(ApiRootPath, tag: tag).ToFilePath(),
            withControllers ? "Controllers" : string.Empty,
            ResolveVariables(ApiFilePath, tag: tag, module: file.Namespace.ModulePath));
    }

    public string GetClassFileName(Class classe, string tag)
    {
        return Path.Combine(
            OutputDirectory,
            GetModelPath(classe, tag),
            "generated",
            (classe.Abstract ? "I" : string.Empty) + classe.NamePascal + ".cs");
    }

    public string GetDbContextFilePath(string tag)
    {
        return Path.Combine(
            OutputDirectory,
            ResolveVariables(DbContextPath!, tag: tag).ToFilePath(),
            "generated",
            $"{GetDbContextName(tag)}.cs");
    }

    /// <summary>
    /// Récupère le nom du DbContext.
    /// </summary>
    /// <param name="tag">tag</param>
    /// <returns>Nom.</returns>
    public string GetDbContextName(string tag)
    {
        return ResolveVariables(DbContextName, tag: tag).Replace(".", string.Empty);
    }

    public string GetDbContextNamespace(string tag)
    {
        return ResolveVariables(DbContextPath!, tag: tag)
            .ToNamespace();
    }

    public string GetMapperFilePath((Class Classe, FromMapper Mapper) mapper, string tag)
    {
        var (ns, modelPath) = GetMapperLocation(mapper, tag);
        return Path.Combine(
            OutputDirectory,
            ResolveVariables(modelPath, tag: tag, module: ns.ModulePath).ToFilePath(),
            "generated",
            $"{GetMapperName(ns, modelPath)}.cs");
    }

    public string GetMapperFilePath((Class Classe, ClassMappings Mapper) mapper, string tag)
    {
        var (ns, modelPath) = GetMapperLocation(mapper, tag);
        return Path.Combine(
            OutputDirectory,
            ResolveVariables(modelPath, tag: tag, module: ns.ModulePath).ToFilePath(),
            "generated",
            $"{GetMapperName(ns, modelPath)}.cs");
    }

    public (Namespace Namespace, string ModelPath) GetMapperLocation((Class Classe, FromMapper Mapper) mapper, string tag)
    {
        var pmp = NoPersistence(tag) ? NonPersistentModelPath : PersistentModelPath;

        if (mapper.Classe.IsPersistent)
        {
            return (mapper.Classe.Namespace, pmp);
        }

        var persistentParam = mapper.Mapper.Params.FirstOrDefault(p => p.Class.IsPersistent);
        if (persistentParam != null)
        {
            return (persistentParam.Class.Namespace, pmp);
        }

        return (mapper.Classe.Namespace, NonPersistentModelPath);
    }

    public (Namespace Namespace, string ModelPath) GetMapperLocation((Class Classe, ClassMappings Mapper) mapper, string tag)
    {
        var pmp = NoPersistence(tag) ? NonPersistentModelPath : PersistentModelPath;

        if (mapper.Classe.IsPersistent)
        {
            return (mapper.Classe.Namespace, pmp);
        }

        if (mapper.Mapper.Class.IsPersistent)
        {
            return (mapper.Mapper.Class.Namespace, pmp);
        }

        return (mapper.Classe.Namespace, NonPersistentModelPath);
    }

    public string GetMapperName(Namespace ns, string modelPath)
    {
        return $"{ns.ModuleFlat}{(modelPath == PersistentModelPath ? string.Empty : "DTO")}Mappers";
    }

    /// <summary>
    /// Récupère le chemin vers un fichier de classe à générer.
    /// </summary>
    /// <param name="classe">La classe.</param>
    /// <param name="tag">Tag.</param>
    /// <returns>Chemin.</returns>
    public string GetModelPath(Class classe, string tag)
    {
        return ResolveVariables(
            classe.IsPersistent && !NoPersistence(tag)
                ? classe.Reference
                    ? PersistentReferencesModelPath
                    : PersistentModelPath
                : NonPersistentModelPath,
            tag: tag,
            module: classe.Namespace.ModulePath).ToFilePath();
    }

    /// <summary>
    /// Récupère le namespace d'un endpoint.
    /// </summary>
    /// <param name="endpoint">L'endpoint.</param>
    /// <param name="tag">Tag.</param>
    /// <returns>Namespace.</returns>
    public string GetNamespace(Endpoint endpoint, string tag)
    {
        return GetNamespace(endpoint.Namespace, Path.Combine(ApiRootPath, ApiFilePath), tag);
    }

    /// <summary>
    /// Récupère le namespace d'une classe.
    /// </summary>
    /// <param name="classe">La classe.</param>
    /// <param name="tag">Tag.</param>
    /// <param name="isPersistent">Surcharge le caractère Persistent</param>
    /// <returns>Namespace.</returns>
    public string GetNamespace(Class classe, string tag, bool? isPersistent = null)
    {
        return GetNamespace(
            classe.Namespace,
            isPersistent.HasValue ? isPersistent.Value ? PersistentModelPath : NonPersistentModelPath : classe.IsPersistent && !NoPersistence(tag) ? classe.Reference ? PersistentReferencesModelPath : PersistentModelPath : NonPersistentModelPath,
            tag);
    }

    public string GetNamespace(Namespace ns, string modelPath, string tag)
    {
        return ResolveVariables(modelPath, tag: tag, module: ns.Module)
            .ToNamespace()
            .Replace(".Dto", string.Empty);
    }

    public string GetReferenceAccessorName(Namespace ns, string tag)
    {
        return ResolveVariables(
            ReferenceAccessorsName,
            tag: tag,
            module: ns.ModuleFlat);
    }

    public string GetReferenceImplementationFilePath(Namespace ns, string tag)
    {
        return Path.Combine(
            OutputDirectory,
            ResolveVariables(
                ReferenceAccessorsImplementationPath,
                tag: tag,
                module: ns.ModulePath).ToFilePath(),
            "generated",
            $"{GetReferenceAccessorName(ns, tag)}.cs");
    }

    public string GetReferenceImplementationNamespace(Namespace ns, string tag)
    {
        return ResolveVariables(
            ReferenceAccessorsImplementationPath,
            tag: tag,
            module: ns.Module).ToNamespace();
    }

    public string GetReferenceInterfaceFilePath(Namespace ns, string tag)
    {
        return Path.Combine(
            OutputDirectory,
            ResolveVariables(
                ReferenceAccessorsInterfacePath,
                tag: tag,
                module: ns.ModulePath).ToFilePath(),
            "generated",
            $"I{GetReferenceAccessorName(ns, tag)}.cs");
    }

    public string GetReferenceInterfaceNamespace(Namespace ns, string tag)
    {
        return ResolveVariables(
            ReferenceAccessorsInterfacePath,
            tag: tag,
            module: ns.Module).ToNamespace();
    }

    public string GetReturnTypeName(IProperty? prop)
    {
        if (prop == null)
        {
            return NoAsyncControllers ? "void" : "async Task";
        }

        var typeName = GetType(prop, nonNullable: true);
        return typeName.StartsWith("IAsyncEnumerable") || NoAsyncControllers
            ? typeName
            : $"async Task<{typeName}>";
    }

    public string GetType(IProperty prop, IEnumerable<Class>? availableClasses = null, bool useClassForAssociation = false, bool nonNullable = false)
    {
        var type = base.GetType(prop, availableClasses, useClassForAssociation);

        if (!nonNullable && prop is IFieldProperty f && GetEnumType(f, f is RegularProperty) == type)
        {
            type += "?";
        }
        else if (nonNullable && type.EndsWith("?"))
        {
            type = type[0..^1];
        }

        return type;
    }

    public override bool IsPersistent(Class classe, string tag)
    {
        return base.IsPersistent(classe, tag) && !NoPersistence(tag);
    }

    public bool NoPersistence(string tag)
    {
        return ResolveVariables(NoPersistenceParam ?? string.Empty, tag) == true.ToString();
    }

    public bool ShouldQuoteValue(IFieldProperty prop)
    {
        return GetType(prop) == "string";
    }

    protected override string GetEnumType(string className, string propName, bool isPrimaryKeyDef = false)
    {
        return $"{(isPrimaryKeyDef ? string.Empty : $"{className.ToPascalCase()}.")}{propName.ToPascalCase()}s";
    }
}