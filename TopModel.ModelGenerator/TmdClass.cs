namespace TopModel.ModelGenerator;

public class TmdClass
{
#nullable disable
    public string Name { get; set; }

    public string SqlName { get; set; }

    public List<TmdProperty> Properties { get; set; } = new();

    public List<TmdClass> Dependencies => Properties.OfType<TmdAssociationProperty>().Select(p => p.ForeignClass!).ToList();

    public string Trigram { get; set; } = string.Empty;

    public List<List<string>> Unique { get; set; } = new();

#nullable enable
    public List<Dictionary<string, string?>> Values { get; set; } = new();

    public TmdFile? File { get; set; }
}