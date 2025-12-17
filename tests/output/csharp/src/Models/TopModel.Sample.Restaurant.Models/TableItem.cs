namespace TopModel.Sample.Restaurant.Models;

public class TableItem : ITableItem
{
    public int? Id => throw new NotImplementedException();

    public string? Numero => throw new NotImplementedException();

    public int? Capacite => throw new NotImplementedException();

    public bool? Disponible => throw new NotImplementedException();

    public int? RestaurantId => throw new NotImplementedException();

    public static ITableItem Create(
        int? id = null,
        string? numero = null,
        int? capacite = null,
        bool? disponible = null,
        int? restaurantId = null
    )
    {
        throw new NotImplementedException();
    }
}
