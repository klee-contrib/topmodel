////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Common;
using TopModel.Sample.Securite.Models.Profil;

namespace TopModel.Sample.Clients.Db.Models.Securite.Profil;

/// <summary>
/// Profil des utilisateurs.
/// </summary>
[Table("profil")]
public partial record Profil
{
    /// <summary>
    /// Id technique.
    /// </summary>
    [Column("pro_id")]
    [Domain(Domains.Id)]
    [Key]
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
    [NotMapped]
    public Droit.Codes[] Droits { get; set; }

    /// <summary>
    /// Association réciproque de Utilisateur.ProfilId.
    /// </summary>
    [Column("uti_id")]
    [Domain(Domains.IdListe)]
    [NotMapped]
    public int[] Utilisateurs { get; set; }

    /// <summary>
    /// Date de création de l'utilisateur.
    /// </summary>
    [Column("pro_date_creation")]
    [Required]
    [Domain(Domains.DateHeure)]
    public DateTime? DateCreation { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date de modification de l'utilisateur.
    /// </summary>
    [Column("pro_date_modification")]
    [Domain(Domains.DateHeure)]
    public DateTime? DateModification { get; set; } = DateTime.UtcNow;
}
