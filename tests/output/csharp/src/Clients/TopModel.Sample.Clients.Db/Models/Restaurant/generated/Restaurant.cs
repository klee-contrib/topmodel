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
public partial record Restaurant : Lieu
{
    /// <summary>
    /// Numéro de téléphone.
    /// </summary>
    [Column("res_telephone")]
    [Domain(Domains.Telephone)]
    [StringLength(20)]
    public string? Telephone { get; set; }

    /// <summary>
    /// Association réciproque de Menu.Restaurant.
    /// </summary>
    public ICollection<Menu> Menus { get; set; } = [];

    /// <summary>
    /// Association réciproque de Plat.Restaurant.
    /// </summary>
    public ICollection<Plat> Plats { get; set; } = [];

    /// <summary>
    /// Association réciproque de Promotion.Restaurant.
    /// </summary>
    public ICollection<Promotion> Promotions { get; set; } = [];

    /// <summary>
    /// Association réciproque de AvisClient.Restaurant.
    /// </summary>
    public ICollection<AvisClient> AvisClients { get; set; } = [];

    /// <summary>
    /// Association réciproque de Table.RestaurantId.
    /// </summary>
    [Domain(Domains.Liste)]
    [NotMapped]
    public ICollection<int>? TableIds { get; set; }

    /// <summary>
    /// Date de création de l'enregistrement.
    /// </summary>
    [Column("res_date_creation")]
    [Required]
    [Domain(Domains.DateHeure)]
    public DateTime? DateCreation { get; init; }
}
