////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'une table en liste.
/// </summary>
public interface ITableItem
{
    /// <summary>
    /// Identifiant de la table.
    /// </summary>
    int? Id { get; }

    /// <summary>
    /// Numéro de la table.
    /// </summary>
    string? Numero { get; }

    /// <summary>
    /// Capacité de la table (nombre de places).
    /// </summary>
    int? Capacite { get; }

    /// <summary>
    /// Indique si la table est disponible.
    /// </summary>
    bool? Disponible { get; }

    /// <summary>
    /// Restaurant auquel appartient la table.
    /// </summary>
    int? RestaurantId { get; }

    /// <summary>
    /// Factory pour instancier la classe.
    /// </summary>
    /// <param name="id">Identifiant de la table.</param>
    /// <param name="numero">Numéro de la table.</param>
    /// <param name="capacite">Capacité de la table (nombre de places).</param>
    /// <param name="disponible">Indique si la table est disponible.</param>
    /// <param name="restaurantId">Restaurant auquel appartient la table.</param>
    /// <returns>Instance de la classe.</returns>
    static abstract ITableItem Create(int? id = null, string? numero = null, int? capacite = null, bool? disponible = null, int? restaurantId = null);
}
