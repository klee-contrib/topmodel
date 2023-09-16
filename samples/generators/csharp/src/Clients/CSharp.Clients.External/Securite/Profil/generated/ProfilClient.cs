////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Models.CSharp.Securite.Profil.Models;

namespace CSharp.Clients.External.Securite.Profil;

/// <summary>
/// Client Profil.
/// </summary>
public partial class ProfilClient
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsOptions = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    /// <summary>
    /// Constructeur.
    /// </summary>
    /// <param name="client">HttpClient injecté.</param>
    public ProfilClient(HttpClient client)
    {
        _client = client;
    }

    /// <summary>
    /// Ajoute un Profil.
    /// </summary>
    /// <param name="profil">Profil à sauvegarder.</param>
    /// <returns>Profil sauvegardé.</returns>
    public async Task<ProfilRead> AddProfil(ProfilWrite profil)
    {
        await EnsureAuthentication();
        using var res = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Post, $"api/profils") { Content = GetBody(profil) }, HttpCompletionOption.ResponseHeadersRead);
        await EnsureSuccess(res);
        return await Deserialize<ProfilRead>(res);
    }

    /// <summary>
    /// Charge le détail d'un Profil.
    /// </summary>
    /// <param name="proId">Id technique.</param>
    /// <returns>Le détail de l'Profil.</returns>
    public async Task<ProfilRead> GetProfil(int proId)
    {
        await EnsureAuthentication();
        using var res = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"api/profils/{proId}"), HttpCompletionOption.ResponseHeadersRead);
        await EnsureSuccess(res);
        return await Deserialize<ProfilRead>(res);
    }

    /// <summary>
    /// Liste tous les Profils.
    /// </summary>
    /// <returns>Profils matchant les critères.</returns>
    public async Task<ICollection<ProfilItem>> GetProfils()
    {
        await EnsureAuthentication();
        using var res = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"api/profils"), HttpCompletionOption.ResponseHeadersRead);
        await EnsureSuccess(res);
        return await Deserialize<ICollection<ProfilItem>>(res);
    }

    /// <summary>
    /// Sauvegarde un Profil.
    /// </summary>
    /// <param name="proId">Id technique.</param>
    /// <param name="profil">Profil à sauvegarder.</param>
    /// <returns>Profil sauvegardé.</returns>
    public async Task<ProfilRead> UpdateProfil(int proId, ProfilWrite profil)
    {
        await EnsureAuthentication();
        using var res = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Put, $"api/profils/{proId}") { Content = GetBody(profil) }, HttpCompletionOption.ResponseHeadersRead);
        await EnsureSuccess(res);
        return await Deserialize<ProfilRead>(res);
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
