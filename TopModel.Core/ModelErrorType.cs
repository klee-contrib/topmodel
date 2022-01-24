
using TopModel.Core.FileModel;

/// <summary>
/// Mode de génération du fichier de ressources.
/// </summary>
public enum ModelErrorType
{
    /// <summary>
    /// L'import '{use.ReferenceName}' n'est pas utilisé."
    /// </summary>
    USESLESS_IMPORT,

    /// <summary>
    /// L'import '{use.ReferenceName}' n'est pas trié par ordre alphabétique
    /// </summary>
    UNSORTED_IMPORT,
}
