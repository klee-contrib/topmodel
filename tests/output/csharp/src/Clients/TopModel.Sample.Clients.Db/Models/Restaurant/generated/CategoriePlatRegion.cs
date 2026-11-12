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
    /// Région.
    /// </summary>
    [Column("reg_code")]
    [Required]
    [ReferencedType(typeof(RegionValues))]
    public RegionValues.Codes? RegionCode { get; init; }

    /// <summary>
    /// Catégorie de plat.
    /// </summary>
    [Required]
    public CategoriePlat? CategoriePlat { get; init; }

    /// <summary>
    /// Convertit une valeur métier en entité de persistance.
    /// </summary>
    /// <param name="value">Valeur métier.</param>
    /// <returns>Entité de persistance.</returns>
    public static CategoriePlatRegion FromValue(CategoriePlatRegionValues value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return new CategoriePlatRegion
        {
            RegionCode = value.RegionCode,
            CategoriePlat = value.CategoriePlat is null ? null : CategoriePlat.FromValue(value.CategoriePlat),
        };
    }
}
