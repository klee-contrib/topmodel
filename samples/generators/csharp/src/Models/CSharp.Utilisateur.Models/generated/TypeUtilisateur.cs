////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CSharp.Common;
using Kinetix.Modeling.Annotations;

namespace Models.CSharp.Utilisateur.Models;

/// <summary>
/// Type d'utilisateur.
/// </summary>
[Reference(true)]
[DefaultProperty(nameof(Libelle))]
[Table("type_utilisateur")]
public partial class TypeUtilisateur
{
    /// <summary>
    /// Constructeur.
    /// </summary>
    public TypeUtilisateur()
    {
        OnCreated();
    }

    /// <summary>
    /// Constructeur par recopie.
    /// </summary>
    /// <param name="bean">Source.</param>
    public TypeUtilisateur(TypeUtilisateur bean)
    {
        if (bean == null)
        {
            throw new ArgumentNullException(nameof(bean));
        }

        Code = bean.Code;
        Libelle = bean.Libelle;

        OnCreated(bean);
    }

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
    [Domain(Domains.Code)]
    [Key]
    public Codes? Code { get; set; }

    /// <summary>
    /// Libellé du type d'utilisateur.
    /// </summary>
    [Column("tut_libelle")]
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(3)]
    public string Libelle { get; set; }

    /// <summary>
    /// Methode d'extensibilité possible pour les constructeurs.
    /// </summary>
    partial void OnCreated();

    /// <summary>
    /// Methode d'extensibilité possible pour les constructeurs par recopie.
    /// </summary>
    /// <param name="bean">Source.</param>
    partial void OnCreated(TypeUtilisateur bean);
}
