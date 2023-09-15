////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using Kinetix.Services.Annotations;
using Models.CSharp.Securite.Profil.Models;

namespace CSharp.Clients.Db.Reference;

/// <summary>
/// This interface was automatically generated. It contains all the operations to load the reference lists declared in module Securite.Profil.
/// </summary>
[RegisterContract]
public partial interface ISecuriteProfilReferenceAccessors
{
    /// <summary>
    /// Reference accessor for type Droit.
    /// </summary>
    /// <returns>List of Droit.</returns>
    [ReferenceAccessor]
    ICollection<Droit> LoadDroits();

    /// <summary>
    /// Reference accessor for type TypeDroit.
    /// </summary>
    /// <returns>List of TypeDroit.</returns>
    [ReferenceAccessor]
    ICollection<TypeDroit> LoadTypeDroits();
}
