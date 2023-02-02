////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using Kinetix.Services.Annotations;
using Models.CSharp.Securite.Models;

namespace CSharp.Clients.Db.Reference;

/// <summary>
/// This interface was automatically generated. It contains all the operations to load the reference lists declared in module Securite.
/// </summary>
[RegisterContract]
public partial interface ISecuriteReferenceAccessors
{
    /// <summary>
    /// Reference accessor for type Droit.
    /// </summary>
    /// <returns>List of Droit.</returns>
    [ReferenceAccessor]
    ICollection<Droit> LoadDroits();

    /// <summary>
    /// Reference accessor for type TypeProfil.
    /// </summary>
    /// <returns>List of TypeProfil.</returns>
    [ReferenceAccessor]
    ICollection<TypeProfil> LoadTypeProfils();
}
