////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Common;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Association entre une promotion et un plat.
/// </summary>
[Table("promotion_plat")]
public partial record PromotionPlat
{
    /// <summary>
    /// Identifiant de l'association.
    /// </summary>
    [Column("ppl_id")]
    [Domain(Domains.Id)]
    [Key]
    public int? Id { get; set; }

    /// <summary>
    /// Promotion concernée.
    /// </summary>
    [Column("pro_id_promotion")]
    [Required]
    [Domain(Domains.Id)]
    public int? PromotionIdPromotion { get; set; }

    /// <summary>
    /// Plat concerné par la promotion.
    /// </summary>
    [Column("pla_id_plat")]
    [Required]
    [Domain(Domains.Id)]
    public int? PlatIdPlat { get; set; }
}
