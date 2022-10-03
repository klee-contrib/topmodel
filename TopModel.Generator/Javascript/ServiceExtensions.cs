using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TopModel.Core;
using static TopModel.Utils.ModelUtils;

namespace TopModel.Generator.Javascript;

public static class ServiceExtensions
{
    public static IServiceCollection AddJavascript(this IServiceCollection services, string dn, IEnumerable<JavascriptConfig>? configs)
    {
        if (configs != null)
        {
            for (var i = 0; i < configs.Count(); i++)
            {
                var config = configs.ElementAt(i);
                var number = i + 1;

                CombinePath(dn, config, c => c.ModelOutputDirectory);
                CombinePath(dn, config, c => c.ResourceOutputDirectory);
                CombinePath(dn, config, c => c.ApiClientOutputDirectory);

                if (config.ModelOutputDirectory != null)
                {
                    services.AddSingleton<IModelWatcher>(p =>
                        new TypescriptDefinitionGenerator(p.GetRequiredService<ILogger<TypescriptDefinitionGenerator>>(), config) { Number = number });

                    if (config.ApiClientOutputDirectory != null)
                    {
                        if (config.TargetFramework == TargetFramework.ANGULAR)
                        {
                            services.AddSingleton<IModelWatcher>(p =>
                               new AngularApiClientGenerator(p.GetRequiredService<ILogger<AngularApiClientGenerator>>(), config) { Number = number });
                        }
                        else
                        {
                            services.AddSingleton<IModelWatcher>(p =>
                               new JavascriptApiClientGenerator(p.GetRequiredService<ILogger<JavascriptApiClientGenerator>>(), config) { Number = number });
                        }
                    }
                }

                if (config.ResourceOutputDirectory != null)
                {
                    services.AddSingleton<IModelWatcher>(p =>
                        new JavascriptResourceGenerator(p.GetRequiredService<ILogger<JavascriptResourceGenerator>>(), config) { Number = number });
                }
            }
        }

        return services;
    }
}
