using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TopModel.Core;
using static TopModel.Utils.ModelUtils;

namespace TopModel.Generator.ProceduralSql;

public static class ServiceExtensions
{
    public static IServiceCollection AddProceduralSql(this IServiceCollection services, string dn, IEnumerable<ProceduralSqlConfig>? configs)
    {
        GeneratorUtils.HandleConfigs(dn, configs, (config, number) =>
        {
            CombinePath(config.OutputDirectory, config, c => c.CrebasFile);
            CombinePath(config.OutputDirectory, config, c => c.IndexFKFile);
            CombinePath(config.OutputDirectory, config, c => c.InitListFile);
            CombinePath(config.OutputDirectory, config, c => c.TypeFile);
            CombinePath(config.OutputDirectory, config, c => c.UniqueKeysFile);

            services.AddSingleton<IModelWatcher>(p =>
                new ProceduralSqlGenerator(p.GetRequiredService<ILogger<ProceduralSqlGenerator>>(), config) { Number = number });
        });

        return services;
    }
}
