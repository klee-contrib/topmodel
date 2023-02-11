////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.Globalization;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using CSharp.Clients.Db.Models.Securite;
using Models.CSharp.Utilisateur.Models;

namespace Clients.CSharp.Clients.External.Securite.Utilisateur;

/// <summary>
/// Client Securite.Utilisateur.
/// </summary>
public partial class UtilisateurApiClient
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsOptions = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    /// <summary>
    /// Constructeur.
    /// </summary>
    /// <param name="client">HttpClient injecté.</param>
    public UtilisateurApiClient(HttpClient client)
    {
        _client = client;
    }

    /// <summary>
    /// Recherche des utilisateurs.
    /// </summary>
    /// <param name="utiId">Id technique.</param>
    /// <returns>Task.</returns>
    public async Task deleteAll(int[] utiId = null)
    {
        await EnsureAuthentication();
        var query = await new FormUrlEncodedContent(new Dictionary<string, string>
        {
        }.Concat(Id?.Select(i => new KeyValuePair<string, string>("Id", i.ToString(CultureInfo.InvariantCulture))) ?? new Dictionary<string, string>())
         .Where(kv => kv.Value != null)).ReadAsStringAsync();
        using var res = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Delete, $"utilisateur/deleteAll?{query}"));
        await EnsureSuccess(res);
    }

    /// <summary>
    /// Charge le détail d'un utilisateur.
    /// </summary>
    /// <param name="utiId">Id technique.</param>
    /// <returns>Le détail de l'utilisateur.</returns>
    public async Task<UtilisateurDto> find(int utiId)
    {
        await EnsureAuthentication();
        using var res = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"utilisateur/{utiId}"), HttpCompletionOption.ResponseHeadersRead);
        await EnsureSuccess(res);
        return await Deserialize<UtilisateurDto>(res);
    }

    /// <summary>
    /// Charge une liste d'utilisateurs par leur type.
    /// </summary>
    /// <param name="typeUtilisateurCode">Type d'utilisateur en Many to one.</param>
    /// <returns>Liste des utilisateurs.</returns>
    public async Task<IEnumerable<UtilisateurDto>> findAllByType(TypeUtilisateur.Codes typeUtilisateurCode = TypeUtilisateur.Codes.ADM)
    {
        await EnsureAuthentication();
        var query = await new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["typeUtilisateurCode"] = typeUtilisateurCode?.ToString(CultureInfo.InvariantCulture),
        }.Where(kv => kv.Value != null)).ReadAsStringAsync();
        using var res = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"utilisateur/list?{query}"), HttpCompletionOption.ResponseHeadersRead);
        await EnsureSuccess(res);
        return await Deserialize<IEnumerable<UtilisateurDto>>(res);
    }

    /// <summary>
    /// Sauvegarde un utilisateur.
    /// </summary>
    /// <param name="utilisateur">Utilisateur à sauvegarder.</param>
    /// <returns>Utilisateur sauvegardé.</returns>
    public async Task<UtilisateurDto> save(UtilisateurDto utilisateur)
    {
        await EnsureAuthentication();
        using var res = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Post, $"utilisateur/save") { Content = GetBody(utilisateur) }, HttpCompletionOption.ResponseHeadersRead);
        await EnsureSuccess(res);
        return await Deserialize<UtilisateurDto>(res);
    }

    /// <summary>
    /// Recherche des utilisateurs.
    /// </summary>
    /// <param name="utiId">Id technique.</param>
    /// <param name="age">Age en années de l'utilisateur.</param>
    /// <param name="profilId">Profil de l'utilisateur.</param>
    /// <param name="email">Email de l'utilisateur.</param>
    /// <param name="nom">Nom de l'utilisateur.</param>
    /// <param name="typeUtilisateurCode">Type d'utilisateur en Many to one.</param>
    /// <param name="dateCreation">Date de création de l'utilisateur.</param>
    /// <param name="dateModification">Date de modification de l'utilisateur.</param>
    /// <returns>Utilisateurs matchant les critères.</returns>
    public async Task<ICollection<UtilisateurDto>> search(int? utiId = null, decimal age = 6l, int? profilId = null, string email = null, string nom = "Jabx", TypeUtilisateur.Codes typeUtilisateurCode = TypeUtilisateur.Codes.ADM, DateOnly? dateCreation = null, DateOnly? dateModification = null)
    {
        await EnsureAuthentication();
        var query = await new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["utiId"] = utiId?.ToString(CultureInfo.InvariantCulture),
            ["age"] = age?.ToString(CultureInfo.InvariantCulture),
            ["profilId"] = profilId?.ToString(CultureInfo.InvariantCulture),
            ["email"] = email,
            ["nom"] = nom,
            ["typeUtilisateurCode"] = typeUtilisateurCode?.ToString(CultureInfo.InvariantCulture),
            ["dateCreation"] = dateCreation?.ToString(CultureInfo.InvariantCulture),
            ["dateModification"] = dateModification?.ToString(CultureInfo.InvariantCulture),
        }.Where(kv => kv.Value != null)).ReadAsStringAsync();
        using var res = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Post, $"utilisateur/search?{query}"), HttpCompletionOption.ResponseHeadersRead);
        await EnsureSuccess(res);
        return await Deserialize<ICollection<UtilisateurDto>>(res);
    }

    /// <summary>
    /// Déserialize le contenu d'une réponse HTTP.
    /// </summary>
    /// <typeparam name="T">Type de destination.</typeparam>
    /// <param name="response">Réponse HTTP.</param>
    /// <returns>Contenu.</returns>
    private async Task<T> Deserialize<T>(HttpResponseMessage response)
    {
        if (response.StatusCode == HttpStatusCode.NoContent)
        {
            return default;
        }

        using var res = await response.Content.ReadAsStreamAsync();
        return await JsonSerializer.DeserializeAsync<T>(res, _jsOptions);
    }

    /// <summary>
    /// Assure que l'authentification est configurée.
    /// </summary>
    private partial Task EnsureAuthentication();

    /// <summary>
    /// Gère les erreurs éventuelles retournées par l'API appelée.
    /// </summary>
    /// <param name="response">Réponse HTTP.</param>
    private partial Task EnsureSuccess(HttpResponseMessage response);

    /// <summary>
    /// Récupère le body d'une requête pour l'objet donné.
    /// </summary>
    /// <typeparam name="T">Type source.</typeparam>
    /// <param name="input">Entrée.</param>
    /// <returns>Contenu.</returns>
    private StringContent GetBody<T>(T input)
    {
        return new StringContent(JsonSerializer.Serialize(input, _jsOptions), Encoding.UTF8, "application/json");
    }
}
