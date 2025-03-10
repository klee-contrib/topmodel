////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using Kinetix.Services.Annotations;
using TopModel.Sample.Securite.Models.Utilisateur;

namespace TopModel.Sample.Clients.Db.Reference;

/// <summary>
/// Accesseurs de listes de référence persistées.
/// </summary>
[RegisterContract]
public partial interface IDbSecuriteUtilisateurReferenceAccessors
{
    /// <summary>
    /// Accesseur de référence pour le type TypeUtilisateur.
    /// </summary>
    /// <returns>Liste de TypeUtilisateur.</returns>
    [ReferenceAccessor]
    ICollection<TypeUtilisateur> LoadTypeUtilisateurs();
}
