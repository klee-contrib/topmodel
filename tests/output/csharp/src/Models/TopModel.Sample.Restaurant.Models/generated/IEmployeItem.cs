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
    /// Identifiant de l'employé.
    /// </summary>
    int? Id { get; set; }

    /// <summary>
    /// Nom de l'employé.
    /// </summary>
    string? Nom { get; set; }

    /// <summary>
    /// Prénom de l'employé.
    /// </summary>
    string? Prenom { get; set; }

    /// <summary>
    /// Matricule de l'employé.
    /// </summary>
    string? Matricule { get; set; }

    /// <summary>
    /// Restaurant où travaille l'employé.
    /// </summary>
    int? RestaurantId { get; set; }
}
