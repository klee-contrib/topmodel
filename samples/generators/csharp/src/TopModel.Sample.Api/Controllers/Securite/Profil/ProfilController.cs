////
//// ATTENTION, CE FICHIER EST PARTIELLEMENT GENERE AUTOMATIQUEMENT !
////

using Microsoft.AspNetCore.Mvc;

namespace TopModel.Sample.Api.Securite.Profil;

public class ProfilController : Controller
{
    /// <summary>
    /// Ajoute un Profil
    /// </summary>
    /// <param name="profil">Profil à sauvegarder</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Profil sauvegardé</returns>
    [HttpPost("api/profils")]
    public async Task<ProfilRead> AddProfil([FromBody] ProfilWrite profil, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Charge le détail d'un Profil
    /// </summary>
    /// <param name="proId">Id technique</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Le détail du profil</returns>
    [HttpGet("api/profils/{proId:int}")]
    public async Task<ProfilRead> GetProfil(int proId, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Liste tous les Profils
    /// </summary>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Profils matchant les critères</returns>
    [HttpGet("api/profils")]
    public async Task<ICollection<ProfilItem>> GetProfils(CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Sauvegarde un Profil
    /// </summary>
    /// <param name="proId">Id technique</param>
    /// <param name="profil">Profil à sauvegarder</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Profil sauvegardé</returns>
    [HttpPut("api/profils/{proId:int}")]
    public async Task<ProfilRead> UpdateProfil(int proId, [FromBody] ProfilWrite profil, CancellationToken ct = default)
    {

    }

}