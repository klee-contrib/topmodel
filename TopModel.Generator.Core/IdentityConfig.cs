namespace TopModel.Generator.Core;

/// <summary>
/// Paramètres pour la génération de SQL procédural.
/// </summary>
public class IdentityConfig
{
    /// <summary>
    /// Mode de génération des séquences.
    /// </summary>
    public IdentityMode Mode { get; set; } = IdentityMode.IDENTITY;

    /// <summary>
    /// Incrément de la séquence générée
    /// </summary>
    public int? Increment { get; set; }

    /// <summary>
    /// Début de la séquence générée
    /// </summary>
    public int? Start { get; set; }
}