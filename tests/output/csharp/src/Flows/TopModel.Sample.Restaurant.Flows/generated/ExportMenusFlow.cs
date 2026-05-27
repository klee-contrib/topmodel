////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using Kinetix.Etl;
using Microsoft.Extensions.Logging;
using TopModel.Sample.Clients.Db.Models.Restaurant;
using TopModel.Sample.Restaurant.Models;

namespace Flows.TopModel.Sample.Restaurant.Flows;

public partial class ExportMenusFlow(ILogger<ExportMenusFlow> logger, ConnectionPool connectionPool, EtlMonitor monitor)
     : DataFlow<MenuRead>(logger, connectionPool, monitor)
{
    private IConnection _primaryConnection1;

    public override string Name => "ExportMenus";

    protected override TargetMode TargetMode => TargetMode.Insert;

    protected override string TargetName => "external";

    public override void Dispose()
    {
        base.Dispose();
        _primaryConnection1?.Dispose();
    }

    protected override async Task<IEnumerable<MenuRead>> GetData()
    {
        _primaryConnection1 = ConnectionPool.GetConnection("primary");

        return (await GetPrimarySource1(_primaryConnection1))
            .Select(Mappers.CreateMenuRead);
    }

    private static async Task<IEnumerable<Menu>> GetPrimarySource1(IConnection connection)
    {
        return await connection.QueryAllAsync<Menu>();
    }
}
