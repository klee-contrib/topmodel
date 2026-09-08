////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;

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
    [Domain(Domains.Id)]
    public int? Id { get; init; }

    /// <summary>
    /// Commande associée à la facture.
    /// </summary>
    [Required]
    [Domain(Domains.Id2)]
    public int? CommandeId { get; init; }
}
