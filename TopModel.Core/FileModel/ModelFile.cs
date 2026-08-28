using TopModel.Core.Model;
using TopModel.Core.Utils;

namespace TopModel.Core.FileModel;

public class ModelFile
{
    private List<IProperty>? _properties;

    public Namespace Namespace { get; internal set; }

    public IList<string> Tags { get; init; } = [];

    public IEnumerable<string> AllTags =>
        Tags.Concat(Classes.SelectMany(c => c.OwnTags)).Concat(Endpoints.SelectMany(e => e.OwnTags)).Distinct();

    public IList<Reference> Uses { get; init; } = [];

    public required string Name { get; init; }

    public required string Path { get; init; }

    public ModelFileOptions Options { get; init; } = new();

    public IDictionary<int, string> Comments { get; init; } = new Dictionary<int, string>();

    public IList<Class> Classes { get; } = [];

    public IList<Annotation> Annotations { get; } = [];

    public IList<Domain> Domains { get; } = [];

    public IList<Converter> Converters { get; } = [];

    public IList<Decorator> Decorators { get; } = [];

    public IList<Endpoint> Endpoints { get; } = [];

    public IList<DataFlow> DataFlows { get; } = [];

    public IList<IProperty> Properties
    {
        get
        {
            _properties ??= Classes
                .SelectMany(c => c.Properties)
                .Concat(Classes.SelectMany(c => c.FromMapperProperties))
                .Concat(Endpoints.SelectMany(e => e.Params))
                .Concat(Endpoints.Select(e => e.Returns!))
                .Concat(Decorators.SelectMany(e => e.Properties))
                .Where(p => p != null && p is not ReverseAssociationProperty)
                .ToList();

            return _properties;
        }
    }

    public IList<IProperty> ReverseProperties =>
        Classes.SelectMany(c => c.Properties).Where(p => p is ReverseAssociationProperty).ToList();

    public IEnumerable<TemplateParameter> Parameters =>
        [
            .. Annotations.SelectMany(d => d.TemplateParameters),
            .. Decorators.SelectMany(d => d.TemplateParameters),
            .. Domains.SelectMany(d => d.TemplateParameters),
        ];

    public IEnumerable<IAnnotationContainer> AnnotationContainers =>
        [
            .. Domains,
            .. Decorators,
            .. Classes,
            .. Endpoints,
            .. Properties,
            .. Properties
                .OfType<AssociationProperty>()
                .Where(ap => ap.WithReverse != null)
                .Select(ap => ap.WithReverse!),
        ];

    public IEnumerable<IPropertyContainer> PropertyContainers => [.. Decorators, .. Classes, .. Endpoints];

    public IEnumerable<IVariableContainer> VariableContainers =>
        [.. Annotations, .. Converters, .. Decorators, .. Domains];

