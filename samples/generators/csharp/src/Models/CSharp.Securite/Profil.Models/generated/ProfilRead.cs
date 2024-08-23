////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CSharp.Common;
using Kinetix.Modeling.Annotations;
using Models.CSharp.Securite.Utilisateur.Models;

namespace Models.CSharp.Securite.Profil.Models;

/// <summary>
/// Détail d'un profil en lecture.
/// </summary>
public partial class ProfilRead
{
    /// <summary>
    /// Id technique.
    /// </summary>
    [Column("pro_id")]
    [Required]
    [Domain(Domains.Id)]
    public int? Id { get; set; }

    /// <summary>
    /// Libellé du profil.
    /// </summary>
    [Column("pro_libelle")]
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string Libelle { get; set; }

    /// <summary>
    /// Liste des droits du profil.
    /// </summary>
    [Column("dro_code")]
    [ReferencedType(typeof(Droit))]
    [Domain(Domains.CodeListe)]
    public Droit.Codes[] Droits { get; set; }

    /// <summary>
    /// Date de création de l'utilisateur.
    /// </summary>
    [Column("pro_date_creation")]
    [Required]
    [Domain(Domains.DateHeure)]
    public DateTime? DateCreation { get; set; }

    /// <summary>
    /// Date de modification de l'utilisateur.
    /// </summary>
    [Column("pro_date_modification")]
    [Domain(Domains.DateHeure)]
    public DateTime? DateModification { get; set; }

    /// <summary>
    /// Utilisateurs ayant ce profil.
    /// </summary>
    public ICollection<UtilisateurItem> Utilisateurs { get; set; } = new List<UtilisateurItem>();
}
