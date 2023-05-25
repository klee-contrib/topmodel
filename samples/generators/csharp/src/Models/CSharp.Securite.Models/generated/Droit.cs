////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CSharp.Common;
using Kinetix.Modeling.Annotations;

namespace Models.CSharp.Securite.Models;

/// <summary>
/// Droits de l'application.
/// </summary>
[Reference(true)]
[DefaultProperty(nameof(Libelle))]
[Table("droit")]
public partial class Droit
{
    /// <summary>
    /// Valeurs possibles de la liste de référence Droit.
    /// </summary>
    public enum Codes
    {
        /// <summary>
        /// Créer.
        /// </summary>
        CRE,

        /// <summary>
        /// Modifier.
        /// </summary>
        MOD,

        /// <summary>
        /// Supprimer.
        /// </summary>
        SUP
    }

    /// <summary>
    /// Code du droit.
    /// </summary>
    [Column("dro_code")]
    [Domain(Domains.Code)]
    [Key]
    public Codes? Code { get; set; }

    /// <summary>
    /// Libellé du droit.
    /// </summary>
    [Column("dro_libelle")]
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(3)]
    public string Libelle { get; set; }

    /// <summary>
    /// Type de profil pouvant faire l'action.
    /// </summary>
    [Column("tpr_code")]
    [ReferencedType(typeof(TypeProfil))]
    [Domain(Domains.Code)]
    public TypeProfil.Codes? TypeProfilCode { get; set; }
}
