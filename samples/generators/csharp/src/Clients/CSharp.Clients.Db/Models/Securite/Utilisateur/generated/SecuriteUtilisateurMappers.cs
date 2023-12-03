////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using Models.CSharp.Securite.Utilisateur.Models;

namespace CSharp.Clients.Db.Models.Securite.Utilisateur;

/// <summary>
/// Mappers pour le module 'Securite.Utilisateur'.
/// </summary>
public static class SecuriteUtilisateurMappers
{
    /// <summary>
    /// Crée une nouvelle instance de 'UtilisateurRead'.
    /// </summary>
    /// <param name="utilisateur">Instance de 'Utilisateur'.</param>
    /// <returns>Une nouvelle instance de 'UtilisateurRead'.</returns>
    public static UtilisateurRead CreateUtilisateurRead(Utilisateur utilisateur)
    {
        ArgumentNullException.ThrowIfNull(utilisateur);

        return new UtilisateurRead
        {
            Id = utilisateur.Id,
            Nom = utilisateur.Nom,
            Prenom = utilisateur.Prenom,
            Email = utilisateur.Email,
            DateNaissance = utilisateur.DateNaissance,
            Adresse = utilisateur.Adresse,
            Actif = utilisateur.Actif,
            ProfilId = utilisateur.ProfilId,
            TypeUtilisateurCode = utilisateur.TypeUtilisateurCode,
            DateCreation = utilisateur.DateCreation,
            DateModification = utilisateur.DateModification
        };
    }

    /// <summary>
    /// Mappe 'UtilisateurWrite' vers 'Utilisateur'.
    /// </summary>
    /// <param name="source">Instance de 'UtilisateurWrite'.</param>
    /// <param name="dest">Instance pré-existante de 'Utilisateur'. Une nouvelle instance sera créée si non spécifié.</param>
    /// <returns>Une instance de 'Utilisateur'.</returns>
    public static Utilisateur ToUtilisateur(this UtilisateurWrite source, Utilisateur dest = null)
    {
        dest ??= new Utilisateur();
        dest.Nom = source.Nom;
        dest.Prenom = source.Prenom;
        dest.Email = source.Email;
        dest.DateNaissance = source.DateNaissance;
        dest.Adresse = source.Adresse;
        dest.Actif = source.Actif;
        dest.ProfilId = source.ProfilId;
        dest.TypeUtilisateurCode = source.TypeUtilisateurCode;
        return dest;
    }
}
