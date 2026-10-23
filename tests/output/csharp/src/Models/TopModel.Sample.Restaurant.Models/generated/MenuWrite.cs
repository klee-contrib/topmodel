////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'un menu en écriture.
/// </summary>
public partial record MenuWrite
{
    /// <summary>
    /// Nom du menu.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string? Nom { get; set; }

    /// <summary>
    /// Description du menu.
    /// </summary>
    [StringLength(100)]
    public string? Description { get; set; }

    /// <summary>
    /// Prix du menu.
    /// </summary>
    [Required]
    public decimal? Prix { get; set; }

    /// <summary>
    /// Indique si le menu est disponible.
    /// </summary>
    [Required]
    public bool? Disponible { get; set; } = true;

    /// <summary>
    /// Date de début de validité du menu.
    /// </summary>
    public DateTime? DateDebut { get; set; }

    /// <summary>
    /// Date de fin de validité du menu.
    /// </summary>
    public DateTime? DateFin { get; set; }

    /// <summary>
    /// Restaurant proposant ce menu.
    /// </summary>
    [Required]
    public int? RestaurantId { get; set; }

    /// <summary>
    /// Catégories de plat disponibles dans le menu.
    /// </summary>
    [Required]
    public ICollection<CategoriePlat.Codes>? CategoriesPlat { get; set; }
}
