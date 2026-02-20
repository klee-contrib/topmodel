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
        return [CategoriePlat.Entree, CategoriePlat.Plat, CategoriePlat.Dessert, CategoriePlat.Boisson];
    }

    /// <inheritdoc cref="IDbRestaurantReferenceAccessors.LoadDepartements" />
    public ICollection<Departement> LoadDepartements()
    {
        return [Departement.Paris, Departement.HautsDeSeine, Departement.SeineSaintDenis, Departement.SeineEtMarne];
    }

    /// <inheritdoc cref="IDbRestaurantReferenceAccessors.LoadRegions" />
    public ICollection<Region> LoadRegions()
    {
        return dbContext.Regions.OrderBy(row => row.Libelle).ToList();
    }
}
