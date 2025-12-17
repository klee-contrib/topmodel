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
}
