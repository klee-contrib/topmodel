namespace TopModel.Sample.Clients.External.Securite.Utilisateur;

/// <summary>
/// Client Utilisateur.
/// </summary>
public partial class UtilisateurClient
{
    private partial Task EnsureAuthentication(CancellationToken ct)
    {
        return Task.CompletedTask;
    }

    private partial Task EnsureSuccess(HttpResponseMessage response, CancellationToken ct)
    {
        response.EnsureSuccessStatusCode();
        return Task.CompletedTask;
    }
}
