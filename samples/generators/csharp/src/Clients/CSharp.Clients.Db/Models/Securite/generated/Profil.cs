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
    /// Constructeur.
    /// </summary>
    public Profil()
    {
        OnCreated();
    }

    /// <summary>
    /// Constructeur par recopie.
    /// </summary>
    /// <param name="bean">Source.</param>
    public Profil(Profil bean)
    {
        if (bean == null)
        {
            throw new ArgumentNullException(nameof(bean));
        }

        Id = bean.Id;
        TypeProfilCode = bean.TypeProfilCode;
        Droits = bean.Droits;
        Secteurs = bean.Secteurs;

        OnCreated(bean);
    }

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
    public Droit.Codes? Droits { get; set; }

    /// <summary>
    /// Liste des secteurs de l'utilisateur.
    /// </summary>
    [Column("sec_id")]
    [Domain(Domains.IdList)]
    public List<int?> Secteurs { get; set; }

    /// <summary>
    /// Methode d'extensibilité possible pour les constructeurs.
    /// </summary>
    partial void OnCreated();

    /// <summary>
    /// Methode d'extensibilité possible pour les constructeurs par recopie.
    /// </summary>
    /// <param name="bean">Source.</param>
    partial void OnCreated(Profil bean);
}
