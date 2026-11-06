////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;

namespace Hello World.Refs.Models;

/// <summary>
/// Type d'utilisateur.
/// </summary>
[Reference]
[DefaultProperty(nameof(Libelle))]
[Table("type_utilisateur")]
public partial record TypeUtilisateur
{
    #region Meta données

    /// <summary>
    /// Type énuméré présentant les noms des colonnes en base.
    /// </summary>
    public enum Cols
    {
        /// <summary>
        /// Nom de la colonne en base associée à la propriété Code.
        /// </summary>
        TUT_CODE,

        /// <summary>
        /// Nom de la colonne en base associée à la propriété Libelle.
        /// </summary>
        TUT_LIBELLE,
    }

    #endregion

    /// <summary>
    /// Valeurs possibles de la liste de référence TypeUtilisateur.
    /// </summary>
    public enum Codes
    {
        /// <summary>
        /// Administrateur.
        /// </summary>
        ADM,

        /// <summary>
        /// Client.
        /// </summary>
        CLI,

        /// <summary>
        /// Gestionnaire.
        /// </summary>
        GES
    }

    /// <summary>
    /// Code du type d'utilisateur.
    /// </summary>
    [Column("tut_code")]
    [Key]
    public Codes? Code { get; set; }

    /// <summary>
    /// Libellé du type d'utilisateur.
    /// </summary>
    [Column("tut_libelle")]
    [Required]
    [StringLength(15)]
    public string Libelle { get; set; }
}
