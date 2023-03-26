using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Generator.Core;

namespace TopModel.Generator.Translation;

public class GeneratorRegistration : IGeneratorRegistration<TranslationConfig>
{
    public void Register(IServiceCollection services, string dn, IEnumerable<TranslationConfig>? configs)
    {
        GeneratorUtils.HandleConfigs(dn, configs, (config, _) =>
        {
            services
                .AddSingleton<IModelWatcher>(p =>
                    new TranslationOutGenerator(p.GetRequiredService<ILogger<TranslationOutGenerator>>(), config, p.GetRequiredService<ModelConfig>(), p.GetRequiredService<TranslationStore>()));
        });
    }
}
