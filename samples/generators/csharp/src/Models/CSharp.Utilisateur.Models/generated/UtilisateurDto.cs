////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CSharp.Common;
using Kinetix.Modeling.Annotations;

namespace Models.CSharp.Utilisateur.Models;

/// <summary>
/// Objet non persisté de communication avec le serveur.
/// </summary>
public partial class UtilisateurDto
{
    /// <summary>
    /// Id technique.
    /// </summary>
    [Column("uti_id")]
    [Required]
    [Domain(Domains.Id)]
    public int? Id { get; set; }

    /// <summary>
    /// Age en années de l'utilisateur.
    /// </summary>
    [Column("uti_age")]
    [Domain(Domains.Number)]
    public decimal? Age { get; set; } = 6;

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
    /// Utilisateur enfants.
    /// </summary>
    [Column("uti_id_enfant")]
    [Domain(Domains.IdList)]
    public int[] UtilisateursEnfant { get; set; }

    /// <summary>
    /// Date de création de l'utilisateur.
    /// </summary>
    [Column("uti_date_creation")]
    [Domain(Domains.DateCreation)]
    public DateOnly? DateCreation { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    /// <summary>
    /// Date de modification de l'utilisateur.
    /// </summary>
    [Column("uti_date_modification")]
    [Domain(Domains.DateModification)]
    public DateOnly? DateModification { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    /// <summary>
    /// UtilisateurParent.
    /// </summary>
    [NotMapped]
    public UtilisateurDto UtilisateurParent { get; set; } = new UtilisateurDto();
}
