////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Common;
using TopModel.Sample.Securite.Models.Profil;

namespace TopModel.Sample.Clients.Db.Models.Securite.Profil;

/// <summary>
/// Association N-N Profils &lt;&gt; Droits.
/// </summary>
[Table("profil_droit")]
public partial record ProfilDroit
{
    /// <summary>
    /// Profil.
    /// </summary>
    [Column("pro_id")]
    [Required]
    [Domain(Domains.Id)]
    public int? ProfilId { get; set; }

    /// <summary>
    /// Droit.
    /// </summary>
    [Column("dro_code")]
    [Required]
    [ReferencedType(typeof(Droit))]
    [Domain(Domains.Code)]
    public Droit.Codes? DroitCode { get; set; }
}
