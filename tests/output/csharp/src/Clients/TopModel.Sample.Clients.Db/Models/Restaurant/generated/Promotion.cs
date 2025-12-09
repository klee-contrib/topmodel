////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Common;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Promotion sur les plats.
/// </summary>
[Table("promotion")]
public partial record Promotion
{
    /// <summary>
    /// Identifiant de la promotion.
    /// </summary>
    [Column("pro_id")]
    [Domain(Domains.Id)]
    [Key]
    public int? Id { get; set; }

    /// <summary>
    /// Libellé de la promotion.
    /// </summary>
    [Column("pro_libelle")]
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string Libelle { get; set; }

    /// <summary>
    /// Pourcentage de réduction (0-100).
    /// </summary>
    [Column("pro_pourcentage_reduction")]
    [Required]
    [Domain(Domains.Quantite)]
    public int? PourcentageReduction { get; set; }

    /// <summary>
    /// Date de début de la promotion.
    /// </summary>
    [Column("pro_date_debut")]
    [Required]
    [Domain(Domains.DateHeure)]
    public DateTime? DateDebut { get; set; }

    /// <summary>
    /// Date de fin de la promotion.
    /// </summary>
    [Column("pro_date_fin")]
    [Required]
    [Domain(Domains.DateHeure)]
    public DateTime? DateFin { get; set; }

    /// <summary>
    /// Indique si la promotion est active.
    /// </summary>
    [Column("pro_active")]
    [Required]
    [Domain(Domains.Booleen)]
    public bool? Active { get; set; } = true;

    /// <summary>
    /// Restaurant concerné par la promotion (null si globale).
    /// </summary>
    [Column("res_id_restaurant")]
    [Domain(Domains.Id)]
    public int? RestaurantIdRestaurant { get; set; }

    /// <summary>
    /// Association réciproque de PromotionPlat.PromotionIdPromotion.
    /// </summary>
    [Domain(Domains.Liste)]
    [NotMapped]
    public ICollection<int> PromotionPlatsPromotion { get; set; }
}
