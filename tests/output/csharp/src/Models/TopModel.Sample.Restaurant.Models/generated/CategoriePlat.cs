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
[Reference(true)]
[DefaultProperty(nameof(Libelle))]
[Table("categorie_plat")]
public partial record CategoriePlat
{
    /// <summary>
    /// Boisson.
    /// </summary>
    public const int BoissonOrdre = 1;

    /// <summary>
    /// Dessert.
    /// </summary>
    public const int DessertOrdre = 4;

    /// <summary>
    /// Entrée.
    /// </summary>
    public const int EntreeOrdre = 2;

    /// <summary>
    /// Plat principal.
    /// </summary>
    public const int PlatOrdre = 3;

    /// <summary>
    /// Valeurs possibles de la liste de référence CategoriePlat.
    /// </summary>
    public enum Codes
    {
        /// <summary>
        /// Boisson.
        /// </summary>
        BOISSON,

        /// <summary>
        /// Dessert.
        /// </summary>
        DESSERT,

        /// <summary>
        /// Entrée.
        /// </summary>
        ENTREE,

        /// <summary>
        /// Plat principal.
        /// </summary>
        PLAT
    }

    /// <summary>
    /// Code de la catégorie.
    /// </summary>
    [Column("cat_code")]
    [Domain(Domains.Code)]
    [Key]
    public Codes? Code { get; set; }

    /// <summary>
    /// Libellé de la catégorie.
    /// </summary>
    [Column("cat_libelle")]
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string? Libelle { get; set; }

    /// <summary>
    /// Ordre d'affichage dans le menu.
    /// </summary>
    [Column("cat_ordre")]
    [Required]
    [Domain(Domains.Quantite)]
    public int? Ordre { get; set; }
}
