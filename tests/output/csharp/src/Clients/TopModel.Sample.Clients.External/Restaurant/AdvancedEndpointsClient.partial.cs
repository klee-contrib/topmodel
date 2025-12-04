namespace TopModel.Sample.Clients.External.Restaurant;

/// <summary>
/// Client AdvancedEndpoints.
/// </summary>
public partial class AdvancedEndpointsClient
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
