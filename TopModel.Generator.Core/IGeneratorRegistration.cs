using Microsoft.Extensions.DependencyInjection;

namespace TopModel.Generator.Core;

/// <summary>
/// Interface d'enregistrement d'un générateur.
/// </summary>
/// <typeparam name="T">Type de la config du générateur.</typeparam>
public interface IGeneratorRegistration<T>
    where T : GeneratorConfigBase
{
    /// <summary>
    /// Enregistre le générateur.
    /// </summary>
    /// <param name="services">ServiceCollection.</param>
    /// <param name="dn">Répertoire de travail.</param>
    /// <param name="configs">Configurations trouvées.</param>
    void Register(IServiceCollection services, string dn, IEnumerable<T>? configs);
}