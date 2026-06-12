////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Définition d'une personne.
/// </summary>
public partial interface IEmployeBase : IPersonneBase
{
    /// <summary>
    /// Numéro de téléphone de l'employé.
    /// </summary>
    string? Telephone { get; set; }
}
