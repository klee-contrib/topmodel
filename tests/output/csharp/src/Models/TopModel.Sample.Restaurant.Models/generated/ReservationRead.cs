////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;

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
    [Domain(Domains.SeqId)]
    public int? Id { get; set; }

    /// <summary>
    /// Date et heure de la réservation.
    /// </summary>
    [Required]
    [Domain(Domains.DateHeure)]
    public DateTime? DateReservation { get; set; }

    /// <summary>
    /// Nombre de personnes.
    /// </summary>
    [Required]
    [Domain(Domains.Quantite)]
    public int? NombrePersonnes { get; set; }

    /// <summary>
    /// Commentaire sur la réservation.
    /// </summary>
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string? Commentaire { get; set; }

    /// <summary>
    /// Indique si la réservation est confirmée.
    /// </summary>
    [Required]
    [Domain(Domains.Booleen)]
    public bool? Confirmee { get; set; } = false;

    /// <summary>
    /// Client ayant fait la réservation.
    /// </summary>
    [Required]
    [Domain(Domains.Id)]
    public int? ClientId { get; set; }

    /// <summary>
    /// Table réservée.
    /// </summary>
    [Domain(Domains.Id)]
    public int? TableId { get; set; }

    /// <summary>
    /// Restaurant concerné par la réservation.
    /// </summary>
    [Required]
    [Domain(Domains.Id)]
    public int? RestaurantId { get; set; }

    /// <summary>
    /// Date de création de l'enregistrement.
    /// </summary>
    [Required]
    [Domain(Domains.DateHeure)]
    public DateTime? DateCreation { get; set; }

    /// <summary>
    /// Informations du client ayant fait la réservation.
    /// </summary>
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string? ClientNom { get; set; }

    /// <summary>
    /// Informations du client ayant fait la réservation.
    /// </summary>
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string? ClientPrenom { get; set; }

    /// <summary>
    /// Informations du client ayant fait la réservation.
    /// </summary>
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string? ClientEmail { get; set; }

    /// <summary>
    /// Table réservée.
    /// </summary>
    [Required]
    [Domain(Domains.Code)]
    [StringLength(10)]
    public string? TableNumero { get; set; }

    /// <summary>
    /// Table réservée.
    /// </summary>
    [Required]
    [Domain(Domains.Quantite)]
    public int? TableCapacite { get; set; }
}
