////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'une ligne de commande en liste.
/// </summary>
public interface ILigneCommandeItem
{
    /// <summary>
    /// Identifiant de la ligne.
    /// </summary>
    int? Id { get; }

    /// <summary>
    /// Quantité commandée.
    /// </summary>
    int? Quantite { get; }

    /// <summary>
    /// Prix unitaire au moment de la commande.
    /// </summary>
    decimal? PrixUnitaire { get; }

    /// <summary>
    /// Prix total de la ligne.
    /// </summary>
    decimal? PrixTotal { get; }

    /// <summary>
    /// Commande à laquelle appartient la ligne.
    /// </summary>
    int? CommandeId { get; }

    /// <summary>
    /// Plat commandé.
    /// </summary>
    int? PlatId { get; }

    /// <summary>
    /// Factory pour instancier la classe.
    /// </summary>
    /// <param name="id">Identifiant de la ligne.</param>
    /// <param name="quantite">Quantité commandée.</param>
    /// <param name="prixUnitaire">Prix unitaire au moment de la commande.</param>
    /// <param name="prixTotal">Prix total de la ligne.</param>
    /// <param name="commandeId">Commande à laquelle appartient la ligne.</param>
    /// <param name="platId">Plat commandé.</param>
    /// <returns>Instance de la classe.</returns>
    static abstract ILigneCommandeItem Create(int? id = null, int? quantite = null, decimal? prixUnitaire = null, decimal? prixTotal = null, int? commandeId = null, int? platId = null);
}
