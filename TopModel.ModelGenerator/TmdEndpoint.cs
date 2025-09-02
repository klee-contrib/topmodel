namespace TopModel.ModelGenerator;

public class TmdEndpoint
{
    public required string Name { get; set; }

    public required string Method { get; set; }

    public required string Route { get; set; }

    public string Comment { get; set; } = "Non documenté";

    public IList<TmdProperty> Params { get; set; } = [];

    public IList<TmdProperty> Properties => Returns != null ? Params.Concat([Returns]).ToList() : Params;

    public IList<TmdClass> Dependencies =>
        [
            .. Properties.OfType<TmdAssociationProperty>().Where(c => c.Association != null).Select(p => p.Association),
            .. Properties.OfType<TmdAliasProperty>().Where(c => c.Class != null).Select(a => a.Class),
            .. Properties.OfType<TmdCompositionProperty>().Where(c => c.Composition != null).Select(c => c.Composition),
        ];

    public bool PreservePropertyCasing { get; set; }

#nullable enable
    public TmdProperty? Returns { get; set; }

    public TmdFile? File { get; set; }
}
