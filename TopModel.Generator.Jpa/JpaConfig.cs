using System.Text;
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
    public virtual string DbSchema { get; set; } = "public";

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
    /// Option pour générer des adders pour les associations multiples
    /// </summary>
    public virtual bool AssociationAdders { get; set; } = false;

    /// <summary>
    /// Option pour générer des removers pour les associations multiples
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
    /// Types de cascade à ajouter sur les associations, par type d'association. Par défaut, aucune cascade n'est ajoutée.
    /// </summary>
    public virtual IDictionary<AssociationType, IEnumerable<CascadeType>> CascadeTypes { get; set; } =
        new Dictionary<AssociationType, IEnumerable<CascadeType>>()
        {
            [AssociationType.OneToOne] = [CascadeType.All],
            [AssociationType.OneToMany] = [CascadeType.All],
        };

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
    public virtual bool MappersInClass { get; set; } = false;

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

    /// <summary>
    /// Si un mapper contient au moins une classe de ces tags, alors il sera généré avec les tags de cette classe (au lieu du comportement par défaut qui priorise les tags de la classe persistée puis de celle qui définit le mapper).
    /// </summary>
    public virtual string[] MapperTagsOverrides { get; set; } = [];

    public override string[] PropertiesWithTagVariableSupport =>
        [
            nameof(EntitiesPath),
            nameof(DaosPath),
            nameof(DtosPath),
            nameof(ApiPath),
            nameof(ResourcesPath),
            nameof(EnumsPath),
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
            nameof(EnumsPath),
            nameof(DataFlowsPath),
            nameof(ApisName),
        ];

    public virtual string GetApiClassName(string defaultValue, string fileName, string tag)
    {
        return ResolveVariables(ApisName ?? defaultValue, tag).Replace("{fileName}", fileName.ToPascalCase());
    }

    public virtual string GetApiPath(ModelFile file, string tag)
    {
        var path = ResolveVariables(ApiPath, tag, module: file.Namespace.Module);
        return Path.Combine(OutputDirectory, path.ToFilePath());
    }

    public virtual IEnumerable<IProperty> GetAvailableProperties(Class classe)
    {
        if (UseJdbc)
        {
            return classe.Properties.Where(p =>
                (!p.AssociationMultiple && !p.IsReverseProperty || !classe.IsPersistent)
                && (p is not { Composition: Class cpc } || AvailableClasses.Contains(cpc))
            );
        }
        return classe.Properties.Where(p => p is not { Composition: Class cpc } || AvailableClasses.Contains(cpc));
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

    public virtual string GetCollector(Domain domain)
    {
        var impl = GetImplementation(domain)!;
        return $"collect(Collectors.{impl.Collector ?? $"to{(impl.GenericType?.Value[0..impl.GenericType.Value.IndexOf('<')] ?? impl.Type)}()"})";
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

    public virtual string GetEnumFileName(Class classe, string tag, IProperty? property = null)
    {
        property ??= classe.EnumKey!;

        return Path.Combine(
            OutputDirectory,
            ResolveVariables(EnumsPath, tag, module: classe.Namespace.Module).ToFilePath(),
            $"{GetEnumType(property)}.java"
        );
    }

    public virtual string GetEnumPackageName(Class classe, string tag)
    {
        return GetPackageName(classe.Namespace, EnumsPath, tag);
    }

    public virtual string GetGetterName(IProperty property)
    {
        var propertyName = property.NameCamel;
        var propertyType = GetType(property);
        var getterPrefix = propertyType == "boolean" ? "is" : "get";
        if (property.Class.PreservePropertyCasing)
        {
            return propertyName.ToFirstUpper().WithPrefix(getterPrefix);
        }

        return propertyName.ToPascalCase().WithPrefix(getterPrefix);
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

    public override string GetReadonlyEnumClassInstanceName(
        Class classe,
        string refName,
        bool internalReference = false
    )
    {
        if (UseJdbc)
        {
            return string.Empty;
        }

        var sb = new StringBuilder();

        if (!internalReference)
        {
            sb.Append($"{classe.NamePascal}.");
        }

        sb.Append(refName.ToConstantCase());
        return sb.ToString();
    }

    public override string GetUniqueValuedName(IProperty property, string refName, bool internalReference = false)
    {
        return $"{property.Class.NamePascal}{property.NamePascal}.{refName.ToPascalCase(strictIfUppercase: true)}";
    }

    public virtual bool HasAnnotation(IAnnotationContainer classe, string annotation)
    {
        return classe
            .Annotations.SelectMany(a => GetImplementation(a.Annotation))
            .Any(a => a.Text.Trim('@') == annotation.Trim('@'));
    }
}
