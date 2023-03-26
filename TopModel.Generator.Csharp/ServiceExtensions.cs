using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Generator.Core;

using static TopModel.Utils.ModelUtils;

namespace TopModel.Generator.Csharp;

public static class ServiceExtensions
{
    public static IServiceCollection AddCSharp(this IServiceCollection services, string dn, IEnumerable<CsharpConfig>? configs)
    {
        GeneratorUtils.HandleConfigs(dn, configs, (config, number) =>
        {
            TrimSlashes(config, c => c.ApiFilePath);
            TrimSlashes(config, c => c.ApiRootPath);
            TrimSlashes(config, c => c.DbContextPath);
            TrimSlashes(config, c => c.ReferenceAccessorsImplementationPath);
            TrimSlashes(config, c => c.ReferenceAccessorsInterfacePath);
            TrimSlashes(config, c => c.NonPersistantModelPath);
            TrimSlashes(config, c => c.PersistantModelPath);
            TrimSlashes(config, c => c.PersistantReferencesModelPath);

            config.ReferenceAccessorsImplementationPath ??= Path.Combine(config.DbContextPath ?? string.Empty, "Reference");
            config.ReferenceAccessorsInterfacePath ??= Path.Combine(config.DbContextPath ?? string.Empty, "Reference");

            services.AddSingleton<IModelWatcher>(p =>
                new CSharpClassGenerator(p.GetRequiredService<ILogger<CSharpClassGenerator>>(), config) { Number = number });

            services.AddSingleton<IModelWatcher>(p =>
                new MapperGenerator(p.GetRequiredService<ILogger<MapperGenerator>>(), config) { Number = number });

            if (config.DbContextPath != null)
            {
                services.AddSingleton<IModelWatcher>(p =>
                    new DbContextGenerator(p.GetRequiredService<ILogger<DbContextGenerator>>(), config) { Number = number });
            }

            if (config.Kinetix)
            {
                services.AddSingleton<IModelWatcher>(p =>
                    new ReferenceAccessorGenerator(p.GetRequiredService<ILogger<ReferenceAccessorGenerator>>(), config) { Number = number });
            }

            if (config.ApiGeneration != null)
            {
                if (config.ApiGeneration != ApiGeneration.Client)
                {
                    services.AddSingleton<IModelWatcher>(p =>
                        new CSharpApiServerGenerator(p.GetRequiredService<ILogger<CSharpApiServerGenerator>>(), config) { Number = number });
                }

                if (config.ApiGeneration != ApiGeneration.Server)
                {
                    services.AddSingleton<IModelWatcher>(p =>
                        new CSharpApiClientGenerator(p.GetRequiredService<ILogger<CSharpApiClientGenerator>>(), config) { Number = number });
                }
            }
        });

        return services;
    }
}
