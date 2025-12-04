////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'un client en liste.
/// </summary>
public interface IClientItem
{
    /// <summary>
    /// Identifiant du client.
    /// </summary>
    int? Id { get; }

    /// <summary>
    /// Nom du client.
    /// </summary>
    string Nom { get; }

    /// <summary>
    /// Prénom du client.
    /// </summary>
    string Prenom { get; }

    /// <summary>
    /// Adresse email du client.
    /// </summary>
    string Email { get; }

    /// <summary>
    /// Factory pour instancier la classe.
    /// </summary>
    /// <param name="id">Identifiant du client.</param>
    /// <param name="nom">Nom du client.</param>
    /// <param name="prenom">Prénom du client.</param>
    /// <param name="email">Adresse email du client.</param>
    /// <returns>Instance de la classe.</returns>
    static abstract IClientItem Create(int? id = null, string nom = null, string prenom = null, string email = null);
}
