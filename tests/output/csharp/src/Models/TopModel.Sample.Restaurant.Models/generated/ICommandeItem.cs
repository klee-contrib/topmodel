////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'une commande en liste.
/// </summary>
public interface ICommandeItem
{
    /// <summary>
    /// Identifiant de la commande.
    /// </summary>
    int? Id { get; set; }

    /// <summary>
    /// Date et heure de la commande.
    /// </summary>
    DateTime? DateCommande { get; set; }

    /// <summary>
    /// Montant total de la commande.
    /// </summary>
    decimal? MontantTotal { get; set; }

    /// <summary>
    /// Statut de la commande.
    /// </summary>
    StatutCommande? StatutCommande { get; set; }

    /// <summary>
    /// Client ayant passé la commande.
    /// </summary>
    int? ClientId { get; set; }
}
