namespace TopModel.Sample.Restaurant.Models;

public class CommandeItem : ICommandeItem
{
    public int? Id => throw new NotImplementedException();

    public DateTime? DateCommande => throw new NotImplementedException();

    public decimal? MontantTotal => throw new NotImplementedException();

    public StatutCommande.Codes? StatutCommandeCode => throw new NotImplementedException();

    public int? ClientId => throw new NotImplementedException();

    public static ICommandeItem Create(
        int? id = null,
        DateTime? dateCommande = null,
        decimal? montantTotal = null,
        StatutCommande.Codes? statutCommandeCode = null,
        int? clientId = null
    )
    {
        throw new NotImplementedException();
    }
}
