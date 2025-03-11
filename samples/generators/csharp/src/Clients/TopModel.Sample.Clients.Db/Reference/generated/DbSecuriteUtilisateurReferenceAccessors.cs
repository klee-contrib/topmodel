////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using Kinetix.Services.Annotations;
using TopModel.Sample.Securite.Models.Utilisateur;

namespace TopModel.Sample.Clients.Db.Reference;

/// <summary>
/// Implémentation de IDbSecuriteUtilisateurReferenceAccessors.
/// </summary>
/// <param name="dbContext">DbContext.</param>
[RegisterImpl]
public partial class DbSecuriteUtilisateurReferenceAccessors(TopModelSampleDbContext dbContext) : IDbSecuriteUtilisateurReferenceAccessors
{
    /// <inheritdoc cref="IDbSecuriteUtilisateurReferenceAccessors.LoadTypeUtilisateurs" />
    public ICollection<TypeUtilisateur> LoadTypeUtilisateurs()
    {
        return dbContext.TypeUtilisateurs.OrderBy(row => row.Libelle).ToList();
    }
}
