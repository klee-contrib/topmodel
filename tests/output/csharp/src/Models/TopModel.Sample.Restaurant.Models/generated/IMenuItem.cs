////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'un menu en liste.
/// </summary>
public interface IMenuItem
{
    /// <summary>
    /// Identifiant du menu.
    /// </summary>
    int? Id { get; }

    /// <summary>
    /// Nom du menu.
    /// </summary>
    string Nom { get; }

    /// <summary>
    /// Prix du menu.
    /// </summary>
    decimal? Prix { get; }

    /// <summary>
    /// Indique si le menu est disponible.
    /// </summary>
    bool? Disponible { get; }

    /// <summary>
    /// Restaurant proposant ce menu.
    /// </summary>
    int? RestaurantId { get; }

    /// <summary>
    /// Factory pour instancier la classe.
    /// </summary>
    /// <param name="id">Identifiant du menu.</param>
    /// <param name="nom">Nom du menu.</param>
    /// <param name="prix">Prix du menu.</param>
    /// <param name="disponible">Indique si le menu est disponible.</param>
    /// <param name="restaurantId">Restaurant proposant ce menu.</param>
    /// <returns>Instance de la classe.</returns>
    static abstract IMenuItem Create(int? id = null, string nom = null, decimal? prix = null, bool? disponible = null, int? restaurantId = null);
}
