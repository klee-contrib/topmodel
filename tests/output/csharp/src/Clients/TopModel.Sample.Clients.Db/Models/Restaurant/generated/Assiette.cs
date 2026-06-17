////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Restaurant.Models;

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
    [Domain(Domains.Quantite)]
    public int? Taille { get; set; }
}
