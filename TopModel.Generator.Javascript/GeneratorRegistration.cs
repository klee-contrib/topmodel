using Microsoft.Extensions.DependencyInjection;
using TopModel.Generator.Core;

using static TopModel.Utils.ModelUtils;

namespace TopModel.Generator.Javascript;

public class GeneratorRegistration : IGeneratorRegistration<JavascriptConfig>
{
    /// <inheritdoc cref="IGeneratorRegistration{T}.Register" />
    public void Register(IServiceCollection services, JavascriptConfig config, int number)
    {
        if (config.ApiClientFilePath != null)
        {
            TrimSlashes(config, c => c.ApiClientFilePath);
            if (!config.ApiClientFilePath.Contains("{fileName}"))
            {
                config.ApiClientFilePath = Path.Combine(config.ApiClientFilePath, "{fileName}").Replace("\\", "/");
            }
        }
        else if (config.ApiMode == TargetFramework.ANGULAR || config.ApiMode == TargetFramework.ANGULAR_PROMISE)
        {
            config.ApiClientFilePath = "{module}/{fileName}.service";
        }
        else
        {
            config.ApiClientFilePath = "{module}/{fileName}";
        }

        TrimSlashes(config, c => c.ApiClientRootPath);
        TrimSlashes(config, c => c.DomainPath);
        TrimSlashes(config, c => c.FetchPath);
        TrimSlashes(config, c => c.ModelRootPath);
        TrimSlashes(config, c => c.ResourceRootPath);

        config.Language ??= "ts";

        if (config.ModelRootPath != null)
        {
            services.AddGenerator<TypescriptDefinitionGenerator, JavascriptConfig>(config, number);
            services.AddGenerator<TypescriptReferenceGenerator, JavascriptConfig>(config, number);

            if (config.ApiClientRootPath != null)
            {
                if (config.ApiMode == TargetFramework.ANGULAR || config.ApiMode == TargetFramework.ANGULAR_PROMISE)
                {
                    services.AddGenerator<AngularApiClientGenerator, JavascriptConfig>(config, number);
                }
                else if (config.ApiMode == TargetFramework.NUXT)
                {
                    services.AddGenerator<NuxtApiClientGenerator, JavascriptConfig>(config, number);
                }
                else
                {
                    services.AddGenerator<JavascriptApiClientGenerator, JavascriptConfig>(config, number);
                }
            }
        }

        if (config.ResourceRootPath != null && (config.TranslateProperties == true || config.TranslateReferences == true))
        {
            services.AddGenerator<JavascriptResourceGenerator, JavascriptConfig>(config, number);
        }
    }
}
