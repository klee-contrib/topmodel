using TopModel.Utils;

namespace TopModel.ModelGenerator;

public class TmdAssociationProperty : TmdRegularProperty
{
    public required TmdClass Association { get; init; }

    public TmdRegularProperty? ForeignProperty { get; set; }

    public string Role =>
        Association != Class
            ? Name.Replace(Association!.Trigram.ToPascalCase() + ForeignProperty!.Name, string.Empty)
            : Name.Replace(
                Association.Properties.OfType<TmdRegularProperty>().First(p => p.PrimaryKey).Name,
                string.Empty
            );
}
