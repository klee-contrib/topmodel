////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Common;

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
    [Domain(Domains.Id)]
    [Key]
    public int? Id { get; set; }

    /// <summary>
    /// Nom du menu.
    /// </summary>
    [Column("men_nom")]
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string Nom { get; set; }

    /// <summary>
    /// Description du menu.
    /// </summary>
    [Column("men_description")]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string Description { get; set; }

    /// <summary>
    /// Prix du menu.
    /// </summary>
    [Column("men_prix")]
    [Required]
    [Domain(Domains.Prix)]
    public decimal? Prix { get; set; }

    /// <summary>
    /// Indique si le menu est disponible.
    /// </summary>
    [Column("men_disponible")]
    [Required]
    [Domain(Domains.Booleen)]
    public bool? Disponible { get; set; } = true;

    /// <summary>
    /// Date de début de validité du menu.
    /// </summary>
    [Column("men_date_debut")]
    [Domain(Domains.DateHeure)]
    public DateTime? DateDebut { get; set; }

    /// <summary>
    /// Date de fin de validité du menu.
    /// </summary>
    [Column("men_date_fin")]
    [Domain(Domains.DateHeure)]
    public DateTime? DateFin { get; set; }

    /// <summary>
    /// Restaurant proposant ce menu.
    /// </summary>
    [Column("res_id_restaurant")]
    [Required]
    [Domain(Domains.Id)]
    public int? RestaurantIdRestaurant { get; set; }

    /// <summary>
    /// Association réciproque de MenuPlat.MenuIdMenu.
    /// </summary>
    [Domain(Domains.Liste)]
    [NotMapped]
    public ICollection<int> MenuPlatsMenu { get; set; }
}
