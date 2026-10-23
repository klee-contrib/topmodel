////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Promotion sur un plat.
/// </summary>
[Table("promotion")]
public partial record Promotion
{
    /// <summary>
    /// Plat concerné par la promotion.
    /// </summary>
    public Plat? Plat { get; set; }

    /// <summary>
    /// Libellé de la promotion.
    /// </summary>
    [Column("pro_libelle")]
    [Required]
    [StringLength(100)]
    public string? Libelle { get; set; }

    /// <summary>
    /// Pourcentage de réduction (0-100).
    /// </summary>
    [Column("pro_pourcentage_reduction")]
    [Required]
    public int? PourcentageReduction { get; set; }

    /// <summary>
    /// Date de début de la promotion.
    /// </summary>
    [Column("pro_date_debut")]
    [Required]
    public DateTime? DateDebut { get; set; }

    /// <summary>
    /// Date de fin de la promotion.
    /// </summary>
    [Column("pro_date_fin")]
    [Required]
    public DateTime? DateFin { get; set; }

    /// <summary>
    /// Indique si la promotion est active.
    /// </summary>
    [Column("pro_active")]
    [Required]
    public bool? Active { get; set; } = true;

    /// <summary>
    /// Restaurant concerné par la promotion (null si globale).
    /// </summary>
    public Restaurant? Restaurant { get; set; }

    /// <summary>
    /// Date de création de l'enregistrement.
    /// </summary>
    [Column("pro_date_creation")]
    [Required]
    public DateTime? DateCreation { get; init; }
}
