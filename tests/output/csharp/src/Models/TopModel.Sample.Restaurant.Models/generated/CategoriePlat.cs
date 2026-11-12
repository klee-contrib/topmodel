////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Catégorie de plat.
/// </summary>
[Reference]
[DefaultProperty(nameof(Libelle))]
[Table("categorie_plat")]
public partial record CategoriePlat
{
    /// <summary>
    /// Code de la catégorie.
    /// </summary>
    [Column("cat_code")]
    [Key]
    public CategoriePlatValues.Codes? Code { get; init; }

    /// <summary>
    /// Libellé de la catégorie.
    /// </summary>
    [Column("cat_libelle")]
    [Required]
    [StringLength(100)]
    public string? Libelle { get; init; }

    /// <summary>
    /// Ordre d'affichage dans le menu.
    /// </summary>
    [Column("cat_ordre")]
    [Required]
    public int? Ordre { get; init; }

    /// <summary>
    /// Prix moyen de la catégorie, à titre indicatif.
    /// </summary>
    [Column("cat_prix_moyen")]
    public decimal? PrixMoyen { get; init; }

    /// <summary>
    /// Convertit une valeur métier en entité de persistance.
    /// </summary>
    /// <param name="value">Valeur métier.</param>
    /// <returns>Entité de persistance.</returns>
    public static CategoriePlat FromValue(CategoriePlatValues value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return new CategoriePlat
        {
            Code = value.Code,
            Libelle = value.Libelle,
            Ordre = value.Ordre,
            PrixMoyen = value.PrixMoyen,
        };
    }
}
