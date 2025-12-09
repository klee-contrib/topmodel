////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using Kinetix.Services.Annotations;
using TopModel.Sample.Restaurant.Models;

namespace TopModel.Sample.Clients.Db.Reference;

/// <summary>
/// Accesseurs de listes de référence persistées.
/// </summary>
[RegisterContract]
public partial interface IDbRestaurantReferenceAccessors
{
    /// <summary>
    /// Accesseur de référence pour le type CategoriePlat.
    /// </summary>
    /// <returns>Liste de CategoriePlat.</returns>
    [ReferenceAccessor]
    ICollection<CategoriePlat> LoadCategoriePlats();

    /// <summary>
    /// Accesseur de référence pour le type StatutCommande.
    /// </summary>
    /// <returns>Liste de StatutCommande.</returns>
    [ReferenceAccessor]
    ICollection<StatutCommande> LoadStatutCommandes();
}
