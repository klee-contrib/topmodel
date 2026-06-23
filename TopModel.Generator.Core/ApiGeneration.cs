namespace TopModel.Generator.Core;

/// <summary>
/// Mode de génération de des endpoints.
/// </summary>
[Obsolete("Utiliser Config.GetApiGenerationMode(tag)")]
public static class ApiGeneration
{
    /// <summary>
    /// Gébération d'un client.
    /// </summary>
    public const string Client = nameof(Client);

    /// <summary>
    /// Génération d'un serveur.
    /// </summary>
    public const string Server = nameof(Server);
}
