////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Common;

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
    [Domain(Domains.Id)]
    public int? RestaurantId { get; set; }

    /// <summary>
    /// Nombre total de commandes.
    /// </summary>
    [Required]
    [Domain(Domains.Quantite)]
    public int? NombreCommandes { get; set; } = 0;

    /// <summary>
    /// Chiffre d'affaires total.
    /// </summary>
    [Required]
    [Domain(Domains.Prix)]
    public decimal? ChiffreAffaires { get; set; } = 0;

    /// <summary>
    /// Nombre de clients uniques.
    /// </summary>
    [Required]
    [Domain(Domains.Quantite)]
    public int? NombreClients { get; set; } = 0;

    /// <summary>
    /// Note moyenne des avis.
    /// </summary>
    [Domain(Domains.Prix)]
    public decimal? NoteMoyenne { get; set; }
}
