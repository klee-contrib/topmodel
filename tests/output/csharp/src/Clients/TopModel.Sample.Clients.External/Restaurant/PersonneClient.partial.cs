namespace TopModel.Sample.Clients.External.Restaurant;

/// <summary>
/// Client Personne.
/// </summary>
public partial class PersonneClient
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
