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
    /// Constructeur.
    /// </summary>
    public UtilisateurDto()
    {
        UtilisateurParent = new UtilisateurDto();

        OnCreated();
    }

    /// <summary>
    /// Constructeur par recopie.
    /// </summary>
    /// <param name="bean">Source.</param>
    public UtilisateurDto(UtilisateurDto bean)
    {
        if (bean == null)
        {
            throw new ArgumentNullException(nameof(bean));
        }

        UtilisateurParent = new UtilisateurDto(bean.UtilisateurParent);
        Id = bean.Id;
        Age = bean.Age;
        ProfilId = bean.ProfilId;
        email = bean.email;
        Nom = bean.Nom;
        TypeUtilisateurCode = bean.TypeUtilisateurCode;
        dateCreation = bean.dateCreation;
        dateModification = bean.dateModification;

        OnCreated(bean);
    }

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
    public decimal? Age { get; set; } = 6l;

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
    public string email { get; set; }

    /// <summary>
    /// Nom de l'utilisateur.
    /// </summary>
    [Column("uti_nom")]
    [Domain(Domains.Libelle)]
    [StringLength(3)]
    public string Nom { get; set; } = "Jabx";

    /// <summary>
    /// Type d'utilisateur en Many to one.
    /// </summary>
    [Column("tut_code")]
    [ReferencedType(typeof(TypeUtilisateur))]
    [Domain(Domains.Code)]
    public TypeUtilisateur.Codes? TypeUtilisateurCode { get; set; } = TypeUtilisateur.Codes.ADM;

    /// <summary>
    /// Date de création de l'utilisateur.
    /// </summary>
    [Column("uti_date_creation")]
    [Domain(Domains.DateCreation)]
    public DateOnly? dateCreation { get; set; }

    /// <summary>
    /// Date de modification de l'utilisateur.
    /// </summary>
    [Column("uti_date_modification")]
    [Domain(Domains.DateModification)]
    public DateOnly? dateModification { get; set; }

    /// <summary>
    /// UtilisateurParent.
    /// </summary>
    [NotMapped]
    public UtilisateurDto UtilisateurParent { get; set; }

    /// <summary>
    /// Methode d'extensibilité possible pour les constructeurs.
    /// </summary>
    partial void OnCreated();

    /// <summary>
    /// Methode d'extensibilité possible pour les constructeurs par recopie.
    /// </summary>
    /// <param name="bean">Source.</param>
    partial void OnCreated(UtilisateurDto bean);
}
