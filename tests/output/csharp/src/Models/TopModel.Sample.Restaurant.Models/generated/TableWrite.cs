////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'une table en écriture.
/// </summary>
public partial record TableWrite
{
    /// <summary>
    /// Numéro de la table.
    /// </summary>
    [Required]
    [StringLength(10)]
    public string? Numero { get; set; }

    /// <summary>
    /// Capacité de la table (nombre de places).
    /// </summary>
    [Required]
    public int? Capacite { get; set; }

    /// <summary>
    /// Indique si la table est disponible.
    /// </summary>
    [Required]
    public bool? Disponible { get; set; } = true;

    /// <summary>
    /// Restaurant auquel appartient la table.
    /// </summary>
    [Required]
    public int? RestaurantId { get; set; }
}
