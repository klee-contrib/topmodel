////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TopModel.Sample.Restaurant.Models;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Plat du menu.
/// </summary>
[Table("plat")]
public partial record Plat
{
    /// <summary>
    /// Identifiant du plat.
    /// </summary>
    [Column("pla_id")]
    [Key]
    public int? Id { get; set; }

    /// <summary>
    /// Nom du plat.
    /// </summary>
    [Column("pla_nom")]
    [Required]
    [StringLength(100)]
    public string? Nom { get; set; }

    /// <summary>
    /// Description du plat.
    /// </summary>
    [Column("pla_description")]
    [StringLength(100)]
    public string? Description { get; set; }

    /// <summary>
    /// Prix du plat.
    /// </summary>
    [Column("pla_prix")]
    [Required]
    public decimal? Prix { get; set; }

    /// <summary>
    /// Indique si le plat est disponible.
    /// </summary>
    [Column("pla_disponible")]
    [Required]
    public bool? Disponible { get; set; } = true;

    /// <summary>
    /// Catégorie du plat.
    /// </summary>
    public virtual CategoriePlat CategoriePlat { get; init; } = CategoriePlat.Autre;

    /// <summary>
    /// Restaurant proposant ce plat.
    /// </summary>
    [Required]
    public Restaurant? Restaurant { get; set; }

    /// <summary>
    /// Restaurant qui a créé ce plat.
    /// </summary>
    [Required]
    public Restaurant? RestaurantCreateur { get; set; }

    /// <summary>
    /// Association réciproque de Promotion.Plat.
    /// </summary>
    public Promotion? Promotion { get; set; }

    /// <summary>
    /// Date de création de l'enregistrement.
    /// </summary>
    [Column("pla_date_creation")]
    [Required]
    public DateTime? DateCreation { get; init; }
}
