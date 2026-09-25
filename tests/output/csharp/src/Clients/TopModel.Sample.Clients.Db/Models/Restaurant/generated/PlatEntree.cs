////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

using TopModel.Sample.Restaurant.Models;

namespace TopModel.Sample.Clients.Db.Models.Restaurant;

/// <summary>
/// Entrée.
/// </summary>
public partial record PlatEntree : Plat
{
    public override CategoriePlat CategoriePlat => CategoriePlat.Entree;
}
