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
/// Type d'utilisateur.
/// </summary>
[Reference(true)]
[DefaultProperty(nameof(Libelle))]
[Table("type_profil")]
public partial class TypeProfil
{
    /// <summary>
    /// Valeurs possibles de la liste de référence TypeProfil.
    /// </summary>
    public enum Codes
    {
        /// <summary>
        /// Administrateur.
        /// </summary>
        ADM,

        /// <summary>
        /// Gestionnaire.
        /// </summary>
        GES
    }

    /// <summary>
    /// Code du type d'utilisateur.
    /// </summary>
    [Column("tpr_code")]
    [Domain(Domains.Code)]
    [Key]
    public Codes? Code { get; set; }

    /// <summary>
    /// Libellé du type d'utilisateur.
    /// </summary>
    [Column("tpr_libelle")]
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(3)]
    public string Libelle { get; set; }
}
