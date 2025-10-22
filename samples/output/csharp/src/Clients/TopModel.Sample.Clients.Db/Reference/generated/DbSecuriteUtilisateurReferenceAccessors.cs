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
        return (
            from row in dbContext.TypeUtilisateurs
            join tra in dbContext.Traductions on row.Libelle equals tra.ResourceKey
            orderby row.Libelle
            select new TypeUtilisateur
            {
                Code = row.Code,
                Libelle = tra.Label
            }
        ).ToList();
    }
}
