////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using TopModel.Sample.Restaurant.Models;

namespace TopModel.Sample.Clients.External.Restaurant;

/// <summary>
/// Client Personne.
/// </summary>
/// <param name="_client">HttpClient injecté.</param>
public partial class PersonneClient(HttpClient _client)
{
    private readonly JsonSerializerOptions _jsOptions = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    /// <summary>
    /// Ajoute un client.
    /// </summary>
    /// <param name="client">Client à créer.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Client créé.</returns>
    public async Task<ClientRead> AddClient(ClientWrite client, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await _client.SendAsync(new(HttpMethod.Post, $"api/restaurants/clients") { Content = JsonContent.Create(client, options: _jsOptions) }, HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<ClientRead>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Ajoute un employé (nécessite le rôle ADMIN).
    /// </summary>
    /// <param name="employe">Employé à créer.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Employé créé.</returns>
    public async Task<EmployeRead> AddEmploye(EmployeWrite employe, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await _client.SendAsync(new(HttpMethod.Post, $"api/restaurants/employes") { Content = JsonContent.Create(employe, options: _jsOptions) }, HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<EmployeRead>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Supprime un client.
    /// </summary>
    /// <param name="perId">Identifiant de la personne.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Task.</returns>
    public async Task DeleteClient(int perId, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await _client.SendAsync(new(HttpMethod.Delete, $"api/restaurants/clients/{perId}"), ct);
        await EnsureSuccess(res, ct);
    }

    /// <summary>
    /// Liste les avis clients avec filtres.
    /// </summary>
    /// <param name="resRestaurantId">Identifiant du restaurant.</param>
    /// <param name="noteMin">Note sur 5.</param>
    /// <param name="approuve">Indique si l'avis est approuvé par le restaurant.</param>
    /// <param name="dateDebut">Date de l'avis.</param>
    /// <param name="dateFin">Date de l'avis.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Liste des avis correspondant aux critères.</returns>
    public async Task<ICollection<AvisClientRead>> GetAvisClients(int? resRestaurantId = null, int? noteMin = null, bool approuve = false, DateTime? dateDebut = null, DateTime? dateFin = null, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        var query = await new FormUrlEncodedContent(new Dictionary<string, string?>
        {
            ["resRestaurantId"] = resRestaurantId?.ToString(),
            ["noteMin"] = noteMin?.ToString(),
            ["approuve"] = approuve.ToString(),
            ["dateDebut"] = dateDebut?.ToString("o"),
            ["dateFin"] = dateFin?.ToString("o"),
        }.Where(kv => kv.Value != null)).ReadAsStringAsync(ct);
        using var res = await _client.SendAsync(new(HttpMethod.Get, $"api/restaurants/avis?{query}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<ICollection<AvisClientRead>>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Charge le détail d'un client.
    /// </summary>
    /// <param name="perId">Identifiant de la personne.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Détail du client.</returns>
    public async Task<ClientRead> GetClient(int perId, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await _client.SendAsync(new(HttpMethod.Get, $"api/restaurants/clients/{perId}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<ClientRead>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Récupère un client avec toutes ses commandes.
    /// </summary>
    /// <param name="perId">Identifiant de la personne.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Client avec ses commandes.</returns>
    public async Task<ClientAvecCommandes> GetClientAvecCommandes(int perId, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await _client.SendAsync(new(HttpMethod.Get, $"api/restaurants/clients/{perId}/avec-commandes"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<ClientAvecCommandes>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Liste les commandes d'un client.
    /// </summary>
    /// <param name="perId">Identifiant de la personne.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Liste des commandes du client.</returns>
    public async Task<ICollection<CommandeItem>> GetClientCommandes(int perId, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await _client.SendAsync(new(HttpMethod.Get, $"api/restaurants/clients/{perId}/commandes"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<ICollection<CommandeItem>>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Liste tous les clients.
    /// </summary>
    /// <param name="nom">Nom de la personne.</param>
    /// <param name="email">Adresse email du client.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Liste des clients.</returns>
    public async Task<ICollection<ClientItem>> GetClients(string? nom = null, string? email = null, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        var query = await new FormUrlEncodedContent(new Dictionary<string, string?>
        {
            ["nom"] = nom,
            ["email"] = email,
        }.Where(kv => kv.Value != null)).ReadAsStringAsync(ct);
        using var res = await _client.SendAsync(new(HttpMethod.Get, $"api/restaurants/clients?{query}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<ICollection<ClientItem>>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Met à jour partiellement un client.
    /// </summary>
    /// <param name="perId">Identifiant de la personne.</param>
    /// <param name="client">Données partielles du client.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Client mis à jour.</returns>
    public async Task<ClientRead> PatchClient(int perId, ClientWrite client, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await _client.SendAsync(new(HttpMethod.Patch, $"api/restaurants/clients/{perId}") { Content = JsonContent.Create(client, options: _jsOptions) }, HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<ClientRead>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Met à jour un client.
    /// </summary>
    /// <param name="perId">Identifiant de la personne.</param>
    /// <param name="client">Client à mettre à jour.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Client mis à jour.</returns>
    public async Task<ClientRead> UpdateClient(int perId, ClientWrite client, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await _client.SendAsync(new(HttpMethod.Put, $"api/restaurants/clients/{perId}") { Content = JsonContent.Create(client, options: _jsOptions) }, HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<ClientRead>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Assure que l'authentification est configurée.
    /// </summary>
    /// <param name="ct">CancellationToken.</param>
    private partial Task EnsureAuthentication(CancellationToken ct = default);

    /// <summary>
    /// Gère les erreurs éventuelles retournées par l'API appelée.
    /// </summary>
    /// <param name="response">Réponse HTTP.</param>
    /// <param name="ct">CancellationToken.</param>
    private partial Task EnsureSuccess(HttpResponseMessage response, CancellationToken ct = default);
}
