using System.Text.RegularExpressions;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Core.Model.Implementation;
using TopModel.Core.Utils;
using TopModel.Generator.Core;
using TopModel.Utils;
using YamlDotNet.Serialization;

namespace TopModel.Generator.Csharp;

/// <summary>
/// Paramètres pour la génération du C#.
/// </summary>
public class CsharpConfig : GeneratorConfigBase
{
    private readonly string[] _builtInNonNullableTypes = ["bool", "short", "ushort", "int", "uint", "long", "ulong", "double", "float", "decimal", "Guid", "DateTime", "DateOnly", "TimeOnly", "TimeSpan"];

    /// <summary>
    /// Localisation du modèle persisté, relative au répertoire de génération. Par défaut : {app}.{module}.Models.
    /// </summary>
    public string PersistentModelPath { get; set; } = "{app}.{module}.Models";

    /// <summary>
    /// Localisation des classes de références, relative au répertoire de génération.
    /// Si non renseigné, ces classes seront générées comme les autres (selon si elles sont persistantes ou non).
    /// </summary>
    public string? ReferencesModelPath { get; set; }

    /// <summary>
    /// Localisation du modèle non persisté, relative au répertoire de génération. Par défaut : {app}.{module}.Models/Dto.
    /// </summary>
    public string NonPersistentModelPath { get; set; } = "{app}.{module}.Models/Dto";

    /// <summary>
    /// Localisation du l'API générée (client ou serveur), relative au répertoire de génération. Par défaut : "{app}.Web".
    /// </summary>
    public string ApiRootPath { get; set; } = "{app}.Web";

    /// <summary>
    /// Chemin vers lequel sont créés les fichiers d'endpoints générés, relative à la racine de l'API. Par défaut : "{module:path}".
    /// </summary>
    public string ApiFilePath { get; set; } = "{module:path}";

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

    /// <summary>
    /// Location des flux de données générés.
    /// </summary>
    public string? DataFlowsPath { get; set; }

    /// <summary>
    /// Chemin vers lequel générer les interfaces d'accesseurs de référence.
    /// </summary>
    public string? ReferenceAccessorsInterfacePath { get; set; }

#nullable disable

    /// <summary>
    /// Chemin vers lequel générer les implémentation d'accesseurs de référence. Par défaut : {DbContextPath}/Reference.
    /// </summary>
    public string ReferenceAccessorsImplementationPath { get; set; }
#nullable enable

    /// <summary>
    /// Nom des accesseurs de référence (préfixé par 'I' pour l'interface, puis 'Db' pour les accesseurs persistés). Par défaut : {module}ReferenceAccessors.
    /// (La variable `{module}` aura toujours la transformation `flat` ajoutée).
    /// </summary>
    public string ReferenceAccessorsName { get; set; } = "{module}ReferenceAccessors";

    /// <summary>
    /// Nom des mappers. Par défaut : {module}Mappers.
    /// (La variable `{module}` aura toujours la transformation `flat` ajoutée).
    /// </summary>
    public string MappersName { get; set; } = "{module}Mappers";

    /// <summary>
    /// Utilise les migrations EF pour créer/mettre à jour la base de données. Par défaut : 'true'.
    /// </summary>
    public bool UseEFMigrations { get; set; } = true;

    /// <summary>
    /// Utilise des noms de tables et de colonnes en lowercase. Par défaut : 'true'.
    /// </summary>
    public bool UseLowerCaseSqlNames { get; set; } = true;

    /// <summary>
    /// Le nom du schéma de base de données à cibler (si non renseigné, EF utilise 'dbo'/"public').
    /// </summary>
    public string? DbSchema { get; set; }

    /// <summary>
    /// Si on génère avec Kinetix.
    /// </summary>
    public bool Kinetix { get; set; } = true;

    /// <summary>
    /// Namespace de l'enum de domaine pour Kinetix. Par défaut : '{app}.Common'.
    /// </summary>
    public string DomainNamespace { get; set; } = "{app}.Common";

    /// <summary>
    /// Types C# que le générateur doit considérer comme étant types valeurs (en plus des plus standard comme 'int', 'bool' ou 'DateTime'),
    /// qu'il faudra wrapper dans un `Nullable` (avec un `?`) pour les rendre nullables.
    /// </summary>
    public string[] ValueTypes { get; set; } = [];

