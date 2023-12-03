namespace CSharp.Clients.External.Securite.Profil;

/// <summary>
/// Client Profil.
/// </summary>
public partial class ProfilClient
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
