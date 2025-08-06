namespace TopModel.Sample.Clients.External.Securite.Profil;

/// <summary>
/// Client Profil.
/// </summary>
public partial class ProfilClient
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
