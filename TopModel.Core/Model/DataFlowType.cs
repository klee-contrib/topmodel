namespace TopModel.Core;

public enum DataFlowType
{
    /// <summary>
    /// Insertion simple (bulk insert).
    /// </summary>
    Insert,

    /// <summary>
    /// Remplacement des données (truncate puis bulk insert).
    /// </summary>
    Replace,

    /// <summary>
    /// Fusion des données (bulk merge).
    /// </summary>
    Merge,

    /// <summary>
    /// Fusion des données et désactivation des données non matchées (bulk merge + bulk update)
    /// </summary>
    MergeDisable
}