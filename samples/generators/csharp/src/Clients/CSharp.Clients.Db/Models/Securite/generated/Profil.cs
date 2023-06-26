////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CSharp.Common;
using Kinetix.Modeling.Annotations;
using Models.CSharp.Securite.Models;

namespace CSharp.Clients.Db.Models.Securite;

/// <summary>
/// Profil des utilisateurs.
/// </summary>
[Table("profil")]
public partial class Profil
{
    /// <summary>
    /// Id technique.
    /// </summary>
    [Column("pro_id")]
    [Domain(Domains.Id)]
    [Key]
    public int? Id { get; set; }

    /// <summary>
    /// Type de profil.
    /// </summary>
    [Column("tpr_code")]
    [ReferencedType(typeof(TypeProfil))]
    [Domain(Domains.Code)]
    public TypeProfil.Codes? TypeProfilCode { get; set; }

    /// <summary>
    /// Liste des droits de l'utilisateur.
    /// </summary>
    [Column("dro_code")]
    [ReferencedType(typeof(Droit))]
    [Domain(Domains.CodeList)]
    [NotMapped]
    public Droit.Codes[] Droits { get; set; }

    /// <summary>
    /// Liste des secteurs de l'utilisateur.
    /// </summary>
    [Column("sec_id")]
    [Domain(Domains.IdList)]
    [NotMapped]
    public int[] Secteurs { get; set; }
}
