////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'un plat en liste.
/// </summary>
public interface IPlatItemReadonly
{
    /// <summary>
    /// Identifiant du plat.
    /// </summary>
    int? Id { get; }

    /// <summary>
    /// Nom du plat.
    /// </summary>
    string? Nom { get; }

    /// <summary>
    /// Catégorie du plat.
    /// </summary>
    CategoriePlat.Codes? CategoriePlatCode { get; }

    /// <summary>
    /// Prix du plat.
    /// </summary>
    decimal? Prix { get; set; }

    /// <summary>
    /// Indique si le plat est disponible.
    /// </summary>
    bool? Disponible { get; set; }
}
