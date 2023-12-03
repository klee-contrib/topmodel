namespace CSharp.Clients.External.Securite.Utilisateur;

/// <summary>
/// Client Utilisateur.
/// </summary>
public partial class UtilisateurClient
{
    private partial Task EnsureAuthentication()
    {
        return Task.CompletedTask;
    }

    private partial Task EnsureSuccess(HttpResponseMessage response)
    {
        response.EnsureSuccessStatusCode();
        return Task.CompletedTask;
    }
}
