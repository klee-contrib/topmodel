////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Statistiques d'un restaurant.
/// </summary>
public partial record StatistiquesRestaurant
{
    /// <summary>
    /// Identifiant du restaurant.
    /// </summary>
    [Required]
    public int? RestaurantId { get; set; }

    /// <summary>
    /// Nombre total de commandes.
    /// </summary>
    [Required]
    public int? NombreCommandes { get; set; } = 0;

    /// <summary>
    /// Chiffre d'affaires total.
    /// </summary>
    [Required]
    public decimal? ChiffreAffaires { get; set; } = 0;

    /// <summary>
    /// Nombre de clients uniques.
    /// </summary>
    [Required]
    public int? NombreClients { get; set; } = 0;

    /// <summary>
    /// Note moyenne des avis.
    /// </summary>
    public decimal? NoteMoyenne { get; set; }
}
