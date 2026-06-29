////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hello World.Common;
using Hello World.Refs.Models;
using Kinetix.Modeling.Annotations;

namespace Hello World.Users.Models;

/// <summary>
/// Utilisateur de l'application.
/// </summary>
[Table("utilisateur")]
public partial record Utilisateur
{
    #region Meta données

    /// <summary>
    /// Type énuméré présentant les noms des colonnes en base.
    /// </summary>
    public enum Cols
    {
        /// <summary>
        /// Nom de la colonne en base associée à la propriété Id.
        /// </summary>
        UTI_ID,

        /// <summary>
        /// Nom de la colonne en base associée à la propriété Email.
        /// </summary>
        UTI_EMAIL,

        /// <summary>
        /// Nom de la colonne en base associée à la propriété Nom.
        /// </summary>
        UTI_NOM,

        /// <summary>
        /// Nom de la colonne en base associée à la propriété DateInscription.
        /// </summary>
        UTI_DATE_INSCRIPTION,

        /// <summary>
        /// Nom de la colonne en base associée à la propriété TypeUtilisateurCode.
        /// </summary>
        TUT_CODE,
    }

    #endregion

    /// <summary>
    /// Identifiant unique de l'utilisateur.
    /// </summary>
    [Column("uti_id")]
    [Domain(Domains.Id)]
    [Key]
    public long? Id { get; set; }

    /// <summary>
    /// Adresse mail de l'utilisateur.
    /// </summary>
    [Column("uti_email")]
    [Required]
    [Domain(Domains.Email)]
    [StringLength(50)]
    public string Email { get; set; }

    /// <summary>
    /// Nom de l'utilisateur.
    /// </summary>
    [Column("uti_nom")]
    [Domain(Domains.Libelle)]
    [StringLength(15)]
    public string Nom { get; set; }

    /// <summary>
    /// Date d'inscription.
    /// </summary>
    [Column("uti_date_inscription")]
    [Domain(Domains.Date)]
    public DateTime? DateInscription { get; set; }

    /// <summary>
    /// Type de l'utilisateur.
    /// </summary>
    [Column("tut_code")]
    [ReferencedType(typeof(TypeUtilisateur))]
    [Domain(Domains.Code)]
    public TypeUtilisateur.Codes? TypeUtilisateurCode { get; set; }
}