    public IDictionary<Reference, object> References =>
        Domains
            .SelectMany(d =>
                d.AsDomains.Keys.Select(adn =>
                    d.AsDomainReferences.TryGetValue(adn, out var adr) && d.AsDomains.TryGetValue(adn, out var ad)
                        ? (adr as Reference, ad as object)
                        : (null!, null!)
                )
            )
            .Concat(
                AnnotationContainers.SelectMany(d =>
                    d.AnnotationReferences.Select(ar =>
                        (
                            ar as Reference,
                            d.Annotations.Select(a => a.Annotation).FirstOrDefault(d => d.Name == ar.ReferenceName)!
                                as object
                        )
                    )
                )
            )
            .Concat(
                AnnotationContainers.SelectMany(d =>
                    d.AnnotationReferences.SelectMany(ar =>
                        ar.ParameterReferences.Keys.Select(
                            (pr, i) =>
                                (
                                    pr as Reference,
                                    d
                                        .Annotations.FirstOrDefault(d => d.Annotation.Name == ar.ReferenceName)
                                        ?.Annotation.TemplateParameters.ElementAtOrDefault(i)! as object
                                )
                        )
                    )
                )
            )
            .Concat(
                AnnotationContainers.SelectMany(d =>
                    d.ExcludedAnnotationReferences.Select(ar =>
                        (
                            ar as Reference,
                            d.ExcludedAnnotations.Select(a => a.Annotation)
                                .FirstOrDefault(d => d.Name == ar.ReferenceName)! as object
                        )
                    )
                )
            )
            .Concat(
                VariableContainers.SelectMany(d =>
                    d.VariableReferences.Select(pr =>
                        (
                            pr as Reference,
                            d.Variables.TryGetValue(pr.ReferenceName, out var variable) ? variable as object : null!
                        )
                    )
                )
            )
            .Concat(
                VariableContainers
                    .SelectMany(d => d.TransformReferences)
                    .Select(tr =>
                        (tr as Reference, new Variable { Description = "Transformation de variable" } as object)
                    )
            )
            .Concat(
                PropertyContainers.SelectMany(c =>
                    c.DecoratorReferences.Select(dr =>
                        (
                            dr as Reference,
                            c.Decorators.Select(d => d.Decorator).FirstOrDefault(d => d.Name == dr.ReferenceName)!
                                as object
                        )
                    )
                )
            )
            .Concat(
                PropertyContainers.SelectMany(c =>
                    c.DecoratorReferences.SelectMany(dr =>
                        dr.ParameterReferences.Keys.Select(
                            (pr, i) =>
                                (
                                    pr as Reference,
                                    c
                                        .Decorators.FirstOrDefault(d => d.Decorator.Name == dr.ReferenceName)
                                        ?.Decorator.TemplateParameters.ElementAtOrDefault(i)! as object
                                )
                        )
                    )
                )
            )
            .Concat(
                PropertyContainers.SelectMany(c =>
                    c.PropertyAnnotationReferences.Select(par =>
                        (
                            par as Reference,
                            c.PropertyAnnotations.Select(d => d.Annotation)
                                .FirstOrDefault(d => d.Name == par.ReferenceName)! as object
                        )
                    )
                )
            )
            .Concat(
                PropertyContainers.SelectMany(c =>
                    c.PropertyAnnotationReferences.SelectMany(par =>
                        par.ParameterReferences.Keys.Select(
                            (pr, i) =>
                                (
                                    pr as Reference,
                                    c
                                        .PropertyAnnotations.FirstOrDefault(d => d.Annotation.Name == par.ReferenceName)
                                        ?.Annotation.TemplateParameters.ElementAtOrDefault(i)! as object
                                )
                        )
                    )
                )
            )
            .Concat(Classes.Select(c => (c.ExtendsReference! as Reference, c.Extends! as object)))
            .Concat(
                Classes.SelectMany(c =>
                    c.ImplementReferences.Select(r =>
                        (r as Reference, c.Implements.FirstOrDefault(i => i.Name == r.ReferenceName)! as object)
                    )
                )
            )
            .Concat(
                Endpoints.SelectMany(e =>
                    e.Route.Variables.Select(pr =>
                        (pr as Reference, e.Params.FirstOrDefault(p => p.GetParamName() == pr.ReferenceName)! as object)
                    )
                )
            )
            .Concat(
                Properties.OfType<RegularProperty>().Select(p => (p.DomainReference as Reference, p.Domain as object))
            )
            .Concat(
                Properties
                    .OfType<RegularProperty>()
                    .SelectMany(p =>
                        p.DomainReference?.ParameterReferences.Keys.Select(
                            (pr, i) => (pr as Reference, p.Domain?.TemplateParameters.ElementAtOrDefault(i)! as object)
                        )
                        ?? []
                    )
            )
            .Concat(
                Properties
                    .OfType<AssociationProperty>()
                    .SelectMany(p =>
                        new (Reference, object)[] { (p.Reference, p.Association), (p.PropertyReference!, p.Property) }
                    )
            )
            .Concat(
                Properties
                    .OfType<CompositionProperty>()
                    .SelectMany(p =>
                        new (Reference, object)[] { (p.Reference, p.Composition), (p.DomainReference!, p.Domain) }
                    )
            )
            .Concat(
                Properties
                    .OfType<CompositionProperty>()
                    .SelectMany(p =>
                        p.DomainReference?.ParameterReferences.Keys.Select(
                            (pr, i) => (pr as Reference, p.Domain?.TemplateParameters.ElementAtOrDefault(i)! as object)
                        )
                        ?? []
                    )
            )
            .Concat(
                Properties
                    .OfType<AliasProperty>()
                    .SelectMany(p =>
                        new (Reference, object)[]
                        {
                            (p.Reference?.ClassReference!, p.OriginalProperty?.Class!),
                            (p.Reference?.EndpointReference!, p.OriginalProperty?.Endpoint!),
                            (p.Reference?.DecoratorReference!, p.OriginalProperty?.Decorator!),
                            (p.PropertyReference!, p.OriginalProperty!),
                            (p.DomainReference!, p.Domain),
                            (p.CompositionReference!, p.Composition!),
                        }
                    )
            )
            .Concat(
                Properties
                    .OfType<AliasProperty>()
                    .SelectMany(p =>
                        p?.Reference?.ExcludeReferences?.Select(er =>
                            (
                                er,
                                p?.OriginalProperty?.Class?.Properties?.FirstOrDefault(p =>
                                    p?.Name == er?.ReferenceName
                                )! as object
                            )
                        )
                        ?? new List<(Reference, object)>()
                    )
            )
            .Concat(
                Properties
                    .OfType<AliasProperty>()
                    .SelectMany(p =>
                        p.DomainReference?.ParameterReferences.Keys.Select(
                            (pr, i) => (pr as Reference, p.Domain?.TemplateParameters.ElementAtOrDefault(i)! as object)
                        )
                        ?? []
                    )
            )
            .Concat(
                Classes.SelectMany(c =>
                    new[]
                    {
                        c.DefaultPropertyReference!,
                        c.OrderPropertyReference!,
                        c.FlagPropertyReference!,
                        c.LocalePropertyReference!,
                        c.DiscriminatorPropertyReference!,
                    }.Select(r => (r, (object)c.ExtendedProperties.FirstOrDefault(p => p.Name == r?.ReferenceName)!))
                )
            )
            .Concat(
                Classes.SelectMany(c =>
                    c.Indexes.SelectMany(uk => uk.PropertyReferences)
                        .Select(propRef =>
                            (propRef, (object)c.Properties.FirstOrDefault(p => p.Name == propRef.ReferenceName)!)
                        )
                )
            )
            .Concat(
                Classes.SelectMany(c =>
                    c.ValueReferences.SelectMany(rv => rv.Value)
                        .Select(prop =>
                            (
                                prop.Key,
                                (object)c.ExtendedProperties.FirstOrDefault(p => p.Name == prop.Key.ReferenceName)!
                            )
                        )
                )
            )
            .Concat(
                Classes
                    .SelectMany(c => c.FromMappers.SelectMany(m => m.ClassParams).Concat(c.ToMappers))
                    .Select(p => (p.ClassReference as Reference, (object)p.Class))
            )
            .Concat(
                Classes
                    .SelectMany(c => c.FromMappers.SelectMany(m => m.PropertyParams))
                    .Select(p => (p.TargetPropertyReference, (object)p.TargetProperty))
            )
            .Concat(
                Classes.SelectMany(c =>
                    c.FromMappers.SelectMany(m => m.ClassParams)
                        .Concat(c.ToMappers)
                        .SelectMany(m =>
                            m.MappingReferences.SelectMany(mr =>
                                new[]
                                {
                                    (
                                        mr.Key,
                                        (object)
                                            c.ExtendedProperties.FirstOrDefault(k => k.Name == mr.Key.ReferenceName)!
                                    ),
                                    (
                                        mr.Value,
                                        mr.Value.ReferenceName == "false"
                                            ? new Keyword { ModelFile = c.ModelFile }
                                            : m.Mappings.Values.FirstOrDefault(k => k.Name == mr.Value.ReferenceName)!
                                    ),
                                }
                            )
                        )
                )
            )
            .Concat(
                Converters.SelectMany(c =>
                    c.DomainsFromReferences.Select(d =>
                        (d as Reference, c.From.FirstOrDefault(dom => dom.Name == d.ReferenceName)! as object)
                    )
                )
            )
            .Concat(
                Converters.SelectMany(c =>
                    c.DomainsToReferences.Select(d =>
                        (d as Reference, c.To.FirstOrDefault(dom => dom.Name == d.ReferenceName)! as object)
                    )
                )
            )
            .Concat(DataFlows.Select(d => (d.ClassReference as Reference, d.Class as object)))
            .Concat(DataFlows.Select(d => (d.ActivePropertyReference!, d.ActiveProperty! as object)))
            .Concat(
                DataFlows.SelectMany(d =>
                    d.DependsOnReference.Select(r =>
                        (r as Reference, d.DependsOn.FirstOrDefault(dd => dd?.Name == r.ReferenceName)! as object)
                    )
                )
            )
            .Concat(
                DataFlows.SelectMany(d => d.Sources).Select(s => (s.ClassReference as Reference, s.Class as object))
            )
            .Concat(
                DataFlows
                    .SelectMany(d => d.Sources)
                    .SelectMany(s =>
                        s.JoinPropertyReferences.Select(j =>
                            (j, s.JoinProperties.FirstOrDefault(jc => jc?.Name == j.ReferenceName)! as object)
                        )
                    )
            )
            .Where(t => t.Item1 is not null && t.Item2 is not null && t.Item2 is not (null, null))
            .DistinctBy(t => t.Item1)
            .ToDictionary(t => t.Item1, t => t.Item2);

    internal IEnumerable<IProperty> OwnProperties =>
        Classes
            .SelectMany(c => c.OwnProperties)
            .Concat(Classes.SelectMany(c => c.FromMapperOwnProperties))
            .Concat(Endpoints.SelectMany(e => e.OwnParams))
            .Concat(Endpoints.Select(e => e.OwnReturns!))
            .Concat(Decorators.SelectMany(e => e.OwnProperties))
            .Where(p => p != null);

    public void ResetPropertyList()
    {
        _properties = null;
    }

    public override string ToString()
    {
        return Name;
    }
}
