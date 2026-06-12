////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Restaurant.Models;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Prestaire du restaurant.
/// </summary>
[Table("prestataire")]
public partial record Prestataire : IEmployeBase
{
    /// <summary>
    /// Identifiant de la personne.
    /// </summary>
    [Column("pst_id")]
    [Domain(Domains.Id)]
    [Key]
    public int? Id { get; set; }

    /// <summary>
    /// Nom de la personne.
    /// </summary>
    [Column("pst_nom")]
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string? Nom { get; set; }

    /// <summary>
    /// Prénom de la personne.
    /// </summary>
    [Column("pst_prenom")]
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string? Prenom { get; set; }

    /// <summary>
    /// Numéro de téléphone de l'employé.
    /// </summary>
    [Column("pst_telephone")]
    [Domain(Domains.Telephone)]
    [StringLength(20)]
    public string? Telephone { get; set; }
}
