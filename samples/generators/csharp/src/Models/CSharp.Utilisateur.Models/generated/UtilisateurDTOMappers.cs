////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace Models.CSharp.Utilisateur.Models;

/// <summary>
/// Mappers pour le module 'Utilisateur'.
/// </summary>
public static class UtilisateurDTOMappers
{
    /// <summary>
    /// Mappe 'UtilisateurDto' vers 'UtilisateurDto'.
    /// </summary>
    /// <param name="source">Instance de 'UtilisateurDto'.</param>
    /// <param name="dest">Instance pré-existante de 'UtilisateurDto'. Une nouvelle instance sera créée si non spécifié.</param>
    /// <returns>Une instance de 'UtilisateurDto'.</returns>
    public static UtilisateurDto ToUtilisateurDto(this UtilisateurDto source, UtilisateurDto dest = null)
    {
        dest ??= new UtilisateurDto();
        dest.Id = source.Id;
        dest.Age = source.Age;
        dest.ProfilId = source.ProfilId;
        dest.Email = source.Email;
        dest.Nom = source.Nom;
        dest.TypeUtilisateurCode = source.TypeUtilisateurCode;
        dest.DateCreation = source.DateCreation;
        dest.DateModification = source.DateModification;
        return dest;
    }
}
