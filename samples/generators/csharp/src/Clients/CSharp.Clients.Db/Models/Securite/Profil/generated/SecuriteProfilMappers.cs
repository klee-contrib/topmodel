////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using Models.CSharp.Securite.Profil.Models;

namespace CSharp.Clients.Db.Models.Securite.Profil;

/// <summary>
/// Mappers pour le module 'Securite.Profil'.
/// </summary>
public static class SecuriteProfilMappers
{
    /// <summary>
    /// Crée une nouvelle instance de 'ProfilRead'.
    /// </summary>
    /// <param name="profil">Instance de 'Profil'.</param>
    /// <returns>Une nouvelle instance de 'ProfilRead'.</returns>
    public static ProfilRead CreateProfilRead(Profil profil)
    {
        if (profil is null)
        {
            throw new ArgumentNullException(nameof(profil));
        }

        return new ProfilRead
        {
            Id = profil.Id,
            Libelle = profil.Libelle,
            Droits = profil.Droits,
            DateCreation = profil.DateCreation,
            DateModification = profil.DateModification
        };
    }

    /// <summary>
    /// Mappe 'Profil' vers 'Profil'.
    /// </summary>
    /// <param name="source">Instance de 'Profil'.</param>
    /// <param name="dest">Instance pré-existante de 'Profil'. Une nouvelle instance sera créée si non spécifié.</param>
    /// <returns>Une instance de 'Profil'.</returns>
    public static Profil ToProfil(this Profil source, Profil dest = null)
    {
        dest ??= new Profil();
        dest.Libelle = source.Libelle;
        dest.Droits = source.Droits;
        return dest;
    }

    /// <summary>
    /// Mappe 'ProfilWrite' vers 'Profil'.
    /// </summary>
    /// <param name="source">Instance de 'ProfilWrite'.</param>
    /// <param name="dest">Instance pré-existante de 'Profil'. Une nouvelle instance sera créée si non spécifié.</param>
    /// <returns>Une instance de 'Profil'.</returns>
    public static Profil ToProfil(this ProfilWrite source, Profil dest = null)
    {
        dest ??= new Profil();
        dest.Libelle = source.Libelle;
        dest.Droits = source.Droits;
        return dest;
    }
}
