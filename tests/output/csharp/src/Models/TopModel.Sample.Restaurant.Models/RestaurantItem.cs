namespace TopModel.Sample.Restaurant.Models;

public class RestaurantItem : IRestaurantItem
{
    public int? Id => throw new NotImplementedException();

    public string? Nom => throw new NotImplementedException();

    public string? Adresse => throw new NotImplementedException();

    public string? Telephone => throw new NotImplementedException();

    public static IRestaurantItem Create(
        int? id = null,
        string? nom = null,
        string? adresse = null,
        string? telephone = null
    )
    {
        throw new NotImplementedException();
    }
}
