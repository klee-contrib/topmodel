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
    /// Association réciproque de Reservation.RestaurantId.
    /// </summary>
    [Domain(Domains.Liste)]
    public ICollection<int> Reservations { get; set; }

    /// <summary>
    /// Association réciproque de Menu.RestaurantId.
    /// </summary>
    [Domain(Domains.Liste)]
    public ICollection<int> Menus { get; set; }

    /// <summary>
    /// Association réciproque de Plat.RestaurantId.
    /// </summary>
    [Domain(Domains.Liste)]
    public ICollection<int> Plats { get; set; }

    /// <summary>
    /// Association réciproque de Promotion.RestaurantId.
    /// </summary>
    [Domain(Domains.Liste)]
    public ICollection<int> Promotions { get; set; }

    /// <summary>
    /// Association réciproque de AvisClient.RestaurantId.
    /// </summary>
    [Domain(Domains.Liste)]
    public ICollection<int> AvisClients { get; set; }

    /// <summary>
    /// Association réciproque de TableRestaurant.RestaurantId.
    /// </summary>
    [Domain(Domains.Liste)]
    public ICollection<int> Tables { get; set; }
}
