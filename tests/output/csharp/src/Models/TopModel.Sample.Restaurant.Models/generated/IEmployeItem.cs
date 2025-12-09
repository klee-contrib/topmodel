////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'un employé en liste.
/// </summary>
public interface IEmployeItem
{
    /// <summary>
    /// Matricule de l'employé.
    /// </summary>
    string Matricule { get; }

    /// <summary>
    /// Restaurant où travaille l'employé.
    /// </summary>
    int? RestaurantIdRestaurant { get; }

    /// <summary>
    /// Identifiant de l'employé.
    /// </summary>
    int? Id { get; }

    /// <summary>
    /// Nom de l'employé.
    /// </summary>
    string Nom { get; }

    /// <summary>
    /// Prénom de l'employé.
    /// </summary>
    string Prenom { get; }

    /// <summary>
    /// Factory pour instancier la classe.
    /// </summary>
    /// <param name="matricule">Matricule de l'employé.</param>
    /// <param name="restaurantIdRestaurant">Restaurant où travaille l'employé.</param>
    /// <param name="id">Identifiant de l'employé.</param>
    /// <param name="nom">Nom de l'employé.</param>
    /// <param name="prenom">Prénom de l'employé.</param>
    /// <returns>Instance de la classe.</returns>
    static abstract IEmployeItem Create(string matricule = null, int? restaurantIdRestaurant = null, int? id = null, string nom = null, string prenom = null);
}