    public string[] AllValueTypes => _builtInNonNullableTypes.Concat(ValueTypes).Distinct().ToArray();

    /// <summary>
    /// Prend en compte l'activation du paramètre `nullable: enable` dans le code généré.
    /// </summary>
    public bool NullableEnable { get; set; }

    /// <summary>
    /// Génère des types non-nullables pour les propriétés obligatoires.
    /// </summary>
    [YamlMember(Alias = "requiredNonNullable")]
    public string? RequiredNonNullableParam { get; set; }

    /// <summary>
    /// Ne génère pas les attributs de colonnes sur les alias dans les classes non persistées. Par défaut : 'true'.
    /// </summary>
    public bool NoColumnOnAlias { get; set; } = true;

    /// <summary>
    /// Considère tous les classes comme étant non-Persistentes (= pas d'attribut SQL).
    /// </summary>
    [YamlMember(Alias = "noPersistence")]
    public string? NoPersistenceParam { get; set; }

    /// <summary>
    /// Si un mapper contient au moins une classe de ces tags, alors il sera généré avec les tags de cette classe (au lieu du comportement par défaut qui priorise les tags de la classe persistée puis de celle qui définit le mapper).
    /// </summary>
    public string[] MapperTagsOverrides { get; set; } = [];

    /// <summary>
    /// Détermine le type de classe prioritaire pour déterminer la localisation des mappers générés (`persisted` ou `non-persisted`). Par défaut : "persistent".
    /// </summary>
    public AnnotationConstraint MapperLocationPriority { get; set; } = AnnotationConstraint.Persisted;

    /// <summary>
    /// Utilise des enums au lieu de strings pour les PKs de listes de référence statiques. Par défaut : 'true'.
    /// </summary>
    public bool EnumsForStaticReferences { get; set; } = true;

    /// <summary>
    /// Annote les tables et les colonnes générées par EF avec les commentaires du modèle (nécessite `UseEFMigrations`). Par défaut : 'true'.
    /// </summary>
    public bool UseEFComments { get; set; }

    /// <summary>
    /// Utilise des records (mutables) au lieu de classes pour la génération de classes.
    /// </summary>
    public bool UseRecords { get; set; } = true;

    /// <summary>
    /// Utilise les constructeurs principaux pour la génération des classes avec dépendances (clients d'API, accesseurs de références). Par défaut : 'true'.
    /// </summary>
    public bool UsePrimaryConstructors { get; set; } = true;

    /// <summary>
    /// Ajoute un CancellationToken en paramètre des endpoints générés (client et serveur).
    /// </summary>
    public bool UseCancellationTokens { get; set; }

    public override string[] PropertiesWithModuleVariableSupport =>
    [
        nameof(PersistentModelPath),
        nameof(ReferencesModelPath),
        nameof(NonPersistentModelPath),
        nameof(ApiFilePath),
        nameof(DbSchema),
        nameof(ReferenceAccessorsName),
        nameof(ReferenceAccessorsInterfacePath),
        nameof(ReferenceAccessorsImplementationPath),
        nameof(MappersName),
        nameof(DataFlowsPath)
    ];

    public override string[] PropertiesWithTagVariableSupport =>
    [
        nameof(PersistentModelPath),
        nameof(ReferencesModelPath),
        nameof(NonPersistentModelPath),
        nameof(NoPersistenceParam),
        nameof(RequiredNonNullableParam),
        nameof(DbContextPath),
        nameof(DbContextName),
        nameof(DbSchema),
        nameof(ReferenceAccessorsName),
        nameof(ReferenceAccessorsInterfacePath),
        nameof(ReferenceAccessorsImplementationPath),
        nameof(ApiGeneration),
        nameof(ApiRootPath),
        nameof(ApiFilePath),
        nameof(DataFlowsPath)
    ];

    public override bool CanClassUseEnums(Class classe, IEnumerable<Class>? availableClasses, IProperty? prop = null)
    {
        return EnumsForStaticReferences && base.CanClassUseEnums(classe, availableClasses, prop);
    }

