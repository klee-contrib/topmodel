////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CSharp.Common;
using Kinetix.Modeling.Annotations;

namespace Models.CSharp.Securite.Models;

/// <summary>
/// Objet métier non persisté représentant Profil.
/// </summary>
public partial class SecteurDto
{
    /// <summary>
    /// Id technique.
    /// </summary>
    [Column("sec_id")]
    [Required]
    [Domain(Domains.Id)]
    public int? Id { get; set; }
}
