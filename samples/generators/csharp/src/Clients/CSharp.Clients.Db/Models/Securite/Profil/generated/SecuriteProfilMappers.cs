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
        ArgumentNullException.ThrowIfNull(profil);

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
    /// <returns>Une nouvelle instance de 'Profil'.</returns>
    public static Profil ToProfil(this Profil source)
    {
        return new Profil
        {
            Libelle = source.Libelle,
            Droits = source.Droits
        };
    }

    /// <summary>
    /// Mappe 'Profil' vers 'Profil'.
    /// </summary>
    /// <param name="source">Instance de 'Profil'.</param>
    /// <param name="dest">Instance pré-existante de 'Profil'.</param>
    /// <returns>L'instance pré-existante de 'Profil'.</returns>
    public static Profil ToProfil(this Profil source, Profil dest)
    {
        dest.Libelle = source.Libelle;
        dest.Droits = source.Droits;
        return dest;
    }

    /// <summary>
    /// Mappe 'ProfilWrite' vers 'Profil'.
    /// </summary>
    /// <param name="source">Instance de 'ProfilWrite'.</param>
    /// <returns>Une nouvelle instance de 'Profil'.</returns>
    public static Profil ToProfil(this ProfilWrite source)
    {
        return new Profil
        {
            Libelle = source.Libelle,
            Droits = source.Droits
        };
    }

    /// <summary>
    /// Mappe 'ProfilWrite' vers 'Profil'.
    /// </summary>
    /// <param name="source">Instance de 'ProfilWrite'.</param>
    /// <param name="dest">Instance pré-existante de 'Profil'.</param>
    /// <returns>L'instance pré-existante de 'Profil'.</returns>
    public static Profil ToProfil(this ProfilWrite source, Profil dest)
    {
        dest.Libelle = source.Libelle;
        dest.Droits = source.Droits;
        return dest;
    }
}
