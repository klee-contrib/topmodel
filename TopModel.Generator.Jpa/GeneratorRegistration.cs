using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Generator.Core;

using static TopModel.Utils.ModelUtils;

namespace TopModel.Generator.Jpa;

public class GeneratorRegistration : IGeneratorRegistration<JpaConfig>
{
    public void Register(IServiceCollection services, string dn, IEnumerable<JpaConfig>? configs)
    {
        GeneratorUtils.HandleConfigs(dn, configs, (config, number) =>
        {
            TrimSlashes(config, c => c.ApiRootPath);
            TrimSlashes(config, c => c.ModelRootPath);
            TrimSlashes(config, c => c.ResourceRootPath);

            if (config.EntitiesPackageName != null || config.DtosPackageName != null)
            {
                services
                    .AddSingleton<IModelWatcher>(p =>
                        new JpaModelGenerator(p.GetRequiredService<ILogger<JpaModelGenerator>>(), config, p.GetRequiredService<ModelConfig>()) { Number = number });
                services
                    .AddSingleton<IModelWatcher>(p =>
                        new JpaModelInterfaceGenerator(p.GetRequiredService<ILogger<JpaModelInterfaceGenerator>>(), config) { Number = number });
            }

            if (config.DaosPackageName != null)
            {
                services
                    .AddSingleton<IModelWatcher>(p =>
                        new JpaDaoGenerator(p.GetRequiredService<ILogger<JpaDaoGenerator>>(), config) { Number = number });
            }

            if (config.ResourceRootPath != null)
            {
                services
                    .AddSingleton<IModelWatcher>(p =>
                        new JpaResourceGenerator(p.GetRequiredService<ILogger<JpaResourceGenerator>>(), config, p.GetRequiredService<TranslationStore>()) { Number = number });
            }

            if (config.ApiRootPath != null && config.ApiGeneration != null)
            {
                if (config.ApiGeneration != ApiGeneration.Client)
                {
                    services
                        .AddSingleton<IModelWatcher>(p =>
                            new SpringServerApiGenerator(p.GetRequiredService<ILogger<SpringServerApiGenerator>>(), config) { Number = number });
                }

                if (config.ApiGeneration != ApiGeneration.Server)
                {
                    services
                        .AddSingleton<IModelWatcher>(p =>
                            new SpringClientApiGenerator(p.GetRequiredService<ILogger<SpringClientApiGenerator>>(), config) { Number = number });
                }
            }

            services.AddSingleton<IModelWatcher>(p =>
                new JpaMapperGenerator(p.GetRequiredService<ILogger<JpaMapperGenerator>>(), config) { Number = number });
        });
    }
}
