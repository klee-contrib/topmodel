////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Catégories de plats disponibles par région.
/// </summary>
public partial record CategoriePlatRegion
{
    /// <summary>
    /// IdfDessert.
    /// </summary>
    public static CategoriePlatRegion IdfDessert { get; } = new() { RegionCode = Region.Codes.IDF, CategoriePlatCode = CategoriePlat.Codes.DESSERT };

    /// <summary>
    /// IdfEntree.
    /// </summary>
    public static CategoriePlatRegion IdfEntree { get; } = new() { RegionCode = Region.Codes.IDF, CategoriePlatCode = CategoriePlat.Codes.ENTREE };

    /// <summary>
    /// Liste des valeurs.
    /// </summary>
    public static IList<CategoriePlatRegion> Values { get; } = [IdfEntree, IdfDessert];

    /// <summary>
    /// Région.
    /// </summary>
    [Required]
    [ReferencedType(typeof(Region))]
    [Domain(Domains.Code)]
    public Region.Codes? RegionCode { get; init; }

    /// <summary>
    /// Catégorie de plat.
    /// </summary>
    [Required]
    [ReferencedType(typeof(CategoriePlat))]
    [Domain(Domains.Code)]
    public CategoriePlat.Codes? CategoriePlatCode { get; init; }
}
