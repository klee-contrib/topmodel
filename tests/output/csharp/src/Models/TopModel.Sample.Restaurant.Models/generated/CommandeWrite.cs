////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Common;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'une commande en écriture.
/// </summary>
public partial record CommandeWrite
{
    /// <summary>
    /// Date et heure de livraison.
    /// </summary>
    [Domain(Domains.DateHeure)]
    public DateTime? DateLivraison { get; set; }

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
    [Domain(Domains.Liste)]
    public ICollection<int> LigneCommandes { get; set; }
}
