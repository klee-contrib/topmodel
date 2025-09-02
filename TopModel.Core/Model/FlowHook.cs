namespace TopModel.Core.Model;

public enum FlowHook
{
    /// <summary>
    /// Avant le flow.
    /// </summary>
    BeforeFlow,

    /// <summary>
    /// Après le chargement de la source.
    /// </summary>
    AfterSource,

    /// <summary>
    /// Au mapping source => target.
    /// </summary>
    Map,

    /// <summary>
    /// Avant la target.
    /// </summary>
    BeforeTarget,

    /// <summary>
    /// Après le flow.
    /// </summary>
    AfterFlow,
}
