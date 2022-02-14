#nullable disable
namespace TopModel.Core.FileModel;

public class ModelFile
{
    public string Module { get; set; }

    public IList<string> Tags { get; set; } = new List<string>();

    public IList<Reference> Uses { get; set; } = new List<Reference>();

    public string Name { get; set; }

    public string Path { get; set; }

    public List<Class> Classes { get; } = new();

    public List<Domain> Domains { get; } = new();

    public List<Endpoint> Endpoints { get; } = new();

    public List<object> ResolvedAliases { get; } = new();

    public IDictionary<Reference, object> References => Classes.Select(c => (c.ExtendsReference as Reference, c.Extends as object))
        .Concat(Properties.OfType<RegularProperty>().Select(p => (p.DomainReference as Reference, p.Domain as object)))
        .Concat(Properties.OfType<AssociationProperty>().Select(p => (p.Reference as Reference, p.Association as object)))
        .Concat(Properties.OfType<CompositionProperty>().SelectMany(p => new (Reference, object)[] { (p.Reference, p.Composition), (p.DomainKindReference, p.DomainKind) }))
        .Concat(Properties.OfType<AliasProperty>().SelectMany(p => new (Reference, object)[] { (p.ClassReference, p.OriginalProperty?.Class), (p.PropertyReference, p.OriginalProperty), (p.ListDomainReference, p.ListDomain) }))
        .Where(t => t.Item1 != null && t.Item2 != null)
        .DistinctBy(t => t.Item1)
        .ToDictionary(t => t.Item1, t => t.Item2);

    public IList<Reference> UselessImports => Uses
        .Where(use => !Aliases.Select(alias => alias.File)
        .Concat(References.Values.Select(r => r.GetFile().Name))
        .Contains(use.ReferenceName)).ToList();

    internal IList<IProperty> Properties => Classes.Where(c => !ResolvedAliases.Contains(c)).SelectMany(c => c.Properties)
        .Concat(Endpoints.Where(e => !ResolvedAliases.Contains(e)).SelectMany(e => e.Params))
        .Concat(Endpoints.Where(e => !ResolvedAliases.Contains(e)).Select(e => e.Returns))
        .Where(p => p != null)
        .ToList();

    internal IList<Alias> Aliases { get; set; } = new List<Alias>();

    public override string ToString()
    {
        return Name;
    }
}