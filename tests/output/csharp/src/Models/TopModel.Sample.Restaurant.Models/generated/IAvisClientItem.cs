////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'un avis en liste.
/// </summary>
public interface IAvisClientItem
{
    /// <summary>
    /// Identifiant de l'avis.
    /// </summary>
    int? Id { get; }

    /// <summary>
    /// Note sur 5.
    /// </summary>
    int? Note { get; }

    /// <summary>
    /// Date de l'avis.
    /// </summary>
    DateTime? DateAvis { get; }

    /// <summary>
    /// Indique si l'avis est approuvé par le restaurant.
    /// </summary>
    bool? Approuve { get; }

    /// <summary>
    /// Client ayant donné l'avis.
    /// </summary>
    int? ClientId { get; }

    /// <summary>
    /// Restaurant concerné par l'avis.
    /// </summary>
    int? RestaurantId { get; }

    /// <summary>
    /// Factory pour instancier la classe.
    /// </summary>
    /// <param name="id">Identifiant de l'avis.</param>
    /// <param name="note">Note sur 5.</param>
    /// <param name="dateAvis">Date de l'avis.</param>
    /// <param name="approuve">Indique si l'avis est approuvé par le restaurant.</param>
    /// <param name="clientId">Client ayant donné l'avis.</param>
    /// <param name="restaurantId">Restaurant concerné par l'avis.</param>
    /// <returns>Instance de la classe.</returns>
    static abstract IAvisClientItem Create(int? id = null, int? note = null, DateTime? dateAvis = null, bool? approuve = null, int? clientId = null, int? restaurantId = null);
}
