namespace TopModel.Generator.Core;

/// <summary>
/// Version de Kinetix.
/// </summary>
public static class ClientApiMode
{
    /// <summary>
    /// Génération d'un client en mode Feign (interface spring controller avec l'annotation Feign).
    /// </summary>
    public const string FeignClient = nameof(FeignClient);

    /// <summary>
    /// Génération d'un client en mode RestClient (interface Exchange).
    /// </summary>
    public const string RestClient = nameof(RestClient);

    /// <summary>
    /// Gébération d'un client en mode template (abstract class à initialiser).
    /// </summary>
    public const string RestTemplate = nameof(RestTemplate);
}
