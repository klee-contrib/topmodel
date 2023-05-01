////
//// ATTENTION, CE FICHIER EST PARTIELLEMENT GENERE AUTOMATIQUEMENT !
////

using Microsoft.AspNetCore.Mvc;

namespace CSharp.Api.Securite.Utilisateur;

public class UtilisateurApiController : Controller
{

    /// <summary>
    /// Recherche des utilisateurs
    /// </summary>
    /// <param name="utiId">Id technique</param>
    /// <returns>Task.</returns>
    [HttpDelete("utilisateur/deleteAll")]
    public async Task DeleteAll(int[] utiId = null)
    {

    }

    /// <summary>
    /// Charge le détail d'un utilisateur
    /// </summary>
    /// <param name="utiId">Id technique</param>
    /// <returns>Le détail de l'utilisateur</returns>
    [HttpGet("utilisateur/{utiId:int}")]
    public async Task<UtilisateurDto> Find(int utiId)
    {

    }

    /// <summary>
    /// Charge une liste d'utilisateurs par leur type
    /// </summary>
    /// <param name="typeUtilisateurCode">Type d'utilisateur en Many to one</param>
    /// <returns>Liste des utilisateurs</returns>
    [HttpGet("utilisateur/list")]
    public async Task<IEnumerable<UtilisateurSearch>> FindAllByType(TypeUtilisateur.Codes typeUtilisateurCode = TypeUtilisateur.Codes.ADM)
    {

    }

    /// <summary>
    /// Sauvegarde un utilisateur
    /// </summary>
    /// <param name="utilisateur">Utilisateur à sauvegarder</param>
    /// <returns>Utilisateur sauvegardé</returns>
    [HttpPost("utilisateur/save")]
    public async Task<UtilisateurDto> Save([FromBody] UtilisateurDto utilisateur)
    {

    }

    /// <summary>
    /// Recherche des utilisateurs
    /// </summary>
    /// <param name="utiId">Id technique</param>
    /// <param name="age">Age en années de l'utilisateur</param>
    /// <param name="profilId">Profil de l'utilisateur</param>
    /// <param name="email">Email de l'utilisateur</param>
    /// <param name="nom">Nom de l'utilisateur</param>
    /// <param name="actif">Si l'utilisateur est actif</param>
    /// <param name="typeUtilisateurCode">Type d'utilisateur en Many to one</param>
    /// <param name="utilisateursEnfant">Utilisateur enfants</param>
    /// <param name="dateCreation">Date de création de l'utilisateur</param>
    /// <param name="dateModification">Date de modification de l'utilisateur</param>
    /// <returns>Utilisateurs matchant les critères</returns>
    [HttpPost("utilisateur/search")]
    public async Task<ICollection<UtilisateurSearch>> Search(int? utiId = null, decimal age = 6l, int? profilId = null, string email = null, string nom = "Jabx", bool? actif = null, TypeUtilisateur.Codes typeUtilisateurCode = TypeUtilisateur.Codes.ADM, int[] utilisateursEnfant = null, DateOnly? dateCreation = null, DateOnly? dateModification = null)
    {

    }

}