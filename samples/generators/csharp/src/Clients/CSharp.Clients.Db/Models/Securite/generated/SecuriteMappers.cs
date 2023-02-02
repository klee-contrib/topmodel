////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using Models.CSharp.Securite.Models;

namespace CSharp.Clients.Db.Models.Securite;

/// <summary>
/// Mappers pour le module 'Securite'.
/// </summary>
public static class SecuriteMappers
{
    /// <summary>
    /// Crée une nouvelle instance de 'ProfilDto'.
    /// </summary>
    /// <param name="profil">Instance de 'Profil'.</param>
    /// <returns>Une nouvelle instance de 'ProfilDto'.</returns>
    public static ProfilDto CreateProfilDto(Profil profil)
    {
        if (profil is null)
        {
            throw new ArgumentNullException(nameof(profil));
        }

        return new ProfilDto
        {
            Id = profil.Id,
            TypeProfilCode = profil.TypeProfilCode,
            Droits = profil.Droits,
            Secteurs = profil.Secteurs
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
        dest.Id = source.Id;
        dest.TypeProfilCode = source.TypeProfilCode;
        dest.Droits = source.Droits;
        dest.Secteurs = source.Secteurs;
        return dest;
    }

    /// <summary>
    /// Mappe 'ProfilDto' vers 'Profil'.
    /// </summary>
    /// <param name="source">Instance de 'ProfilDto'.</param>
    /// <param name="dest">Instance pré-existante de 'Profil'. Une nouvelle instance sera créée si non spécifié.</param>
    /// <returns>Une instance de 'Profil'.</returns>
    public static Profil ToProfil(this ProfilDto source, Profil dest = null)
    {
        dest ??= new Profil();
        dest.Id = source.Id;
        dest.TypeProfilCode = source.TypeProfilCode;
        dest.Droits = source.Droits;
        dest.Secteurs = source.Secteurs;
        return dest;
    }
}
