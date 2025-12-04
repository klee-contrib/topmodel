////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Common;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Restaurant.
/// </summary>
[Table("restaurant")]
public partial record Restaurant
{
    /// <summary>
    /// Identifiant du restaurant.
    /// </summary>
    [Column("res_id")]
    [Domain(Domains.Id)]
    [Key]
    public int? Id { get; set; }

    /// <summary>
    /// Nom du restaurant.
    /// </summary>
    [Column("res_nom")]
    [Required]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string Nom { get; set; }

    /// <summary>
    /// Adresse du restaurant.
    /// </summary>
    [Column("res_adresse")]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string Adresse { get; set; }

    /// <summary>
    /// Numéro de téléphone.
    /// </summary>
    [Column("res_telephone")]
    [Domain(Domains.Telephone)]
    [StringLength(20)]
    public string Telephone { get; set; }

    /// <summary>
    /// Association réciproque de TableClient.RestaurantIdRestaurant.
    /// </summary>
    [Domain(Domains.Liste)]
    [NotMapped]
    public ICollection<int> TableClientsRestaurant { get; set; }

    /// <summary>
    /// Association réciproque de Plat.RestaurantIdRestaurant.
    /// </summary>
    [Domain(Domains.Liste)]
    [NotMapped]
    public ICollection<int> PlatsRestaurant { get; set; }

    /// <summary>
    /// Association réciproque de AvisClient.RestaurantIdRestaurant.
    /// </summary>
    [Domain(Domains.Liste)]
    [NotMapped]
    public ICollection<int> AvisClientsRestaurant { get; set; }

    /// <summary>
    /// Association réciproque de Menu.RestaurantIdRestaurant.
    /// </summary>
    [Domain(Domains.Liste)]
    [NotMapped]
    public ICollection<int> MenusRestaurant { get; set; }

    /// <summary>
    /// Association réciproque de Reservation.RestaurantIdRestaurant.
    /// </summary>
    [Domain(Domains.Liste)]
    [NotMapped]
    public ICollection<int> ReservationsRestaurant { get; set; }

    /// <summary>
    /// Association réciproque de Promotion.RestaurantIdRestaurant.
    /// </summary>
    [Domain(Domains.Liste)]
    [NotMapped]
    public ICollection<int> PromotionsRestaurant { get; set; }
}
