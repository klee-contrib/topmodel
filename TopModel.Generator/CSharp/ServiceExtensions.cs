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
            foreach (var config in configs)
            {
                CombinePath(dn, config, c => c.OutputDirectory);

                services.AddSingleton<IModelWatcher>(p =>
                    new CSharpClassGenerator(p.GetRequiredService<ILogger<CSharpClassGenerator>>(), config));

                services.AddSingleton<IModelWatcher>(p =>
                    new MapperGenerator(p.GetRequiredService<ILogger<MapperGenerator>>(), config));

                if (config.DbContextPath != null)
                {
                    services.AddSingleton<IModelWatcher>(p =>
                        new DbContextGenerator(p.GetRequiredService<ILogger<DbContextGenerator>>(), config, app));
                }

                if (config.Kinetix != KinetixVersion.None)
                {
                    services.AddSingleton<IModelWatcher>(p =>
                        new ReferenceAccessorGenerator(p.GetRequiredService<ILogger<ReferenceAccessorGenerator>>(), config));
                }

                if (config.ApiGeneration == ApiGeneration.Server)
                {
                    services.AddSingleton<IModelWatcher>(p =>
                        new CSharpApiServerGenerator(p.GetRequiredService<ILogger<CSharpApiServerGenerator>>(), config));
                }
                else if (config.ApiGeneration == ApiGeneration.Client)
                {
                    services.AddSingleton<IModelWatcher>(p =>
                        new CSharpApiClientGenerator(p.GetRequiredService<ILogger<CSharpApiClientGenerator>>(), config));
                }
            }
        }

        return services;
    }
}
