////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using Kinetix.Services.Annotations;
using Models.CSharp.Securite.Utilisateur.Models;

namespace CSharp.Clients.Db.Reference;

/// <summary>
/// This interface was automatically generated. It contains all the operations to load the reference lists declared in module Securite.Utilisateur.
/// </summary>
[RegisterContract]
public partial interface ISecuriteUtilisateurReferenceAccessors
{
    /// <summary>
    /// Reference accessor for type TypeUtilisateur.
    /// </summary>
    /// <returns>List of TypeUtilisateur.</returns>
    [ReferenceAccessor]
    ICollection<TypeUtilisateur> LoadTypeUtilisateurs();
}
