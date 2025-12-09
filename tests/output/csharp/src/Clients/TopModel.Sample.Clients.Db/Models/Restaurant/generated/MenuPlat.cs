////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Common;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Plat dans un menu.
/// </summary>
[Table("menu_plat")]
public partial record MenuPlat
{
    /// <summary>
    /// Identifiant de la relation.
    /// </summary>
    [Column("mpl_id")]
    [Domain(Domains.Id)]
    [Key]
    public int? Id { get; set; }

    /// <summary>
    /// Ordre d'affichage du plat dans le menu.
    /// </summary>
    [Column("mpl_ordre")]
    [Required]
    [Domain(Domains.Quantite)]
    public int? Ordre { get; set; }

    /// <summary>
    /// Menu contenant ce plat.
    /// </summary>
    [Column("men_id_menu")]
    [Required]
    [Domain(Domains.Id)]
    public int? MenuIdMenu { get; set; }

    /// <summary>
    /// Plat du menu.
    /// </summary>
    [Column("pla_id_plat")]
    [Required]
    [Domain(Domains.Id)]
    public int? PlatIdPlat { get; set; }
}
