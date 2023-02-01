using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TopModel.Core;
using static TopModel.Utils.ModelUtils;

namespace TopModel.Generator.Ssdt;

public static class ServiceExtensions
{
    public static IServiceCollection AddSsdt(this IServiceCollection services, string dn, IEnumerable<SsdtConfig>? configs)
    {
        GeneratorUtils.HandleConfigs(dn, configs, (config, number) =>
        {
            CombinePath(config.OutputDirectory, config, c => c.InitListScriptFolder);
            CombinePath(config.OutputDirectory, config, c => c.TableScriptFolder);
            CombinePath(config.OutputDirectory, config, c => c.TableTypeScriptFolder);

            services.AddSingleton<IModelWatcher>(p =>
                new SsdtGenerator(p.GetRequiredService<ILogger<SsdtGenerator>>(), config) { Number = number });
        });

        return services;
    }
}
