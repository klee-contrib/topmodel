////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'un client en liste.
/// </summary>
public partial record ClientItem : PersonneItem
{
    /// <summary>
    /// Nom complet du client (calculé).
    /// </summary>
    [StringLength(100)]
    public string? NomComplet { get; init; }
}
