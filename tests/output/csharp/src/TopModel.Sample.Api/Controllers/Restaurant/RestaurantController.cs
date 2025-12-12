////
//// ATTENTION, CE FICHIER EST PARTIELLEMENT GENERE AUTOMATIQUEMENT !
////

using Microsoft.AspNetCore.Mvc;

namespace TopModel.Sample.Api.Restaurant;

public class RestaurantController : Controller
{
    /// <summary>
    /// Ajoute un restaurant
    /// </summary>
    /// <param name="restaurant">Restaurant à créer</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Restaurant créé</returns>
    [HttpPost("api/restaurants")]
    public async Task<RestaurantRead> AddRestaurant([FromBody] RestaurantWrite restaurant, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Ajoute une table
    /// </summary>
    /// <param name="table">Table à créer</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Table créée</returns>
    [HttpPost("api/restaurants/tables")]
    public async Task<TableRead> AddTable([FromBody] TableWrite table, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Supprime un restaurant
    /// </summary>
    /// <param name="resId">Identifiant du restaurant</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Task.</returns>
    [HttpDelete("api/restaurants/{resId:int}")]
    public async Task DeleteRestaurant(int resId, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Supprime une table
    /// </summary>
    /// <param name="tabId">Identifiant de la table</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Task.</returns>
    [HttpDelete("api/restaurants/tables/{tabId:int}")]
    public async Task DeleteTable(int tabId, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Charge le détail d'un restaurant
    /// </summary>
    /// <param name="resId">Identifiant du restaurant</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Détail du restaurant</returns>
    [HttpGet("api/restaurants/{resId:int}")]
    public async Task<RestaurantRead> GetRestaurant(int resId, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Récupère un menu spécifique d'un restaurant
    /// </summary>
    /// <param name="resId">Identifiant du restaurant</param>
    /// <param name="menId">Identifiant du menu</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Menu du restaurant</returns>
    [HttpGet("api/restaurants/{resId:int}/menus/{menId:int}")]
    public async Task<MenuComplet> GetRestaurantMenu(int resId, int menId, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Liste les plats d'un restaurant
    /// </summary>
    /// <param name="resId">Identifiant du restaurant</param>
    /// <param name="disponible">Indique si le plat est disponible</param>
    /// <param name="categoriePlatCode">Catégorie du plat</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Liste des plats du restaurant</returns>
    [HttpGet("api/restaurants/{resId:int}/plats")]
    public async Task<ICollection<PlatItem>> GetRestaurantPlats(int resId, bool disponible = true, [Required] CategoriePlat.Codes? categoriePlatCode = null, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Récupère les statistiques d'un restaurant
    /// </summary>
    /// <param name="resId">Identifiant du restaurant</param>
    /// <param name="dateDebut">Date de début pour le calcul des statistiques</param>
    /// <param name="dateFin">Date de fin pour le calcul des statistiques</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Statistiques du restaurant</returns>
    [Authorize]
    [HttpGet("api/restaurants/{resId:int}/statistiques")]
    public async Task<StatistiquesRestaurant> GetRestaurantStatistiques(int resId, [Required] DateTime? dateDebut = null, [Required] DateTime? dateFin = null, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Liste les tables d'un restaurant
    /// </summary>
    /// <param name="resId">Identifiant du restaurant</param>
    /// <param name="disponible">Indique si la table est disponible</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Liste des tables du restaurant</returns>
    [HttpGet("api/restaurants/{resId:int}/tables")]
    public async Task<ICollection<TableItem>> GetRestaurantTables(int resId, bool disponible = true, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Liste tous les restaurants
    /// </summary>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Liste des restaurants</returns>
    [HttpGet("api/restaurants")]
    public async Task<ICollection<RestaurantItem>> GetRestaurants(CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Charge le détail d'une table
    /// </summary>
    /// <param name="tabId">Identifiant de la table</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Détail de la table</returns>
    [HttpGet("api/restaurants/tables/{tabId:int}")]
    public async Task<TableRead> GetTable(int tabId, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Liste toutes les tables
    /// </summary>
    /// <param name="restaurantId">Restaurant auquel appartient la table</param>
    /// <param name="disponible">Indique si la table est disponible</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Liste des tables</returns>
    [HttpGet("api/restaurants/tables")]
    public async Task<ICollection<TableItem>> GetTables([Required] int? restaurantId = null, bool disponible = true, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Recherche avancée de restaurants
    /// </summary>
    /// <param name="nom">Nom du restaurant (recherche partielle)</param>
    /// <param name="adresse">Adresse du restaurant (recherche partielle)</param>
    /// <param name="noteMin">Note minimum requise</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Liste des restaurants correspondant aux critères</returns>
    [HttpGet("api/restaurants/search")]
    public async Task<ICollection<RestaurantAvecStatistiques>> SearchRestaurants([Required] string nom = null, string adresse = null, [Required] int? noteMin = null, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Met à jour un restaurant
    /// </summary>
    /// <param name="resId">Identifiant du restaurant</param>
    /// <param name="restaurant">Restaurant à mettre à jour</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Restaurant mis à jour</returns>
    [HttpPut("api/restaurants/{resId:int}")]
    public async Task<RestaurantRead> UpdateRestaurant(int resId, [FromBody] RestaurantWrite restaurant, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Met à jour une table
    /// </summary>
    /// <param name="tabId">Identifiant de la table</param>
    /// <param name="table">Table à mettre à jour</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Table mise à jour</returns>
    [HttpPut("api/restaurants/tables/{tabId:int}")]
    public async Task<TableRead> UpdateTable(int tabId, [FromBody] TableWrite table, CancellationToken ct = default)
    {

    }

}