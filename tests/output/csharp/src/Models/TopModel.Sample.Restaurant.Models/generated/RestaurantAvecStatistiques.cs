////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Restaurant avec ses statistiques.
/// </summary>
public partial record RestaurantAvecStatistiques
{
    /// <summary>
    /// Identifiant du restaurant.
    /// </summary>
    [Required]
    public int? Id { get; set; }

    /// <summary>
    /// Nom du restaurant.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string? Nom { get; set; }

    /// <summary>
    /// Adresse du restaurant.
    /// </summary>
    [StringLength(100)]
    public string? Adresse { get; set; }

    /// <summary>
    /// Numéro de téléphone.
    /// </summary>
    [StringLength(20)]
    public string? Telephone { get; set; } = "XX.XX.XX.XX.XX";

    /// <summary>
    /// Association réciproque de Menu.Restaurant.
    /// </summary>
    public ICollection<int>? Menus { get; set; }

    /// <summary>
    /// Association réciproque de Plat.Restaurant.
    /// </summary>
    public ICollection<int>? Plats { get; set; }

    /// <summary>
    /// Association réciproque de Promotion.Restaurant.
    /// </summary>
    public ICollection<int>? Promotions { get; set; }

    /// <summary>
    /// Association réciproque de AvisClient.Restaurant.
    /// </summary>
    public ICollection<int>? AvisClients { get; set; }

    /// <summary>
    /// Association réciproque de Table.RestaurantId.
    /// </summary>
    public ICollection<int>? TableIds { get; set; }

    /// <summary>
    /// Date de création de l'enregistrement.
    /// </summary>
    [Required]
    public DateTime? DateCreation { get; set; }

    /// <summary>
    /// Tables du restaurant.
    /// </summary>
    [Required]
    public ICollection<TableRead> Tables { get; set; } = [];

    /// <summary>
    /// Nombre de plats du restaurant.
    /// </summary>
    [Required]
    public int? NombrePlats { get; set; } = 0;

    /// <summary>
    /// Note moyenne des avis clients.
    /// </summary>
    public decimal? NoteMoyenne { get; set; }
}
