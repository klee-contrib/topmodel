using Microsoft.Extensions.DependencyInjection;
using TopModel.Core;

namespace TopModel.Generator.Core;

public static class GeneratorUtils
{
    /// <summary>
    /// Enregistre un générateur dans la DI.
    /// </summary>
    /// <typeparam name="TGenerator">Type du générateur.</typeparam>
    /// <typeparam name="TConfig">Type de sa config.</typeparam>
    /// <param name="services">ServiceCollection.</param>
    /// <param name="config">Config.</param>
    /// <param name="number">Numéro du générateur.</param>
    /// <returns>ServiceCollection.</returns>
    public static IServiceCollection AddGenerator<TGenerator, TConfig>(this IServiceCollection services, TConfig config, int number)
        where TGenerator : GeneratorBase<TConfig>
        where TConfig : GeneratorConfigBase
    {
        return services.AddSingleton<IModelWatcher>(p =>
        {
            var generator = ActivatorUtilities.CreateInstance<TGenerator>(p)!;
            generator.Config = config;
            generator.Number = number;
            return generator;
        });
    }
}
