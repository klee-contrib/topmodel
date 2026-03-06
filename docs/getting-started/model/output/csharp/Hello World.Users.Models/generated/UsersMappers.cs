////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using Hello World.Refs.Models;

namespace Hello World.Users.Models;

/// <summary>
/// Mappers pour le module 'Users'.
/// </summary>
public static class UsersMappers
{
    /// <summary>
    /// Crée une nouvelle instance de 'UtilisateurDetailDto'.
    /// </summary>
    /// <param name="utilisateur">Instance de 'Utilisateur'.</param>
    /// <param name="typeUtilisateur">Instance de 'TypeUtilisateur'.</param>
    /// <returns>Une nouvelle instance de 'UtilisateurDetailDto'.</returns>
    public static UtilisateurDetailDto CreateUtilisateurDetailDto(Utilisateur utilisateur, TypeUtilisateur typeUtilisateur)
    {
        ArgumentNullException.ThrowIfNull(utilisateur);
        ArgumentNullException.ThrowIfNull(typeUtilisateur);

        return new UtilisateurDetailDto
        {
            Email = utilisateur.Email,
            Nom = utilisateur.Nom,
            DateInscription = utilisateur.DateInscription,
            TypeUtilisateurCode = utilisateur.TypeUtilisateurCode,
            LibelleTypeUtilisateur = typeUtilisateur.Libelle
        };
    }

    /// <summary>
    /// Crée une nouvelle instance de 'UtilisateurSearchResultDto'.
    /// </summary>
    /// <param name="utilisateur">Instance de 'Utilisateur'.</param>
    /// <param name="typeUtilisateur">Instance de 'TypeUtilisateur'.</param>
    /// <returns>Une nouvelle instance de 'UtilisateurSearchResultDto'.</returns>
    public static UtilisateurSearchResultDto CreateUtilisateurSearchResultDto(Utilisateur utilisateur, TypeUtilisateur typeUtilisateur)
    {
        ArgumentNullException.ThrowIfNull(utilisateur);
        ArgumentNullException.ThrowIfNull(typeUtilisateur);

        return new UtilisateurSearchResultDto
        {
            Email = utilisateur.Email,
            Nom = utilisateur.Nom,
            DateInscription = utilisateur.DateInscription,
            TypeUtilisateurCode = utilisateur.TypeUtilisateurCode,
            LibelleTypeUtilisateur = typeUtilisateur.Libelle
        };
    }

    /// <summary>
    /// Mappe 'UtilisateurCreateDto' vers 'Utilisateur'.
    /// </summary>
    /// <param name="source">Instance de 'UtilisateurCreateDto'.</param>
    /// <param name="id">Identifiant unique de l'utilisateur.</param>
    /// <returns>Une nouvelle instance de 'Utilisateur'.</returns>
    public static Utilisateur ToUtilisateur(this UtilisateurCreateDto source, long? id = null)
    {
        return new Utilisateur
        {
            Email = source.UtilisateurEmail,
            Nom = source.UtilisateurNom,
            DateInscription = source.UtilisateurDateInscription,
            TypeUtilisateurCode = source.UtilisateurTypeUtilisateurCode,
            Id = id
        };
    }

    /// <summary>
    /// Mappe 'UtilisateurCreateDto' vers 'Utilisateur'.
    /// </summary>
    /// <param name="source">Instance de 'UtilisateurCreateDto'.</param>
    /// <param name="dest">Instance pré-existante de 'Utilisateur'.</param>
    /// <returns>L'instance pré-existante de 'Utilisateur'.</returns>
    public static Utilisateur ToUtilisateur(this UtilisateurCreateDto source, Utilisateur dest)
    {
        dest.Email = source.UtilisateurEmail;
        dest.Nom = source.UtilisateurNom;
        dest.DateInscription = source.UtilisateurDateInscription;
        dest.TypeUtilisateurCode = source.UtilisateurTypeUtilisateurCode;
        return dest;
    }

    /// <summary>
    /// Mappe 'UtilisateurUpdateDto' vers 'Utilisateur'.
    /// </summary>
    /// <param name="source">Instance de 'UtilisateurUpdateDto'.</param>
    /// <param name="id">Identifiant unique de l'utilisateur.</param>
    /// <param name="email">Adresse mail de l'utilisateur.</param>
    /// <returns>Une nouvelle instance de 'Utilisateur'.</returns>
    public static Utilisateur ToUtilisateur(this UtilisateurUpdateDto source, long? id = null, string email = null)
    {
        return new Utilisateur
        {
            Nom = source.Nom,
            Id = id,
            Email = email
        };
    }

    /// <summary>
    /// Mappe 'UtilisateurUpdateDto' vers 'Utilisateur'.
    /// </summary>
    /// <param name="source">Instance de 'UtilisateurUpdateDto'.</param>
    /// <param name="dest">Instance pré-existante de 'Utilisateur'.</param>
    /// <returns>L'instance pré-existante de 'Utilisateur'.</returns>
    public static Utilisateur ToUtilisateur(this UtilisateurUpdateDto source, Utilisateur dest)
    {
        dest.Nom = source.Nom;
        return dest;
    }
}
