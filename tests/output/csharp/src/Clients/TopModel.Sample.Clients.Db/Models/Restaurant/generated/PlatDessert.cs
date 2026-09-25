////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using TopModel.Sample.Restaurant.Models;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Dessert.
/// </summary>
public partial record PlatDessert : Plat
{
    /// <inheritdoc />
    public override CategoriePlat CategoriePlat { get; init; } = CategoriePlat.Dessert;
}
