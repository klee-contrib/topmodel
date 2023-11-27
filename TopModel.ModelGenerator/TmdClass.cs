namespace TopModel.ModelGenerator;

public class TmdClass
{
#nullable disable
    public string Name { get; set; }

    public string Comment { get; set; } = "Non documenté";

    public string SqlName { get; set; }

    public List<TmdProperty> Properties { get; set; } = new();

    public List<TmdClass> Dependencies => Properties.OfType<TmdAssociationProperty>().Select(p => p.Association!).Concat(Properties.OfType<TmdAliasProperty>().Select(a => a.Class)).ToList();

    public string Trigram { get; set; } = string.Empty;

    public List<List<string>> Unique { get; set; } = new();

    public bool PreservePropertyCasing { get; set; } = false;

#nullable enable

    public string? Extends { get; set; }
    public List<Dictionary<string, string?>> Values { get; set; } = new();

    public TmdFile? File { get; set; }
}