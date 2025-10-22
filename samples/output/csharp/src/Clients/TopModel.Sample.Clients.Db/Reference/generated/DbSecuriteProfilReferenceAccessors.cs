////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using Kinetix.Services.Annotations;
using TopModel.Sample.Securite.Models.Profil;

namespace TopModel.Sample.Clients.Db.Reference;

/// <summary>
/// Implémentation de IDbSecuriteProfilReferenceAccessors.
/// </summary>
/// <param name="dbContext">DbContext.</param>
[RegisterImpl]
public partial class DbSecuriteProfilReferenceAccessors(TopModelSampleDbContext dbContext) : IDbSecuriteProfilReferenceAccessors
{
    /// <inheritdoc cref="IDbSecuriteProfilReferenceAccessors.LoadDroits" />
    public ICollection<Droit> LoadDroits()
    {
        return (
            from row in dbContext.Droits
            join tra in dbContext.Traductions on row.Libelle equals tra.ResourceKey
            orderby row.Code
            select new Droit
            {
                Code = row.Code,
                Libelle = tra.Label,
                TypeDroitCode = row.TypeDroitCode
            }
        ).ToList();
    }

    /// <inheritdoc cref="IDbSecuriteProfilReferenceAccessors.LoadTypeDroits" />
    public ICollection<TypeDroit> LoadTypeDroits()
    {
        return (
            from row in dbContext.TypeDroits
            join tra in dbContext.Traductions on row.Libelle equals tra.ResourceKey
            orderby row.Libelle
            select new TypeDroit
            {
                Code = row.Code,
                Libelle = tra.Label
            }
        ).ToList();
    }
}
