////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using TopModel.Sample.Restaurant.Models;

namespace TopModel.Sample.Clients.External.Restaurant;

/// <summary>
/// Client Restaurant.
/// </summary>
/// <param name="client">HttpClient injecté.</param>
public partial class RestaurantClient(HttpClient client)
{
    private readonly JsonSerializerOptions _jsOptions = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    /// <summary>
    /// Ajoute un restaurant.
    /// </summary>
    /// <param name="restaurant">Restaurant à créer.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Restaurant créé.</returns>
    public async Task<RestaurantRead> AddRestaurant(RestaurantWrite restaurant, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Post, $"api/restaurants") { Content = JsonContent.Create(restaurant, options: _jsOptions) }, HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<RestaurantRead>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Ajoute une table.
    /// </summary>
    /// <param name="table">Table à créer.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Table créée.</returns>
    public async Task<TableRead> AddTable(TableWrite table, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Post, $"api/restaurants/tables") { Content = JsonContent.Create(table, options: _jsOptions) }, HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<TableRead>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Supprime un restaurant.
    /// </summary>
    /// <param name="resId">Identifiant du restaurant.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Task.</returns>
    public async Task DeleteRestaurant(int resId, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Delete, $"api/restaurants/{resId}"), ct);
        await EnsureSuccess(res, ct);
    }

    /// <summary>
    /// Supprime une table.
    /// </summary>
    /// <param name="tabId">Identifiant de la table.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Task.</returns>
    public async Task DeleteTable(int tabId, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Delete, $"api/restaurants/tables/{tabId}"), ct);
        await EnsureSuccess(res, ct);
    }

    /// <summary>
    /// Charge le détail d'un restaurant.
    /// </summary>
    /// <param name="resId">Identifiant du restaurant.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Détail du restaurant.</returns>
    public async Task<RestaurantRead> GetRestaurant(int resId, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/{resId}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<RestaurantRead>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Récupère un menu spécifique d'un restaurant.
    /// </summary>
    /// <param name="resId">Identifiant du restaurant.</param>
    /// <param name="menId">Identifiant du menu.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Menu du restaurant.</returns>
    public async Task<MenuRead> GetRestaurantMenu(int resId, int menId, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/{resId}/menus/{menId}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<MenuRead>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Liste les plats d'un restaurant.
    /// </summary>
    /// <param name="resId">Identifiant du restaurant.</param>
    /// <param name="categoriePlatCode">Catégorie du plat.</param>
    /// <param name="disponible">Indique si le plat est disponible.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Liste des plats du restaurant.</returns>
    public async Task<ICollection<IPlatItem>> GetRestaurantPlats(int resId, CategoriePlat.Codes categoriePlatCode, bool disponible = true, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        var query = await new FormUrlEncodedContent(new Dictionary<string, string?>
        {
            ["categoriePlatCode"] = categoriePlatCode.ToString(),
            ["disponible"] = disponible.ToString(),
        }.Where(kv => kv.Value != null)).ReadAsStringAsync(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/{resId}/plats?{query}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<ICollection<IPlatItem>>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Récupère les statistiques d'un restaurant.
    /// </summary>
    /// <param name="resId">Identifiant du restaurant.</param>
    /// <param name="dateDebut">Date de début pour le calcul des statistiques.</param>
    /// <param name="dateFin">Date de fin pour le calcul des statistiques.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Statistiques du restaurant.</returns>
    public async Task<StatistiquesRestaurant> GetRestaurantStatistiques(int resId, DateTime dateDebut, DateTime dateFin, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        var query = await new FormUrlEncodedContent(new Dictionary<string, string?>
        {
            ["dateDebut"] = dateDebut.ToString("o"),
            ["dateFin"] = dateFin.ToString("o"),
        }.Where(kv => kv.Value != null)).ReadAsStringAsync(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/{resId}/statistiques?{query}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<StatistiquesRestaurant>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Liste les tables d'un restaurant.
    /// </summary>
    /// <param name="resId">Identifiant du restaurant.</param>
    /// <param name="disponible">Indique si la table est disponible.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Liste des tables du restaurant.</returns>
    public async Task<ICollection<ITableItem>> GetRestaurantTables(int resId, bool disponible = true, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        var query = await new FormUrlEncodedContent(new Dictionary<string, string?>
        {
            ["disponible"] = disponible.ToString(),
        }.Where(kv => kv.Value != null)).ReadAsStringAsync(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/{resId}/tables?{query}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<ICollection<ITableItem>>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Liste tous les restaurants.
    /// </summary>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Liste des restaurants.</returns>
    public async Task<ICollection<IRestaurantItem>> GetRestaurants(CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<ICollection<IRestaurantItem>>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Charge le détail d'une table.
    /// </summary>
    /// <param name="tabId">Identifiant de la table.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Détail de la table.</returns>
    public async Task<TableRead> GetTable(int tabId, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/tables/{tabId}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<TableRead>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Liste toutes les tables.
    /// </summary>
    /// <param name="restaurantId">Restaurant auquel appartient la table.</param>
    /// <param name="disponible">Indique si la table est disponible.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Liste des tables.</returns>
    public async Task<ICollection<ITableItem>> GetTables(int restaurantId, bool disponible = true, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        var query = await new FormUrlEncodedContent(new Dictionary<string, string?>
        {
            ["restaurantId"] = restaurantId.ToString(),
            ["disponible"] = disponible.ToString(),
        }.Where(kv => kv.Value != null)).ReadAsStringAsync(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/tables?{query}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<ICollection<ITableItem>>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Recherche avancée de restaurants.
    /// </summary>
    /// <param name="nom">Nom du restaurant (recherche partielle).</param>
    /// <param name="adresse">Adresse du restaurant (recherche partielle).</param>
    /// <param name="noteMin">Note minimum requise.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Liste des restaurants correspondant aux critères.</returns>
    public async Task<ICollection<RestaurantAvecStatistiques>> SearchRestaurants(string nom, string? adresse = null, int? noteMin = null, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        var query = await new FormUrlEncodedContent(new Dictionary<string, string?>
        {
            ["nom"] = nom,
            ["adresse"] = adresse,
            ["noteMin"] = noteMin?.ToString(),
        }.Where(kv => kv.Value != null)).ReadAsStringAsync(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/search?{query}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<ICollection<RestaurantAvecStatistiques>>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Met à jour un restaurant.
    /// </summary>
    /// <param name="resId">Identifiant du restaurant.</param>
    /// <param name="restaurant">Restaurant à mettre à jour.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Restaurant mis à jour.</returns>
    public async Task<RestaurantRead> UpdateRestaurant(int resId, RestaurantWrite restaurant, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Put, $"api/restaurants/{resId}") { Content = JsonContent.Create(restaurant, options: _jsOptions) }, HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<RestaurantRead>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Met à jour une table.
    /// </summary>
    /// <param name="tabId">Identifiant de la table.</param>
    /// <param name="table">Table à mettre à jour.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Table mise à jour.</returns>
    public async Task<TableRead> UpdateTable(int tabId, TableWrite table, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Put, $"api/restaurants/tables/{tabId}") { Content = JsonContent.Create(table, options: _jsOptions) }, HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<TableRead>(_jsOptions, ct))!;
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
