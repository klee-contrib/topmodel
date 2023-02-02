////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using Kinetix.Services.Annotations;
using Models.CSharp.Securite.Models;

namespace CSharp.Clients.Db.Reference;

/// <summary>
/// This interface was automatically generated. It contains all the operations to load the reference lists declared in module Securite.
/// </summary>
[RegisterImpl]
public partial class SecuriteReferenceAccessors : ISecuriteReferenceAccessors
{
    private readonly CSharpDbContext _dbContext;

    /// <summary>
    /// Constructeur.
    /// </summary>
    /// <param name="dbContext">DbContext.</param>
    public SecuriteReferenceAccessors(CSharpDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc cref="ISecuriteReferenceAccessors.LoadDroits" />
    public ICollection<Droit> LoadDroits()
    {
        return _dbContext.Droits.OrderBy(row => row.Libelle).ToList();
    }

    /// <inheritdoc cref="ISecuriteReferenceAccessors.LoadTypeProfils" />
    public ICollection<TypeProfil> LoadTypeProfils()
    {
        return _dbContext.TypeProfils.OrderBy(row => row.Libelle).ToList();
    }
}
