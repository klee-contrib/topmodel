////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using TopModel.Sample.Restaurant.Models;

namespace TopModel.Sample.Clients.External.Restaurant;

/// <summary>
/// Client Commande.
/// </summary>
/// <param name="client">HttpClient injecté.</param>
public partial class CommandeClient(HttpClient client)
{
    private readonly JsonSerializerOptions _jsOptions = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    /// <summary>
    /// Crée une nouvelle commande.
    /// </summary>
    /// <param name="commande">Commande à créer.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Commande créée.</returns>
    public async Task<CommandeRead> AddCommande(CommandeWrite commande, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Post, $"api/restaurants/commandes") { Content = JsonContent.Create(commande, options: _jsOptions) }, HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<CommandeRead>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Crée une réservation.
    /// </summary>
    /// <param name="reservation">Réservation à créer.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Réservation créée.</returns>
    public async Task<ReservationRead> CreateReservation(ReservationWrite reservation, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Post, $"api/restaurants/reservations") { Content = JsonContent.Create(reservation, options: _jsOptions) }, HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<ReservationRead>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Supprime une commande.
    /// </summary>
    /// <param name="comId">Identifiant de la commande.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Task.</returns>
    public async Task DeleteCommande(int comId, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Delete, $"api/restaurants/commandes/{comId}"), ct);
        await EnsureSuccess(res, ct);
    }

    /// <summary>
    /// Supprime une commande.
    /// </summary>
    /// <param name="commandeItem">Commande item à supprimer dans le body.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Détail de la suppression.</returns>
    public async Task<CommandeDeleteResult> DeleteCommandeWithBody(ICommandeItem commandeItem, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Delete, $"api/restaurants/commandes") { Content = JsonContent.Create(commandeItem, options: _jsOptions) }, HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<CommandeDeleteResult>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Exporte les commandes au format CSV.
    /// </summary>
    /// <param name="dateDebut">Date et heure de la commande.</param>
    /// <param name="dateFin">Date et heure de la commande.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Fichier CSV des commandes.</returns>
    public async Task<byte[]?> ExportCommandes(DateTime? dateDebut = null, DateTime? dateFin = null, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        var query = await new FormUrlEncodedContent(new Dictionary<string, string?>
        {
            ["dateDebut"] = dateDebut?.ToString("o"),
            ["dateFin"] = dateFin?.ToString("o"),
        }.Where(kv => kv.Value != null)).ReadAsStringAsync(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/commandes/export?{query}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        if (res.StatusCode == HttpStatusCode.NoContent)
        {
            return null;
        }

        return await res.Content.ReadFromJsonAsync<byte[]?>(_jsOptions, ct);
    }

    /// <summary>
    /// Charge le détail d'une commande.
    /// </summary>
    /// <param name="comId">Identifiant de la commande.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Détail de la commande.</returns>
    public async Task<CommandeRead> GetCommande(int comId, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/commandes/{comId}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<CommandeRead>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Liste toutes les commandes.
    /// </summary>
    /// <param name="clientId">Client ayant passé la commande.</param>
    /// <param name="statutCommande">Statut de la commande.</param>
    /// <param name="tableId">Table associée à la commande.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Liste des commandes.</returns>
    public async Task<ICollection<ICommandeItem>> GetCommandes(int? clientId = null, StatutCommande statutCommande = StatutCommande.EN_ATT, int? tableId = null, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        var query = await new FormUrlEncodedContent(new Dictionary<string, string?>
        {
            ["clientId"] = clientId?.ToString(),
            ["statutCommande"] = statutCommande.ToString(),
            ["tableId"] = tableId?.ToString(),
        }.Where(kv => kv.Value != null)).ReadAsStringAsync(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/commandes?{query}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<ICollection<ICommandeItem>>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Récupère les commandes par date.
    /// </summary>
    /// <param name="dateCommande">Date et heure de la commande.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Commandes pour la date spécifiée.</returns>
    public async Task<ICollection<ICommandeItem>> GetCommandesByDate(DateTime? dateCommande = null, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        var query = await new FormUrlEncodedContent(new Dictionary<string, string?>
        {
            ["dateCommande"] = dateCommande?.ToString("o"),
        }.Where(kv => kv.Value != null)).ReadAsStringAsync(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/commandes/by-date?{query}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<ICollection<ICommandeItem>>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Liste tous les statuts de commande.
    /// </summary>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Liste des statuts de commande.</returns>
    public async Task<ICollection<StatutCommande>> GetStatutCommandes(CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/statuts-commande"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<ICollection<StatutCommande>>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Met à jour partiellement une commande.
    /// </summary>
    /// <param name="comId">Identifiant de la commande.</param>
    /// <param name="commande">Données partielles de la commande.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Commande mise à jour.</returns>
    public async Task<CommandeRead> PatchCommande(int comId, CommandeWrite commande, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Patch, $"api/restaurants/commandes/{comId}") { Content = JsonContent.Create(commande, options: _jsOptions) }, HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<CommandeRead>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Met à jour une commande.
    /// </summary>
    /// <param name="comId">Identifiant de la commande.</param>
    /// <param name="commande">Commande à mettre à jour.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Commande mise à jour.</returns>
    public async Task<CommandeRead> UpdateCommande(int comId, CommandeWrite commande, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Put, $"api/restaurants/commandes/{comId}") { Content = JsonContent.Create(commande, options: _jsOptions) }, HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<CommandeRead>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Met à jour uniquement le statut d'une commande.
    /// </summary>
    /// <param name="comId">Identifiant de la commande.</param>
    /// <param name="statutCommande">Statut de la commande.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Commande avec le statut mis à jour.</returns>
    public async Task<CommandeRead> UpdateCommandeStatut(int comId, StatutCommande statutCommande = StatutCommande.EN_ATT, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        var query = await new FormUrlEncodedContent(new Dictionary<string, string?>
        {
            ["statutCommande"] = statutCommande.ToString(),
        }.Where(kv => kv.Value != null)).ReadAsStringAsync(ct);
        using var res = await client.SendAsync(new(HttpMethod.Patch, $"api/restaurants/commandes/{comId}/statut?{query}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<CommandeRead>(_jsOptions, ct))!;
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
