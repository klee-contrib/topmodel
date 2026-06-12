////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Définition d'une personne.
/// </summary>
public partial interface IPersonneBase
{
    /// <summary>
    /// Identifiant de la personne.
    /// </summary>
    int? Id { get; set; }

    /// <summary>
    /// Nom de la personne.
    /// </summary>
    string? Nom { get; set; }

    /// <summary>
    /// Prénom de la personne.
    /// </summary>
    string? Prenom { get; set; }
}
