////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Common;

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
    [Domain(Domains.Id)]
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
    public string Commentaire { get; set; }

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
    public int? ClientIdClient { get; set; }

    /// <summary>
    /// Table réservée.
    /// </summary>
    [Domain(Domains.Id)]
    public int? TableClientIdTable { get; set; }

    /// <summary>
    /// Restaurant concerné par la réservation.
    /// </summary>
    [Required]
    [Domain(Domains.Id)]
    public int? RestaurantIdRestaurant { get; set; }
}
