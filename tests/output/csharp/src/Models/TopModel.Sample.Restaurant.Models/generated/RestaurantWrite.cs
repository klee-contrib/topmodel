////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Common;

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'un restaurant en écriture.
/// </summary>
public partial record RestaurantWrite
{
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
}
