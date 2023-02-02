////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using Kinetix.Services.Annotations;
using Models.CSharp.Utilisateur.Models;

namespace CSharp.Clients.Db.Reference;

/// <summary>
/// This interface was automatically generated. It contains all the operations to load the reference lists declared in module Utilisateur.
/// </summary>
[RegisterImpl]
public partial class UtilisateurReferenceAccessors : IUtilisateurReferenceAccessors
{
    private readonly CSharpDbContext _dbContext;

    /// <summary>
    /// Constructeur.
    /// </summary>
    /// <param name="dbContext">DbContext.</param>
    public UtilisateurReferenceAccessors(CSharpDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc cref="IUtilisateurReferenceAccessors.LoadTypeUtilisateurs" />
    public ICollection<TypeUtilisateur> LoadTypeUtilisateurs()
    {
        return _dbContext.TypeUtilisateurs.OrderBy(row => row.Libelle).ToList();
    }
}
