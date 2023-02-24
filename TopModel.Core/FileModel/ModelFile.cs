#nullable disable

namespace TopModel.Core.FileModel;

public class ModelFile
{
    public Namespace Namespace { get; set; }

    public List<string> Tags { get; set; } = new();

    public List<Reference> Uses { get; set; } = new();

    public string Name { get; set; }

    public string Path { get; set; }

    public ModelFileOptions Options { get; set; } = new();

    public List<Class> Classes { get; } = new();

    public List<Domain> Domains { get; } = new();

    public List<Converter> Converters { get; } = new();

    public List<Decorator> Decorators { get; } = new();

    public List<Endpoint> Endpoints { get; } = new();

    public List<object> ResolvedAliases { get; } = new();

    public IDictionary<Reference, object> References => Domains.Select(d => (d.ListDomainReference as Reference, d.ListDomain as object))
        .Concat(Classes.Select(c => (c.ExtendsReference as Reference, c.Extends as object)))
        .Concat(Classes.SelectMany(c => c.DecoratorReferences.Select(dr => (dr as Reference, c.Decorators.FirstOrDefault(d => d.Decorator.Name == dr.ReferenceName) as object))))
        .Concat(Endpoints.SelectMany(c => c.DecoratorReferences.Select(dr => (dr as Reference, c.Decorators.FirstOrDefault(d => d.Decorator.Name == dr.ReferenceName) as object))))
        .Concat(Properties.OfType<RegularProperty>().Select(p => (p.DomainReference as Reference, p.Domain as object)))
        .Concat(Properties.OfType<AssociationProperty>().SelectMany(p => new (Reference, object)[]
        {
            (p.Reference, p.Association),
            (p.PropertyReference, p.Property)
        }))
        .Concat(Properties.OfType<CompositionProperty>().SelectMany(p => new (Reference, object)[]
        {
            (p.Reference, p.Composition),
            (p.DomainKindReference, p.DomainKind)
        }))
        .Concat(Properties.OfType<AliasProperty>().SelectMany(p => new (Reference, object)[]
        {
            (p.Reference, p.OriginalProperty?.Class),
            (p.PropertyReference, p.OriginalProperty),
            (p.DomainReference, p.Domain)
        }))
        .Concat(Properties.OfType<AliasProperty>()
            .SelectMany(p => p?.Reference?.ExcludeReferences?
                .Select(er => (er, p?.OriginalProperty?.Class?.Properties?.FirstOrDefault(p => p?.Name == er?.ReferenceName) as object))
            ?? new List<(Reference, object)>()))
        .Concat(Classes.SelectMany(c => new[] { c.DefaultPropertyReference, c.OrderPropertyReference, c.FlagPropertyReference }.Select(r => (r, (object)c.Properties.FirstOrDefault(p => p.Name == r?.ReferenceName)))))
        .Concat(Classes.SelectMany(c => c.UniqueKeyReferences.SelectMany(uk => uk).Select(propRef => (propRef, (object)c.Properties.FirstOrDefault(p => p.Name == propRef.ReferenceName)))))
        .Concat(Classes.SelectMany(c => c.ValueReferences.SelectMany(rv => rv.Value).Select(prop => (prop.Key, (object)c.Properties.FirstOrDefault(p => p.Name == prop.Key.ReferenceName)))))
        .Concat(Classes.SelectMany(c => c.FromMappers.SelectMany(m => m.Params).Concat(c.ToMappers)).Select(p => (p.ClassReference as Reference, (object)p.Class)))
        .Concat(Classes.SelectMany(c => c.FromMappers.SelectMany(m => m.Params).Concat(c.ToMappers).SelectMany(m => m.MappingReferences.SelectMany(mr => new[] { (mr.Key, (object)c.Properties.FirstOrDefault(k => k.Name == mr.Key.ReferenceName)), (mr.Value, mr.Value.ReferenceName == "this" || mr.Value.ReferenceName == "false" ? new Keyword { ModelFile = c.ModelFile } : m.Mappings.Values.FirstOrDefault(k => k.Name == mr.Value.ReferenceName)) }))))
        .Concat(Aliases.SelectMany(a => a.Classes).Select(c => (c as Reference, ResolvedAliases.OfType<Class>().FirstOrDefault(ra => ra.Name == c.ReferenceName) as object)))
        .Concat(Converters.SelectMany(c => c.DomainsFromReferences.Select(d => (d as Reference, c.From.FirstOrDefault(dom => dom.Name == d.ReferenceName) as object))))
        .Concat(Converters.SelectMany(c => c.DomainsToReferences.Select(d => (d as Reference, c.To.FirstOrDefault(dom => dom.Name == d.ReferenceName) as object))))
        .Where(t => t.Item1 is not null && t.Item2 is not null && t.Item2 is not (null, null))
        .DistinctBy(t => t.Item1)
        .ToDictionary(t => t.Item1, t => t.Item2);

    public IList<Reference> UselessImports => Uses
        .Where(use => !Aliases.Select(alias => alias.File.ReferenceName)
            .Concat(References.Values.Select(r => r.GetFile().Name))
            .Contains(use.ReferenceName))
        .ToList();

    public IList<IProperty> Properties => Classes.Where(c => !ResolvedAliases.Contains(c)).SelectMany(c => c.Properties)
        .Concat(Endpoints.Where(e => !ResolvedAliases.Contains(e)).SelectMany(e => e.Params))
        .Concat(Endpoints.Where(e => !ResolvedAliases.Contains(e)).Select(e => e.Returns))
        .Concat(Decorators.SelectMany(e => e.Properties))
        .Where(p => p != null)
        .ToList();

    internal IList<Alias> Aliases { get; set; } = new List<Alias>();

    public override string ToString()
    {
        return Name;
    }
}