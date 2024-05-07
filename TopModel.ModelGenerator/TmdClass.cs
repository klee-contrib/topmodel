namespace TopModel.ModelGenerator;

public class TmdClass
{
#nullable disable
    public string Name { get; set; }

    public string SqlName { get; set; }

#nullable enable
    public string Comment { get; set; } = "Non documenté";

    public List<TmdProperty> Properties { get; set; } = [];

    public List<TmdClass> Dependencies => Properties.OfType<TmdAssociationProperty>().Select(p => p.Association!).Concat(Properties.OfType<TmdAliasProperty>().Select(a => a.Class)).ToList();

    public string Trigram { get; set; } = string.Empty;

    public List<List<string>> Unique { get; set; } = [];

    public bool PreservePropertyCasing { get; set; }

    public string? Extends { get; set; }

    public List<Dictionary<string, string?>> Values { get; set; } = new();

    public TmdFile? File { get; set; }
}