////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'une facture.
/// </summary>
public partial record FactureItem
{
    /// <summary>
    /// Identifiant de la facture.
    /// </summary>
    [Required]
    public int? Id { get; init; }

    /// <summary>
    /// Commande associée à la facture.
    /// </summary>
    [Required]
    public int? CommandeId { get; init; }

    /// <summary>
    /// type de facture.
    /// </summary>
    public TypeFacture.Codes? TypeFactureCode { get; init; }
}
