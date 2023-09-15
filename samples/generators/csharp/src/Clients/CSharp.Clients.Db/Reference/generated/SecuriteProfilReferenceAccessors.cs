////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using Kinetix.Services.Annotations;
using Models.CSharp.Securite.Profil.Models;

namespace CSharp.Clients.Db.Reference;

/// <summary>
/// This interface was automatically generated. It contains all the operations to load the reference lists declared in module Securite.Profil.
/// </summary>
[RegisterImpl]
public partial class SecuriteProfilReferenceAccessors : ISecuriteProfilReferenceAccessors
{
    private readonly CSharpDbContext _dbContext;

    /// <summary>
    /// Constructeur.
    /// </summary>
    /// <param name="dbContext">DbContext.</param>
    public SecuriteProfilReferenceAccessors(CSharpDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc cref="ISecuriteProfilReferenceAccessors.LoadDroits" />
    public ICollection<Droit> LoadDroits()
    {
        return _dbContext.Droits.OrderBy(row => row.Code).ToList();
    }

    /// <inheritdoc cref="ISecuriteProfilReferenceAccessors.LoadTypeDroits" />
    public ICollection<TypeDroit> LoadTypeDroits()
    {
        return _dbContext.TypeDroits.OrderBy(row => row.Libelle).ToList();
    }
}
