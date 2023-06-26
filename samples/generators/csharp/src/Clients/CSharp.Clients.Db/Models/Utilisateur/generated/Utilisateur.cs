////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CSharp.Common;
using Kinetix.Modeling.Annotations;
using Models.CSharp.Utilisateur.Models;

namespace CSharp.Clients.Db.Models.Utilisateur;

/// <summary>
/// Utilisateur de l'application.
/// </summary>
[Table("utilisateur")]
public partial class Utilisateur
{
    /// <summary>
    /// Id technique.
    /// </summary>
    [Column("uti_id")]
    [Domain(Domains.Id)]
    [Key]
    public int? Id { get; set; }

    /// <summary>
    /// Age en années de l'utilisateur.
    /// </summary>
    [Column("uti_age")]
    [Domain(Domains.Number)]
    public decimal? Age { get; set; } = 6l;

    /// <summary>
    /// Profil de l'utilisateur.
    /// </summary>
    [Column("pro_id")]
    [Domain(Domains.Id)]
    public int? ProfilId { get; set; }

    /// <summary>
    /// Email de l'utilisateur.
    /// </summary>
    [Column("uti_email")]
    [Domain(Domains.Email)]
    [StringLength(50)]
    public string Email { get; set; }

    /// <summary>
    /// Nom de l'utilisateur.
    /// </summary>
    [Column("uti_nom")]
    [Domain(Domains.Libelle)]
    [StringLength(3)]
    public string Nom { get; set; } = "Jabx";

    /// <summary>
    /// Si l'utilisateur est actif.
    /// </summary>
    [Column("uti_actif")]
    [Domain(Domains.Boolean)]
    public bool? Actif { get; set; }

    /// <summary>
    /// Type d'utilisateur en Many to one.
    /// </summary>
    [Column("tut_code")]
    [ReferencedType(typeof(TypeUtilisateur))]
    [Domain(Domains.Code)]
    public TypeUtilisateur.Codes? TypeUtilisateurCode { get; set; } = TypeUtilisateur.Codes.ADM;

    /// <summary>
    /// Utilisateur parent.
    /// </summary>
    [Column("uti_id_parent")]
    [Domain(Domains.Id)]
    public int? UtilisateurIdParent { get; set; }

    /// <summary>
    /// Utilisateur enfants.
    /// </summary>
    [Column("uti_id_enfant")]
    [Domain(Domains.IdList)]
    [NotMapped]
    public int[] UtilisateursEnfant { get; set; }

    /// <summary>
    /// Date de création de l'utilisateur.
    /// </summary>
    [Column("uti_date_creation")]
    [Domain(Domains.DateCreation)]
    public DateOnly? DateCreation { get; set; }

    /// <summary>
    /// Date de modification de l'utilisateur.
    /// </summary>
    [Column("uti_date_modification")]
    [Domain(Domains.DateModification)]
    public DateOnly? DateModification { get; set; }
}
