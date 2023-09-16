////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CSharp.Common;
using Kinetix.Modeling.Annotations;

namespace Models.CSharp.Securite.Utilisateur.Models;

/// <summary>
/// Détail d'un utilisateur en écriture.
/// </summary>
public partial class UtilisateurWrite
{
    /// <summary>
    /// Nom de l'utilisateur.
    /// </summary>
    [Column("uti_nom")]
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string Nom { get; set; }

    /// <summary>
    /// Nom de l'utilisateur.
    /// </summary>
    [Column("uti_prenom")]
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string Prenom { get; set; }

    /// <summary>
    /// Email de l'utilisateur.
    /// </summary>
    [Column("uti_email")]
    [Required]
    [Domain(Domains.Email)]
    [StringLength(50)]
    public string Email { get; set; }

    /// <summary>
    /// Age de l'utilisateur.
    /// </summary>
    [Column("uti_date_naissance")]
    [Domain(Domains.DateHeure)]
    public DateTime? DateNaissance { get; set; }

    /// <summary>
    /// Si l'utilisateur est actif.
    /// </summary>
    [Column("uti_actif")]
    [Required]
    [Domain(Domains.Booleen)]
    public bool? Actif { get; set; } = true;

    /// <summary>
    /// Profil de l'utilisateur.
    /// </summary>
    [Column("pro_id")]
    [Required]
    [Domain(Domains.Id)]
    public int? ProfilId { get; set; }

    /// <summary>
    /// Type d'utilisateur.
    /// </summary>
    [Column("tut_code")]
    [Required]
    [ReferencedType(typeof(TypeUtilisateur))]
    [Domain(Domains.Code)]
    public TypeUtilisateur.Codes? TypeUtilisateurCode { get; set; } = TypeUtilisateur.Codes.GEST;
}
