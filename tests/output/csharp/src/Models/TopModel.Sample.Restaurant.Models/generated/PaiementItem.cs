////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;

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
    public int? FactureId { get; init; }

    /// <summary>
    /// Carte Swile utilisée pour le paiement.
    /// </summary>
    [Required]
    public int? SwileCardId { get; init; }

    /// <summary>
    /// Date et heure de la commande.
    /// </summary>
    [Required]
    public DateTime? DateCommande { get; init; }

    /// <summary>
    /// Montant total de la commande.
    /// </summary>
    [Required]
    public decimal? MontantTotal { get; init; }
}
