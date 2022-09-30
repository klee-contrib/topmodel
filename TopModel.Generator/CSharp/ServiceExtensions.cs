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
                    new CSharpGenerator(p.GetRequiredService<ILogger<CSharpGenerator>>(), config, app));

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
