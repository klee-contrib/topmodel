////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kinetix.Modeling.Annotations;
using TopModel.Sample.Restaurant.Models;

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
    public string? Nom { get; set; }

    /// <summary>
    /// Adresse du restaurant.
    /// </summary>
    [Column("res_adresse")]
    [Domain(Domains.Libelle)]
    [StringLength(100)]
    public string? Adresse { get; set; }

    /// <summary>
    /// Numéro de téléphone.
    /// </summary>
    [Column("res_telephone")]
    [Domain(Domains.Telephone)]
    [StringLength(20)]
    public string? Telephone { get; set; }

    /// <summary>
    /// Association réciproque de Menu.RestaurantId.
    /// </summary>
    [Domain(Domains.Liste)]
    [NotMapped]
    public ICollection<int>? Menus { get; set; }

    /// <summary>
    /// Association réciproque de Plat.RestaurantId.
    /// </summary>
    [Domain(Domains.Liste)]
    [NotMapped]
    public ICollection<int>? Plats { get; set; }

    /// <summary>
    /// Association réciproque de Promotion.RestaurantId.
    /// </summary>
    [Domain(Domains.Liste)]
    [NotMapped]
    public ICollection<int>? Promotions { get; set; }

    /// <summary>
    /// Association réciproque de AvisClient.RestaurantId.
    /// </summary>
    [Domain(Domains.Liste)]
    [NotMapped]
    public ICollection<int>? AvisClients { get; set; }

    /// <summary>
    /// Association réciproque de TableRestaurant.RestaurantId.
    /// </summary>
    [Domain(Domains.Liste)]
    [NotMapped]
    public ICollection<int>? Tables { get; set; }
}
