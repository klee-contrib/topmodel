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
    /// Constructeur.
    /// </summary>
    public TypeProfil()
    {
        OnCreated();
    }

    /// <summary>
    /// Constructeur par recopie.
    /// </summary>
    /// <param name="bean">Source.</param>
    public TypeProfil(TypeProfil bean)
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
    [Domain(Domains.CODE)]
    [Key]
    public Codes? Code { get; set; }

    /// <summary>
    /// Libellé du type d'utilisateur.
    /// </summary>
    [Column("tpr_libelle")]
    [Required]
    [Domain(Domains.LIBELLE)]
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
    partial void OnCreated(TypeProfil bean);
}
