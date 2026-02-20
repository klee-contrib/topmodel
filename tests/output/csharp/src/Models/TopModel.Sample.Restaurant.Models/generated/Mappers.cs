////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Mappers pour le module 'Restaurant'.
/// </summary>
public static class Mappers
{
    /// <summary>
    /// Crée une nouvelle instance de 'IPlatItemReadonly'.
    /// </summary>
    /// <param name="platItem">Instance de 'PlatItem'.</param>
    /// <returns>Une nouvelle instance de 'IPlatItemReadonly'.</returns>
    public static IPlatItemReadonly CreatePlatItemReadonly<T>(IPlatItem platItem)
        where T : IPlatItemReadonly, new()
    {
        ArgumentNullException.ThrowIfNull(platItem);

        return new T
        {
            Prix = platItem.Prix,
            Disponible = platItem.Disponible
        };
    }

    /// <summary>
    /// Mappe 'IPlatItemReadonly' vers 'IPlatItem'.
    /// </summary>
    /// <param name="source">Instance de 'IPlatItemReadonly'.</param>
    /// <returns>Une nouvelle instance de 'IPlatItem'.</returns>
    public static IPlatItem ToPlatItem<T>(this IPlatItemReadonly source)
        where T : IPlatItem, new()
    {
        return new T
        {
            Id = source.Id,
            Nom = source.Nom,
            CategoriePlatCode = source.CategoriePlatCode,
            Prix = source.Prix,
            Disponible = source.Disponible
        };
    }
}
