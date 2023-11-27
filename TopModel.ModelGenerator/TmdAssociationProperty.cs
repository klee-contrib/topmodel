using TopModel.Utils;

namespace TopModel.ModelGenerator;

public class TmdAssociationProperty : TmdRegularProperty
{
#nullable disable

    public TmdClass Association { get; set; }

#nullable enable
    public TmdRegularProperty? ForeignProperty { get; set; }

    public string Role => Association != Class ? Name.Replace(Association!.Trigram.ToPascalCase() + ForeignProperty!.Name, string.Empty) : string.Empty;
}