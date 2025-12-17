////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'un restaurant en liste.
/// </summary>
public interface IRestaurantItem
{
    /// <summary>
    /// Identifiant du restaurant.
    /// </summary>
    int? Id { get; }

    /// <summary>
    /// Nom du restaurant.
    /// </summary>
    string? Nom { get; }

    /// <summary>
    /// Adresse du restaurant.
    /// </summary>
    string? Adresse { get; }

    /// <summary>
    /// Numéro de téléphone.
    /// </summary>
    string? Telephone { get; }

    /// <summary>
    /// Factory pour instancier la classe.
    /// </summary>
    /// <param name="id">Identifiant du restaurant.</param>
    /// <param name="nom">Nom du restaurant.</param>
    /// <param name="adresse">Adresse du restaurant.</param>
    /// <param name="telephone">Numéro de téléphone.</param>
    /// <returns>Instance de la classe.</returns>
    static abstract IRestaurantItem Create(int? id = null, string? nom = null, string? adresse = null, string? telephone = null);
}
