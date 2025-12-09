////
//// ATTENTION, CE FICHIER EST PARTIELLEMENT GENERE AUTOMATIQUEMENT !
////

using Microsoft.AspNetCore.Mvc;

namespace TopModel.Sample.Api.Restaurant;

public class EndpointsController : Controller
{
    /// <summary>
    /// Ajoute un client
    /// </summary>
    /// <param name="client">Client à créer</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Client créé</returns>
    [HttpPost("api/restaurants/clients")]
    public async Task<ClientRead> AddClient([FromBody] ClientWrite client, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Crée une nouvelle commande
    /// </summary>
    /// <param name="commande">Commande à créer</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Commande créée</returns>
    [HttpPost("api/restaurants/commandes")]
    public async Task<CommandeRead> AddCommande([FromBody] CommandeWrite commande, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Ajoute une ligne de commande
    /// </summary>
    /// <param name="ligneCommande">Ligne de commande à créer</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Ligne de commande créée</returns>
    [HttpPost("api/restaurants/ligne-commandes")]
    public async Task<LigneCommandeRead> AddLigneCommande([FromBody] LigneCommandeWrite ligneCommande, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Ajoute un plat
    /// </summary>
    /// <param name="plat">Plat à créer</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Plat créé</returns>
    [HttpPost("api/restaurants/plats")]
    public async Task<PlatRead> AddPlat([FromBody] PlatWrite plat, CancellationToken ct = default)
    {

    }

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
    public async Task<TableClientRead> AddTable([FromBody] TableClientWrite table, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Supprime un client
    /// </summary>
    /// <param name="cliId">Identifiant du client</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Task.</returns>
    [HttpDelete("api/restaurants/clients/{cliId:int}")]
    public async Task DeleteClient(int cliId, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Supprime une commande
    /// </summary>
    /// <param name="comId">Identifiant de la commande</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Task.</returns>
    [HttpDelete("api/restaurants/commandes/{comId:int}")]
    public async Task DeleteCommande(int comId, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Supprime une ligne de commande
    /// </summary>
    /// <param name="ligId">Identifiant de la ligne</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Task.</returns>
    [HttpDelete("api/restaurants/ligne-commandes/{ligId:int}")]
    public async Task DeleteLigneCommande(int ligId, CancellationToken ct = default)
    {

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
    /// Liste toutes les catégories de plats
    /// </summary>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Liste des catégories de plats</returns>
    [HttpGet("api/restaurants/categorie-plats")]
    public async Task<ICollection<CategoriePlat>> GetCategoriePlats(CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Charge le détail d'un client
    /// </summary>
    /// <param name="cliId">Identifiant du client</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Détail du client</returns>
    [HttpGet("api/restaurants/clients/{cliId:int}")]
    public async Task<ClientRead> GetClient(int cliId, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Liste les commandes d'un client
    /// </summary>
    /// <param name="cliId">Identifiant du client</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Liste des commandes du client</returns>
    [HttpGet("api/restaurants/clients/{cliId:int}/commandes")]
    public async Task<ICollection<CommandeItem>> GetClientCommandes(int cliId, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Liste tous les clients
    /// </summary>
    /// <param name="nom">Nom du client</param>
    /// <param name="email">Adresse email du client</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Liste des clients</returns>
    [HttpGet("api/restaurants/clients")]
    public async Task<ICollection<ClientItem>> GetClients([Required] string nom = null, string email = null, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Charge le détail d'une commande
    /// </summary>
    /// <param name="comId">Identifiant de la commande</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Détail de la commande</returns>
    [HttpGet("api/restaurants/commandes/{comId:int}")]
    public async Task<CommandeRead> GetCommande(int comId, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Liste les lignes d'une commande
    /// </summary>
    /// <param name="comId">Identifiant de la commande</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Liste des lignes de la commande</returns>
    [HttpGet("api/restaurants/commandes/{comId:int}/lignes")]
    public async Task<ICollection<LigneCommandeItem>> GetCommandeLignes(int comId, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Liste toutes les commandes
    /// </summary>
    /// <param name="clientId">Client ayant passé la commande</param>
    /// <param name="statutCommandeCode">Statut de la commande</param>
    /// <param name="tableClientId">Table associée à la commande</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Liste des commandes</returns>
    [HttpGet("api/restaurants/commandes")]
    public async Task<ICollection<CommandeItem>> GetCommandes([Required] int? clientId = null, StatutCommande.Codes statutCommandeCode = StatutCommande.Codes.EN_ATT, int? tableClientId = null, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Récupère les commandes par date
    /// </summary>
    /// <param name="dateCommande">Date et heure de la commande</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Commandes pour la date spécifiée</returns>
    [HttpGet("api/restaurants/commandes/by-date")]
    public async Task<ICollection<CommandeItem>> GetCommandesByDate([Required] DateTime? dateCommande = null, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Charge le détail d'une ligne de commande
    /// </summary>
    /// <param name="ligId">Identifiant de la ligne</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Détail de la ligne de commande</returns>
    [HttpGet("api/restaurants/ligne-commandes/{ligId:int}")]
    public async Task<LigneCommandeRead> GetLigneCommande(int ligId, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Liste toutes les lignes de commande
    /// </summary>
    /// <param name="commandeId">Commande à laquelle appartient la ligne</param>
    /// <param name="platId">Plat commandé</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Liste des lignes de commande</returns>
    [HttpGet("api/restaurants/ligne-commandes")]
    public async Task<ICollection<LigneCommandeItem>> GetLigneCommandes([Required] int? commandeId = null, [Required] int? platId = null, CancellationToken ct = default)
    {

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

    }

    /// <summary>
    /// Liste tous les plats
    /// </summary>
    /// <param name="disponible">Indique si le plat est disponible</param>
    /// <param name="restaurantIdRestaurant">Restaurant proposant ce plat</param>
    /// <param name="categoriePlatCodeCategoriePlat">Catégorie du plat</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Liste des plats</returns>
    [HttpGet("api/restaurants/plats")]
    public async Task<ICollection<PlatItem>> GetPlats(bool disponible = true, [Required] int? restaurantIdRestaurant = null, [Required] CategoriePlat.Codes? categoriePlatCodeCategoriePlat = null, CancellationToken ct = default)
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
    /// Liste les plats d'un restaurant
    /// </summary>
    /// <param name="resId">Identifiant du restaurant</param>
    /// <param name="disponible">Indique si le plat est disponible</param>
    /// <param name="categoriePlatCodeCategoriePlat">Catégorie du plat</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Liste des plats du restaurant</returns>
    [HttpGet("api/restaurants/{resId:int}/plats")]
    public async Task<ICollection<PlatItem>> GetRestaurantPlats(int resId, bool disponible = true, [Required] CategoriePlat.Codes? categoriePlatCodeCategoriePlat = null, CancellationToken ct = default)
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
    public async Task<ICollection<TableClientItem>> GetRestaurantTables(int resId, bool disponible = true, CancellationToken ct = default)
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
    /// Liste tous les statuts de commande
    /// </summary>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Liste des statuts de commande</returns>
    [HttpGet("api/restaurants/statuts-commande")]
    public async Task<ICollection<StatutCommande>> GetStatutCommandes(CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Charge le détail d'une table
    /// </summary>
    /// <param name="tabId">Identifiant de la table</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Détail de la table</returns>
    [HttpGet("api/restaurants/tables/{tabId:int}")]
    public async Task<TableClientRead> GetTable(int tabId, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Liste toutes les tables
    /// </summary>
    /// <param name="restaurantIdRestaurant">Restaurant auquel appartient la table</param>
    /// <param name="disponible">Indique si la table est disponible</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Liste des tables</returns>
    [HttpGet("api/restaurants/tables")]
    public async Task<ICollection<TableClientItem>> GetTables([Required] int? restaurantIdRestaurant = null, bool disponible = true, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Met à jour partiellement un client
    /// </summary>
    /// <param name="cliId">Identifiant du client</param>
    /// <param name="client">Données partielles du client</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Client mis à jour</returns>
    [HttpPatch("api/restaurants/clients/{cliId:int}")]
    public async Task<ClientRead> PatchClient(int cliId, [FromBody] ClientWrite client, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Met à jour partiellement une commande
    /// </summary>
    /// <param name="comId">Identifiant de la commande</param>
    /// <param name="commande">Données partielles de la commande</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Commande mise à jour</returns>
    [HttpPatch("api/restaurants/commandes/{comId:int}")]
    public async Task<CommandeRead> PatchCommande(int comId, [FromBody] CommandeWrite commande, CancellationToken ct = default)
    {

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

    }

    /// <summary>
    /// Recherche de plats avec critères multiples
    /// </summary>
    /// <param name="nom">Nom du plat</param>
    /// <param name="restaurantIdRestaurant">Restaurant proposant ce plat</param>
    /// <param name="categoriePlatCodeCategoriePlat">Catégorie du plat</param>
    /// <param name="disponible">Indique si le plat est disponible</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Plats correspondant aux critères de recherche</returns>
    [HttpGet("api/restaurants/plats/search")]
    public async Task<ICollection<PlatItem>> SearchPlats([Required] string nom = null, [Required] int? restaurantIdRestaurant = null, [Required] CategoriePlat.Codes? categoriePlatCodeCategoriePlat = null, bool disponible = true, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Met à jour un client
    /// </summary>
    /// <param name="cliId">Identifiant du client</param>
    /// <param name="client">Client à mettre à jour</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Client mis à jour</returns>
    [HttpPut("api/restaurants/clients/{cliId:int}")]
    public async Task<ClientRead> UpdateClient(int cliId, [FromBody] ClientWrite client, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Met à jour une commande
    /// </summary>
    /// <param name="comId">Identifiant de la commande</param>
    /// <param name="commande">Commande à mettre à jour</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Commande mise à jour</returns>
    [HttpPut("api/restaurants/commandes/{comId:int}")]
    public async Task<CommandeRead> UpdateCommande(int comId, [FromBody] CommandeWrite commande, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Met à jour uniquement le statut d'une commande
    /// </summary>
    /// <param name="comId">Identifiant de la commande</param>
    /// <param name="statutCommandeCode">Statut de la commande</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Commande avec le statut mis à jour</returns>
    [HttpPatch("api/restaurants/commandes/{comId:int}/statut")]
    public async Task<CommandeRead> UpdateCommandeStatut(int comId, StatutCommande.Codes statutCommandeCode = StatutCommande.Codes.EN_ATT, CancellationToken ct = default)
    {

    }

    /// <summary>
    /// Met à jour une ligne de commande
    /// </summary>
    /// <param name="ligId">Identifiant de la ligne</param>
    /// <param name="ligneCommande">Ligne de commande à mettre à jour</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Ligne de commande mise à jour</returns>
    [HttpPut("api/restaurants/ligne-commandes/{ligId:int}")]
    public async Task<LigneCommandeRead> UpdateLigneCommande(int ligId, [FromBody] LigneCommandeWrite ligneCommande, CancellationToken ct = default)
    {

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
    public async Task<TableClientRead> UpdateTable(int tabId, [FromBody] TableClientWrite table, CancellationToken ct = default)
    {

    }

}