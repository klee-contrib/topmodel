////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TopModel.Sample.Restaurant.Models;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Commande pour historique avec préservation des clés primaires.
/// </summary>
[Table("commande_historique")]
public partial record CommandeHistorique
{
    /// <summary>
    /// Identifiant de la commande.
    /// </summary>
    [Column("com_id")]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int? Id { get; set; }

    /// <summary>
    /// Date et heure de la commande.
    /// </summary>
    [Column("com_date_commande")]
    [Required]
    public DateTime? DateCommande { get; set; }

    /// <summary>
    /// Date et heure de livraison.
    /// </summary>
    [Column("com_date_livraison")]
    public DateTime? DateLivraison { get; set; }

    /// <summary>
    /// Montant total de la commande.
    /// </summary>
    [Column("com_montant_total")]
    [Required]
    public decimal? MontantTotal { get; set; }

    /// <summary>
    /// Client ayant passé la commande.
    /// </summary>
    [Column("per_id")]
    [Required]
    public int? ClientId { get; set; }

    /// <summary>
    /// Table associée à la commande.
    /// </summary>
    [Column("tab_id")]
    public int? TableId { get; set; }

    /// <summary>
    /// Réservation associée à la commande.
    /// </summary>
    [Column("rev_id")]
    public int? ReservationId { get; set; }

    /// <summary>
    /// Statut de la commande.
    /// </summary>
    [Required]
    public StatutCommande? StatutCommande { get; set; } = Sample.Restaurant.Models.StatutCommande.EN_ATT;

    /// <summary>
    /// Avis laissé par le client sur la commande.
    /// </summary>
    [Column("avi_id")]
    public int? AvisClientId { get; set; }

    /// <summary>
    /// Date de création de l'enregistrement.
    /// </summary>
    [Column("com_date_creation")]
    [Required]
    public DateTime? DateCreation { get; set; }
}
