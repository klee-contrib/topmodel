////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Catégories de plats disponibles par région.
/// </summary>
public partial record CategoriePlatRegionValues
{
    /// <summary>
    /// IdfDessert.
    /// </summary>
    public static CategoriePlatRegionValues IdfDessert { get; } = new() { RegionCode = RegionValues.Codes.IDF, CategoriePlat = CategoriePlatValues.Dessert };

    /// <summary>
    /// IdfEntree.
    /// </summary>
    public static CategoriePlatRegionValues IdfEntree { get; } = new() { RegionCode = RegionValues.Codes.IDF, CategoriePlat = CategoriePlatValues.Entree };

    /// <summary>
    /// Liste des valeurs.
    /// </summary>
    public static IList<CategoriePlatRegionValues> Values { get; } = [IdfEntree, IdfDessert];

    /// <summary>
    /// Région.
    /// </summary>
    public RegionValues.Codes? RegionCode { get; init; }

    /// <summary>
    /// Catégorie de plat.
    /// </summary>
    public CategoriePlatValues? CategoriePlat { get; init; }

}
