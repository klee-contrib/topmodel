namespace TopModel.Generator.Ssdt;

/// <summary>
/// Paramètres pour la génération SSDT.
/// </summary>
public class SsdtConfig : GeneratorConfigBase
{
    /// <summary>
    /// Dossier du projet pour les scripts de déclaration de table.
    /// </summary>
    public string? TableScriptFolder { get; set; }

    /// <summary>
    /// Dossier du projet pour les scripts de déclaration de type table.
    /// </summary>
    public string? TableTypeScriptFolder { get; set; }

    /// <summary>
    /// Dossier du projet pour les scripts d'initialisation des listes statiques.
    /// </summary>
    public string? InitListScriptFolder { get; set; }

    /// <summary>
    /// Fichier du projet référençant les scripts d'initialisation des listes statiques.
    /// </summary>
    public string? InitListMainScriptName { get; set; }

    /// <summary>
    /// Désactive les colonnes d'identité.
    /// </summary>
    public bool DisableIdentity { get; set; }
}