////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Restaurant.Models;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Avis d'un client sur un restaurant.
/// </summary>
[Table("avis_client")]
public partial record AvisClient : NombreVuesBase
{
    /// <summary>
    /// Identifiant de l'avis.
    /// </summary>
    [Column("avi_id")]
    [Domain(Domains.Id)]
    [Key]
    public int? Id { get; set; }

    /// <summary>
    /// Note sur 5.
    /// </summary>
    [Column("avi_note")]
    [Required]
    [Domain(Domains.Quantite)]
    public int? Note { get; set; }

    /// <summary>
    /// Commentaire de l'avis.
    /// </summary>
    [Column("avi_commentaire")]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string? Commentaire { get; set; }

    /// <summary>
    /// Date de l'avis.
    /// </summary>
    [Column("avi_date_avis")]
    [Required]
    [Domain(Domains.DateHeure)]
    public DateTime? DateAvis { get; set; }

    /// <summary>
    /// Indique si l'avis est approuvé par le restaurant.
    /// </summary>
    [Column("avi_approuve")]
    [Required]
    [Domain(Domains.Booleen)]
    public bool? Approuve { get; set; } = false;

    /// <summary>
    /// Client ayant donné l'avis.
    /// </summary>
    [Required]
    public Client? Client { get; set; }

    /// <summary>
    /// Restaurant concerné par l'avis.
    /// </summary>
    [Required]
    public Restaurant? Restaurant { get; set; }

    /// <summary>
    /// Date de création de l'enregistrement.
    /// </summary>
    [Column("avi_date_creation")]
    [Required]
    [Domain(Domains.DateHeure)]
    public DateTime? DateCreation { get; init; }
}
