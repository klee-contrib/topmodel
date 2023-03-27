using Microsoft.Extensions.DependencyInjection;
using TopModel.Generator.Core;

using static TopModel.Utils.ModelUtils;

namespace TopModel.Generator.Jpa;

public class GeneratorRegistration : IGeneratorRegistration<JpaConfig>
{
    public void Register(IServiceCollection services, JpaConfig config, int number)
    {
        TrimSlashes(config, c => c.ApiRootPath);
        TrimSlashes(config, c => c.ModelRootPath);
        TrimSlashes(config, c => c.ResourceRootPath);

        if (config.EntitiesPackageName != null || config.DtosPackageName != null)
        {
            services.AddGenerator<JpaModelGenerator, JpaConfig>(config, number);
            services.AddGenerator<JpaModelInterfaceGenerator, JpaConfig>(config, number);
        }

        if (config.DaosPackageName != null)
        {
            services.AddGenerator<JpaDaoGenerator, JpaConfig>(config, number);
        }

        if (config.ResourceRootPath != null)
        {
            services.AddGenerator<JpaResourceGenerator, JpaConfig>(config, number);
        }

        if (config.ApiRootPath != null && config.ApiGeneration != null)
        {
            if (config.ApiGeneration != ApiGeneration.Client)
            {
                services.AddGenerator<SpringServerApiGenerator, JpaConfig>(config, number);
            }

            if (config.ApiGeneration != ApiGeneration.Server)
            {
                services.AddGenerator<SpringClientApiGenerator, JpaConfig>(config, number);
            }
        }

        services.AddGenerator<JpaMapperGenerator, JpaConfig>(config, number);
    }
}
