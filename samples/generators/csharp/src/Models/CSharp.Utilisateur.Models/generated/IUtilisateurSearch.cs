////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////


namespace Models.CSharp.Utilisateur.Models;

/// <summary>
/// Classe abstraite servant à faire des projections spring.
/// </summary>
public interface IUtilisateurSearch
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
    string Email { get; }

    /// <summary>
    /// Nom de l'utilisateur.
    /// </summary>
    string Nom { get; }

    /// <summary>
    /// Si l'utilisateur est actif.
    /// </summary>
    bool? Actif { get; }

    /// <summary>
    /// Type d'utilisateur en Many to one.
    /// </summary>
    TypeUtilisateur.Codes? TypeUtilisateurCode { get; }

    /// <summary>
    /// Utilisateur enfants.
    /// </summary>
    int[] UtilisateursEnfant { get; }

    /// <summary>
    /// Date de création de l'utilisateur.
    /// </summary>
    DateOnly? DateCreation { get; }

    /// <summary>
    /// Date de modification de l'utilisateur.
    /// </summary>
    DateOnly? DateModification { get; }
}
