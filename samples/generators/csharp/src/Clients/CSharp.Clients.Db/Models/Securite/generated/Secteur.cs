////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CSharp.Common;
using Kinetix.Modeling.Annotations;

namespace CSharp.Clients.Db.Models.Securite;

/// <summary>
/// Secteur d'application du profil.
/// </summary>
[Table("secteur")]
public partial class Secteur
{
    /// <summary>
    /// Id technique.
    /// </summary>
    [Column("sec_id")]
    [Domain(Domains.Id)]
    [Key]
    public int? Id { get; set; }
}
