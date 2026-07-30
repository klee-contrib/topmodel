////
//// ATTENTION, CE FICHIER EST PARTIELLEMENT GENERE AUTOMATIQUEMENT !
////

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopModel.Sample.Restaurant.Models;

namespace TopModel.Sample.Api.Restaurant;

public class PersonneController : Controller
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
        throw new NotImplementedException();
    }

    /// <summary>
    /// Ajoute un employé (nécessite le rôle ADMIN)
    /// </summary>
    /// <param name="employe">Employé à créer</param>
    /// <param name="token">Token</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Employé créé</returns>
    [Authorize]
    [Authorize(Roles = "ADMIN")]
    [HttpPost("api/restaurants/employes")]
    public async Task<EmployeRead> AddEmploye([FromBody] EmployeWrite employe, string? token = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Supprime un client
    /// </summary>
    /// <param name="perId">Identifiant de la personne</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Task.</returns>
    [HttpDelete("api/restaurants/clients/{perId:int}")]
    public async Task DeleteClient(int perId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Liste les avis clients avec filtres
    /// </summary>
    /// <param name="resId">Identifiant du restaurant</param>
    /// <param name="noteMin">Note sur 5</param>
    /// <param name="approuve">Indique si l'avis est approuvé par le restaurant</param>
    /// <param name="dateDebut">Date de l'avis</param>
    /// <param name="dateFin">Date de l'avis</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Liste des avis correspondant aux critères</returns>
    [HttpGet("api/restaurants/avis")]
    public async Task<ICollection<AvisClientRead>> GetAvisClients([Required] int? resId = null, int? noteMin = null, bool approuve = false, DateTime? dateDebut = null, DateTime? dateFin = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Charge le détail d'un client
    /// </summary>
    /// <param name="perId">Identifiant de la personne</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Détail du client</returns>
    [HttpGet("api/restaurants/clients/{perId:int}")]
    public async Task<ClientRead> GetClient(int perId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Récupère un client avec toutes ses commandes
    /// </summary>
    /// <param name="perId">Identifiant de la personne</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Client avec ses commandes</returns>
    [HttpGet("api/restaurants/clients/{perId:int}/avec-commandes")]
    public async Task<ClientAvecCommandes> GetClientAvecCommandes(int perId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Liste les commandes d'un client
    /// </summary>
    /// <param name="perId">Identifiant de la personne</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Liste des commandes du client</returns>
    [HttpGet("api/restaurants/clients/{perId:int}/commandes")]
    public async Task<ICollection<ICommandeItem>> GetClientCommandes(int perId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Liste tous les clients
    /// </summary>
    /// <param name="nom">Nom de la personne</param>
    /// <param name="email">Adresse email du client</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Liste des clients</returns>
    [HttpGet("api/restaurants/clients")]
    public async Task<ICollection<ClientItem>> GetClients([Required] string? nom = null, string? email = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Met à jour partiellement un client
    /// </summary>
    /// <param name="perId">Identifiant de la personne</param>
    /// <param name="client">Données partielles du client</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Client mis à jour</returns>
    [HttpPatch("api/restaurants/clients/{perId:int}")]
    public async Task<ClientRead> PatchClient(int perId, [FromBody] ClientWrite client, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Met à jour un client
    /// </summary>
    /// <param name="perId">Identifiant de la personne</param>
    /// <param name="client">Client à mettre à jour</param>
    /// <param name="ct">CancellationToken (HttpContext.RequestAborted).</param>
    /// <returns>Client mis à jour</returns>
    [HttpPut("api/restaurants/clients/{perId:int}")]
    public async Task<ClientRead> UpdateClient(int perId, [FromBody] ClientWrite client, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
