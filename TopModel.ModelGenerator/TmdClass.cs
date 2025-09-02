namespace TopModel.ModelGenerator;

public class TmdClass
{
#nullable disable
    public string Name { get; set; }

    public string SqlName { get; set; }

#nullable enable
    public string Comment { get; set; } = "Non documenté";

    public IList<TmdProperty> Properties { get; set; } = [];

    public IList<TmdClass> Dependencies =>
        Properties
            .OfType<TmdAssociationProperty>()
            .Select(p => p.Association!)
            .Concat(Properties.OfType<TmdAliasProperty>().Select(a => a.Class))
            .Where(c => c != this)
            .Distinct()
            .ToList();

    public string Trigram { get; set; } = string.Empty;

    public IList<IList<string>> Unique { get; set; } = [];

    public bool PreservePropertyCasing { get; set; }

    public string? Extends { get; set; }

    public IList<IDictionary<string, string?>> Values { get; set; } = [];

    public TmdFile? File { get; set; }
}
