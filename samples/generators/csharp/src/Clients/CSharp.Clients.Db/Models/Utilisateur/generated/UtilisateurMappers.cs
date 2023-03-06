////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using Models.CSharp.Utilisateur.Models;

namespace CSharp.Clients.Db.Models.Utilisateur;

/// <summary>
/// Mappers pour le module 'Utilisateur'.
/// </summary>
public static class UtilisateurMappers
{
    /// <summary>
    /// Crée une nouvelle instance de 'UtilisateurDto'.
    /// </summary>
    /// <param name="utilisateur">Instance de 'Utilisateur'.</param>
    /// <returns>Une nouvelle instance de 'UtilisateurDto'.</returns>
    public static UtilisateurDto CreateUtilisateurDto(Utilisateur utilisateur)
    {
        if (utilisateur is null)
        {
            throw new ArgumentNullException(nameof(utilisateur));
        }

        return new UtilisateurDto
        {
            UtilisateurParent = new() { Id = utilisateur.UtilisateurIdParent },
            Id = utilisateur.Id,
            Age = utilisateur.Age,
            ProfilId = utilisateur.ProfilId,
            Email = utilisateur.Email,
            Nom = utilisateur.Nom,
            Actif = utilisateur.Actif,
            TypeUtilisateurCode = utilisateur.TypeUtilisateurCode,
            DateCreation = utilisateur.DateCreation,
            DateModification = utilisateur.DateModification
        };
    }

    /// <summary>
    /// Mappe 'UtilisateurDto' vers 'Utilisateur'.
    /// </summary>
    /// <param name="source">Instance de 'UtilisateurDto'.</param>
    /// <param name="dest">Instance pré-existante de 'Utilisateur'. Une nouvelle instance sera créée si non spécifié.</param>
    /// <returns>Une instance de 'Utilisateur'.</returns>
    public static Utilisateur ToUtilisateur(this UtilisateurDto source, Utilisateur dest = null)
    {
        dest ??= new Utilisateur();
        dest.UtilisateurIdParent = source.UtilisateurParent?.Id;
        dest.Id = source.Id;
        dest.Age = source.Age;
        dest.ProfilId = source.ProfilId;
        dest.Email = source.Email;
        dest.Nom = source.Nom;
        dest.Actif = source.Actif;
        dest.TypeUtilisateurCode = source.TypeUtilisateurCode;
        dest.DateCreation = source.DateCreation;
        dest.DateModification = source.DateModification;
        return dest;
    }
}