    public virtual string GetApiPath(ModelFile file, string tag, bool withControllers = false)
    {
        return Path.Combine(
            OutputDirectory,
            ResolveVariables(ApiRootPath, tag: tag).ToFilePath(),
            withControllers ? "Controllers" : string.Empty,
            ResolveVariables(ApiFilePath, tag: tag, module: file.Namespace.Module));
    }

    public virtual string GetClassFileName(Class classe, string tag)
    {
        return Path.Combine(
            OutputDirectory,
            GetModelPath(classe, tag),
            "generated",
            (classe.Abstract ? "I" : string.Empty) + classe.NamePascal + ".cs");
    }

    public virtual string GetConvertedValue(string value, Domain? fromDomain, Domain? toDomain, bool nullableValueType)
    {
        if (nullableValueType && fromDomain != null && toDomain != null)
        {
            var converter = fromDomain.GetConverter(toDomain);
            if (converter != null)
            {
                var text = GetImplementation(converter)?.Text;
                if (text != null && text.Contains("{value}."))
                {
                    value += "?";
                }
            }
        }

        return GetConvertedValue(value, fromDomain, toDomain);
    }

    public virtual string GetDataFlowFilePath(DataFlow df, string tag)
    {
        return Path.Combine(
            OutputDirectory,
            ResolveVariables(DataFlowsPath!, tag: tag, module: df.ModelFile.Namespace.Module).ToFilePath(),
            "generated",
            $"{df.Name.ToPascalCase()}Flow.cs");
    }

    public virtual string GetDataFlowRegistrationFilePath(DataFlow df, string tag)
    {
        return Path.Combine(
            OutputDirectory,
            ResolveVariables(DataFlowsPath!, tag: tag, module: df.ModelFile.Namespace.Module).ToFilePath(),
            "generated",
            $"ServiceExtensions.cs");
    }

    public virtual string GetDbContextFilePath(string tag)
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
    public virtual string GetDbContextName(string tag)
    {
        return ResolveVariables(DbContextName, tag: tag).Replace(".", string.Empty);
    }

    public virtual string GetDbContextNamespace(string tag)
    {
        return ResolveVariables(DbContextPath!, tag: tag)
            .ToNamespace();
    }

    public virtual string GetEnumTypeNamespace(IProperty fp, string tag)
    {
        var op = fp switch
        {
            AssociationProperty a => a.Property,
            AliasProperty { Property: AssociationProperty a } => a.Property,
            AliasProperty alp => alp.Property,
            _ => fp
        };

        return op is AssociationProperty ap
            ? GetNamespace(ap.Association, tag)
            : op is RegularProperty rp
            ? GetNamespace(rp.Class, tag)
            : string.Empty;
    }

    public virtual string GetMapperFilePath((Class Class, FromMapper Mapper) mapper, string tag)
    {
        var (ns, modelPath) = GetMapperLocation(mapper, tag);
        return Path.Combine(
            OutputDirectory,
            ResolveVariables(modelPath, tag: tag, module: ns.Module).ToFilePath(),
            "generated",
            $"{GetMapperName(ns)}.cs");
    }

    public virtual string GetMapperFilePath((Class Class, ClassMappings Mapper) mapper, string tag)
    {
        var (ns, modelPath) = GetMapperLocation(mapper, tag);
        return Path.Combine(
            OutputDirectory,
            ResolveVariables(modelPath, tag: tag, module: ns.Module).ToFilePath(),
            "generated",
            $"{GetMapperName(ns)}.cs");
    }

    public virtual (Namespace Namespace, string ModelPath) GetMapperLocation((Class Class, FromMapper Mapper) mapper, string tag)
    {
        var pmp = NoPersistence(tag) ? NonPersistentModelPath : PersistentModelPath;
        if (MapperLocationPriority == AnnotationConstraint.Persisted)
        {
            if (mapper.Class.IsPersistent)
            {
                return (mapper.Class.Namespace, pmp);
            }

            var persistentParam = mapper.Mapper.ClassParams.FirstOrDefault(p => p.Class.IsPersistent && (!p.Class.Reference || ReferencesModelPath == null));
            if (persistentParam != null)
            {
                return (persistentParam.Class.Namespace, pmp);
            }

            return (mapper.Class.Namespace, NonPersistentModelPath);
        }
        else
        {
            if (!mapper.Class.IsPersistent)
            {
                return (mapper.Class.Namespace, NonPersistentModelPath);
            }

            var nonPersistentParam = mapper.Mapper.ClassParams.FirstOrDefault(p => !p.Class.IsPersistent);
            if (nonPersistentParam != null)
            {
                return (nonPersistentParam.Class.Namespace, NonPersistentModelPath);
            }

            return (mapper.Class.Namespace, pmp);
        }
    }

