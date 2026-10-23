////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TopModel.Sample.Restaurant.Models;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Plat principal.
/// </summary>
public partial record PlatPrincipal : Plat
{
    /// <inheritdoc />
    public override CategoriePlat CategoriePlat { get; init; } = CategoriePlat.Principal;

    /// <summary>
    /// Si le plat est végétarien.
    /// </summary>
    [Column("ppr_vegetarien")]
    [Required]
    public bool? Vegetarien { get; set; }
}
