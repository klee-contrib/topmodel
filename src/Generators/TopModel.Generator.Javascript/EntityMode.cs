namespace TopModel.Generator.Javascript;

/// <summary>
/// Mode de génération des entités TypeScript.
/// </summary>
public enum EntityMode
{
    /// <summary>
    /// Ne pas typer les entités.
    /// </summary>
    UNTYPED,

    /// <summary>
    /// Typer les entités.
    /// </summary>
    TYPED,

    /// <summary>
    /// DTO sans entité.
    /// </summary>
    NONE,

    /// <summary>
    /// Définition d'entités @focus4/entities
    /// </summary>
    FOCUS,
}
