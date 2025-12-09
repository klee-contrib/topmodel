////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using Kinetix.Services.Annotations;
using TopModel.Sample.Restaurant.Models;

namespace TopModel.Sample.Clients.Db.Reference;

/// <summary>
/// Implémentation de IDbRestaurantReferenceAccessors.
/// </summary>
/// <param name="dbContext">DbContext.</param>
[RegisterImpl]
public partial class DbRestaurantReferenceAccessors(TopModelSampleDbContext dbContext) : IDbRestaurantReferenceAccessors
{
    /// <inheritdoc cref="IDbRestaurantReferenceAccessors.LoadCategoriePlats" />
    public ICollection<CategoriePlat> LoadCategoriePlats()
    {
        return dbContext.CategoriePlats.OrderBy(row => row.Libelle).ToList();
    }

    /// <inheritdoc cref="IDbRestaurantReferenceAccessors.LoadStatutCommandes" />
    public ICollection<StatutCommande> LoadStatutCommandes()
    {
        return dbContext.StatutCommandes.OrderBy(row => row.Libelle).ToList();
    }
}
