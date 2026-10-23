////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Menu du restaurant.
/// </summary>
[Table("menu")]
public partial record Menu
{
    /// <summary>
    /// Identifiant du menu.
    /// </summary>
    [Column("men_id")]
    [Key]
    public int? Id { get; set; }

    /// <summary>
    /// Nom du menu.
    /// </summary>
    [Column("men_nom")]
    [Required]
    [StringLength(100)]
    public string? Nom { get; set; }

    /// <summary>
    /// Description du menu.
    /// </summary>
    [Column("men_description")]
    [StringLength(100)]
    public string? Description { get; set; }

    /// <summary>
    /// Prix du menu.
    /// </summary>
    [Column("men_prix")]
    [Required]
    public decimal? Prix { get; set; }

    /// <summary>
    /// Indique si le menu est disponible.
    /// </summary>
    [Column("men_disponible")]
    [Required]
    public bool? Disponible { get; set; } = true;

    /// <summary>
    /// Date de début de validité du menu.
    /// </summary>
    [Column("men_date_debut")]
    public DateTime? DateDebut { get; set; }

    /// <summary>
    /// Date de fin de validité du menu.
    /// </summary>
    [Column("men_date_fin")]
    public DateTime? DateFin { get; set; }

    /// <summary>
    /// Restaurant proposant ce menu.
    /// </summary>
    [Required]
    public Restaurant? Restaurant { get; set; }

    /// <summary>
    /// Association réciproque de MenuPlat.Menu.
    /// </summary>
    public ICollection<MenuPlat> Plats { get; set; } = [];

    /// <summary>
    /// Date de création de l'enregistrement.
    /// </summary>
    [Column("men_date_creation")]
    [Required]
    public DateTime? DateCreation { get; init; }
}
