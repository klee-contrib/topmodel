////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Common;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Réservation d'une table.
/// </summary>
[Table("reservation")]
public partial record Reservation
{
    /// <summary>
    /// Identifiant de la réservation.
    /// </summary>
    [Column("rev_id")]
    [Domain(Domains.Id)]
    [Key]
    public int? Id { get; set; }

    /// <summary>
    /// Date et heure de la réservation.
    /// </summary>
    [Column("rev_date_reservation")]
    [Required]
    [Domain(Domains.DateHeure)]
    public DateTime? DateReservation { get; set; }

    /// <summary>
    /// Nombre de personnes.
    /// </summary>
    [Column("rev_nombre_personnes")]
    [Required]
    [Domain(Domains.Quantite)]
    public int? NombrePersonnes { get; set; }

    /// <summary>
    /// Commentaire sur la réservation.
    /// </summary>
    [Column("rev_commentaire")]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string Commentaire { get; set; }

    /// <summary>
    /// Indique si la réservation est confirmée.
    /// </summary>
    [Column("rev_confirmee")]
    [Required]
    [Domain(Domains.Booleen)]
    public bool? Confirmee { get; set; } = false;

    /// <summary>
    /// Client ayant fait la réservation.
    /// </summary>
    [Column("cli_id_client")]
    [Required]
    [Domain(Domains.Id)]
    public int? ClientIdClient { get; set; }

    /// <summary>
    /// Table réservée.
    /// </summary>
    [Column("tab_id_table")]
    [Domain(Domains.Id)]
    public int? TableClientIdTable { get; set; }

    /// <summary>
    /// Restaurant concerné par la réservation.
    /// </summary>
    [Column("res_id_restaurant")]
    [Required]
    [Domain(Domains.Id)]
    public int? RestaurantIdRestaurant { get; set; }
}
