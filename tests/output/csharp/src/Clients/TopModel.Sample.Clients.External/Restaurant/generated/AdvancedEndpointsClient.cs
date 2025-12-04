////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using TopModel.Sample.Restaurant.Models;

namespace TopModel.Sample.Clients.External.Restaurant;

/// <summary>
/// Client AdvancedEndpoints.
/// </summary>
/// <param name="client">HttpClient injecté.</param>
public partial class AdvancedEndpointsClient(HttpClient client)
{
    private readonly JsonSerializerOptions _jsOptions = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    /// <summary>
    /// Ajoute un employé (nécessite le rôle ADMIN).
    /// </summary>
    /// <param name="employe">Employé à créer.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Employé créé.</returns>
    public async Task<EmployeRead> AddEmploye(EmployeWrite employe, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Post, $"api/restaurants/employes") { Content = JsonContent.Create(employe, options: _jsOptions) }, HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<EmployeRead>(_jsOptions, ct);
    }

    /// <summary>
    /// Crée un menu avec ses plats.
    /// </summary>
    /// <param name="menu">Menu à créer.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Menu créé avec ses plats.</returns>
    public async Task<MenuComplet> CreateMenu(MenuWrite menu, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Post, $"api/restaurants/menus") { Content = JsonContent.Create(menu, options: _jsOptions) }, HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<MenuComplet>(_jsOptions, ct);
    }

    /// <summary>
    /// Crée une réservation.
    /// </summary>
    /// <param name="reservation">Réservation à créer.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Réservation créée.</returns>
    public async Task<ReservationAvecDetails> CreateReservation(ReservationWrite reservation, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Post, $"api/restaurants/reservations") { Content = JsonContent.Create(reservation, options: _jsOptions) }, HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<ReservationAvecDetails>(_jsOptions, ct);
    }

    /// <summary>
    /// Exporte les commandes au format CSV.
    /// </summary>
    /// <param name="dateDebut">Date et heure de la commande.</param>
    /// <param name="dateFin">Date et heure de la commande.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Fichier CSV des commandes.</returns>
    public async Task<byte[]> ExportCommandes(DateTime? dateDebut = null, DateTime? dateFin = null, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        var query = await new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["dateDebut"] = dateDebut?.ToString(CultureInfo.InvariantCulture),
            ["dateFin"] = dateFin?.ToString(CultureInfo.InvariantCulture),
        }.Where(kv => kv.Value != null)).ReadAsStringAsync(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/commandes/export?{query}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        if (res.StatusCode == HttpStatusCode.NoContent)
        {
            return null;
        }

        using var ms = new MemoryStream();
        (await res.Content.ReadAsStreamAsync(ct)).CopyTo(ms);
        return ms.ToArray();
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
        var query = await new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["resRestaurantId"] = resRestaurantId?.ToString(CultureInfo.InvariantCulture),
            ["noteMin"] = noteMin?.ToString(CultureInfo.InvariantCulture),
            ["approuve"] = approuve?.ToString(CultureInfo.InvariantCulture),
            ["dateDebut"] = dateDebut?.ToString(CultureInfo.InvariantCulture),
            ["dateFin"] = dateFin?.ToString(CultureInfo.InvariantCulture),
        }.Where(kv => kv.Value != null)).ReadAsStringAsync(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/avis?{query}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<ICollection<AvisClientRead>>(_jsOptions, ct);
    }

    /// <summary>
    /// Récupère un client avec toutes ses commandes.
    /// </summary>
    /// <param name="cliId">Identifiant du client.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Client avec ses commandes.</returns>
    public async Task<ClientAvecCommandes> GetClientAvecCommandes(int cliId, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/clients/{cliId}/avec-commandes"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<ClientAvecCommandes>(_jsOptions, ct);
    }

    /// <summary>
    /// Récupère le détail complet d'une commande avec ses lignes.
    /// </summary>
    /// <param name="comId">Identifiant de la commande.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Détail complet de la commande.</returns>
    public async Task<CommandeDetailRead> GetCommandeDetail(int comId, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/commandes/{comId}/detail"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<CommandeDetailRead>(_jsOptions, ct);
    }

    /// <summary>
    /// Récupère un menu spécifique d'un restaurant.
    /// </summary>
    /// <param name="resId">Identifiant du restaurant.</param>
    /// <param name="menId">Identifiant du menu.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Menu du restaurant.</returns>
    public async Task<MenuComplet> GetRestaurantMenu(int resId, int menId, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/{resId}/menus/{menId}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<MenuComplet>(_jsOptions, ct);
    }

    /// <summary>
    /// Récupère les statistiques d'un restaurant.
    /// </summary>
    /// <param name="resId">Identifiant du restaurant.</param>
    /// <param name="dateDebut">Date de début pour le calcul des statistiques.</param>
    /// <param name="dateFin">Date de fin pour le calcul des statistiques.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Statistiques du restaurant.</returns>
    public async Task<StatistiquesRestaurant> GetRestaurantStatistiques(int resId, DateTime? dateDebut = null, DateTime? dateFin = null, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        var query = await new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["dateDebut"] = dateDebut?.ToString(CultureInfo.InvariantCulture),
            ["dateFin"] = dateFin?.ToString(CultureInfo.InvariantCulture),
        }.Where(kv => kv.Value != null)).ReadAsStringAsync(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/{resId}/statistiques?{query}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<StatistiquesRestaurant>(_jsOptions, ct);
    }

    /// <summary>
    /// Met à jour partiellement une promotion.
    /// </summary>
    /// <param name="proId">Identifiant de la promotion.</param>
    /// <param name="promotion">Données partielles de la promotion.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Promotion mise à jour.</returns>
    public async Task<PromotionRead> PatchPromotion(int proId, PromotionWrite promotion, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Patch, $"api/restaurants/promotions/{proId}") { Content = JsonContent.Create(promotion, options: _jsOptions) }, HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<PromotionRead>(_jsOptions, ct);
    }

    /// <summary>
    /// Recherche avancée de restaurants.
    /// </summary>
    /// <param name="nom">Nom du restaurant (recherche partielle).</param>
    /// <param name="adresse">Adresse du restaurant (recherche partielle).</param>
    /// <param name="noteMin">Note minimum requise.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Liste des restaurants correspondant aux critères.</returns>
    public async Task<ICollection<RestaurantAvecStatistiques>> SearchRestaurants(string nom = null, string adresse = null, int? noteMin = null, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        var query = await new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["nom"] = nom,
            ["adresse"] = adresse,
            ["noteMin"] = noteMin?.ToString(CultureInfo.InvariantCulture),
        }.Where(kv => kv.Value != null)).ReadAsStringAsync(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/search?{query}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<ICollection<RestaurantAvecStatistiques>>(_jsOptions, ct);
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
