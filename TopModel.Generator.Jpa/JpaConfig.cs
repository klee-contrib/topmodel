using System.Text.RegularExpressions;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Core.Model.Implementation;
using TopModel.Generator.Core;
using TopModel.Utils;

namespace TopModel.Generator.Jpa;

public class JpaConfig : GeneratorConfigBase
{
    /// <summary>
    /// Si on doit générer le meta model
    /// </summary>
    public virtual bool MetaModel { get; set; } = false;

    /// <summary>
    /// Transforme les classes contenant des values en enum. Par défaut, false.
    /// </summary>
    public virtual bool EnumsAsEnums { get; set; } = false;

    /// <summary>
    /// Localisation des classes persistées du modèle, relative au répertoire de génération. Par défaut, 'javagen/{app:path}/entities/{module:path}'.
    /// </summary>
    public virtual string EntitiesPath { get; set; } = "javagen:{app:path}/entities/{module:path}";

    /// <summary>
    /// Localisation des enums, relative au répertoire de génération. Par défaut, 'javagen:{app:path}/enums/{module:path}'.
    /// </summary>
    public virtual string EnumsPath { get; set; } = "javagen:{app:path}/enums/{module:path}";

    /// <summary>
    /// Localisation des DAOs, relative au répertoire de génération.
    /// </summary>
    public virtual string? DaosPath { get; set; }

    /// <summary>
    /// Localisation des classses non persistées du modèle, relative au répertoire de génération. Par défaut, 'javagen/{app:path}/dtos/{module:path}'.
    /// </summary>
    public virtual string DtosPath { get; set; } = "javagen:{app:path}/dtos/{module:path}";

    /// <summary>
    /// Localisation du l'API générée (client ou serveur), relative au répertoire de génération. Par défaut, 'javagen/{app:path}/api/{module:path}'.
    /// </summary>
    public virtual string ApiPath { get; set; } = "javagen:{app:path}/api/{module:path}";

    /// <summary>
    /// Si les annotation swagger-annotation-jakarta doivent être ajoutées aux interface
    /// </summary>
    public virtual bool OpenApiAnnotations { get; set; } = false;

    /// <summary>
    /// Mode de génération de l'API ("Client" ou "Server").
    /// </summary>
    public virtual string? ApiGeneration { get; set; }

    /// <summary>
    /// Mode de génération de l'API Client (RestClient, RestTemplate ou FeignClient).
    /// </summary>
    public virtual string? ClientApiGeneration { get; set; } = ClientApiMode.RestClient;

    /// <summary>
    /// Localisation des ressources, relative au répertoire de génération.
    /// </summary>
    public virtual string? ResourcesPath { get; set; }

    /// <summary>
    /// Localisation des ressources, relative au répertoire de génération.
    /// </summary>
    public virtual ResourcesEncoding? ResourcesEncoding { get; set; } = Jpa.ResourcesEncoding.Latin1;

    /// <summary>
    /// Nom du schéma sur lequel les entités sont sauvegardées
    /// </summary>
    public virtual string? DbSchema { get; set; }

    /// <summary>
    /// Nom complet de la classe permettant de convertir les compositions stockées en json dans la bdd
    /// </summary>
    public virtual string CompositionConverterCanonicalName { get; set; } = "{package}.{class}Converter";

    public virtual string CompositionConverterSimpleName => CompositionConverterCanonicalName.Split('.')[^1];

    public virtual JavaAnnotation GeneratedAnnotation =>
        new JavaAnnotation("Generated", imports: "jakarta.annotation.Generated").AddAttribute(
            "value",
            "\"TopModel : https://github.com/klee-contrib/topmodel\""
        );

    public override Dictionary<string, List<string>> TemplateAttributes =>
        new()
        {
            [nameof(CompositionConverterCanonicalName)] = ["package", "class"],
            [nameof(DaosName)] = ["class"],
            [nameof(ApisName)] = ["fileName"],
        };