    public virtual (Namespace Namespace, string ModelPath) GetMapperLocation((Class Class, ClassMappings Mapper) mapper, string tag)
    {
        var pmp = NoPersistence(tag) ? NonPersistentModelPath : PersistentModelPath;
        if (MapperLocationPriority == AnnotationConstraint.Persisted)
        {
            if (mapper.Class.IsPersistent)
            {
                return (mapper.Class.Namespace, pmp);
            }

            if (mapper.Mapper.Class.IsPersistent)
            {
                return (mapper.Mapper.Class.Namespace, pmp);
            }

            return (mapper.Class.Namespace, NonPersistentModelPath);
        }
        else
        {
            if (!mapper.Class.IsPersistent)
            {
                return (mapper.Class.Namespace, NonPersistentModelPath);
            }

            if (!mapper.Mapper.Class.IsPersistent)
            {
                return (mapper.Mapper.Class.Namespace, NonPersistentModelPath);
            }

            return (mapper.Class.Namespace, pmp);
        }
    }

    public virtual string GetMapperName(Namespace ns)
    {
        return ResolveVariables(AddModuleFlat(MappersName), module: ns.Module);
    }

    /// <summary>
    /// Récupère le chemin vers un fichier de classe à générer.
    /// </summary>
    /// <param name="classe">La classe.</param>
    /// <param name="tag">Tag.</param>
    /// <returns>Chemin.</returns>
    public virtual string GetModelPath(Class classe, string tag)
    {
        return ResolveVariables(
            GetModelPathRaw(classe, tag),
            tag: tag,
            module: classe.Namespace.Module).ToFilePath();
    }

    /// <summary>
    /// Récupère le namespace d'un endpoint.
    /// </summary>
    /// <param name="endpoint">L'endpoint.</param>
    /// <param name="tag">Tag.</param>
    /// <returns>Namespace.</returns>
    public virtual string GetNamespace(Endpoint endpoint, string tag)
    {
        return GetNamespace(endpoint.Namespace, Path.Combine(ApiRootPath, ApiFilePath), tag);
    }

    /// <summary>
    /// Récupère le namespace d'un flux de données.
    /// </summary>
    /// <param name="dataFlow">Le flux de données.</param>
    /// <param name="tag">Tag.</param>
    /// <returns>Namespace.</returns>
    public virtual string GetNamespace(DataFlow dataFlow, string tag)
    {
        return GetNamespace(dataFlow.ModelFile.Namespace, DataFlowsPath!, tag);
    }

    /// <summary>
    /// Récupère le namespace d'une classe.
    /// </summary>
    /// <param name="classe">La classe.</param>
    /// <param name="tag">Tag.</param>
    /// <returns>Namespace.</returns>
    public virtual string GetNamespace(Class classe, string tag)
    {
        return GetNamespace(classe.Namespace, GetModelPathRaw(classe, tag), tag);
    }

    public virtual string GetNamespace(Namespace ns, string modelPath, string tag)
    {
        return ResolveVariables(modelPath, tag: tag, module: ns.Module)
            .ToNamespace()
            .Replace(".Dto", string.Empty);
    }

    public virtual string GetReferenceAccessorName(Namespace ns, string tag)
    {
        return ResolveVariables(
            AddModuleFlat(ReferenceAccessorsName),
            tag: tag,
            module: ns.Module);
    }

    public virtual string GetReferenceImplementationFilePath(Namespace ns, string tag)
    {
        return Path.Combine(
            OutputDirectory,
            ResolveVariables(
                ReferenceAccessorsImplementationPath,
                tag: tag,
                module: ns.Module).ToFilePath(),
            "generated",
            $"Db{GetReferenceAccessorName(ns, tag)}.cs");
    }

