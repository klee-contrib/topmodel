////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using TopModel.Sample.Restaurant.Models;

namespace TopModel.Sample.Clients.External.Restaurant;

/// <summary>
/// Client Menu.
/// </summary>
/// <param name="client">HttpClient injecté.</param>
public partial class MenuClient(HttpClient client)
{
    private readonly JsonSerializerOptions _jsOptions = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    /// <summary>
    /// Ajoute un plat.
    /// </summary>
    /// <param name="plat">Plat à créer.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Plat créé.</returns>
    public async Task<PlatRead> AddPlat(PlatWrite plat, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Post, $"api/restaurants/plats") { Content = JsonContent.Create(plat, options: _jsOptions) }, HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<PlatRead>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Crée un menu avec ses plats.
    /// </summary>
    /// <param name="menu">Menu à créer.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Menu créé avec ses plats.</returns>
    public async Task<MenuRead> CreateMenu(MenuWrite menu, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Post, $"api/restaurants/menus") { Content = JsonContent.Create(menu, options: _jsOptions) }, HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<MenuRead>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Supprime un plat.
    /// </summary>
    /// <param name="plaId">Identifiant du plat.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Task.</returns>
    public async Task DeletePlat(int plaId, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Delete, $"api/restaurants/plats/{plaId}"), ct);
        await EnsureSuccess(res, ct);
    }

    /// <summary>
    /// Liste toutes les catégories de plats.
    /// </summary>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Liste des catégories de plats.</returns>
    public async Task<ICollection<CategoriePlat>> GetCategoriePlats(CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/categorie-plats"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<ICollection<CategoriePlat>>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Charge le détail d'un plat.
    /// </summary>
    /// <param name="plaId">Identifiant du plat.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Détail du plat.</returns>
    public async Task<PlatRead> GetPlat(int plaId, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/plats/{plaId}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<PlatRead>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Liste tous les plats.
    /// </summary>
    /// <param name="disponible">Indique si le plat est disponible.</param>
    /// <param name="restaurantId">Restaurant proposant ce plat.</param>
    /// <param name="categoriePlatCode">Catégorie du plat.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Liste des plats.</returns>
    public async Task<ICollection<PlatItem>> GetPlats(bool disponible = true, int? restaurantId = null, CategoriePlat.Codes? categoriePlatCode = null, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        var query = await new FormUrlEncodedContent(new Dictionary<string, string?>
        {
            ["disponible"] = disponible.ToString(),
            ["restaurantId"] = restaurantId?.ToString(),
            ["categoriePlatCode"] = categoriePlatCode?.ToString(),
        }.Where(kv => kv.Value != null)).ReadAsStringAsync(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/plats?{query}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<ICollection<PlatItem>>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Met à jour partiellement un plat.
    /// </summary>
    /// <param name="plaId">Identifiant du plat.</param>
    /// <param name="plat">Données partielles du plat.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Plat mis à jour.</returns>
    public async Task<PlatRead> PatchPlat(int plaId, PlatWrite plat, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Patch, $"api/restaurants/plats/{plaId}") { Content = JsonContent.Create(plat, options: _jsOptions) }, HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<PlatRead>(_jsOptions, ct))!;
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

        return (await res.Content.ReadFromJsonAsync<PromotionRead>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Recherche de plats avec critères multiples.
    /// </summary>
    /// <param name="nom">Nom du plat.</param>
    /// <param name="restaurantId">Restaurant proposant ce plat.</param>
    /// <param name="categoriePlatCode">Catégorie du plat.</param>
    /// <param name="disponible">Indique si le plat est disponible.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Plats correspondant aux critères de recherche.</returns>
    public async Task<ICollection<PlatItem>> SearchPlats(string? nom = null, int? restaurantId = null, CategoriePlat.Codes? categoriePlatCode = null, bool disponible = true, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        var query = await new FormUrlEncodedContent(new Dictionary<string, string?>
        {
            ["nom"] = nom,
            ["restaurantId"] = restaurantId?.ToString(),
            ["categoriePlatCode"] = categoriePlatCode?.ToString(),
            ["disponible"] = disponible.ToString(),
        }.Where(kv => kv.Value != null)).ReadAsStringAsync(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/plats/search?{query}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<ICollection<PlatItem>>(_jsOptions, ct))!;
    }

    /// <summary>
    /// Met à jour un plat.
    /// </summary>
    /// <param name="plaId">Identifiant du plat.</param>
    /// <param name="plat">Plat à mettre à jour.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Plat mis à jour.</returns>
    public async Task<PlatRead> UpdatePlat(int plaId, PlatWrite plat, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Put, $"api/restaurants/plats/{plaId}") { Content = JsonContent.Create(plat, options: _jsOptions) }, HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return (await res.Content.ReadFromJsonAsync<PlatRead>(_jsOptions, ct))!;
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
