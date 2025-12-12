namespace TopModel.Sample.Clients.External.Restaurant;

/// <summary>
/// Client Restaurant.
/// </summary>
public partial class RestaurantClient
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
