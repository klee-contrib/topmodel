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
        return (
            from row in dbContext.CategoriePlats
            join tra in dbContext.Translations on row.Libelle equals tra.ResourceKey
            orderby row.Ordre
            select new CategoriePlat
            {
                Code = row.Code,
                Libelle = tra.Value,
                Ordre = row.Ordre,
                PrixMoyen = row.PrixMoyen
            }
        ).ToList();
    }

    /// <inheritdoc cref="IDbRestaurantReferenceAccessors.LoadDepartements" />
    public ICollection<Departement> LoadDepartements()
    {
        return (
            from row in dbContext.Departements
            join tra in dbContext.Translations on row.Libelle equals tra.ResourceKey
            orderby row.Libelle
            select new Departement
            {
                Code = row.Code,
                Libelle = tra.Value,
                RegionCode = row.RegionCode
            }
        ).ToList();
    }

    /// <inheritdoc cref="IDbRestaurantReferenceAccessors.LoadRegions" />
    public ICollection<Region> LoadRegions()
    {
        return (
            from row in dbContext.Regions
            join tra in dbContext.Translations on row.Libelle equals tra.ResourceKey
            orderby row.Libelle
            select new Region
            {
                Code = row.Code,
                Libelle = tra.Value,
                NomResponsable = row.NomResponsable
            }
        ).ToList();
    }
}
