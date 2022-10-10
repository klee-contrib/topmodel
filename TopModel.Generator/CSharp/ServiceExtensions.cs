using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TopModel.Core;
using static TopModel.Utils.ModelUtils;

namespace TopModel.Generator.CSharp;

public static class ServiceExtensions
{
    public static IServiceCollection AddCSharp(this IServiceCollection services, string dn, string app, IEnumerable<CSharpConfig>? configs)
    {
        if (configs != null)
        {
            for (var i = 0; i < configs.Count(); i++)
            {
                var config = configs.ElementAt(i);
                var number = i + 1;

                CombinePath(dn, config, c => c.OutputDirectory);
                TrimSlashes(config, c => c.ApiFilePath);
                TrimSlashes(config, c => c.ApiRootPath);
                TrimSlashes(config, c => c.NonPersistantModelPath);
                TrimSlashes(config, c => c.PersistantModelPath);

                services.AddSingleton<IModelWatcher>(p =>
                    new CSharpClassGenerator(p.GetRequiredService<ILogger<CSharpClassGenerator>>(), config) { Number = number });

                services.AddSingleton<IModelWatcher>(p =>
                    new MapperGenerator(p.GetRequiredService<ILogger<MapperGenerator>>(), config) { Number = number });

                if (config.DbContextPath != null)
                {
                    services.AddSingleton<IModelWatcher>(p =>
                        new DbContextGenerator(p.GetRequiredService<ILogger<DbContextGenerator>>(), config, app) { Number = number });
                }

                if (config.Kinetix != KinetixVersion.None)
                {
                    services.AddSingleton<IModelWatcher>(p =>
                        new ReferenceAccessorGenerator(p.GetRequiredService<ILogger<ReferenceAccessorGenerator>>(), config) { Number = number });
                }

                if (config.ApiGeneration == ApiGeneration.Server)
                {
                    services.AddSingleton<IModelWatcher>(p =>
                        new CSharpApiServerGenerator(p.GetRequiredService<ILogger<CSharpApiServerGenerator>>(), config) { Number = number });
                }
                else if (config.ApiGeneration == ApiGeneration.Client)
                {
                    services.AddSingleton<IModelWatcher>(p =>
                        new CSharpApiClientGenerator(p.GetRequiredService<ILogger<CSharpApiClientGenerator>>(), config) { Number = number });
                }
            }
        }

        return services;
    }
}
