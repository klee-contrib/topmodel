////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////


namespace Models.CSharp.Utilisateur.Models;

/// <summary>
/// Objet non persisté de communication avec le serveur.
/// </summary>
public interface IIUtilisateur
{
    /// <summary>
    /// Id technique.
    /// </summary>
    int? Id { get; }

    /// <summary>
    /// Age en années de l'utilisateur.
    /// </summary>
    decimal? Age { get; }

    /// <summary>
    /// Profil de l'utilisateur.
    /// </summary>
    int? ProfilId { get; }

    /// <summary>
    /// Email de l'utilisateur.
    /// </summary>
    string email { get; }

    /// <summary>
    /// Nom de l'utilisateur.
    /// </summary>
    string Nom { get; }

    /// <summary>
    /// Type d'utilisateur en Many to one.
    /// </summary>
    TypeUtilisateur.Codes? TypeUtilisateurCode { get; }

    /// <summary>
    /// Date de création de l'utilisateur.
    /// </summary>
    DateOnly? dateCreation { get; }

    /// <summary>
    /// Date de modification de l'utilisateur.
    /// </summary>
    DateOnly? dateModification { get; }

    /// <summary>
    /// Factory pour instancier la classe.
    /// </summary>
    /// <param name="id">Id technique.</param>
    /// <param name="age">Age en années de l'utilisateur.</param>
    /// <param name="profilId">Profil de l'utilisateur.</param>
    /// <param name="email">Email de l'utilisateur.</param>
    /// <param name="nom">Nom de l'utilisateur.</param>
    /// <param name="typeUtilisateurCode">Type d'utilisateur en Many to one.</param>
    /// <param name="dateCreation">Date de création de l'utilisateur.</param>
    /// <param name="dateModification">Date de modification de l'utilisateur.</param>
    /// <returns>Instance de la classe.</returns>
    static abstract IIUtilisateur Create(int? id = null, decimal? age = null, int? profilId = null, string email = null, string nom = null, TypeUtilisateur.Codes? typeUtilisateurCode = null, DateOnly? dateCreation = null, DateOnly? dateModification = null);
}
