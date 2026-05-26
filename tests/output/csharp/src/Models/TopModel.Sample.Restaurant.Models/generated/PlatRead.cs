////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'un plat en lecture.
/// </summary>
public partial record PlatRead
{
    /// <summary>
    /// Identifiant du plat.
    /// </summary>
    [Required]
    [Domain(Domains.Id)]
    public int? Id { get; set; }

    /// <summary>
    /// Nom du plat.
    /// </summary>
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string? Nom { get; set; }

    /// <summary>
    /// Description du plat.
    /// </summary>
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string? Description { get; set; }

    /// <summary>
    /// Prix du plat.
    /// </summary>
    [Required]
    [Domain(Domains.Prix)]
    public decimal? Prix { get; set; }

    /// <summary>
    /// Indique si le plat est disponible.
    /// </summary>
    [Required]
    [Domain(Domains.Booleen)]
    public bool? Disponible { get; set; } = true;

    /// <summary>
    /// Catégorie du plat.
    /// </summary>
    [Required]
    [ReferencedType(typeof(CategoriePlat))]
    [Domain(Domains.Code)]
    public CategoriePlat.Codes? CategoriePlatCode { get; set; }

    /// <summary>
    /// Restaurant proposant ce plat.
    /// </summary>
    [Required]
    [Domain(Domains.Id)]
    public int? RestaurantId { get; set; }

    /// <summary>
    /// Date de création de l'enregistrement.
    /// </summary>
    [Required]
    [Domain(Domains.DateHeure)]
    public DateTime? DateCreation { get; set; }
}
