////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using TopModel.Sample.Restaurant.Models;

namespace TopModel.Sample.Clients.External.Restaurant;

/// <summary>
/// Client Endpoints.
/// </summary>
/// <param name="client">HttpClient injecté.</param>
public partial class EndpointsClient(HttpClient client)
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
        using var res = await client.SendAsync(new(HttpMethod.Post, $"api/restaurants/clients") { Content = JsonContent.Create(client, options: _jsOptions) }, HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<ClientRead>(_jsOptions, ct);
    }

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

        return await res.Content.ReadFromJsonAsync<CommandeRead>(_jsOptions, ct);
    }

    /// <summary>
    /// Ajoute une ligne de commande.
    /// </summary>
    /// <param name="ligneCommande">Ligne de commande à créer.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Ligne de commande créée.</returns>
    public async Task<LigneCommandeRead> AddLigneCommande(LigneCommandeWrite ligneCommande, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Post, $"api/restaurants/ligne-commandes") { Content = JsonContent.Create(ligneCommande, options: _jsOptions) }, HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<LigneCommandeRead>(_jsOptions, ct);
    }

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

        return await res.Content.ReadFromJsonAsync<PlatRead>(_jsOptions, ct);
    }

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

        return await res.Content.ReadFromJsonAsync<RestaurantRead>(_jsOptions, ct);
    }

    /// <summary>
    /// Ajoute une table.
    /// </summary>
    /// <param name="table">Table à créer.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Table créée.</returns>
    public async Task<TableClientRead> AddTable(TableClientWrite table, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Post, $"api/restaurants/tables") { Content = JsonContent.Create(table, options: _jsOptions) }, HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<TableClientRead>(_jsOptions, ct);
    }

    /// <summary>
    /// Supprime un client.
    /// </summary>
    /// <param name="cliId">Identifiant du client.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Task.</returns>
    public async Task DeleteClient(int cliId, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Delete, $"api/restaurants/clients/{cliId}"), ct);
        await EnsureSuccess(res, ct);
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
    /// Supprime une ligne de commande.
    /// </summary>
    /// <param name="ligId">Identifiant de la ligne.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Task.</returns>
    public async Task DeleteLigneCommande(int ligId, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Delete, $"api/restaurants/ligne-commandes/{ligId}"), ct);
        await EnsureSuccess(res, ct);
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
    /// Liste toutes les catégories de plats.
    /// </summary>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Liste des catégories de plats.</returns>
    public async Task<ICollection<CategoriePlat>> GetCategoriePlats(CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/categorie-plats"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<ICollection<CategoriePlat>>(_jsOptions, ct);
    }

    /// <summary>
    /// Charge le détail d'un client.
    /// </summary>
    /// <param name="cliId">Identifiant du client.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Détail du client.</returns>
    public async Task<ClientRead> GetClient(int cliId, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/clients/{cliId}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<ClientRead>(_jsOptions, ct);
    }

    /// <summary>
    /// Liste les commandes d'un client.
    /// </summary>
    /// <param name="cliId">Identifiant du client.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Liste des commandes du client.</returns>
    public async Task<ICollection<CommandeItem>> GetClientCommandes(int cliId, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/clients/{cliId}/commandes"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<ICollection<CommandeItem>>(_jsOptions, ct);
    }

    /// <summary>
    /// Liste tous les clients.
    /// </summary>
    /// <param name="nom">Nom du client.</param>
    /// <param name="email">Adresse email du client.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Liste des clients.</returns>
    public async Task<ICollection<ClientItem>> GetClients(string nom = null, string email = null, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        var query = await new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["nom"] = nom,
            ["email"] = email,
        }.Where(kv => kv.Value != null)).ReadAsStringAsync(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/clients?{query}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<ICollection<ClientItem>>(_jsOptions, ct);
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

        return await res.Content.ReadFromJsonAsync<CommandeRead>(_jsOptions, ct);
    }

    /// <summary>
    /// Liste les lignes d'une commande.
    /// </summary>
    /// <param name="comId">Identifiant de la commande.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Liste des lignes de la commande.</returns>
    public async Task<ICollection<LigneCommandeItem>> GetCommandeLignes(int comId, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/commandes/{comId}/lignes"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<ICollection<LigneCommandeItem>>(_jsOptions, ct);
    }

    /// <summary>
    /// Liste toutes les commandes.
    /// </summary>
    /// <param name="clientId">Client ayant passé la commande.</param>
    /// <param name="statutCommandeCode">Statut de la commande.</param>
    /// <param name="tableClientId">Table associée à la commande.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Liste des commandes.</returns>
    public async Task<ICollection<CommandeItem>> GetCommandes(int? clientId = null, StatutCommande.Codes statutCommandeCode = StatutCommande.Codes.EN_ATT, int? tableClientId = null, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        var query = await new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["clientId"] = clientId?.ToString(CultureInfo.InvariantCulture),
            ["statutCommandeCode"] = statutCommandeCode?.ToString(CultureInfo.InvariantCulture),
            ["tableClientId"] = tableClientId?.ToString(CultureInfo.InvariantCulture),
        }.Where(kv => kv.Value != null)).ReadAsStringAsync(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/commandes?{query}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<ICollection<CommandeItem>>(_jsOptions, ct);
    }

    /// <summary>
    /// Récupère les commandes par date.
    /// </summary>
    /// <param name="dateCommande">Date et heure de la commande.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Commandes pour la date spécifiée.</returns>
    public async Task<ICollection<CommandeItem>> GetCommandesByDate(DateTime? dateCommande = null, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        var query = await new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["dateCommande"] = dateCommande?.ToString(CultureInfo.InvariantCulture),
        }.Where(kv => kv.Value != null)).ReadAsStringAsync(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/commandes/by-date?{query}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<ICollection<CommandeItem>>(_jsOptions, ct);
    }

    /// <summary>
    /// Charge le détail d'une ligne de commande.
    /// </summary>
    /// <param name="ligId">Identifiant de la ligne.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Détail de la ligne de commande.</returns>
    public async Task<LigneCommandeRead> GetLigneCommande(int ligId, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/ligne-commandes/{ligId}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<LigneCommandeRead>(_jsOptions, ct);
    }

    /// <summary>
    /// Liste toutes les lignes de commande.
    /// </summary>
    /// <param name="commandeId">Commande à laquelle appartient la ligne.</param>
    /// <param name="platId">Plat commandé.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Liste des lignes de commande.</returns>
    public async Task<ICollection<LigneCommandeItem>> GetLigneCommandes(int? commandeId = null, int? platId = null, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        var query = await new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["commandeId"] = commandeId?.ToString(CultureInfo.InvariantCulture),
            ["platId"] = platId?.ToString(CultureInfo.InvariantCulture),
        }.Where(kv => kv.Value != null)).ReadAsStringAsync(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/ligne-commandes?{query}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<ICollection<LigneCommandeItem>>(_jsOptions, ct);
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

        return await res.Content.ReadFromJsonAsync<PlatRead>(_jsOptions, ct);
    }

    /// <summary>
    /// Liste tous les plats.
    /// </summary>
    /// <param name="disponible">Indique si le plat est disponible.</param>
    /// <param name="restaurantIdRestaurant">Restaurant proposant ce plat.</param>
    /// <param name="categoriePlatCodeCategoriePlat">Catégorie du plat.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Liste des plats.</returns>
    public async Task<ICollection<PlatItem>> GetPlats(bool disponible = true, int? restaurantIdRestaurant = null, CategoriePlat.Codes? categoriePlatCodeCategoriePlat = null, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        var query = await new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["disponible"] = disponible?.ToString(CultureInfo.InvariantCulture),
            ["restaurantIdRestaurant"] = restaurantIdRestaurant?.ToString(CultureInfo.InvariantCulture),
            ["categoriePlatCodeCategoriePlat"] = categoriePlatCodeCategoriePlat?.ToString(CultureInfo.InvariantCulture),
        }.Where(kv => kv.Value != null)).ReadAsStringAsync(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/plats?{query}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<ICollection<PlatItem>>(_jsOptions, ct);
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

        return await res.Content.ReadFromJsonAsync<RestaurantRead>(_jsOptions, ct);
    }

    /// <summary>
    /// Liste les plats d'un restaurant.
    /// </summary>
    /// <param name="resId">Identifiant du restaurant.</param>
    /// <param name="disponible">Indique si le plat est disponible.</param>
    /// <param name="categoriePlatCodeCategoriePlat">Catégorie du plat.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Liste des plats du restaurant.</returns>
    public async Task<ICollection<PlatItem>> GetRestaurantPlats(int resId, bool disponible = true, CategoriePlat.Codes? categoriePlatCodeCategoriePlat = null, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        var query = await new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["disponible"] = disponible?.ToString(CultureInfo.InvariantCulture),
            ["categoriePlatCodeCategoriePlat"] = categoriePlatCodeCategoriePlat?.ToString(CultureInfo.InvariantCulture),
        }.Where(kv => kv.Value != null)).ReadAsStringAsync(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/{resId}/plats?{query}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<ICollection<PlatItem>>(_jsOptions, ct);
    }

    /// <summary>
    /// Liste les tables d'un restaurant.
    /// </summary>
    /// <param name="resId">Identifiant du restaurant.</param>
    /// <param name="disponible">Indique si la table est disponible.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Liste des tables du restaurant.</returns>
    public async Task<ICollection<TableClientItem>> GetRestaurantTables(int resId, bool disponible = true, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        var query = await new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["disponible"] = disponible?.ToString(CultureInfo.InvariantCulture),
        }.Where(kv => kv.Value != null)).ReadAsStringAsync(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/{resId}/tables?{query}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<ICollection<TableClientItem>>(_jsOptions, ct);
    }

    /// <summary>
    /// Liste tous les restaurants.
    /// </summary>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Liste des restaurants.</returns>
    public async Task<ICollection<RestaurantItem>> GetRestaurants(CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<ICollection<RestaurantItem>>(_jsOptions, ct);
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

        return await res.Content.ReadFromJsonAsync<ICollection<StatutCommande>>(_jsOptions, ct);
    }

    /// <summary>
    /// Charge le détail d'une table.
    /// </summary>
    /// <param name="tabId">Identifiant de la table.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Détail de la table.</returns>
    public async Task<TableClientRead> GetTable(int tabId, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/tables/{tabId}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<TableClientRead>(_jsOptions, ct);
    }

    /// <summary>
    /// Liste toutes les tables.
    /// </summary>
    /// <param name="restaurantIdRestaurant">Restaurant auquel appartient la table.</param>
    /// <param name="disponible">Indique si la table est disponible.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Liste des tables.</returns>
    public async Task<ICollection<TableClientItem>> GetTables(int? restaurantIdRestaurant = null, bool disponible = true, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        var query = await new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["restaurantIdRestaurant"] = restaurantIdRestaurant?.ToString(CultureInfo.InvariantCulture),
            ["disponible"] = disponible?.ToString(CultureInfo.InvariantCulture),
        }.Where(kv => kv.Value != null)).ReadAsStringAsync(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/tables?{query}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<ICollection<TableClientItem>>(_jsOptions, ct);
    }

    /// <summary>
    /// Met à jour partiellement un client.
    /// </summary>
    /// <param name="cliId">Identifiant du client.</param>
    /// <param name="client">Données partielles du client.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Client mis à jour.</returns>
    public async Task<ClientRead> PatchClient(int cliId, ClientWrite client, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Patch, $"api/restaurants/clients/{cliId}") { Content = JsonContent.Create(client, options: _jsOptions) }, HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<ClientRead>(_jsOptions, ct);
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

        return await res.Content.ReadFromJsonAsync<CommandeRead>(_jsOptions, ct);
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

        return await res.Content.ReadFromJsonAsync<PlatRead>(_jsOptions, ct);
    }

    /// <summary>
    /// Recherche de plats avec critères multiples.
    /// </summary>
    /// <param name="nom">Nom du plat.</param>
    /// <param name="restaurantIdRestaurant">Restaurant proposant ce plat.</param>
    /// <param name="categoriePlatCodeCategoriePlat">Catégorie du plat.</param>
    /// <param name="disponible">Indique si le plat est disponible.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Plats correspondant aux critères de recherche.</returns>
    public async Task<ICollection<PlatItem>> SearchPlats(string nom = null, int? restaurantIdRestaurant = null, CategoriePlat.Codes? categoriePlatCodeCategoriePlat = null, bool disponible = true, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        var query = await new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["nom"] = nom,
            ["restaurantIdRestaurant"] = restaurantIdRestaurant?.ToString(CultureInfo.InvariantCulture),
            ["categoriePlatCodeCategoriePlat"] = categoriePlatCodeCategoriePlat?.ToString(CultureInfo.InvariantCulture),
            ["disponible"] = disponible?.ToString(CultureInfo.InvariantCulture),
        }.Where(kv => kv.Value != null)).ReadAsStringAsync(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/restaurants/plats/search?{query}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<ICollection<PlatItem>>(_jsOptions, ct);
    }

    /// <summary>
    /// Met à jour un client.
    /// </summary>
    /// <param name="cliId">Identifiant du client.</param>
    /// <param name="client">Client à mettre à jour.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Client mis à jour.</returns>
    public async Task<ClientRead> UpdateClient(int cliId, ClientWrite client, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Put, $"api/restaurants/clients/{cliId}") { Content = JsonContent.Create(client, options: _jsOptions) }, HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<ClientRead>(_jsOptions, ct);
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

        return await res.Content.ReadFromJsonAsync<CommandeRead>(_jsOptions, ct);
    }

    /// <summary>
    /// Met à jour uniquement le statut d'une commande.
    /// </summary>
    /// <param name="comId">Identifiant de la commande.</param>
    /// <param name="statutCommandeCode">Statut de la commande.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Commande avec le statut mis à jour.</returns>
    public async Task<CommandeRead> UpdateCommandeStatut(int comId, StatutCommande.Codes statutCommandeCode = StatutCommande.Codes.EN_ATT, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        var query = await new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["statutCommandeCode"] = statutCommandeCode?.ToString(CultureInfo.InvariantCulture),
        }.Where(kv => kv.Value != null)).ReadAsStringAsync(ct);
        using var res = await client.SendAsync(new(HttpMethod.Patch, $"api/restaurants/commandes/{comId}/statut?{query}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<CommandeRead>(_jsOptions, ct);
    }

    /// <summary>
    /// Met à jour une ligne de commande.
    /// </summary>
    /// <param name="ligId">Identifiant de la ligne.</param>
    /// <param name="ligneCommande">Ligne de commande à mettre à jour.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Ligne de commande mise à jour.</returns>
    public async Task<LigneCommandeRead> UpdateLigneCommande(int ligId, LigneCommandeWrite ligneCommande, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Put, $"api/restaurants/ligne-commandes/{ligId}") { Content = JsonContent.Create(ligneCommande, options: _jsOptions) }, HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<LigneCommandeRead>(_jsOptions, ct);
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

        return await res.Content.ReadFromJsonAsync<PlatRead>(_jsOptions, ct);
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

        return await res.Content.ReadFromJsonAsync<RestaurantRead>(_jsOptions, ct);
    }

    /// <summary>
    /// Met à jour une table.
    /// </summary>
    /// <param name="tabId">Identifiant de la table.</param>
    /// <param name="table">Table à mettre à jour.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Table mise à jour.</returns>
    public async Task<TableClientRead> UpdateTable(int tabId, TableClientWrite table, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Put, $"api/restaurants/tables/{tabId}") { Content = JsonContent.Create(table, options: _jsOptions) }, HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<TableClientRead>(_jsOptions, ct);
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
