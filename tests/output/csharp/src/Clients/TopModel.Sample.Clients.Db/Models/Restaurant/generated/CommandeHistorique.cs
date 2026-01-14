////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;
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
    [Domain(Domains.Id)]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
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
    [Required]
    public Client? Client { get; set; }

    /// <summary>
    /// Table associée à la commande.
    /// </summary>
    public TableRestaurant? Table { get; set; }

    /// <summary>
    /// Réservation associée à la commande.
    /// </summary>
    public Reservation? Reservation { get; set; }

    /// <summary>
    /// Statut de la commande.
    /// </summary>
    [Required]
    [ReferencedType(typeof(StatutCommande))]
    public StatutCommande? StatutCommande { get; set; }

    /// <summary>
    /// Avis laissé par le client sur la commande.
    /// </summary>
    public AvisClient? AvisClient { get; set; }

    /// <summary>
    /// Association réciproque de LigneCommandeHistorique.CommandeHistorique.
    /// </summary>
    public ICollection<LigneCommandeHistorique> Lignes { get; set; } = [];
}
