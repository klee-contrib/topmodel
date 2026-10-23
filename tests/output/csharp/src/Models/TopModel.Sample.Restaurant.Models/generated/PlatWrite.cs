////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'un plat en écriture.
/// </summary>
public partial record PlatWrite
{
    /// <summary>
    /// Nom du plat.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string? Nom { get; set; }

    /// <summary>
    /// Description du plat.
    /// </summary>
    [StringLength(100)]
    public string? Description { get; set; }

    /// <summary>
    /// Prix du plat.
    /// </summary>
    [Required]
    public decimal? Prix { get; set; }

    /// <summary>
    /// Indique si le plat est disponible.
    /// </summary>
    [Required]
    public bool? Disponible { get; set; } = true;

    /// <summary>
    /// Catégorie du plat.
    /// </summary>
    [Required]
    [ReferencedType(typeof(CategoriePlat))]
    public CategoriePlat.Codes? CategoriePlatCode { get; set; }

    /// <summary>
    /// Restaurant proposant ce plat.
    /// </summary>
    [Required]
    public int? RestaurantId { get; set; }

    /// <summary>
    /// Restaurant qui a créé ce plat.
    /// </summary>
    [Required]
    public int? RestaurantIdCreateur { get; set; }

    /// <summary>
    /// Date de création de l'enregistrement.
    /// </summary>
    [Required]
    public DateTime? DateCreation { get; set; }
}
