namespace TopModel.Core.Model.Implementation;

public enum Target
{
    /// <summary>
    /// Non utilisé.
    /// </summary>
    None,

    /// <summary>
    /// Classes persistées.
    /// </summary>
    Persisted,

    /// <summary>
    /// Classes non persistées.
    /// </summary>
    Dto,

    /// <summary>
    /// Toutes les classes.
    /// </summary>
    Persisted_Dto,

    /// <summary>
    /// Endpoints d'API.
    /// </summary>
    Api,

    /// <summary>
    /// Endpoints d'API et classes persistées.
    /// </summary>
    Api_Persisted,

    /// <summary>
    /// Endpoints d'API et classes non persistées.
    /// </summary>
    Api_Dto,

    /// <summary>
    /// Partout.
    /// </summary>
    Api_Dto_Persisted
}