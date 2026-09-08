////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'un paiement.
/// </summary>
public partial record PaiementItem
{
    /// <summary>
    /// Facture associée au paiement.
    /// </summary>
    [Required]
    [Domain(Domains.Id)]
    public int? FactureId { get; init; }

    /// <summary>
    /// Carte Swile utilisée pour le paiement.
    /// </summary>
    [Required]
    [Domain(Domains.Id)]
    public int? SwileCardId { get; init; }

    /// <summary>
    /// Date et heure de la commande.
    /// </summary>
    [Required]
    [Domain(Domains.DateHeure)]
    public DateTime? DateCommande { get; init; }

    /// <summary>
    /// Montant total de la commande.
    /// </summary>
    [Required]
    [Domain(Domains.Prix)]
    public decimal? MontantTotal { get; init; }
}
