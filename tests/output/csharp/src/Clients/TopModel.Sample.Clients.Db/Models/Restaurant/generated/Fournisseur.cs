////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Restaurant.
/// </summary>
public partial record Fournisseur : Lieu
{
    /// <summary>
    /// Numéro de téléphone.
    /// </summary>
    [Column("frn_telephone")]
    [StringLength(20)]
    public string? Telephone { get; set; } = "xx.xx.xx.xx.xx";

    /// <summary>
    /// Si le fournisseur fait du bio.
    /// </summary>
    [Column("frn_bio")]
    public bool? Bio { get; set; }
}
