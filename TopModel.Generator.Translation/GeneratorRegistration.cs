using Microsoft.Extensions.DependencyInjection;
using TopModel.Generator.Core;

namespace TopModel.Generator.Translation;

public class GeneratorRegistration : IGeneratorRegistration<TranslationConfig>
{
    public void Register(IServiceCollection services, TranslationConfig config, int number)
    {
        services.AddGenerator<TranslationOutGenerator, TranslationConfig>(config, number);
    }
}
