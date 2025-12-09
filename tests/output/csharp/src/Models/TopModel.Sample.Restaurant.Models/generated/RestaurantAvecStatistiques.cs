////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Common;

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
    public string Nom { get; set; }

    /// <summary>
    /// Adresse du restaurant.
    /// </summary>
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string Adresse { get; set; }

    /// <summary>
    /// Numéro de téléphone.
    /// </summary>
    [Domain(Domains.Telephone)]
    [StringLength(20)]
    public string Telephone { get; set; }

    /// <summary>
    /// Association réciproque de TableClient.RestaurantIdRestaurant.
    /// </summary>
    [Domain(Domains.Liste)]
    public ICollection<int> TableClientsRestaurant { get; set; }

    /// <summary>
    /// Association réciproque de Plat.RestaurantIdRestaurant.
    /// </summary>
    [Domain(Domains.Liste)]
    public ICollection<int> PlatsRestaurant { get; set; }

    /// <summary>
    /// Association réciproque de AvisClient.RestaurantIdRestaurant.
    /// </summary>
    [Domain(Domains.Liste)]
    public ICollection<int> AvisClientsRestaurant { get; set; }

    /// <summary>
    /// Association réciproque de Menu.RestaurantIdRestaurant.
    /// </summary>
    [Domain(Domains.Liste)]
    public ICollection<int> MenusRestaurant { get; set; }

    /// <summary>
    /// Association réciproque de Reservation.RestaurantIdRestaurant.
    /// </summary>
    [Domain(Domains.Liste)]
    public ICollection<int> ReservationsRestaurant { get; set; }

    /// <summary>
    /// Association réciproque de Promotion.RestaurantIdRestaurant.
    /// </summary>
    [Domain(Domains.Liste)]
    public ICollection<int> PromotionsRestaurant { get; set; }

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
