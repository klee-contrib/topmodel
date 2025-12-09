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
    int? Id { get; }

    /// <summary>
    /// Date et heure de la commande.
    /// </summary>
    DateTime? DateCommande { get; }

    /// <summary>
    /// Montant total de la commande.
    /// </summary>
    decimal? MontantTotal { get; }

    /// <summary>
    /// Statut de la commande.
    /// </summary>
    StatutCommande.Codes? StatutCommandeCode { get; }

    /// <summary>
    /// Client ayant passé la commande.
    /// </summary>
    int? ClientId { get; }

    /// <summary>
    /// Factory pour instancier la classe.
    /// </summary>
    /// <param name="id">Identifiant de la commande.</param>
    /// <param name="dateCommande">Date et heure de la commande.</param>
    /// <param name="montantTotal">Montant total de la commande.</param>
    /// <param name="statutCommandeCode">Statut de la commande.</param>
    /// <param name="clientId">Client ayant passé la commande.</param>
    /// <returns>Instance de la classe.</returns>
    static abstract ICommandeItem Create(int? id = null, DateTime? dateCommande = null, decimal? montantTotal = null, StatutCommande.Codes? statutCommandeCode = null, int? clientId = null);
}
