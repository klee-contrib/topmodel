using Microsoft.Extensions.DependencyInjection;
using TopModel.Generator.Core;

using static TopModel.Utils.ModelUtils;

namespace TopModel.Generator.Jpa;

public class GeneratorRegistration : IGeneratorRegistration<JpaConfig>
{
    public void Register(IServiceCollection services, JpaConfig config, int number)
    {
        TrimSlashes(config, c => c.EntitiesPath);
        TrimSlashes(config, c => c.DaosPath);
        TrimSlashes(config, c => c.DtosPath);
        TrimSlashes(config, c => c.ApiPath);
        TrimSlashes(config, c => c.ResourcesPath);

        services.AddGenerator<JpaModelGenerator, JpaConfig>(config, number);
        services.AddGenerator<JpaModelInterfaceGenerator, JpaConfig>(config, number);
        services.AddGenerator<JpaMapperGenerator, JpaConfig>(config, number);

        if (config.DaosPath != null)
        {
            services.AddGenerator<JpaDaoGenerator, JpaConfig>(config, number);
        }

        if (config.ResourcesPath != null)
        {
            services.AddGenerator<JpaResourceGenerator, JpaConfig>(config, number);
        }

        if (config.ApiGeneration != null)
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
    }
}
