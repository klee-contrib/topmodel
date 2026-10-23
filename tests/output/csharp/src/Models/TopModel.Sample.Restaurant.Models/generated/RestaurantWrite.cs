////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;

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
    public string? Telephone { get; set; }

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
    [Required]
    public ICollection<ITableItem> Tables { get; set; } = [];
}
