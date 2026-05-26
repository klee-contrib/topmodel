////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'une promotion en lecture.
/// </summary>
public partial record PromotionRead
{
    /// <summary>
    /// Plat concerné par la promotion.
    /// </summary>
    [Required]
    [Domain(Domains.Id)]
    public int? PlatId { get; set; }

    /// <summary>
    /// Libellé de la promotion.
    /// </summary>
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string? Libelle { get; set; }

    /// <summary>
    /// Pourcentage de réduction (0-100).
    /// </summary>
    [Required]
    [Domain(Domains.Quantite)]
    public int? PourcentageReduction { get; set; }

    /// <summary>
    /// Date de début de la promotion.
    /// </summary>
    [Required]
    [Domain(Domains.DateHeure)]
    public DateTime? DateDebut { get; set; }

    /// <summary>
    /// Date de fin de la promotion.
    /// </summary>
    [Required]
    [Domain(Domains.DateHeure)]
    public DateTime? DateFin { get; set; }

    /// <summary>
    /// Indique si la promotion est active.
    /// </summary>
    [Required]
    [Domain(Domains.Booleen)]
    public bool? Active { get; set; } = true;

    /// <summary>
    /// Restaurant concerné par la promotion (null si globale).
    /// </summary>
    [Domain(Domains.Id)]
    public int? RestaurantId { get; set; }

    /// <summary>
    /// Date de création de l'enregistrement.
    /// </summary>
    [Required]
    [Domain(Domains.DateHeure)]
    public DateTime? DateCreation { get; set; }
}
