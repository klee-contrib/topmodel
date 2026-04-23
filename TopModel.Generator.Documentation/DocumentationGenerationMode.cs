namespace TopModel.Generator.Documentation;

/// <summary>
/// Mode de découpage des fichiers de documentation générés.
/// </summary>
public enum DocumentationGenerationMode
{
    /// <summary>
    /// Un seul fichier global regroupant toutes les classes / endpoints.
    /// </summary>
    All,

    /// <summary>
    /// Un fichier par module.
    /// </summary>
    Module,

    /// <summary>
    /// Un fichier par fichier de modèle.
    /// </summary>
    File,
}
