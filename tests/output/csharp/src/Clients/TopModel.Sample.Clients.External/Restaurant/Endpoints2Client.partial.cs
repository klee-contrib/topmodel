namespace TopModel.Sample.Clients.External.Restaurant;

/// <summary>
/// Client Endpoints2.
/// </summary>
public partial class Endpoints2Client
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
