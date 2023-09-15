////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////


namespace Models.CSharp.Securite.Utilisateur.Models;

/// <summary>
/// Détail d'un utilisateur en liste.
/// </summary>
public interface IUtilisateurItem
{
    /// <summary>
    /// Id de l'utilisateur.
    /// </summary>
    int? Id { get; }

    /// <summary>
    /// Nom de l'utilisateur.
    /// </summary>
    string Nom { get; }

    /// <summary>
    /// Nom de l'utilisateur.
    /// </summary>
    string Prenom { get; }

    /// <summary>
    /// Email de l'utilisateur.
    /// </summary>
    string Email { get; }

    /// <summary>
    /// Type d'utilisateur.
    /// </summary>
    TypeUtilisateur.Codes? TypeUtilisateurCode { get; }

    /// <summary>
    /// Factory pour instancier la classe.
    /// </summary>
    /// <param name="id">Id de l'utilisateur.</param>
    /// <param name="nom">Nom de l'utilisateur.</param>
    /// <param name="prenom">Nom de l'utilisateur.</param>
    /// <param name="email">Email de l'utilisateur.</param>
    /// <param name="typeUtilisateurCode">Type d'utilisateur.</param>
    /// <returns>Instance de la classe.</returns>
    static abstract IUtilisateurItem Create(int? id = null, string nom = null, string prenom = null, string email = null, TypeUtilisateur.Codes? typeUtilisateurCode = null);
}
