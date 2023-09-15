////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using Kinetix.Services.Annotations;
using Models.CSharp.Securite.Utilisateur.Models;

namespace CSharp.Clients.Db.Reference;

/// <summary>
/// This interface was automatically generated. It contains all the operations to load the reference lists declared in module Securite.Utilisateur.
/// </summary>
[RegisterImpl]
public partial class SecuriteUtilisateurReferenceAccessors : ISecuriteUtilisateurReferenceAccessors
{
    private readonly CSharpDbContext _dbContext;

    /// <summary>
    /// Constructeur.
    /// </summary>
    /// <param name="dbContext">DbContext.</param>
    public SecuriteUtilisateurReferenceAccessors(CSharpDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc cref="ISecuriteUtilisateurReferenceAccessors.LoadTypeUtilisateurs" />
    public ICollection<TypeUtilisateur> LoadTypeUtilisateurs()
    {
        return _dbContext.TypeUtilisateurs.OrderBy(row => row.Libelle).ToList();
    }
}
