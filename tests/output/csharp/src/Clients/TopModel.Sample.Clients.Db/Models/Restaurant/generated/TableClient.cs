////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Common;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Table du restaurant.
/// </summary>
[Table("table_client")]
public partial record TableClient
{
    /// <summary>
    /// Identifiant de la table.
    /// </summary>
    [Column("tab_id")]
    [Domain(Domains.Id)]
    [Key]
    public int? Id { get; set; }

    /// <summary>
    /// Numéro de la table.
    /// </summary>
    [Column("tab_numero")]
    [Required]
    [Domain(Domains.Code)]
    [StringLength(10)]
    public string Numero { get; set; }

    /// <summary>
    /// Capacité de la table (nombre de places).
    /// </summary>
    [Column("tab_capacite")]
    [Required]
    [Domain(Domains.Quantite)]
    public int? Capacite { get; set; }

    /// <summary>
    /// Indique si la table est disponible.
    /// </summary>
    [Column("tab_disponible")]
    [Required]
    [Domain(Domains.Booleen)]
    public bool? Disponible { get; set; } = true;

    /// <summary>
    /// Restaurant auquel appartient la table.
    /// </summary>
    [Column("res_id_restaurant")]
    [Required]
    [Domain(Domains.Id)]
    public int? RestaurantIdRestaurant { get; set; }

    /// <summary>
    /// Association réciproque de Commande.TableClientId.
    /// </summary>
    [Domain(Domains.Liste)]
    [NotMapped]
    public ICollection<int> Commandes { get; set; }

    /// <summary>
    /// Association réciproque de Reservation.TableClientIdTable.
    /// </summary>
    [Domain(Domains.Liste)]
    [NotMapped]
    public ICollection<int> ReservationsTable { get; set; }
}
