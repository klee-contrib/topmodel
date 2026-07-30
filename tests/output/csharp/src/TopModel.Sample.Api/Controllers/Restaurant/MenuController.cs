////
//// ATTENTION, CE FICHIER EST PARTIELLEMENT GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopModel.Sample.Restaurant.Models;

namespace TopModel.Sample.Api.Restaurant;

public class MenuController : Controller
{
    /// <summary>
    /// Ajoute un plat
    /// </summary>
    /// <param name="plat">Plat à créer</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Plat créé</returns>
    [HttpPost("api/restaurants/plats")]
    public async Task<PlatRead> AddPlat([FromBody] PlatWrite plat, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Crée un menu avec ses plats
    /// </summary>
    /// <param name="menu">Menu à créer</param>
    /// <param name="regCodeOrigine">Code de la région.</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Menu créé avec ses plats</returns>
    [Authorize]
    [Authorize(Roles = "MANAGER")]
    [HttpPost("api/restaurants/menus")]
    public async Task<MenuRead> CreateMenu([FromForm] MenuWrite menu = new(), [FromForm][Required] Region.Codes? regCodeOrigine = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Supprime un plat
    /// </summary>
    /// <param name="plaId">Identifiant du plat</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Task.</returns>
    [HttpDelete("api/restaurants/plats/{plaId:int}")]
    public async Task DeletePlat(int plaId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Liste toutes les catégories de plats
    /// </summary>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Liste des catégories de plats</returns>
    [HttpGet("api/restaurants/categorie-plats")]
    public async Task<ICollection<CategoriePlat>> GetCategoriePlats(CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Charge le détail d'un plat
    /// </summary>
    /// <param name="plaId">Identifiant du plat</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Détail du plat</returns>
    [HttpGet("api/restaurants/plats/{plaId:int}")]
    public async Task<PlatRead> GetPlat(int plaId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Liste tous les plats
    /// </summary>
    /// <param name="restaurantId">Restaurant proposant ce plat</param>
    /// <param name="categoriePlatCode">Catégorie du plat</param>
    /// <param name="disponible">Indique si le plat est disponible</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Liste des plats</returns>
    [HttpGet("api/restaurants/plats")]
    public async Task<ICollection<IPlatItem>> GetPlats([Required] int? restaurantId = null, [Required] CategoriePlat.Codes? categoriePlatCode = null, bool disponible = true, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Met à jour partiellement un plat
    /// </summary>
    /// <param name="plaId">Identifiant du plat</param>
    /// <param name="plat">Données partielles du plat</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Plat mis à jour</returns>
    [HttpPatch("api/restaurants/plats/{plaId:int}")]
    public async Task<PlatRead> PatchPlat(int plaId, [FromBody] PlatWrite plat, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Met à jour partiellement une promotion
    /// </summary>
    /// <param name="plaId">Identifiant du plat</param>
    /// <param name="promotion">Données partielles de la promotion</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Promotion mise à jour</returns>
    [Authorize]
    [HttpPatch("api/restaurants/plats/{plaId:int}/promotion")]
    public async Task<PromotionRead> PatchPromotion(int plaId, [FromBody] PromotionWrite promotion, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Recherche de plats avec critères multiples
    /// </summary>
    /// <param name="nom">Nom du plat</param>
    /// <param name="restaurantId">Restaurant proposant ce plat</param>
    /// <param name="categoriePlatCode">Catégorie du plat</param>
    /// <param name="disponible">Indique si le plat est disponible</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Plats correspondant aux critères de recherche</returns>
    [HttpGet("api/restaurants/plats/search")]
    public async Task<ICollection<IPlatItem>> SearchPlats([Required] string? nom = null, [Required] int? restaurantId = null, [Required] CategoriePlat.Codes? categoriePlatCode = null, bool disponible = true, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Met à jour un plat
    /// </summary>
    /// <param name="plaId">Identifiant du plat</param>
    /// <param name="plat">Plat à mettre à jour</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Plat mis à jour</returns>
    [HttpPut("api/restaurants/plats/{plaId:int}")]
    public async Task<PlatRead> UpdatePlat(int plaId, [FromBody] PlatWrite plat, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
