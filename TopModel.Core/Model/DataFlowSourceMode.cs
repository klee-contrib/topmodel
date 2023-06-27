namespace TopModel.Core;

public enum DataFlowSourceMode
{
    /// <summary>
    /// Implémentation manuelle.
    /// </summary>
    Partial,

    /// <summary>
    /// Implémentation automatique qui sélectionne tous les éléments.
    /// </summary>
    QueryAll
}