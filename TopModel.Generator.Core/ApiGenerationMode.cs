namespace TopModel.Generator.Core;

/// <summary>
/// Mode de génération des endpoints.
/// </summary>
public enum ApiGenerationMode
{
    /// <summary>
    /// Pas de génération.
    /// </summary>
    None,

    /// <summary>
    /// Génération en mode client.
    /// </summary>
    Client,

    /// <summary>
    /// Génération en mode serveur.
    /// </summary>
    Server,
}
