////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Résumé d'une commande pour liste.
/// </summary>
public interface ICommandeResume
{
    /// <summary>
    /// Identifiant de la commande.
    /// </summary>
    int? Id { get; }

    /// <summary>
    /// Factory pour instancier la classe.
    /// </summary>
    /// <param name="id">Identifiant de la commande.</param>
    /// <returns>Instance de la classe.</returns>
    static abstract ICommandeResume Create(int? id = null);
}
