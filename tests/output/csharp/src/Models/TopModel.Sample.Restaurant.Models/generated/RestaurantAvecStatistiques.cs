////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;

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
    [Domain(Domains.Id)]
    public int? Id { get; set; }

    /// <summary>
    /// Nom du restaurant.
    /// </summary>
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string? Nom { get; set; }

    /// <summary>
    /// Adresse du restaurant.
    /// </summary>
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string? Adresse { get; set; }

    /// <summary>
    /// Numéro de téléphone.
    /// </summary>
    [Domain(Domains.Telephone)]
    [StringLength(20)]
    public string? Telephone { get; set; }

    /// <summary>
    /// Association réciproque de Menu.Restaurant.
    /// </summary>
    [Domain(Domains.Liste)]
    public ICollection<int>? Menus { get; set; }

    /// <summary>
    /// Association réciproque de Plat.Restaurant.
    /// </summary>
    [Domain(Domains.Liste)]
    public ICollection<int>? Plats { get; set; }

    /// <summary>
    /// Association réciproque de Promotion.Restaurant.
    /// </summary>
    [Domain(Domains.Liste)]
    public ICollection<int>? Promotions { get; set; }

    /// <summary>
    /// Association réciproque de AvisClient.Restaurant.
    /// </summary>
    [Domain(Domains.Liste)]
    public ICollection<int>? AvisClients { get; set; }

    /// <summary>
    /// Association réciproque de TableRestaurant.RestaurantId.
    /// </summary>
    [Domain(Domains.Liste)]
    public ICollection<int>? TableIds { get; set; }

    /// <summary>
    /// Nombre de plats du restaurant.
    /// </summary>
    [Required]
    [Domain(Domains.Quantite)]
    public int? NombrePlats { get; set; } = 0;

    /// <summary>
    /// Nombre de tables du restaurant.
    /// </summary>
    [Required]
    [Domain(Domains.Quantite)]
    public int? NombreTables { get; set; } = 0;

    /// <summary>
    /// Note moyenne des avis clients.
    /// </summary>
    [Domain(Domains.Prix)]
    public decimal? NoteMoyenne { get; set; }
}
