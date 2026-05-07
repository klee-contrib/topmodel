////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Restaurant.Models;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Catégories de plats disponibles par région.
/// </summary>
[Table("categorie_plat_region")]
public partial record CategoriePlatRegion
{
    /// <summary>
    /// IdfDessert.
    /// </summary>
    public static CategoriePlatRegion IdfDessert { get; } = new() { RegionCode = Region.Codes.IDF, CategoriePlat = CategoriePlat.Dessert };

    /// <summary>
    /// IdfEntree.
    /// </summary>
    public static CategoriePlatRegion IdfEntree { get; } = new() { RegionCode = Region.Codes.IDF, CategoriePlat = CategoriePlat.Entree };

    /// <summary>
    /// Liste des valeurs.
    /// </summary>
    public static IList<CategoriePlatRegion> Values { get; } = [IdfEntree, IdfDessert];

    /// <summary>
    /// Région.
    /// </summary>
    [Column("reg_code")]
    [Required]
    [ReferencedType(typeof(Region))]
    [Domain(Domains.Code)]
    public Region.Codes? RegionCode { get; init; }

    /// <summary>
    /// Catégorie de plat.
    /// </summary>
    [Required]
    public CategoriePlat? CategoriePlat { get; init; }
}
