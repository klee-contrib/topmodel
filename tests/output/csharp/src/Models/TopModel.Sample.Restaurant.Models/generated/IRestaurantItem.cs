////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'un restaurant en liste.
/// </summary>
public partial interface IRestaurantItem
{
    /// <summary>
    /// Identifiant du restaurant.
    /// </summary>
    int? Id { get; set; }

    /// <summary>
    /// Nom du restaurant.
    /// </summary>
    string? Nom { get; set; }

    /// <summary>
    /// Adresse du restaurant.
    /// </summary>
    string? Adresse { get; set; }

    /// <summary>
    /// Numéro de téléphone.
    /// </summary>
    string? Telephone { get; set; }
}