    /// <summary>
    /// Option pour générer des adders pour les associations oneToMany et ManyToMany
    /// </summary>
    public virtual bool AssociationAdders { get; set; } = false;

    /// <summary>
    /// Option pour générer des removers pour les associations oneToMany et ManyToMany
    /// </summary>
    public virtual bool AssociationRemovers { get; set; } = false;

    /// <summary>
    /// Option pour générer l'annotation @Generated("TopModel : https://github.com/klee-contrib/topmodel")
    /// </summary>
    public virtual bool GeneratedHint { get; set; } = true;

    /// <summary>
    /// Option pour générer une enum des champs des classes persistées ou non persistées.
    /// </summary>
    public virtual IEnumerable<AnnotationConstraint> FieldsEnum { get; set; } = [];

    /// <summary>
    /// Précise l'interface des fields enum générés.
    /// </summary>
    public virtual string? FieldsEnumInterface { get; set; }

    /// <summary>
    /// Mode de génération des séquences.
    /// </summary>
    public virtual IdentityConfig Identity { get; set; } = new() { Mode = IdentityMode.IDENTITY };

    /// <summary>
    /// Location des flux de données générés.
    /// </summary>
    public virtual string? DataFlowsPath { get; set; }

    /// <summary>
    /// Writer à utiliser pour les flux de données.
    /// </summary>
    public virtual DataFlowsWriter DataFlowsWriter { get; set; } = DataFlowsWriter.Jpa;

    /// <summary>
    /// Génération en mode JDBC.
    /// </summary>
    public virtual bool UseJdbc { get; set; } = false;

    /// <summary>
    /// Génération d'interface Abstract avec @NoRepositoryBean permettant de mettre à jour le code généré.
    /// </summary>
    public virtual bool DaosAbstract { get; set; } = false;

    /// <summary>
    /// Nom des Daos générés.
    /// </summary>
    public virtual string? DaosName { get; set; }

    /// <summary>
    /// Nom des classes d'apis générées. La valeur par défaut dépend du type d'api générée.
    /// </summary>
    public virtual string? ApisName { get; set; }

    /// <summary>
    /// Précise l'interface des Daos générés.
    /// </summary>
    public virtual string? DaosInterface { get; set; }

    /// <summary>
    /// Indique s'il faut ajouter les mappers en tant méthode ou constructeur dans les classes qui les déclarent.
    /// </summary>
    public virtual bool MappersInClass { get; set; } = true;

    /// <summary>
    /// Taille des chunks à extraire et insérer
    /// </summary>
    public virtual long DataFlowsBulkSize { get; set; } = 100000;

    /// <summary>
    /// Listeners à ajouter aux dataflows
    /// </summary>
    public virtual IList<string> DataFlowsListeners { get; set; } = [];

    public override string? DefaultLanguage => "java";

    public override string[] PropertiesWithLangVariableSupport => [nameof(ResourcesPath)];

    public override string[] PropertiesWithTagVariableSupport =>
        [
            nameof(EntitiesPath),
            nameof(DaosPath),
            nameof(DtosPath),
            nameof(ApiPath),
            nameof(ResourcesPath),
            nameof(EnumsValuesPath),
            nameof(EnumsPath),
            nameof(EnumsValuesPath),
            nameof(DataFlowsPath),
            nameof(ApiGeneration),
            nameof(DbSchema),
            nameof(ApisName),
        ];

    public override string[] PropertiesWithModuleVariableSupport =>
        [
            nameof(EntitiesPath),
            nameof(DaosPath),
            nameof(DtosPath),
            nameof(ApiPath),
            nameof(ResourcesPath),
            nameof(EnumsValuesPath),
            nameof(EnumsPath),
            nameof(EnumsValuesPath),
            nameof(DataFlowsPath),
            nameof(ApisName),
        ];

