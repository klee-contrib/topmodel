namespace TopModel.ModelGenerator;

public class TmdEndpoint
{
    public required string Name { get; set; }

    public required string Method { get; set; }

    public required string Route { get; set; }

    public string Comment { get; set; } = "Non documenté";

    public List<TmdProperty> Params { get; set; } = new();

    public List<TmdProperty> Properties => Returns != null ? Params.Concat([Returns]).ToList() : Params;

    public List<TmdClass> Dependencies => Properties.OfType<TmdAssociationProperty>().Select(p => p.Association!)
        .Concat(Properties.OfType<TmdAliasProperty>().Select(a => a.Class))
        .Concat(Properties.OfType<TmdCompositionProperty>().Where(c => c.Composition != null).Select(c => c.Composition))
        .ToList();

    public bool PreservePropertyCasing { get; set; }

#nullable enable
    public TmdProperty? Returns { get; set; }

    public TmdFile? File { get; set; }
}