using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Core.Model.Implementation;
using TopModel.Core.Utils;
using TopModel.Generator.Core;
using YamlDotNet.Serialization;

namespace TopModel.Generator.Csharp;

/// <summary>
/// Paramètres pour la génération du C#.
/// </summary>
public class CsharpConfig : GeneratorConfigBase
{
    private readonly string[] _builtInNonNullableTypes =
    [
        "bool",
        "short",
        "ushort",
        "int",
        "uint",
        "long",
        "ulong",
        "double",
        "float",
        "decimal",
        "Guid",
        "DateTime",
        "DateOnly",
        "TimeOnly",
        "TimeSpan",
    ];

    /// <summary>
    /// Localisation du modèle persisté, relative au répertoire de génération. Par défaut : {app}.{module}.Models.
    /// </summary>
    public virtual string PersistentModelPath { get; set; } = "{app}.{module}.Models";

    /// <summary>
    /// Localisation des classes de références, relative au répertoire de génération.
    /// Si non renseigné, ces classes seront générées comme les autres (selon si elles sont persistantes ou non).
    /// </summary>
    public virtual string? ReferencesModelPath { get; set; }

    /// <summary>
    /// Localisation du modèle non persisté, relative au répertoire de génération. Par défaut : {app}.{module}.Models/Dto.
    /// </summary>
    public virtual string NonPersistentModelPath { get; set; } = "{app}.{module}.Models/Dto";

    /// <summary>
    /// Localisation du l'API générée (client ou serveur), relative au répertoire de génération. Par défaut : "{app}.Web".
    /// </summary>
    public virtual string ApiRootPath { get; set; } = "{app}.Web";

    /// <summary>
    /// Chemin vers lequel sont créés les fichiers d'endpoints générés, relative à la racine de l'API. Par défaut : "{module:path}".
    /// </summary>
    public virtual string ApiFilePath { get; set; } = "{module:path}";

    /// <summary>
    /// Génère des contrôleurs d'API synchrones.
    /// </summary>
    public virtual bool NoAsyncControllers { get; set; }

    /// <summary>
    /// Localisation du DbContext, relative au répertoire de génération.
    /// </summary>
    public virtual string? DbContextPath { get; set; }

    /// <summary>
    /// Nom du DbContext. Par défaut : {app}DbContext.
    /// </summary>
    public virtual string DbContextName { get; set; } = "{app}DbContext";

    /// <summary>
    /// Location des flux de données générés.
    /// </summary>
    public virtual string? DataFlowsPath { get; set; }

    /// <summary>
    /// Chemin vers lequel générer les interfaces d'accesseurs de référence.
    /// </summary>
    public virtual string? ReferenceAccessorsInterfacePath { get; set; }

#nullable disable

    /// <summary>
    /// Chemin vers lequel générer les implémentation d'accesseurs de référence. Par défaut : {DbContextPath}/Reference.
    /// </summary>
    public virtual string ReferenceAccessorsImplementationPath { get; set; }

#nullable enable

    /// <summary>
    /// Nom des accesseurs de référence (préfixé par 'I' pour l'interface, puis 'Db' pour les accesseurs persistés). Par défaut : {module}ReferenceAccessors.
    /// (La variable `{module}` aura toujours la transformation `flat` ajoutée).
    /// </summary>
    public virtual string ReferenceAccessorsName { get; set; } = "{module}ReferenceAccessors";

    /// <summary>
    /// Nom des mappers. Par défaut : {module}Mappers.
    /// (La variable `{module}` aura toujours la transformation `flat` ajoutée).
    /// </summary>
    public virtual string MappersName { get; set; } = "{module}Mappers";

    /// <summary>
    /// Utilise les migrations EF pour créer/mettre à jour la base de données. Par défaut : 'true'.
    /// </summary>
    public virtual bool UseEFMigrations { get; set; } = true;

    /// <summary>
    /// Utilise des noms de tables et de colonnes en lowercase. Par défaut : 'true'.
    /// </summary>
    public virtual bool UseLowerCaseSqlNames { get; set; } = true;

