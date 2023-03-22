using TopModel.Utils;

namespace TopModel.ModelGenerator;

public class TmdAssociationProperty : TmdProperty
{
#nullable disable

    public TmdClass ForeignClass { get; set; }

#nullable enable
    public TmdProperty? ForeignProperty { get; set; }

    public string Role => ForeignClass != Class ? Name.Replace(ForeignClass!.Trigram.ToPascalCase() + ForeignProperty!.Name, string.Empty) : string.Empty;
}