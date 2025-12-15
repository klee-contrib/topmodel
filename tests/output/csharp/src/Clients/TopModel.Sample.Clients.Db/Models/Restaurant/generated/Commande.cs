////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Common;
using TopModel.Sample.Restaurant.Models;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Commande d'un client.
/// </summary>
[Table("commande")]
public partial record Commande
{
    /// <summary>
    /// Identifiant de la commande.
    /// </summary>
    [Column("com_id")]
    [Domain(Domains.Id)]
    [Key]
    public int? Id { get; set; }

    /// <summary>
    /// Date et heure de la commande.
    /// </summary>
    [Column("com_date_commande")]
    [Required]
    [Domain(Domains.DateHeure)]
    public DateTime? DateCommande { get; set; }

    /// <summary>
    /// Date et heure de livraison.
    /// </summary>
    [Column("com_date_livraison")]
    [Domain(Domains.DateHeure)]
    public DateTime? DateLivraison { get; set; }

    /// <summary>
    /// Montant total de la commande.
    /// </summary>
    [Column("com_montant_total")]
    [Required]
    [Domain(Domains.Prix)]
    public decimal? MontantTotal { get; set; }

    /// <summary>
    /// Client ayant passé la commande.
    /// </summary>
    [Column("per_id")]
    [Required]
    [Domain(Domains.Id)]
    public int? ClientId { get; set; }

    /// <summary>
    /// Table associée à la commande.
    /// </summary>
    [Column("tab_id")]
    [Domain(Domains.Id)]
    public int? TableId { get; set; }

    /// <summary>
    /// Réservation associée à la commande.
    /// </summary>
    [Column("rev_id")]
    [Domain(Domains.Id)]
    public int? ReservationId { get; set; }

    /// <summary>
    /// Statut de la commande.
    /// </summary>
    [Column("stc_code")]
    [Required]
    [ReferencedType(typeof(StatutCommande))]
    [Domain(Domains.Code)]
    public StatutCommande.Codes? StatutCommandeCode { get; set; } = StatutCommande.Codes.EN_ATT;

    /// <summary>
    /// Association réciproque de LigneCommande.CommandeId.
    /// </summary>
    [Domain(Domains.Liste)]
    [NotMapped]
    public ICollection<int> Lignes { get; set; }
}
