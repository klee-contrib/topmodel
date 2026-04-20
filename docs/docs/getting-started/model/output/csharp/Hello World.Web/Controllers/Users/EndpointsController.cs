////
//// ATTENTION, CE FICHIER EST PARTIELLEMENT GENERE AUTOMATIQUEMENT !
////

using Microsoft.AspNetCore.Mvc;

namespace Hello World.Web.Users;

public class EndpointsController : Controller
{
    /// <summary>
    /// Créé un nouvel Utilisateur
    /// </summary>
    /// <param name="detail">Le détail de l'utilisateur à créer</param>
    /// <returns>Le détail de l'utilisateur créé</returns>
    [HttpPost("Utilisateur")]
    public async Task<UtilisateurDetailDto> CreateUtilisateur([FromBody] UtilisateurCreateDto detail)
    {

    }

    /// <summary>
    /// Supprime un Utilisateur
    /// </summary>
    /// <param name="utilisateurId">Identifiant unique de l'utilisateur</param>
    /// <returns>Task.</returns>
    [HttpDelete("Utilisateur/{utilisateurId}")]
    public async Task DeleteUtilisateur(long utilisateurId)
    {

    }

    /// <summary>
    /// Charge le détail d'un Utilisateur
    /// </summary>
    /// <param name="utilisateurId">Identifiant unique de l'utilisateur</param>
    /// <returns>Le détail d'un Utilisateur</returns>
    [HttpGet("Utilisateur/{utilisateurId}")]
    public async Task<UtilisateurDetailDto> GetUtilisateur(long utilisateurId)
    {

    }

    /// <summary>
    /// Modifie un Utilisateur
    /// </summary>
    /// <param name="detail">Le détail de l'utilisateur à modifier</param>
    /// <param name="utilisateurId">Identifiant unique de l'utilisateur</param>
    /// <returns>Le détail de l'utilisateur modifié</returns>
    [HttpPatch("Utilisateur/{utilisateurId}")]
    public async Task<UtilisateurDetailDto> UpdateUtilisateur([FromBody] UtilisateurUpdateDto detail, long utilisateurId)
    {

    }

}