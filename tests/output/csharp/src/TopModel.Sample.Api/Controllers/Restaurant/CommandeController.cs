////
//// ATTENTION, CE FICHIER EST PARTIELLEMENT GENERE AUTOMATIQUEMENT !
////

using Microsoft.AspNetCore.Mvc;

namespace TopModel.Sample.Api.Restaurant;

public class CommandeController : Controller
{
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
    /// Crée une réservation
    /// </summary>
    /// <param name="reservation">Réservation à créer</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Réservation créée</returns>
    [HttpPost("api/restaurants/reservations")]
    public async Task<ReservationRead> CreateReservation([FromBody] ReservationWrite reservation, CancellationToken ct = default)
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
    /// Supprime une commande
    /// </summary>
    /// <param name="commandeItem">Commande item à supprimer dans le body</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Task.</returns>
    [HttpDelete("api/restaurants/commandes")]
    public async Task DeleteCommandeWithBody([FromBody] CommandeItem commandeItem, CancellationToken ct = default)
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
    /// Liste toutes les commandes
    /// </summary>
    /// <param name="clientId">Client ayant passé la commande</param>
    /// <param name="statutCommandeCode">Statut de la commande</param>
    /// <param name="tableId">Table associée à la commande</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Liste des commandes</returns>
    [HttpGet("api/restaurants/commandes")]
    public async Task<ICollection<CommandeItem>> GetCommandes([Required] int? clientId = null, StatutCommande.Codes statutCommandeCode = StatutCommande.Codes.EN_ATT, int? tableId = null, CancellationToken ct = default)
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
    /// Liste tous les statuts de commande
    /// </summary>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Liste des statuts de commande</returns>
    [HttpGet("api/restaurants/statuts-commande")]
    public async Task<ICollection<StatutCommande>> GetStatutCommandes(CancellationToken ct = default)
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

}