    /// <summary>
    /// Localisation des enums de valeurs, relative au répertoire de génération. Par défaut, 'javagen:{app:path}/enums/{module:path}'.
    /// </summary>
    public virtual string EnumsValuesPath { get; set; } = "default";

    public override bool CanClassUseEnums(Class classe, IProperty? prop = null)
    {
        return !UseJdbc
            && base.CanClassUseEnums(classe, prop)
            && !classe
                .Properties.OfType<AssociationProperty>()
                .Any(a => a.Association != classe && !CanClassUseEnums(a.Association));
    }

    public virtual string GetApiClassName(string defaultValue, string fileName, string tag)
    {
        return ResolveVariables(ApisName ?? defaultValue, tag).Replace("{fileName}", fileName.ToPascalCase());
    }

    public virtual string GetApiPath(ModelFile file, string tag)
    {
        return Path.Combine(
            OutputDirectory,
            ResolveVariables(ApiPath!, tag, module: file.Namespace.Module).ToFilePath()
        );
    }

    public virtual string GetClassFileName(Class classe, string tag)
    {
        return Path.Combine(
            OutputDirectory,
            ResolveVariables(classe.IsPersistent ? EntitiesPath : DtosPath, tag, module: classe.Namespace.Module)
                .ToFilePath(),
            $"{classe.NamePascal}.java"
        );
    }

    public virtual string GetDataFlowConfigFilePath(string module)
    {
        return Path.Combine(
            OutputDirectory,
            ResolveVariables(DataFlowsPath!, module: module).ToFilePath().ToFilePath(),
            $"{module.ToPascalCase()}JobConfiguration.java"
        );
    }

    public virtual string GetDataFlowFilePath(DataFlow df, string tag)
    {
        return Path.Combine(
            OutputDirectory,
            ResolveVariables(DataFlowsPath!, tag: tag, module: df.ModelFile.Namespace.ModulePath).ToFilePath(),
            $"{df.Name.ToPascalCase()}Flow.java"
        );
    }

    public virtual string GetDataFlowPartialFilePath(DataFlow df, string tag)
    {
        return Path.Combine(
            OutputDirectory,
            ResolveVariables(DataFlowsPath!, tag: tag, module: df.ModelFile.Namespace.ModulePath).ToFilePath(),
            $"{df.Name.ToPascalCase()}PartialFlow.java"
        );
    }

    public virtual IEnumerable<JavaAnnotation> GetDomainJavaAnnotations(IProperty property, string tag)
    {
        return GetAnnotations(property, tag)
            .Select(a =>
            {
                return new JavaAnnotation(name: a.Annotation, imports: a.Imports.ToArray());
            });
    }

    public virtual string GetEnumFileName(IProperty property, Class classe, string tag)
    {
        return Path.Combine(
            OutputDirectory,
            ResolveVariables(EnumsPath, tag, module: classe.Namespace.Module).ToFilePath(),
            $"{GetEnumName(property, classe)}.java"
        );
    }

    public virtual string GetEnumName(IProperty property, Class classe)
    {
        return $"{classe.NamePascal}{property.Name.ToPascalCase()}";
    }

    public virtual string GetEnumPackageName(Class classe, string tag)
    {
        return GetPackageName(classe.Namespace, EnumsPath, tag);
    }

    public virtual string GetEnumValueFileName(Class classe, string tag)
    {
        return Path.Combine(
            OutputDirectory,
            ResolveVariables(EnumsValuesPath, tag, module: classe.Namespace.Module).ToFilePath(),
            $"{classe.NamePascal}.java"
        );
    }

    public virtual string GetEnumValuePackageName(Class classe, string tag)
    {
        return GetPackageName(classe.Namespace, EnumsValuesPath, tag);
    }

    public virtual string GetMapperFilePath((Class Classe, FromMapper Mapper) mapper, string tag)
    {
        var (ns, modelPath) = GetMapperLocation(mapper);
        return Path.Combine(
            OutputDirectory,
            ResolveVariables(modelPath, tag: tag, module: ns.Module).ToFilePath(),
            $"{GetMapperName(ns, modelPath)}.java"
        );
    }

