////
//// ATTENTION, CE FICHIER EST PARTIELLEMENT GENERE AUTOMATIQUEMENT !
////

using Microsoft.AspNetCore.Mvc;

namespace CSharp.Api.Securite.Profil;

public class ProfilController : Controller
{
    /// <summary>
    /// Ajoute un Profil
    /// </summary>
    /// <param name="profil">Profil à sauvegarder</param>
    /// <returns>Profil sauvegardé</returns>
    [HttpPost("api/profils")]
    public async Task<ProfilRead> AddProfil([FromBody] ProfilWrite profil)
    {

    }

    /// <summary>
    /// Charge le détail d'un Profil
    /// </summary>
    /// <param name="proId">Id technique</param>
    /// <returns>Le détail de l'Profil</returns>
    [HttpGet("api/profils/{proId:int}")]
    public async Task<ProfilRead> GetProfil(int proId)
    {

    }

    /// <summary>
    /// Liste tous les Profils
    /// </summary>
    /// <returns>Profils matchant les critères</returns>
    [HttpGet("api/profils")]
    public async Task<ICollection<ProfilItem>> GetProfils()
    {

    }

    /// <summary>
    /// Sauvegarde un Profil
    /// </summary>
    /// <param name="proId">Id technique</param>
    /// <param name="profil">Profil à sauvegarder</param>
    /// <returns>Profil sauvegardé</returns>
    [HttpPut("api/profils/{proId:int}")]
    public async Task<ProfilRead> UpdateProfil(int proId, [FromBody] ProfilWrite profil)
    {

    }

}