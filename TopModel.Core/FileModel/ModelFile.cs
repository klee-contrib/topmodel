#nullable disable
namespace TopModel.Core.FileModel;

public class ModelFile
{
    public string Module { get; set; }

    public IList<string> Tags { get; set; } = new List<string>();

    public IList<Reference> Uses { get; set; } = new List<Reference>();

    public string Name { get; set; }

    public string Path { get; set; }

    public IList<Class> Classes { get; set; } = new List<Class>();

    public IList<Domain> Domains { get; set; } = new List<Domain>();

    public IList<Endpoint> Endpoints { get; set; } = new List<Endpoint>();

    public IDictionary<Reference, object> References => Classes.Select(c => (c.ExtendsReference as Reference, c.Extends as object))
        .Concat(Properties.OfType<RegularProperty>().Select(p => (p.DomainReference as Reference, p.Domain as object)))
        .Concat(Properties.OfType<AssociationProperty>().Select(p => (p.Reference as Reference, p.Association as object)))
        .Concat(Properties.OfType<CompositionProperty>().SelectMany(p => new (Reference, object)[] { (p.Reference, p.Composition), (p.DomainKindReference, p.DomainKind) }))
        .Concat(Properties.OfType<AliasProperty>().SelectMany(p => new (Reference, object)[] { (p.ClassReference, p.Property?.Class), (p.PropertyReference, p.Property), (p.ListDomainReference, p.ListDomain) }))
        .Where(t => t.Item1 != null && t.Item2 != null)
        .Distinct()
        .ToDictionary(t => t.Item1, t => t.Item2);

    internal IList<IProperty> Properties => Classes.Where(c => !ResolvedAliases.Contains(c)).SelectMany(c => c.Properties)
        .Concat(Endpoints.Where(e => !ResolvedAliases.Contains(e)).SelectMany(e => e.Params))
        .Concat(Endpoints.Where(e => !ResolvedAliases.Contains(e)).Select(e => e.Returns))
        .Where(p => p != null)
        .ToList();

    internal IList<Alias> Aliases { get; set; } = new List<Alias>();

    internal List<object> ResolvedAliases { get; set; } = new();

    public override string ToString()
    {
        return Name;
    }
}