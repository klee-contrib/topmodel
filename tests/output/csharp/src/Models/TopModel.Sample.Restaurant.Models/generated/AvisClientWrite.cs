////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'un avis en écriture.
/// </summary>
public partial record AvisClientWrite
{
    /// <summary>
    /// Note sur 5.
    /// </summary>
    [Required]
    public int? Note { get; set; }

    /// <summary>
    /// Commentaire de l'avis.
    /// </summary>
    [StringLength(100)]
    public string? Commentaire { get; set; }

    /// <summary>
    /// Indique si l'avis est approuvé par le restaurant.
    /// </summary>
    [Required]
    public bool? Approuve { get; set; } = false;

    /// <summary>
    /// Client ayant donné l'avis.
    /// </summary>
    [Required]
    public int? ClientId { get; set; }

    /// <summary>
    /// Restaurant concerné par l'avis.
    /// </summary>
    [Required]
    public int? RestaurantId { get; set; }
}
