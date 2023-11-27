namespace TopModel.ModelGenerator;

public class TmdEndpoint
{
#nullable disable
    public string Name { get; set; }

    public string Method { get; set; }

    public string Route { get; set; }

    public string Comment { get; set; } = "Non documenté";

    public List<TmdProperty> Params { get; set; } = new();

    public List<TmdProperty> Properties => Returns != null ? Params.Concat(new List<TmdProperty>() { Returns }).ToList() : Params;

    public List<TmdClass> Dependencies => Properties.OfType<TmdAssociationProperty>().Select(p => p.Association!).Concat(
                                          Properties.OfType<TmdAliasProperty>().Select(a => a.Class)).Concat(
                                          Properties.OfType<TmdCompositionProperty>().Where(c => c.Composition != null).Select(c => c.Composition))
                                        .ToList();

    public bool PreservePropertyCasing { get; set; } = false;

#nullable enable
    public TmdProperty? Returns { get; set; }

    public TmdFile? File { get; set; }
}