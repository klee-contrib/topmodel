////
//// ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !
////

namespace TopModel.Sample.Restaurant.Models;

public class PlatItem : IPlatItem
{
    public int? Id => throw new NotImplementedException();

    public string? Nom => throw new NotImplementedException();

    public decimal? Prix => throw new NotImplementedException();

    public bool? Disponible => throw new NotImplementedException();

    public CategoriePlat.Codes? CategoriePlatCode => throw new NotImplementedException();

    public static IPlatItem Create(
        int? id = null,
        string? nom = null,
        decimal? prix = null,
        bool? disponible = null,
        CategoriePlat.Codes? categoriePlatCode = null
    )
    {
        throw new NotImplementedException();
    }
}
