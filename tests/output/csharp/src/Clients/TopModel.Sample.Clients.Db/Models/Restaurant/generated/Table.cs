////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Table du restaurant.
/// </summary>
[Table("table")]
public partial record Table
{
    /// <summary>
    /// Identifiant de la table.
    /// </summary>
    [Column("tab_id")]
    [Key]
    public int? Id { get; set; }

    /// <summary>
    /// Numéro de la table.
    /// </summary>
    [Column("tab_numero")]
    [Required]
    [StringLength(10)]
    public string? Numero { get; set; }

    /// <summary>
    /// Capacité de la table (nombre de places).
    /// </summary>
    [Column("tab_capacite")]
    [Required]
    public int? Capacite { get; set; }

    /// <summary>
    /// Indique si la table est disponible.
    /// </summary>
    [Column("tab_disponible")]
    [Required]
    public bool? Disponible { get; set; } = true;

    /// <summary>
    /// Restaurant auquel appartient la table.
    /// </summary>
    [Column("lie_id")]
    [Required]
    public int? RestaurantId { get; set; }

    /// <summary>
    /// Date de création de l'enregistrement.
    /// </summary>
    [Column("tab_date_creation")]
    [Required]
    public DateTime? DateCreation { get; init; }
}
