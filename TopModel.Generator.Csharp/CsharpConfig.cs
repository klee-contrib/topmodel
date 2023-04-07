using System.Text.RegularExpressions;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Generator.Core;
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

    /// <summary>
    /// Détermine si une classe peut utiliser une enum pour sa clé primaire.
    /// </summary>
    /// <param name="classe">Classe.</param>
    /// <param name="prop">Propriété à vérifier (si c'est pas la clé primaire).</param>
    /// <returns>Oui/non.</returns>
    public bool CanClassUseEnums(Class classe, IFieldProperty? prop = null)
    {
        prop ??= classe.EnumKey;

        bool CheckProperty(IFieldProperty fp)
        {
            return (fp == classe.EnumKey || classe.UniqueKeys.Where(uk => uk.Count == 1).Select(uk => uk.Single()).Contains(prop))
                && classe.Values.All(r => r.Value.ContainsKey(fp) && !Regex.IsMatch(r.Value[fp].ToString() ?? string.Empty, "^\\d"));
        }

        return EnumsForStaticReferences && classe.Enum && CheckProperty(prop!);
    }

    /// <summary>
    /// Récupère la valeur par défaut d'une propriété en C#.
    /// </summary>
    /// <param name="property">La propriété.</param>
    /// <param name="availableClasses">Classes disponibles dans le générateur.</param>
    /// <returns>La valeur par défaut.</returns>
    public string GetDefaultValue(IProperty property, IEnumerable<Class> availableClasses)
    {
        var fp = property as IFieldProperty;

        if (fp?.DefaultValue == null || fp.DefaultValue == "null" || fp.DefaultValue == "undefined")
        {
            return "null";
        }

        var prop = fp is AliasProperty alp ? alp.Property : fp;
        var ap = prop as AssociationProperty;

        var classe = ap != null ? ap.Association : prop.Class;
        var targetProp = ap != null ? ap.Property : prop;

        if (classe.Enum && availableClasses.Contains(classe))
        {
            if (CanClassUseEnums(classe, targetProp))
            {
                return $"{classe}.{targetProp}s.{fp.DefaultValue}";
            }
            else
            {
                var refName = classe.Values.SingleOrDefault(rv => rv.Value[targetProp] == fp.DefaultValue)?.Name;
                if (refName != null)
                {
                    return $"{classe}.{refName}";
                }
            }
        }

        if (fp.Domain?.CSharp?.Type == "string")
        {
            return $@"""{fp.DefaultValue}""";
        }

        return fp.DefaultValue;
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

    public string GetApiPath(ModelFile file, string tag, bool withControllers = false)
    {
        return Path.Combine(
            OutputDirectory,
            ResolveVariables(ApiRootPath, tag: tag).ToFilePath(),
            withControllers ? "Controllers" : string.Empty,
            ResolveVariables(ApiFilePath, tag: tag, module: file.Namespace.ModulePath));
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
        return ResolveVariables(
            isPersistent.HasValue
                ? isPersistent.Value
                    ? PersistentModelPath
                    : NonPersistentModelPath
                : classe.IsPersistent && !NoPersistence(tag)
                    ? classe.Reference
                        ? PersistentReferencesModelPath
                        : PersistentModelPath
                    : NonPersistentModelPath,
            tag: tag,
            module: classe.Namespace.Module)
        .ToNamespace()
        .Replace(".Dto", string.Empty);
    }

    /// <summary>
    /// Récupère le namespace d'un endpoint.
    /// </summary>
    /// <param name="endpoint">L'endpoint.</param>
    /// <param name="tag">Tag.</param>
    /// <returns>Namespace.</returns>
    public string GetNamespace(Endpoint endpoint, string tag)
    {
        return ResolveVariables(
             Path.Combine(ApiRootPath, ApiFilePath),
             tag: tag,
             module: endpoint.Namespace.Module)
        .ToNamespace();
    }

    public bool NoPersistence(string tag)
    {
        return ResolveVariables(NoPersistenceParam ?? string.Empty, tag) == true.ToString();
    }
}