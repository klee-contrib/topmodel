////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    [Key]
    public int? Id { get; set; }

    /// <summary>
    /// Nom du restaurant.
    /// </summary>
    [Column("lie_nom")]
    [Required]
    [StringLength(100)]
    public string? Nom { get; set; }

    /// <summary>
    /// Adresse du restaurant.
    /// </summary>
    [Column("lie_adresse")]
    [StringLength(100)]
    public string? Adresse { get; set; }
}
