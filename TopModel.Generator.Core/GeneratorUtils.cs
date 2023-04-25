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

    public static List<AssociationProperty> GetReverseProperties(this Class classe, IEnumerable<Class> availableClasses, bool reverseAll = false)
    {
        if (classe.Reference)
        {
            return new List<AssociationProperty>();
        }

        return availableClasses
            .SelectMany(c => c.Properties)
            .OfType<AssociationProperty>()
            .Where(p => p is not ReverseAssociationProperty)
            .Where(p => p.Type != AssociationType.OneToOne)
            .Where(p => p.Association == classe
                && (reverseAll || p.Type != AssociationType.OneToOne && p.Class.Namespace.RootModule == classe.Namespace.RootModule))
            .ToList();
    }

    public static IList<IProperty> GetProperties(this Class classe, IEnumerable<Class> availableClasses, string tag, bool reverseAll = false)
    {
        if (classe.Reference)
        {
            return classe.Properties;
        }

        return classe.Properties.Concat(classe.GetReverseProperties(availableClasses, reverseAll).Select(p => new ReverseAssociationProperty()
        {
            Association = p.Class,
            Type = p.Type == AssociationType.OneToMany ? AssociationType.ManyToOne
                : p.Type == AssociationType.ManyToOne ? AssociationType.OneToMany
                : p.Type == AssociationType.OneToOne ? AssociationType.OneToOne
                : AssociationType.ManyToMany,
            Comment = $"Association réciproque de {p.Class.NamePascal}.{p.Name}",
            Class = classe,
            ReverseProperty = p,
            Role = p.Role
        })).ToList();
    }

    public static bool IsToMany(this AssociationType associationType)
    {
        return associationType == AssociationType.ManyToMany || associationType == AssociationType.OneToMany;
    }
}
