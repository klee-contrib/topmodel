////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CSharp.Common;
using Kinetix.Modeling.Annotations;
using Models.CSharp.Utilisateur.Models;

namespace Models.CSharp.Securite.Models;

/// <summary>
/// Objet métier non persisté représentant Profil.
/// </summary>
public partial class ProfilDto
{
    /// <summary>
    /// Constructeur.
    /// </summary>
    public ProfilDto()
    {
        Utilisateurs = new List<UtilisateurDto>();
        Secteurs = new List<SecteurDto>();

        OnCreated();
    }

    /// <summary>
    /// Constructeur par recopie.
    /// </summary>
    /// <param name="bean">Source.</param>
    public ProfilDto(ProfilDto bean)
    {
        if (bean == null)
        {
            throw new ArgumentNullException(nameof(bean));
        }

        Utilisateurs = new List<UtilisateurDto>(bean.Utilisateurs);
        Secteurs = new List<SecteurDto>(bean.Secteurs);
        Id = bean.Id;
        TypeProfilCode = bean.TypeProfilCode;
        Droits = bean.Droits;

        OnCreated(bean);
    }

    /// <summary>
    /// Id technique.
    /// </summary>
    [Column("pro_id")]
    [Required]
    [Domain(Domains.Id)]
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
    public Droit.Codes[] Droits { get; set; }

    /// <summary>
    /// Liste paginée des utilisateurs de ce profil.
    /// </summary>
    [NotMapped]
    public ICollection<UtilisateurDto> Utilisateurs { get; set; }

    /// <summary>
    /// Liste des secteurs du profil.
    /// </summary>
    [NotMapped]
    public ICollection<SecteurDto> Secteurs { get; set; }

    /// <summary>
    /// Methode d'extensibilité possible pour les constructeurs.
    /// </summary>
    partial void OnCreated();

    /// <summary>
    /// Methode d'extensibilité possible pour les constructeurs par recopie.
    /// </summary>
    /// <param name="bean">Source.</param>
    partial void OnCreated(ProfilDto bean);
}
