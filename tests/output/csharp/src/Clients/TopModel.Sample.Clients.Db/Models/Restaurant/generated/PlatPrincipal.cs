////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Restaurant.Models;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Plat principal.
/// </summary>
public partial record PlatPrincipal : Plat
{
    /// <summary>
    /// Si le plat est végétarien.
    /// </summary>
    [Column("ppr_vegetarien")]
    [Required]
    [Domain(Domains.Booleen)]
    public bool? Vegetarien { get; set; }
}
