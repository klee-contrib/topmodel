////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'une commande en lecture.
/// </summary>
public partial record CommandeRead
{
    /// <summary>
    /// Identifiant de la commande.
    /// </summary>
    [Required]
    [Domain(Domains.Id)]
    public int? Id { get; set; }

    /// <summary>
    /// Date et heure de la commande.
    /// </summary>
    [Required]
    [Domain(Domains.DateHeure)]
    public DateTime? DateCommande { get; set; }

    /// <summary>
    /// Date et heure de livraison.
    /// </summary>
    [Domain(Domains.DateHeure)]
    public DateTime? DateLivraison { get; set; }

    /// <summary>
    /// Montant total de la commande.
    /// </summary>
    [Required]
    [Domain(Domains.Prix)]
    public decimal? MontantTotal { get; set; }

    /// <summary>
    /// Table associée à la commande.
    /// </summary>
    [Domain(Domains.Id)]
    public int? TableId { get; set; }

    /// <summary>
    /// Statut de la commande.
    /// </summary>
    [Required]
    [ReferencedType(typeof(StatutCommande))]
    [Domain(Domains.Code)]
    public StatutCommande.Codes? StatutCommandeCode { get; set; } = StatutCommande.Codes.EN_ATT;

    /// <summary>
    /// Avis laissé par le client sur la commande.
    /// </summary>
    [Domain(Domains.Id)]
    public int? AvisClientId { get; set; }

    /// <summary>
    /// Client ayant passé la commande.
    /// </summary>
    [Required]
    public ClientRead Client { get; set; } = new();

    /// <summary>
    /// Réservation.
    /// </summary>
    public ReservationRead? Reservation { get; set; }

    /// <summary>
    /// Association réciproque de LigneCommande.Commande.
    /// </summary>
    [Required]
    public ICollection<LigneCommandeRead> Lignes { get; set; } = [];
}
