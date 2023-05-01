using Microsoft.Extensions.DependencyInjection;
using TopModel.Generator.Core;

using static TopModel.Utils.ModelUtils;

namespace TopModel.Generator.Php;

public class GeneratorRegistration : IGeneratorRegistration<PhpConfig>
{
    public void Register(IServiceCollection services, PhpConfig config, int number)
    {
        TrimSlashes(config, c => c.EntitiesPath);
        TrimSlashes(config, c => c.RepositoriesPath);
        TrimSlashes(config, c => c.DtosPath);

        config.Language ??= "php";
        services.AddGenerator<PhpModelGenerator, PhpConfig>(config, number);
        services.AddGenerator<PhpRepositoryGenerator, PhpConfig>(config, number);
    }
}
