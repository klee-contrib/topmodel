using TopModel.Core.Model.Implementation;
using TopModel.Generator.Core;

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
}