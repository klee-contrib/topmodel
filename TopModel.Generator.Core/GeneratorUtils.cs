#pragma warning disable KTA1200

using Microsoft.Extensions.DependencyInjection;
using TopModel.Core;
using TopModel.Core.Model;

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
    public static IServiceCollection AddGenerator<TGenerator, TConfig>(
        this IServiceCollection services,
        TConfig config,
        int number
    )
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

    extension(UniqueValueGenerationMode mode)
    {
        public bool CanEnum => (mode & UniqueValueGenerationMode.EnumOnly) > 0;

        public bool CanConst => (mode & UniqueValueGenerationMode.ConstOnly) > 0;
    }

    extension(IndexDefinition index)
    {
        public string SqlName =>
            $"{(index.Unique ? $"UK_{index.Class.SqlName}" : $"IDX_{index.Class.Trigram ?? index.Class.SqlName}")}_{string.Join('_', index.Properties.Select(c => c.SqlName))}";
    }

    extension(IProperty prop)
    {
        public string? ForeignKeyName => prop.Association != null ? $"FK_{prop.Class.SqlName}_{prop.SqlName}" : null;

        public string? IndexForeignKeyName =>
            prop.Association != null ? $"IDX_{prop.Class.Trigram ?? prop.Class.SqlName}_{prop.SqlName}_FK" : null;
    }
}
