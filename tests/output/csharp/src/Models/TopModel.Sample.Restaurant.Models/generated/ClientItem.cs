////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'un client en liste.
/// </summary>
public partial record ClientItem : PersonneItem
{
    /// <summary>
    /// Nom complet du client (calculé).
    /// </summary>
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string? NomComplet { get; init; }
}