    public virtual string GetMapperFilePath((Class Classe, ClassMappings Mapper) mapper, string tag)
    {
        var (ns, modelPath) = GetMapperLocation(mapper);
        return Path.Combine(
            OutputDirectory,
            ResolveVariables(modelPath, tag: tag, module: ns.Module).ToFilePath(),
            $"{GetMapperName(ns, modelPath)}.java"
        );
    }

    public virtual string GetMapperImport(Namespace ns, string modelPath, string tag)
    {
        return $@"{GetPackageName(ns, modelPath, tag)}.{GetMapperName(ns, modelPath)}";
    }

    public virtual (Namespace Namespace, string ModelPath) GetMapperLocation((Class Classe, FromMapper Mapper) mapper)
    {
        if (mapper.Classe.IsPersistent)
        {
            return (mapper.Classe.Namespace, EntitiesPath);
        }

        var persistentParam = mapper.Mapper.ClassParams.FirstOrDefault(p => p.Class.IsPersistent);
        if (persistentParam != null)
        {
            return (persistentParam.Class.Namespace, EntitiesPath);
        }

        return (mapper.Classe.Namespace, DtosPath);
    }

    public virtual (Namespace Namespace, string ModelPath) GetMapperLocation(
        (Class Classe, ClassMappings Mapper) mapper
    )
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

    public virtual string GetMapperName(Namespace ns, string modelPath)
    {
        return $"{string.Join(string.Empty, ns.Module.Split('.').Select(m => m.ToPascalCase()))}{(modelPath == EntitiesPath ? string.Empty : "DTO")}Mappers".ToPascalCase();
    }

    public virtual string GetPackageName(Endpoint endpoint, string tag)
    {
        return GetPackageName(endpoint.Namespace, ApiPath, tag);
    }

    public virtual string GetPackageName(Class classe, string tag, bool? isPersistent = null)
    {
        return GetPackageName(
            classe.Namespace,
            isPersistent.HasValue
                ? isPersistent.Value
                    ? EntitiesPath
                    : DtosPath
                : classe.IsPersistent
                    ? EntitiesPath
                    : DtosPath,
            tag
        );
    }

    public virtual string GetPackageName(Namespace ns, string modelPath, string tag)
    {
        return ResolveVariables(modelPath, tag, module: ns.Module).ToPackageName();
    }

    public virtual bool HasAnnotation(IAnnotationContainer classe, string annotation)
    {
        return classe
            .Annotations.SelectMany(a => GetImplementation(a.Annotation))
            .Any(a => a.Text.Trim('@') == annotation.Trim('@'));
    }

    public virtual bool IsEnumNameJavaValid(string name)
    {
        return IsEnumNameValid(name);
    }

    protected override string GetConstEnumName(string className, string refName)
    {
        if (UseJdbc)
        {
            return @$"""{refName}""";
        }

        return $"{className.ToPascalCase()}.{refName}";
    }

    protected override string GetEnumType(string className, string propName, bool isPrimaryKeyDef = false)
    {
        if (EnumsAsEnums)
        {
            return $"{className.ToPascalCase()}";
        }

        return $"{className.ToPascalCase()}{propName.ToPascalCase()}";
    }

    protected override bool IsEnumNameValid(string name)
    {
        return base.IsEnumNameValid(name)
            && !Regex.IsMatch(
                name ?? string.Empty,
                "(?<=[^$\\w'\"\\])(?!(abstract|assert|boolean|break|byte|case|catch|char|class|const|continue|default|double|do|else|enum|extends|false|final|finally|float|for|goto|if|implements|import|instanceof|int|interface|long|native|new|null|package|private|protected|public|return|short|static|strictfp|super|switch|synchronized|this|throw|throws|transient|true|try|void|volatile|while|_\\b))([A-Za-z_$][$\\w]*)"
            );
    }
}
