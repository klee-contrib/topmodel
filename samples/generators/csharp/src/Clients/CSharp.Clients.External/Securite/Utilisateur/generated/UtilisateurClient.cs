////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.Globalization;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Models.CSharp.Securite.Utilisateur.Models;

namespace CSharp.Clients.External.Securite.Utilisateur;

/// <summary>
/// Client Utilisateur.
/// </summary>
public partial class UtilisateurClient
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsOptions = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    /// <summary>
    /// Constructeur.
    /// </summary>
    /// <param name="client">HttpClient injecté.</param>
    public UtilisateurClient(HttpClient client)
    {
        _client = client;
    }

    /// <summary>
    /// Ajoute un utilisateur.
    /// </summary>
    /// <param name="utilisateur">Utilisateur à sauvegarder.</param>
    /// <returns>Utilisateur sauvegardé.</returns>
    public async Task<UtilisateurRead> AddUtilisateur(UtilisateurWrite utilisateur)
    {
        await EnsureAuthentication();
        using var res = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Post, $"utilisateur") { Content = GetBody(utilisateur) }, HttpCompletionOption.ResponseHeadersRead);
        await EnsureSuccess(res);
        return await Deserialize<UtilisateurRead>(res);
    }

    /// <summary>
    /// Supprime un utilisateur.
    /// </summary>
    /// <param name="utiId">Id de l'utilisateur.</param>
    /// <returns>Task.</returns>
    public async Task DeleteUtilisateur(int utiId)
    {
        await EnsureAuthentication();
        using var res = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Delete, $"utilisateur/{utiId}"));
        await EnsureSuccess(res);
    }

    /// <summary>
    /// Charge le détail d'un utilisateur.
    /// </summary>
    /// <param name="utiId">Id de l'utilisateur.</param>
    /// <returns>Le détail de l'utilisateur.</returns>
    public async Task<UtilisateurRead> GetUtilisateur(int utiId)
    {
        await EnsureAuthentication();
        using var res = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"utilisateur/{utiId}"), HttpCompletionOption.ResponseHeadersRead);
        await EnsureSuccess(res);
        return await Deserialize<UtilisateurRead>(res);
    }

    /// <summary>
    /// Recherche des utilisateurs.
    /// </summary>
    /// <param name="nom">Nom de l'utilisateur.</param>
    /// <param name="prenom">Nom de l'utilisateur.</param>
    /// <param name="email">Email de l'utilisateur.</param>
    /// <param name="dateNaissance">Age de l'utilisateur.</param>
    /// <param name="actif">Si l'utilisateur est actif.</param>
    /// <param name="profilId">Profil de l'utilisateur.</param>
    /// <param name="typeUtilisateurCode">Type d'utilisateur.</param>
    /// <returns>Utilisateurs matchant les critères.</returns>
    public async Task<ICollection<UtilisateurItem>> SearchUtilisateur(string nom = null, string prenom = null, string email = null, DateTime? dateNaissance = null, bool? actif = null, int? profilId = null, TypeUtilisateur.Codes? typeUtilisateurCode = null)
    {
        await EnsureAuthentication();
        var query = await new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["nom"] = nom,
            ["prenom"] = prenom,
            ["email"] = email,
            ["dateNaissance"] = dateNaissance?.ToString(CultureInfo.InvariantCulture),
            ["actif"] = actif?.ToString(CultureInfo.InvariantCulture),
            ["profilId"] = profilId?.ToString(CultureInfo.InvariantCulture),
            ["typeUtilisateurCode"] = typeUtilisateurCode?.ToString(CultureInfo.InvariantCulture),
        }.Where(kv => kv.Value != null)).ReadAsStringAsync();
        using var res = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"utilisateur?{query}"), HttpCompletionOption.ResponseHeadersRead);
        await EnsureSuccess(res);
        return await Deserialize<ICollection<UtilisateurItem>>(res);
    }

    /// <summary>
    /// Sauvegarde un utilisateur.
    /// </summary>
    /// <param name="utiId">Id de l'utilisateur.</param>
    /// <param name="utilisateur">Utilisateur à sauvegarder.</param>
    /// <returns>Utilisateur sauvegardé.</returns>
    public async Task<UtilisateurRead> UpdateUtilisateur(int utiId, UtilisateurWrite utilisateur)
    {
        await EnsureAuthentication();
        using var res = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Put, $"utilisateur/{utiId}") { Content = GetBody(utilisateur) }, HttpCompletionOption.ResponseHeadersRead);
        await EnsureSuccess(res);
        return await Deserialize<UtilisateurRead>(res);
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
