////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;
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
    [Domain(Domains.Id)]
    [Key]
    public int? Id { get; set; }

    /// <summary>
    /// Nom du plat.
    /// </summary>
    [Column("pla_nom")]
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string? Nom { get; set; }

    /// <summary>
    /// Description du plat.
    /// </summary>
    [Column("pla_description")]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string? Description { get; set; }

    /// <summary>
    /// Prix du plat.
    /// </summary>
    [Column("pla_prix")]
    [Required]
    [Domain(Domains.Prix)]
    public decimal? Prix { get; set; }

    /// <summary>
    /// Indique si le plat est disponible.
    /// </summary>
    [Column("pla_disponible")]
    [Required]
    [Domain(Domains.Booleen)]
    public bool? Disponible { get; set; } = true;

    /// <summary>
    /// Catégorie du plat.
    /// </summary>
    [Column("cat_code")]
    [Required]
    [ReferencedType(typeof(CategoriePlat))]
    [Domain(Domains.Code)]
    public CategoriePlat.Codes? CategoriePlatCode { get; set; }

    /// <summary>
    /// Restaurant proposant ce plat.
    /// </summary>
    [Column("res_id")]
    [Required]
    [Domain(Domains.Id)]
    public int? RestaurantId { get; set; }

    /// <summary>
    /// Association réciproque de PromotionPlat.PlatId.
    /// </summary>
    [Domain(Domains.Liste)]
    [NotMapped]
    public ICollection<int>? Promotions { get; set; }
}
