////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'une réservation en liste.
/// </summary>
public interface IReservationItem
{
    /// <summary>
    /// Identifiant de la réservation.
    /// </summary>
    int? Id { get; }

    /// <summary>
    /// Date et heure de la réservation.
    /// </summary>
    DateTime? DateReservation { get; }

    /// <summary>
    /// Nombre de personnes.
    /// </summary>
    int? NombrePersonnes { get; }

    /// <summary>
    /// Indique si la réservation est confirmée.
    /// </summary>
    bool? Confirmee { get; }

    /// <summary>
    /// Client ayant fait la réservation.
    /// </summary>
    int? ClientId { get; }

    /// <summary>
    /// Restaurant concerné par la réservation.
    /// </summary>
    int? RestaurantId { get; }

    /// <summary>
    /// Factory pour instancier la classe.
    /// </summary>
    /// <param name="id">Identifiant de la réservation.</param>
    /// <param name="dateReservation">Date et heure de la réservation.</param>
    /// <param name="nombrePersonnes">Nombre de personnes.</param>
    /// <param name="confirmee">Indique si la réservation est confirmée.</param>
    /// <param name="clientId">Client ayant fait la réservation.</param>
    /// <param name="restaurantId">Restaurant concerné par la réservation.</param>
    /// <returns>Instance de la classe.</returns>
    static abstract IReservationItem Create(int? id = null, DateTime? dateReservation = null, int? nombrePersonnes = null, bool? confirmee = null, int? clientId = null, int? restaurantId = null);
}
