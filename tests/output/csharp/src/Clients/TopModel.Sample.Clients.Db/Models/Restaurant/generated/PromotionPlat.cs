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
    /// Promotion concernée.
    /// </summary>
    [Column("pro_id")]
    [Required]
    [Domain(Domains.Id)]
    public int? PromotionId { get; set; }

    /// <summary>
    /// Plat concerné par la promotion.
    /// </summary>
    [Column("pla_id")]
    [Required]
    [Domain(Domains.Id)]
    public int? PlatId { get; set; }
}
