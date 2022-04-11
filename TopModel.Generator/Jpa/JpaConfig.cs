#nullable disable

namespace TopModel.Generator.Jpa;

public class JpaConfig : GeneratorConfigBase
{
    /// <summary>
    /// Dossier de sortie pour le modèle.
    /// </summary>
    public string ModelOutputDirectory { get; set; }

    /// <summary>
    /// Précise le nom du package dans lequel générer les classes du modèle.
    /// </summary>
    public string DaoPackageName { get; set; }

    /// <summary>
    /// Option pour générer une enum des champs des classes persistées
    /// </summary>
    public bool FieldsEnum { get; set; }

    /// <summary>
    /// Option pour ajouter l'annotation @SuperBuilder
    /// </summary>
    public bool LombokBuilder { get; set; }

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

#nullable enable

    /// <summary>
    /// Dossier de sortie pour les Controllers.
    /// </summary>
    public string? ApiOutputDirectory { get; set; }
}