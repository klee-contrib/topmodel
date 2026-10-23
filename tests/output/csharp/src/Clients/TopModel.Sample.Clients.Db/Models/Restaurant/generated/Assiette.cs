////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Assiette.
/// </summary>
[Table("assiette")]
public partial record Assiette : Vaisselle
{
    /// <summary>
    /// Taille de l'assiette.
    /// </summary>
    [Column("ast_taille")]
    [Required]
    public int? Taille { get; set; }
}
