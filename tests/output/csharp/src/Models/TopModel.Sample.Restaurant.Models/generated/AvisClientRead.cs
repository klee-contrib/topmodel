////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'un avis en lecture.
/// </summary>
public partial record AvisClientRead
{
    /// <summary>
    /// Identifiant de l'avis.
    /// </summary>
    [Required]
    public int? Id { get; set; }

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
    /// Date de l'avis.
    /// </summary>
    [Required]
    public DateTime? DateAvis { get; set; }

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

    /// <summary>
    /// Date de création de l'enregistrement.
    /// </summary>
    [Required]
    public DateTime? DateCreation { get; set; }

    /// <summary>
    /// Nombre de vues de l'avis (calculé).
    /// </summary>
    [Required]
    public int? NombreVues { get; set; } = 0;
}
