using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TopModel.Core;
using static TopModel.Utils.ModelUtils;

namespace TopModel.Generator.Jpa;

public static class ServiceExtensions
{
    public static IServiceCollection AddJpa(this IServiceCollection services, string dn, IEnumerable<JpaConfig>? configs)
    {
        if (configs != null)
        {
            foreach (var config in configs)
            {
                CombinePath(dn, config, c => c.ModelOutputDirectory);
                CombinePath(dn, config, c => c.ApiOutputDirectory);

                if (config.EntitiesPackageName != null || config.DtosPackageName != null)
                {
                    services
                        .AddSingleton<IModelWatcher>(p =>
                            new JpaModelGenerator(p.GetRequiredService<ILogger<JpaModelGenerator>>(), config));
                    services
                        .AddSingleton<IModelWatcher>(p =>
                            new JpaModelInterfaceGenerator(p.GetRequiredService<ILogger<JpaModelInterfaceGenerator>>(), config));
                }

                if (config.DaosPackageName != null)
                {
                    services
                        .AddSingleton<IModelWatcher>(p =>
                            new JpaDaoGenerator(p.GetRequiredService<ILogger<JpaDaoGenerator>>(), config));
                }

                if (config.ResourcesOutputDirectory != null)
                {
                    services
                        .AddSingleton<IModelWatcher>(p =>
                            new JpaResourceGenerator(p.GetRequiredService<ILogger<JpaResourceGenerator>>(), config));
                }

                if (config.ApiOutputDirectory != null)
                {
                    if (config.ApiGeneration == ApiGeneration.Server)
                    {
                        services
                            .AddSingleton<IModelWatcher>(p =>
                                new SpringServerApiGenerator(p.GetRequiredService<ILogger<SpringServerApiGenerator>>(), config));
                    }
                    else if (config.ApiGeneration == ApiGeneration.Client)
                    {
                        services
                            .AddSingleton<IModelWatcher>(p =>
                                new SpringClientApiGenerator(p.GetRequiredService<ILogger<SpringClientApiGenerator>>(), config));
                    }
                }
            }
        }

        return services;
    }
}
