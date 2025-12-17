////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'une commande en écriture.
/// </summary>
public partial record CommandeWrite
{
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
    /// Client ayant passé la commande.
    /// </summary>
    [Required]
    [Domain(Domains.Id)]
    public int? ClientId { get; set; }

    /// <summary>
    /// Table associée à la commande.
    /// </summary>
    [Domain(Domains.Id)]
    public int? TableId { get; set; }

    /// <summary>
    /// Réservation associée à la commande.
    /// </summary>
    [Domain(Domains.Id)]
    public int? ReservationId { get; set; }

    /// <summary>
    /// Statut de la commande.
    /// </summary>
    [Required]
    [ReferencedType(typeof(StatutCommande))]
    [Domain(Domains.Code)]
    public StatutCommande.Codes? StatutCommandeCode { get; set; } = StatutCommande.Codes.EN_ATT;

    /// <summary>
    /// Association réciproque de LigneCommande.CommandeId.
    /// </summary>
    [Required]
    public ICollection<LigneCommandeWrite> Lignes { get; set; } = [];
}
