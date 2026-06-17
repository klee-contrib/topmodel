////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Restaurant.Models;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Lieu.
/// </summary>
[Table("lieu")]
public abstract partial record Lieu
{
    /// <summary>
    /// Identifiant du restaurant.
    /// </summary>
    [Column("lie_id")]
    [Domain(Domains.Id)]
    [Key]
    public int? Id { get; set; }

    /// <summary>
    /// Nom du restaurant.
    /// </summary>
    [Column("lie_nom")]
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string? Nom { get; set; }

    /// <summary>
    /// Adresse du restaurant.
    /// </summary>
    [Column("lie_adresse")]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string? Adresse { get; set; }
}