    public virtual string GetReferenceImplementationNamespace(Namespace ns, string tag)
    {
        return ResolveVariables(
            ReferenceAccessorsImplementationPath,
            tag: tag,
            module: ns.Module).ToNamespace();
    }

    public virtual string GetReferenceInterfaceFilePath(Namespace ns, string tag, string prefix = "")
    {
        return Path.Combine(
            OutputDirectory,
            ResolveVariables(
                ReferenceAccessorsInterfacePath!,
                tag: tag,
                module: ns.Module).ToFilePath(),
            "generated",
            $"I{prefix}{GetReferenceAccessorName(ns, tag)}.cs");
    }

    public virtual string GetReferenceInterfaceNamespace(Namespace ns, string tag)
    {
        return ResolveVariables(
            ReferenceAccessorsInterfacePath!,
            tag: tag,
            module: ns.Module).ToNamespace();
    }

    public virtual string GetReturnTypeName(IProperty? prop)
    {
        if (prop == null)
        {
            return NoAsyncControllers ? "void" : "async Task";
        }

        var typeName = GetType(prop, nonNullable: prop.Required);
        return typeName.StartsWith("IAsyncEnumerable") || NoAsyncControllers
            ? typeName
            : $"async Task<{typeName}>";
    }

    public virtual string GetType(IProperty prop, IEnumerable<Class>? availableClasses = null, bool useClassForAssociation = false, bool nonNullable = false)
    {
        var type = base.GetType(prop, availableClasses, useClassForAssociation);

        if (!nonNullable && (NullableEnable || AllValueTypes.Contains(type) || GetEnumType(prop, prop is RegularProperty) == type))
        {
            type += "?";
        }

        return type;
    }

    public override bool IsPersistent(Class classe, string tag)
    {
        return base.IsPersistent(classe, tag) && !NoPersistence(tag);
    }

    public virtual bool IsValueType(IProperty prop, IEnumerable<Class>? availableClasses)
    {
        return prop switch
        {
            AssociationProperty ap when CanClassUseEnums(ap.Association, availableClasses, ap.Property) => true,
            AliasProperty { Property: AssociationProperty ap } alp when CanClassUseEnums(ap.Association, availableClasses) && string.IsNullOrEmpty(GetImplementation(alp.Domain)?.GenericType) => true,
            RegularProperty { Class: not null } rp when CanClassUseEnums(rp.Class, availableClasses, rp) => true,
            AliasProperty { Property: RegularProperty { Class: not null } rp } alp when CanClassUseEnums(rp.Class, availableClasses, rp) && string.IsNullOrEmpty(GetImplementation(alp.Domain)?.GenericType) => true,
            CompositionProperty => false,
            _ => AllValueTypes.Contains(GetType(prop, availableClasses, nonNullable: true))
        };
    }

    public virtual bool NoPersistence(string tag)
    {
        return ResolveVariables(NoPersistenceParam ?? string.Empty, tag) == true.ToString();
    }

    public virtual bool RequiredNonNullable(string tag)
    {
        return ResolveVariables(RequiredNonNullableParam ?? string.Empty, tag) == true.ToString();
    }

    protected override string GetEnumType(string className, string propName, bool isPrimaryKeyDef = false)
    {
        return $"{(isPrimaryKeyDef ? string.Empty : $"{className.ToPascalCase()}.")}{propName.ToPascalCase()}{(!propName.EndsWith('s') ? "s" : string.Empty)}";
    }

    protected virtual string GetModelPathRaw(Class classe, string tag)
    {
        return classe.Reference && ReferencesModelPath != null
            ? ReferencesModelPath
            : classe.IsPersistent && !NoPersistence(tag)
                ? PersistentModelPath
                : NonPersistentModelPath;
    }

    protected override bool IsEnumNameValid(string name)
    {
        return base.IsEnumNameValid(name) && !name.Contains('-') && name.FirstOrDefault() != name.ToLower().FirstOrDefault();
    }

    private static string AddModuleFlat(string name)
    {
        return Regex.Replace(name, @"\{module(:[^}]*)?\}", "{module$1:flat}");
    }
}