    /// <summary>
    /// Le nom du schéma de base de données à cibler (si non renseigné, EF utilise 'dbo'/"public').
    /// </summary>
    public virtual string? DbSchema { get; set; }

    /// <summary>
    /// Si on génère avec Kinetix.
    /// </summary>
    public virtual bool Kinetix { get; set; } = true;

    /// <summary>
    /// Namespace de l'enum de domaine pour Kinetix. Par défaut : '{app}.Common'.
    /// </summary>
    public virtual string DomainNamespace { get; set; } = "{app}.Common";

    /// <summary>
    /// Types C# que le générateur doit considérer comme étant types valeurs (en plus des plus standard comme 'int', 'bool' ou 'DateTime'),
    /// qu'il faudra wrapper dans un `Nullable` (avec un `?`) pour les rendre nullables.
    /// </summary>
    public virtual string[] ValueTypes { get; set; } = [];

    public virtual string[] AllValueTypes => _builtInNonNullableTypes.Concat(ValueTypes).Distinct().ToArray();

    /// <summary>
    /// Prend en compte l'activation du paramètre `nullable: enable` dans le code généré.
    /// </summary>
    public virtual bool NullableEnable { get; set; }

    /// <summary>
    /// Génère des types non-nullables pour les propriétés obligatoires.
    /// </summary>
    [YamlMember(Alias = "requiredNonNullable")]
    public virtual string? RequiredNonNullableParam { get; set; }

    /// <summary>
    /// Ne génère pas les attributs de colonnes sur les alias dans les classes non persistées. Par défaut : 'true'.
    /// </summary>
    public virtual bool NoColumnOnAlias { get; set; } = true;

    /// <summary>
    /// Considère tous les classes comme étant non-Persistentes (= pas d'attribut SQL).
    /// </summary>
    [YamlMember(Alias = "noPersistence")]
    public virtual string? NoPersistenceParam { get; set; }

    /// <summary>
    /// Si un mapper contient au moins une classe de ces tags, alors il sera généré avec les tags de cette classe (au lieu du comportement par défaut qui priorise les tags de la classe persistée puis de celle qui définit le mapper).
    /// </summary>
    public virtual string[] MapperTagsOverrides { get; set; } = [];

    /// <summary>
    /// Détermine le type de classe prioritaire pour déterminer la localisation des mappers générés (`persisted` ou `non-persisted`). Par défaut : "persistent".
    /// </summary>
    public virtual AnnotationConstraint MapperLocationPriority { get; set; } = AnnotationConstraint.Persisted;

    /// <summary>
    /// Annote les tables et les colonnes générées par EF avec les commentaires du modèle (nécessite `UseEFMigrations`). Par défaut : 'true'.
    /// </summary>
    public virtual bool UseEFComments { get; set; }

    /// <summary>
    /// Permet de spécifier la version de .NET cible, afin de savoir si certaines fonctionnalités sont disponibles pour la génération. Par défaut : 10.
    /// </summary>
    public virtual int DotnetVersion { get; set; } = 10;

    /// <summary>
    /// Utilise des records (mutables) au lieu de classes pour la génération de classes.
    /// </summary>
    public virtual bool UseRecords { get; set; } = true;

    /// <summary>
    /// Ajoute un CancellationToken en paramètre des endpoints générés (client et serveur).
    /// </summary>
    public virtual bool UseCancellationTokens { get; set; }

    /// <summary>
    /// Génère des accesseurs de référence asynchrones (pour Kinetix 10+).
    /// </summary>
    public virtual bool UseAsyncReferenceAccessors { get; set; }

    /// <summary>
    /// Chemin vers les fichiers de resources (*.resx) à générer pour les traductions de libellés de propriétés et de listes de références.
    /// </summary>
    public string? ResourcesResxPath { get; set; }

    /// <summary>
    /// Précise les resources à générer dans les fichiers resx (si `ResourcesResxPath` est renseigné), au lieu de les générer en base de données.
    /// </summary>
    public ResxResource ResourcesInResx { get; set; } = ResxResource.All;

    /// <summary>
    /// Génère les enums de nom de colonnes dans les classes persistées. Automatiquement activé si `DbContextPath` est null et si `Kinetix` est `true`.
    /// </summary>
    public virtual bool EnumCols { get; set; }

