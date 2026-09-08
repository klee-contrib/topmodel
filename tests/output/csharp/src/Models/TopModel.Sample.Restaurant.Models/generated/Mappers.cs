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
    /// Mappe 'PaiementItem' vers 'FactureItem'.
    /// </summary>
    /// <param name="source">Instance de 'PaiementItem'.</param>
    /// <param name="commandeId">Commande associée à la facture.</param>
    /// <returns>Une nouvelle instance de 'FactureItem'.</returns>
    public static FactureItem ToFactureItem(this PaiementItem source, int? commandeId = null)
    {
        return new FactureItem
        {
            Id = source.FactureId,
            CommandeId = commandeId
        };
    }

    /// <summary>
    /// Mappe 'PaiementItem' vers 'FactureItem'.
    /// </summary>
    /// <param name="source">Instance de 'PaiementItem'.</param>
    /// <param name="dest">Instance pré-existante de 'FactureItem'.</param>
    /// <returns>L'instance pré-existante de 'FactureItem'.</returns>
    public static FactureItem ToFactureItem(this PaiementItem source, FactureItem dest)
    {
        return dest;
    }

    /// <summary>
    /// Mappe 'MenuWrite' vers 'MenuRead'.
    /// </summary>
    /// <param name="source">Instance de 'MenuWrite'.</param>
    /// <param name="id">Identifiant du menu.</param>
    /// <param name="dateCreation">Date de création de l'enregistrement.</param>
    /// <returns>Une nouvelle instance de 'MenuRead'.</returns>
    public static MenuRead ToMenuRead(this MenuWrite source, int? id = null, DateTime? dateCreation = null)
    {
        return new MenuRead
        {
            Nom = source.Nom,
            Description = source.Description,
            Prix = source.Prix,
            Disponible = source.Disponible,
            DateDebut = source.DateDebut,
            DateFin = source.DateFin,
            RestaurantId = source.RestaurantId,
            CategoriesPlat = source.CategoriesPlat?.Select(CategoriePlat.GetValue).ToList() ?? [],
            Id = id,
            DateCreation = dateCreation
        };
    }

    /// <summary>
    /// Mappe 'MenuWrite' vers 'MenuRead'.
    /// </summary>
    /// <param name="source">Instance de 'MenuWrite'.</param>
    /// <param name="dest">Instance pré-existante de 'MenuRead'.</param>
    /// <returns>L'instance pré-existante de 'MenuRead'.</returns>
    public static MenuRead ToMenuRead(this MenuWrite source, MenuRead dest)
    {
        dest.Nom = source.Nom;
        dest.Description = source.Description;
        dest.Prix = source.Prix;
        dest.Disponible = source.Disponible;
        dest.DateDebut = source.DateDebut;
        dest.DateFin = source.DateFin;
        dest.RestaurantId = source.RestaurantId;
        dest.CategoriesPlat = source.CategoriesPlat?.Select(CategoriePlat.GetValue).ToList() ?? [];
        return dest;
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

    /// <summary>
    /// Mappe 'IPlatItemReadonly' vers 'IPlatItem'.
    /// </summary>
    /// <param name="source">Instance de 'IPlatItemReadonly'.</param>
    /// <param name="dest">Instance pré-existante de 'IPlatItem'.</param>
    /// <returns>L'instance pré-existante de 'IPlatItem'.</returns>
    public static IPlatItem ToPlatItem(this IPlatItemReadonly source, IPlatItem dest)
    {
        dest.Id = source.Id;
        dest.Nom = source.Nom;
        dest.CategoriePlatCode = source.CategoriePlatCode;
        dest.Prix = source.Prix;
        dest.Disponible = source.Disponible;
        return dest;
    }
}
