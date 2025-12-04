////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Common;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Menu avec tous ses plats.
/// </summary>
public partial record MenuComplet
{
    /// <summary>
    /// Identifiant du menu.
    /// </summary>
    [Required]
    [Domain(Domains.Id)]
    public int? Id { get; set; }

    /// <summary>
    /// Nom du menu.
    /// </summary>
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string Nom { get; set; }

    /// <summary>
    /// Description du menu.
    /// </summary>
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string Description { get; set; }

    /// <summary>
    /// Prix du menu.
    /// </summary>
    [Required]
    [Domain(Domains.Prix)]
    public decimal? Prix { get; set; }

    /// <summary>
    /// Indique si le menu est disponible.
    /// </summary>
    [Required]
    [Domain(Domains.Booleen)]
    public bool? Disponible { get; set; } = true;

    /// <summary>
    /// Date de début de validité du menu.
    /// </summary>
    [Domain(Domains.DateHeure)]
    public DateTime? DateDebut { get; set; }

    /// <summary>
    /// Date de fin de validité du menu.
    /// </summary>
    [Domain(Domains.DateHeure)]
    public DateTime? DateFin { get; set; }

    /// <summary>
    /// Restaurant proposant ce menu.
    /// </summary>
    [Required]
    [Domain(Domains.Id)]
    public int? RestaurantIdRestaurant { get; set; }

    /// <summary>
    /// Association réciproque de MenuPlat.MenuIdMenu.
    /// </summary>
    [Domain(Domains.Liste)]
    public ICollection<int> MenuPlatsMenu { get; set; }

    /// <summary>
    /// Liste des plats du menu.
    /// </summary>
    [Required]
    public ICollection<PlatItem> Plats { get; set; } = new List<PlatItem>();
}
