////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Common;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail complet d'une commande avec ses lignes.
/// </summary>
public partial record CommandeDetailRead
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
    /// Client ayant passé la commande.
    /// </summary>
    [Required]
    [Domain(Domains.Id)]
    public int? ClientId { get; set; }

    /// <summary>
    /// Table associée à la commande.
    /// </summary>
    [Domain(Domains.Id)]
    public int? TableClientId { get; set; }

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

    /// <summary>
    /// Liste des lignes de commande.
    /// </summary>
    [Required]
    public ICollection<LigneCommandeItem> Lignes { get; set; } = new List<LigneCommandeItem>();
}
