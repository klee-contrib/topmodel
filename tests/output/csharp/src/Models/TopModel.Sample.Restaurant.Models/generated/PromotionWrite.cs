////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'une promotion en écriture.
/// </summary>
public partial record PromotionWrite
{
    /// <summary>
    /// Libellé de la promotion.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string? Libelle { get; set; }

    /// <summary>
    /// Pourcentage de réduction (0-100).
    /// </summary>
    [Required]
    public int? PourcentageReduction { get; set; }

    /// <summary>
    /// Date de début de la promotion.
    /// </summary>
    [Required]
    public DateTime? DateDebut { get; set; }

    /// <summary>
    /// Date de fin de la promotion.
    /// </summary>
    [Required]
    public DateTime? DateFin { get; set; }

    /// <summary>
    /// Indique si la promotion est active.
    /// </summary>
    [Required]
    public bool? Active { get; set; } = true;

    /// <summary>
    /// Restaurant concerné par la promotion (null si globale).
    /// </summary>
    public int? RestaurantId { get; set; }
}
