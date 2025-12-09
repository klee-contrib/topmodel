////
//// ATTENTION, CE FICHIER EST PARTIELLEMENT GENERE AUTOMATIQUEMENT !
////

using Microsoft.AspNetCore.Mvc;

namespace TopModel.Sample.Api.Restaurant;

public class AdvancedEndpointsController : Controller
{
    /// <summary>
    /// Ajoute un employé (nécessite le rôle ADMIN)
    /// </summary>
    /// <param name="employe">Employé à créer</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Employé créé</returns>
    [Authorize]
    [Authorize(Roles = "ADMIN")]
    [HttpPost("api/restaurants/employes")]
    public async Task<EmployeRead> AddEmploye([FromBody] EmployeWrite employe, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Crée un menu avec ses plats
    /// </summary>
    /// <param name="menu">Menu à créer</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Menu créé avec ses plats</returns>
    [Authorize]
    [Authorize(Roles = "MANAGER")]
    [HttpPost("api/restaurants/menus")]
    public async Task<MenuComplet> CreateMenu([FromBody] MenuWrite menu, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Crée une réservation
    /// </summary>
    /// <param name="reservation">Réservation à créer</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Réservation créée</returns>
    [HttpPost("api/restaurants/reservations")]
    public async Task<ReservationAvecDetails> CreateReservation([FromBody] ReservationWrite reservation, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Exporte les commandes au format CSV
    /// </summary>
    /// <param name="dateDebut">Date et heure de la commande</param>
    /// <param name="dateFin">Date et heure de la commande</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Fichier CSV des commandes</returns>
    [Produces("application/octet-stream")]
    [Authorize]
    [Authorize(Roles = "ADMIN")]
    [HttpGet("api/restaurants/commandes/export")]
    public async Task<byte[]> ExportCommandes([Required] DateTime? dateDebut = null, [Required] DateTime? dateFin = null, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Liste les avis clients avec filtres
    /// </summary>
    /// <param name="resRestaurantId">Identifiant du restaurant</param>
    /// <param name="noteMin">Note sur 5</param>
    /// <param name="approuve">Indique si l'avis est approuvé par le restaurant</param>
    /// <param name="dateDebut">Date de l'avis</param>
    /// <param name="dateFin">Date de l'avis</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Liste des avis correspondant aux critères</returns>
    [HttpGet("api/restaurants/avis")]
    public async Task<ICollection<AvisClientRead>> GetAvisClients([Required] int? resRestaurantId = null, [Required] int? noteMin = null, bool approuve = false, [Required] DateTime? dateDebut = null, [Required] DateTime? dateFin = null, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Récupère un client avec toutes ses commandes
    /// </summary>
    /// <param name="cliId">Identifiant du client</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Client avec ses commandes</returns>
    [HttpGet("api/restaurants/clients/{cliId:int}/avec-commandes")]
    public async Task<ClientAvecCommandes> GetClientAvecCommandes(int cliId, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Récupère le détail complet d'une commande avec ses lignes
    /// </summary>
    /// <param name="comId">Identifiant de la commande</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Détail complet de la commande</returns>
    [HttpGet("api/restaurants/commandes/{comId:int}/detail")]
    public async Task<CommandeDetailRead> GetCommandeDetail(int comId, CancellationToken ct = default)
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
    /// Met à jour partiellement une promotion
    /// </summary>
    /// <param name="proId">Identifiant de la promotion</param>
    /// <param name="promotion">Données partielles de la promotion</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Promotion mise à jour</returns>
    [Authorize]
    [HttpPatch("api/restaurants/promotions/{proId:int}")]
    public async Task<PromotionRead> PatchPromotion(int proId, [FromBody] PromotionWrite promotion, CancellationToken ct = default)
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

}