#nullable disable

using TopModel.Core;

namespace TopModel.Generator.Jpa;

public class JpaConfig : GeneratorConfigBase
{
    /// <summary>
    /// Localisation du modèle, relative au répertoire de génération.
    /// </summary>
    public string ModelRootPath { get; set; }

    /// <summary>
    /// Localisation des ressources, relative au répertoire de génération.
    /// </summary>
    public string ResourceRootPath { get; set; }

    /// <summary>
    /// Localisation du l'API générée (client ou serveur), relatif au répertoire de génération.
    /// </summary>
    public string ApiRootPath { get; set; }

    /// <summary>
    /// Précise le nom du package dans lequel générer les classes persistées du modèle.
    /// </summary>
    public string EntitiesPackageName { get; set; }

    /// <summary>
    /// Précise le nom du package dans lequel générer les classes non persistées du modèle.
    /// </summary>
    public string DtosPackageName { get; set; }

    /// <summary>
    /// Précise le nom du package dans lequel générer les classes non persistées du modèle.
    /// </summary>
    public string DaosPackageName { get; set; }

    /// <summary>
    /// Option pour générer une enum des champs des classes persistées
    /// </summary>
    public Target FieldsEnum { get; set; } = Target.None;

    /// <summary>
    /// Option pour générer des getters et setters vers l'enum des références plutôt que sur la table
    /// </summary>
    public bool EnumShortcutMode { get; set; }

    /// <summary>
    /// Précise l'interface des fields enum générés.
    /// </summary>
    public string FieldsEnumInterface { get; set; }

    /// <summary>
    /// Précise le nom du package dans lequel générer les controllers.
    /// </summary>
    public string ApiPackageName { get; set; }

    /// <summary>
    /// Précise le nom du package dans lequel générer les controllers.
    /// </summary>
    public PersistenceMode PersistenceMode { get; set; } = PersistenceMode.Javax;

#nullable enable

    /// <summary>
    /// Mode de génération de l'API ("Client" ou "Server").
    /// </summary>
    public string? ApiGeneration { get; set; }

    /// <summary>
    /// Mode de génération des séquences.
    /// </summary>
    public IdentityConfig Identity { get; set; } = new IdentityConfig()
    {
        Mode = IdentityMode.IDENTITY
    };

    public override string[] PropertiesWithLangVariableSupport => new[]
    {
        nameof(ResourceRootPath)
    };

    public override string[] PropertiesWithTagVariableSupport => new[]
    {
        nameof(ModelRootPath),
        nameof(EntitiesPackageName),
        nameof(DaosPackageName),
        nameof(DtosPackageName),
        nameof(ResourceRootPath),
        nameof(ApiGeneration),
        nameof(ApiRootPath),
        nameof(ApiPackageName)
    };
}