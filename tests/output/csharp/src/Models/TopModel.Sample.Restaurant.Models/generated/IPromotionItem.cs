////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'une promotion en liste.
/// </summary>
public interface IPromotionItem
{
    /// <summary>
    /// Identifiant de la promotion.
    /// </summary>
    int? Id { get; }

    /// <summary>
    /// Libellé de la promotion.
    /// </summary>
    string Libelle { get; }

    /// <summary>
    /// Pourcentage de réduction (0-100).
    /// </summary>
    int? PourcentageReduction { get; }

    /// <summary>
    /// Date de début de la promotion.
    /// </summary>
    DateTime? DateDebut { get; }

    /// <summary>
    /// Date de fin de la promotion.
    /// </summary>
    DateTime? DateFin { get; }

    /// <summary>
    /// Indique si la promotion est active.
    /// </summary>
    bool? Active { get; }

    /// <summary>
    /// Restaurant concerné par la promotion (null si globale).
    /// </summary>
    int? RestaurantId { get; }

    /// <summary>
    /// Factory pour instancier la classe.
    /// </summary>
    /// <param name="id">Identifiant de la promotion.</param>
    /// <param name="libelle">Libellé de la promotion.</param>
    /// <param name="pourcentageReduction">Pourcentage de réduction (0-100).</param>
    /// <param name="dateDebut">Date de début de la promotion.</param>
    /// <param name="dateFin">Date de fin de la promotion.</param>
    /// <param name="active">Indique si la promotion est active.</param>
    /// <param name="restaurantId">Restaurant concerné par la promotion (null si globale).</param>
    /// <returns>Instance de la classe.</returns>
    static abstract IPromotionItem Create(int? id = null, string libelle = null, int? pourcentageReduction = null, DateTime? dateDebut = null, DateTime? dateFin = null, bool? active = null, int? restaurantId = null);
}