    public bool PersistedReferencesResources =>
        TranslateReferences == true
        && (string.IsNullOrWhiteSpace(ResourcesResxPath) || ResourcesInResx == ResxResource.Properties);

    public bool PersistedPropertiesResources =>
        TranslateProperties == true
        && (string.IsNullOrWhiteSpace(ResourcesResxPath) || ResourcesInResx == ResxResource.References);

    public override string? DefaultLanguage => "csharp";

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
            nameof(DataFlowsPath),
            nameof(ResourcesResxPath),
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
            nameof(DataFlowsPath),
            nameof(ResourcesResxPath),
        ];

    public override string[] PropertiesWithLangVariableSupport => [nameof(ResourcesResxPath)];

    protected virtual IDictionary<string, string> CollectionTypes { get; } =
        new Dictionary<string, string>()
        {
            ["IEnumerable"] = "List",
            ["ICollection"] = "List",
            ["IList"] = "List",
            ["List"] = "List",
            ["HashSet"] = "HashSet",
        };

    public virtual string GetApiPath(ModelFile file, string tag, bool withControllers = false)
    {
        return Path.Combine(
            OutputDirectory,
            ResolveVariables(ApiRootPath, tag: tag).ToFilePath(),
            withControllers ? "Controllers" : string.Empty,
            ResolveVariables(ApiFilePath, tag: tag, module: file.Namespace.Module)
        );
    }

    public virtual string GetClassFileName(Class classe, string tag)
    {
        return Path.Combine(OutputDirectory, GetModelPath(classe, tag), "generated", GetTypeName(classe) + ".cs");
    }

    public virtual string GetCollector(Domain domain)
    {
        var impl = GetImplementation(domain)!;
        return impl.Collector
            ?? $"To{(impl.GenericType?.Value[0..impl.GenericType.Value.IndexOf('<')] ?? impl.Type).TrimStart("I")}()";
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
            $"{df.Name.ToPascalCase()}Flow.cs"
        );
    }

    public virtual string GetDataFlowRegistrationFilePath(DataFlow df, string tag)
    {
        return Path.Combine(
            OutputDirectory,
            ResolveVariables(DataFlowsPath!, tag: tag, module: df.ModelFile.Namespace.Module).ToFilePath(),
            "generated",
            $"ServiceExtensions.cs"
        );
    }

    public virtual string GetDbContextFilePath(string tag)
    {
        return Path.Combine(
            OutputDirectory,
            ResolveVariables(DbContextPath!, tag: tag).ToFilePath(),
            "generated",
            $"{GetDbContextName(tag)}.cs"
        );
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
        return ResolveVariables(DbContextPath!, tag: tag).ToNamespace();
    }

    public virtual string GetDefaultValue(IProperty property, string tag)
    {
        var defaultValue = GetValue(property);
        var type = GetType(property, nonNullable: true);

        if (
            property.EnumProperty != null
            && GetProperties(property.Class).Any(p => p.NamePascal == defaultValue.Split(".")[0])
        )
        {
            defaultValue =
                $"{GetNamespace(
                    property.EnumProperty!.Class,
                    GetBestClassTag(property.EnumProperty!.Class, tag), GetNamespace(property.Class, tag)
                )}.{defaultValue}";
        }

        if (
            property.Composition != null && property.Required
            || property.AssociationMultiple && property.UseClassForAssociation
        )
        {
            var genericType = type.Split('<')[0];

            if (type == GetTypeName(property.Composition))
            {
                if (!RequiredNonNullable(tag))
                {
                    defaultValue = $"new()";
                }
            }
            else if (CollectionTypes.TryGetValue(genericType, out var collectionType))
            {
                defaultValue = DotnetVersion >= 8 ? "[]" : $"new {type.Replace(genericType, collectionType)}()";
            }
        }

        return defaultValue;
    }

    public virtual string GetEnumTypeNamespace(IProperty fp, string tag)
    {
        return fp is { EnumProperty: IProperty ep } ? GetNamespace(ep.Class, tag) : string.Empty;
    }

    public virtual string GetMapperFilePath((Class Class, FromMapper Mapper) mapper, string tag)
    {
        var (ns, modelPath) = GetMapperLocation(mapper, tag);
        return Path.Combine(
            OutputDirectory,
            ResolveVariables(modelPath, tag: tag, module: ns.Module).ToFilePath(),
            "generated",
            $"{GetMapperName(ns)}.cs"
        );
    }

    public virtual string GetMapperFilePath((Class Class, ClassMappings Mapper) mapper, string tag)
    {
        var (ns, modelPath) = GetMapperLocation(mapper, tag);
        return Path.Combine(
            OutputDirectory,
            ResolveVariables(modelPath, tag: tag, module: ns.Module).ToFilePath(),
            "generated",
            $"{GetMapperName(ns)}.cs"
        );
    }

    public virtual (string Name, string Namespace, string Module) GetMapperInfo(
        (Class Class, FromMapper Mapper) mapper,
        string tag
    )
    {
        var (ns, modelPath) = GetMapperLocation(mapper, tag);
        var nsText = GetNamespace(ns, modelPath, tag);
        var name = GetMapperName(ns);
        return (name, nsText, ns.Module);
    }

    public virtual (string Name, string Namespace, string Module) GetMapperInfo(
        (Class Class, ClassMappings Mapper) mapper,
        string tag
    )
    {
        var (ns, modelPath) = GetMapperLocation(mapper, tag);
        var nsText = GetNamespace(ns, modelPath, tag);
        var name = GetMapperName(ns);
        return (name, nsText, ns.Module);
    }

    /// <summary>
    /// Récupère le chemin vers un fichier de classe à générer.
    /// </summary>
    /// <param name="classe">La classe.</param>
    /// <param name="tag">Tag.</param>
    /// <returns>Chemin.</returns>
    public virtual string GetModelPath(Class classe, string tag)
    {
        return ResolveVariables(GetModelPathRaw(classe, tag), tag: tag, module: classe.Namespace.Module).ToFilePath();
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
    /// <param name="containingNs">Si on a besoin du namespace de la classe dans un autre namespace, pour chercher à le simplifier.</param>
    /// <returns>Namespace.</returns>
    public virtual string GetNamespace(Class classe, string tag, string? containingNs = null)
    {
        var ns = GetNamespace(classe.Namespace, GetModelPathRaw(classe, tag), tag);

        if (containingNs == null)
        {
            return ns;
        }

        var containingNsSplit = containingNs.Split('.');
        var nsStack = new Stack<string>(ns.Split('.'));

        var classes = AvailableClasses.Select(GetTypeName).ToHashSet();

        var finalNs = string.Empty;
        while (nsStack.TryPop(out var item))
        {
            finalNs = $"{item}.{finalNs}";

            if (
                !nsStack.Reverse().SkipWhile((spl, i) => spl == containingNsSplit.ElementAtOrDefault(i)).Any()
                && !classes.Contains(item)
            )
            {
                break;
            }
        }

        return finalNs.TrimEnd('.');
    }

    public virtual string GetNamespace(Namespace ns, string modelPath, string tag)
    {
        return ResolveVariables(modelPath, tag: tag, module: ns.Module).ToNamespace().Replace(".Dto", string.Empty);
    }

    public virtual string GetReferenceAccessorName(Namespace ns, string tag)
    {
        return ResolveVariables(AddModuleFlat(ReferenceAccessorsName), tag: tag, module: ns.Module);
    }

    public virtual string GetReferenceImplementationFilePath(Namespace ns, string tag)
    {
        return Path.Combine(
            OutputDirectory,
            ResolveVariables(ReferenceAccessorsImplementationPath, tag: tag, module: ns.Module).ToFilePath(),
            "generated",
            $"Db{GetReferenceAccessorName(ns, tag)}.cs"
        );
    }

    public virtual string GetReferenceImplementationNamespace(Namespace ns, string tag)
    {
        return ResolveVariables(ReferenceAccessorsImplementationPath, tag: tag, module: ns.Module).ToNamespace();
    }

    public virtual string GetReferenceInterfaceFilePath(Namespace ns, string tag, string prefix = "")
    {
        return Path.Combine(
            OutputDirectory,
            ResolveVariables(ReferenceAccessorsInterfacePath!, tag: tag, module: ns.Module).ToFilePath(),
            "generated",
            $"I{prefix}{GetReferenceAccessorName(ns, tag)}.cs"
        );
    }

    public virtual string GetReferenceInterfaceNamespace(Namespace ns, string tag)
    {
        return ResolveVariables(ReferenceAccessorsInterfacePath!, tag: tag, module: ns.Module).ToNamespace();
    }

    public virtual string GetResourcesResxPath(Namespace ns, string tag, string lang)
    {
        return Path.Combine(
            OutputDirectory,
            ResolveVariables(ResourcesResxPath!, module: ns.Module, tag: tag, lang: lang).ToFilePath(),
            $"{GetRootModule(ns)}.{lang}.resx".Replace("..", ".")
        );
    }

    public virtual string GetReturnTypeName(IProperty? prop)
    {
        if (prop == null)
        {
            return NoAsyncControllers ? "void" : "async Task";
        }

        var typeName = GetType(prop, nonNullable: prop.Required);
        return typeName.StartsWith("IAsyncEnumerable") || NoAsyncControllers ? typeName : $"async Task<{typeName}>";
    }

    public virtual string GetType(
        IProperty prop,
        bool forceAssociationPropertyType = false,
        int skipChain = 0,
        bool nonNullable = false
    )
    {
        var type = base.GetType(prop, forceAssociationPropertyType, skipChain);

        if (
            !nonNullable
            && (
                NullableEnable
                || AllValueTypes.Contains(type)
                || GetEnumType(prop, prop.EnumProperty?.Class == prop.Class) == type
            )
        )
        {
            type += "?";
        }

        return type;
    }

    [return: NotNullIfNotNull(nameof(classe))]
    public override string? GetTypeName(Class? classe)
    {
        if (classe == null)
        {
            return null;
        }

        return $"{(classe.Type == ClassType.Interface ? "I" : string.Empty)}{classe.NamePascal}";
    }

    public override bool IsPersistent(Class classe, string tag)
    {
        return base.IsPersistent(classe, tag) && !NoPersistence(tag);
    }

    public virtual bool IsValueType(IProperty prop)
    {
        return prop switch
        {
            { EnumProperty: not null } when string.IsNullOrEmpty(GetImplementation(prop.Domain)?.GenericType) => true,
            { Composition: not null } => false,
            _ => AllValueTypes.Contains(GetType(prop!, nonNullable: true)),
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

    protected override string GetEnumInEnumClassType(string className, string propName, bool internalReference = false)
    {
        return $"{(internalReference ? string.Empty : $"{className}.")}{propName}{(!propName.EndsWith('s') ? "s" : "es")}";
    }

    protected virtual (Namespace Namespace, string ModelPath) GetMapperLocation(
        (Class Class, FromMapper Mapper) mapper,
        string tag
    )
    {
        var pmp = NoPersistence(tag) ? NonPersistentModelPath : PersistentModelPath;
        if (MapperLocationPriority == AnnotationConstraint.Persisted)
        {
            if (mapper.Class.IsPersistent)
            {
                return (mapper.Class.Namespace, pmp);
            }

            var persistentParam = mapper.Mapper.ClassParams.FirstOrDefault(p =>
                p.Class.IsPersistent && (!p.Class.Reference || ReferencesModelPath == null)
            );
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

    protected virtual (Namespace Namespace, string ModelPath) GetMapperLocation(
        (Class Class, ClassMappings Mapper) mapper,
        string tag
    )
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

    protected virtual string GetMapperName(Namespace ns)
    {
        return ResolveVariables(AddModuleFlat(MappersName), module: ns.Module);
    }

    protected virtual string GetModelPathRaw(Class classe, string tag)
    {
        return classe.Reference && ReferencesModelPath != null ? ReferencesModelPath
            : classe.IsPersistent && !NoPersistence(tag) ? PersistentModelPath
            : NonPersistentModelPath;
    }

    private static string AddModuleFlat(string name)
    {
        return Regex.Replace(name, @"\{module(:[^}]*)?\}", "{module$1:flat}");
    }
}
