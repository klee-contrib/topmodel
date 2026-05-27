////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using Kinetix.Etl;
using Microsoft.Extensions.Logging;
using TopModel.Sample.Clients.Db.Models.Restaurant;
using TopModel.Sample.Restaurant.Models;

namespace Flows.TopModel.Sample.Restaurant.Flows;

public partial class ExportRestaurantsFlow(ILogger<ExportRestaurantsFlow> logger, ConnectionPool connectionPool, EtlMonitor monitor)
     : DataFlow<RestaurantRead>(logger, connectionPool, monitor)
{
    private IConnection _primaryConnection1;

    public override string Name => "ExportRestaurants";

    protected override TargetMode TargetMode => TargetMode.Insert;

    protected override string TargetName => "external";

    public override string[] DependsOn => ["ExportMenus"];

    public override void Dispose()
    {
        base.Dispose();
        _primaryConnection1?.Dispose();
    }

    protected override async Task<IEnumerable<RestaurantRead>> GetData()
    {
        _primaryConnection1 = ConnectionPool.GetConnection("primary");

        return (await GetPrimarySource1(_primaryConnection1))
            .Select(Mappers.CreateRestaurantRead);
    }

    private static async Task<IEnumerable<Restaurant>> GetPrimarySource1(IConnection connection)
    {
        return await connection.QueryAllAsync<Restaurant>();
    }
}
