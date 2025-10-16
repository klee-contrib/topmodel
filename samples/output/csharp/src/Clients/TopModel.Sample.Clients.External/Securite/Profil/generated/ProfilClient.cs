////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using TopModel.Sample.Securite.Models.Profil;

namespace TopModel.Sample.Clients.External.Securite.Profil;

/// <summary>
/// Client Profil.
/// </summary>
/// <param name="client">HttpClient injecté.</param>
public partial class ProfilClient(HttpClient client)
{
    private readonly JsonSerializerOptions _jsOptions = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    /// <summary>
    /// Ajoute un Profil.
    /// </summary>
    /// <param name="profil">Profil à sauvegarder.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Profil sauvegardé.</returns>
    public async Task<ProfilRead> AddProfil(ProfilWrite profil, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Post, $"api/profils") { Content = JsonContent.Create(profil, options: _jsOptions) }, HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<ProfilRead>(_jsOptions, ct);
    }

    /// <summary>
    /// Charge le détail d'un Profil.
    /// </summary>
    /// <param name="proId">Id technique.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Le détail du profil.</returns>
    public async Task<ProfilRead> GetProfil(int proId, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/profils/{proId}"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<ProfilRead>(_jsOptions, ct);
    }

    /// <summary>
    /// Liste tous les Profils.
    /// </summary>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Profils matchant les critères.</returns>
    public async Task<ICollection<ProfilItem>> GetProfils(CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Get, $"api/profils"), HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<ICollection<ProfilItem>>(_jsOptions, ct);
    }

    /// <summary>
    /// Sauvegarde un Profil.
    /// </summary>
    /// <param name="proId">Id technique.</param>
    /// <param name="profil">Profil à sauvegarder.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Profil sauvegardé.</returns>
    public async Task<ProfilRead> UpdateProfil(int proId, ProfilWrite profil, CancellationToken ct = default)
    {
        await EnsureAuthentication(ct);
        using var res = await client.SendAsync(new(HttpMethod.Put, $"api/profils/{proId}") { Content = JsonContent.Create(profil, options: _jsOptions) }, HttpCompletionOption.ResponseHeadersRead, ct);
        await EnsureSuccess(res, ct);

        return await res.Content.ReadFromJsonAsync<ProfilRead>(_jsOptions, ct);
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
