using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TopModel.Core;
using static TopModel.Utils.ModelUtils;

namespace TopModel.Generator.Javascript;

public static class ServiceExtensions
{
    public static IServiceCollection AddJavascript(this IServiceCollection services, string dn, IEnumerable<JavascriptConfig>? configs)
    {
        GeneratorUtils.HandleConfigs(dn, configs, (config, number) =>
        {
            TrimSlashes(config, c => c.ApiClientFilePath);
            TrimSlashes(config, c => c.ApiClientRootPath);
            TrimSlashes(config, c => c.DomainPath);
            TrimSlashes(config, c => c.FetchPath);
            TrimSlashes(config, c => c.ModelRootPath);
            TrimSlashes(config, c => c.ResourceRootPath);

            if (config.ModelRootPath != null)
            {
                services.AddSingleton<IModelWatcher>(p =>
                    new TypescriptDefinitionGenerator(p.GetRequiredService<ILogger<TypescriptDefinitionGenerator>>(), config) { Number = number });
                services.AddSingleton<IModelWatcher>(p =>
                    new TypescriptReferenceGenerator(p.GetRequiredService<ILogger<TypescriptReferenceGenerator>>(), config, p.GetRequiredService<ModelConfig>()) { Number = number });

                if (config.ApiClientRootPath != null)
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

            if (config.ResourceRootPath != null)
            {
                services.AddSingleton<IModelWatcher>(p =>
                    new JavascriptResourceGenerator(p.GetRequiredService<ILogger<JavascriptResourceGenerator>>(), config, p.GetRequiredService<TranslationStore>(), p.GetRequiredService<ModelConfig>()) { Number = number });
            }
        });

        return services;
    }
}
