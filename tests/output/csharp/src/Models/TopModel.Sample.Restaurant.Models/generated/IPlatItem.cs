////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'un plat en liste.
/// </summary>
public partial interface IPlatItem
{
    /// <summary>
    /// Identifiant du plat.
    /// </summary>
    int? Id { get; set; }

    /// <summary>
    /// Nom du plat.
    /// </summary>
    string? Nom { get; set; }

    /// <summary>
    /// Prix du plat.
    /// </summary>
    decimal? Prix { get; set; }

    /// <summary>
    /// Indique si le plat est disponible.
    /// </summary>
    bool? Disponible { get; set; }

    /// <summary>
    /// Catégorie du plat.
    /// </summary>
    CategoriePlat.Codes? CategoriePlatCode { get; set; }
}
