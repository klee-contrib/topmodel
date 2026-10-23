////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Verre.
/// </summary>
[Table("verre")]
public partial record Verre : Vaisselle
{
    /// <summary>
    /// Si le verre est à pied ou non.
    /// </summary>
    [Column("vrr_a_pied")]
    [Required]
    public bool? APied { get; set; }
}
