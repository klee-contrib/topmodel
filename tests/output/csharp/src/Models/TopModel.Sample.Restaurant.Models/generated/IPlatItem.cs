////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace TopModel.Sample.Restaurant.Models;

/// <summary>
/// Détail d'un plat en liste.
/// </summary>
public interface IPlatItem
{
    /// <summary>
    /// Identifiant du plat.
    /// </summary>
    int? Id { get; }

    /// <summary>
    /// Nom du plat.
    /// </summary>
    string Nom { get; }

    /// <summary>
    /// Prix du plat.
    /// </summary>
    decimal? Prix { get; }

    /// <summary>
    /// Indique si le plat est disponible.
    /// </summary>
    bool? Disponible { get; }

    /// <summary>
    /// Catégorie du plat.
    /// </summary>
    CategoriePlat.Codes? CategoriePlatCode { get; }

    /// <summary>
    /// Factory pour instancier la classe.
    /// </summary>
    /// <param name="id">Identifiant du plat.</param>
    /// <param name="nom">Nom du plat.</param>
    /// <param name="prix">Prix du plat.</param>
    /// <param name="disponible">Indique si le plat est disponible.</param>
    /// <param name="categoriePlatCode">Catégorie du plat.</param>
    /// <returns>Instance de la classe.</returns>
    static abstract IPlatItem Create(int? id = null, string nom = null, decimal? prix = null, bool? disponible = null, CategoriePlat.Codes? categoriePlatCode = null);
}
