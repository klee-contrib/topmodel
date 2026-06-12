////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'une table en liste.
/// </summary>
public partial interface ITableItem
{
    /// <summary>
    /// Identifiant de la table.
    /// </summary>
    int? Id { get; set; }

    /// <summary>
    /// Numéro de la table.
    /// </summary>
    string? Numero { get; set; }

    /// <summary>
    /// Capacité de la table (nombre de places).
    /// </summary>
    int? Capacite { get; set; }

    /// <summary>
    /// Indique si la table est disponible.
    /// </summary>
    bool? Disponible { get; set; }

    /// <summary>
    /// Restaurant auquel appartient la table.
    /// </summary>
    int? RestaurantId { get; set; }
}
