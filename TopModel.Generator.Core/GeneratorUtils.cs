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

    public static IList<IProperty> GetProperties(this Class classe, IEnumerable<Class> availableClasses)
    {
        if (classe.Reference)
        {
            return classe.Properties;
        }

        return classe.Properties.Concat(classe.GetReverseProperties(availableClasses)).ToList();
    }

    private static IEnumerable<ReverseAssociationProperty> GetReverseProperties(
        this Class classe,
        IEnumerable<Class> availableClasses
    )
    {
        if (classe.Reference)
        {
            return [];
        }

#pragma warning disable CS0618
        return availableClasses
            .SelectMany(c => c.Properties)
            .OfType<AssociationProperty>()
            .Where(p =>
                p.Type != AssociationType.OneToOne
                && p.Class.IsPersistent
                && (p.Association.PrimaryKey.Count() == 1 || p.Type == AssociationType.ManyToOne)
                && p.Association == classe
                && (p.Type == AssociationType.OneToMany || p.Class.Namespace.RootModule == classe.Namespace.RootModule)
            )
            .Select(p => new ReverseAssociationProperty { Class = classe, ReverseProperty = p });
    }
}
