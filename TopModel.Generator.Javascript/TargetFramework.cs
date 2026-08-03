namespace TopModel.Generator.Javascript;

/// <summary>
/// Framework cible pour la génération des clients d'API JavaScript.
/// </summary>
public enum TargetFramework
{
    /// <summary>
    /// Fetch.
    /// </summary>
    FETCH,

    /// <summary>
    /// Angular.
    /// </summary>
    ANGULAR,

    /// <summary>
    /// Angular qui retourne des promesses au lieu d'observables.
    /// </summary>
    ANGULAR_PROMISE,

    /// <summary>
    /// Legacy.
    /// </summary>
    LEGACY,
}
