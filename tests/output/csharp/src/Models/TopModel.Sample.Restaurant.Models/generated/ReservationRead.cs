////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'une réservation en lecture.
/// </summary>
public partial record ReservationRead
{
    /// <summary>
    /// Identifiant de la réservation.
    /// </summary>
    [Required]
    public int? Id { get; set; }

    /// <summary>
    /// Date et heure de la réservation.
    /// </summary>
    [Required]
    public DateTime? DateReservation { get; set; }

    /// <summary>
    /// Nombre de personnes.
    /// </summary>
    [Required]
    public int? NombrePersonnes { get; set; }

    /// <summary>
    /// Commentaire sur la réservation.
    /// </summary>
    [StringLength(100)]
    public string? Commentaire { get; set; }

    /// <summary>
    /// Indique si la réservation est confirmée.
    /// </summary>
    [Required]
    public bool? Confirmee { get; set; } = false;

    /// <summary>
    /// Client ayant fait la réservation.
    /// </summary>
    [Required]
    public int? ClientId { get; set; }

    /// <summary>
    /// Table réservée.
    /// </summary>
    public int? TableId { get; set; }

    /// <summary>
    /// Restaurant concerné par la réservation.
    /// </summary>
    [Required]
    public int? RestaurantId { get; set; }

    /// <summary>
    /// Date de création de l'enregistrement.
    /// </summary>
    [Required]
    public DateTime? DateCreation { get; set; }

    /// <summary>
    /// Informations du client ayant fait la réservation.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string? ClientNom { get; set; }

    /// <summary>
    /// Informations du client ayant fait la réservation.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string? ClientPrenom { get; set; }

    /// <summary>
    /// Informations du client ayant fait la réservation.
    /// </summary>
    [StringLength(100)]
    public string? ClientEmail { get; set; }

    /// <summary>
    /// Table réservée.
    /// </summary>
    [Required]
    [StringLength(10)]
    public string? TableNumero { get; set; }

    /// <summary>
    /// Table réservée.
    /// </summary>
    [Required]
    public int? TableCapacite { get; set; }
}
