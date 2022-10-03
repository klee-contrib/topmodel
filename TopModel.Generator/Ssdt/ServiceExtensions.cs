using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TopModel.Core;
using static TopModel.Utils.ModelUtils;

namespace TopModel.Generator.Ssdt;

public static class ServiceExtensions
{
    public static IServiceCollection AddSsdt(this IServiceCollection services, string dn, IEnumerable<SsdtConfig>? configs)
    {
        if (configs != null)
        {
            for (var i = 0; i < configs.Count(); i++)
            {
                var config = configs.ElementAt(i);
                var number = i + 1;

                CombinePath(dn, config, c => c.InitListScriptFolder);
                CombinePath(dn, config, c => c.TableScriptFolder);
                CombinePath(dn, config, c => c.TableTypeScriptFolder);

                services.AddSingleton<IModelWatcher>(p =>
                    new SsdtGenerator(p.GetRequiredService<ILogger<SsdtGenerator>>(), config) { Number = number });
            }
        }

        return services;
    }
}
