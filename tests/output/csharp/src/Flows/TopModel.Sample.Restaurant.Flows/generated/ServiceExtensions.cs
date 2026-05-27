////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using Kinetix.Etl;
using Microsoft.Extensions.DependencyInjection;

namespace Flows.TopModel.Sample.Restaurant.Flows;

public static class ServiceExtensions
{
    public static IServiceCollection AddRestaurantDataFlows(this IServiceCollection services)
    {
        return services
            .AddSingleton<IDataFlow, ExportMenusFlow>()
            .AddSingleton<IDataFlow, ExportRestaurantsFlow>();
    }
}
