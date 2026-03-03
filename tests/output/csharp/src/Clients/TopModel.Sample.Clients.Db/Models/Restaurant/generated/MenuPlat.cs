////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Restaurant.Models;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Plat dans un menu.
/// </summary>
[Table("menu_plat")]
public partial record MenuPlat
{
    /// <summary>
    /// Menu contenant ce plat.
    /// </summary>
    [Required]
    public Menu? Menu { get; set; }

    /// <summary>
    /// Plat du menu.
    /// </summary>
    [Required]
    public Plat? Plat { get; set; }

    /// <summary>
    /// Ordre d'affichage du plat dans le menu.
    /// </summary>
    [Column("mpl_ordre")]
    [Required]
    [Domain(Domains.Quantite)]
    public int? Ordre { get; set; }
}
