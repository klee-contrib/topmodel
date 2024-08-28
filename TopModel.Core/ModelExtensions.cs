using OneOf;
using TopModel.Core.FileModel;

namespace TopModel.Core;

public static class ModelExtensions
{
    public static IEnumerable<(ClassReference Reference, ModelFile File)> GetClassReferences(this ModelStore modelStore, Class classe)
    {
        return modelStore.Classes.SelectMany(c => c.Properties)
            .Concat(modelStore.Classes.SelectMany(c => c.FromMapperProperties))
            .Concat(modelStore.Endpoints.SelectMany(e => e.Properties))
            .Concat(modelStore.Decorators.SelectMany(d => d.Properties))
            .Where(p =>
                p is AliasProperty alp && alp.OriginalProperty?.Class == classe
                || p is AssociationProperty ap && ap.Association == classe
                || p is CompositionProperty cp && cp.Composition == classe)
            .Select(p =>
            {
                return (Reference: p switch
                {
                    AssociationProperty ap => ap.Reference,
                    CompositionProperty cp => cp.Reference,
                    AliasProperty alp => alp.Reference!,
                    _ => null! // Impossible
                }, File: p.GetFile());
            })
            .Concat(modelStore.Classes.Where(c => c.Extends == classe)
                .Select(c => (Reference: c.ExtendsReference!, File: c.GetFile())))
            .Concat(modelStore.DataFlows.Where(d => d.Class == classe).Select(d => (Reference: d.ClassReference, File: d.GetFile())))
            .Concat(modelStore.DataFlows.SelectMany(d => d.Sources.Where(s => s.Class == classe).Select(s => (Reference: s.ClassReference, File: d.GetFile()))))
            .Concat(modelStore.Classes.SelectMany(c => c.FromMappers.SelectMany(c => c.ClassParams).Concat(c.ToMappers).Where(m => m.Class == classe).Select(m => (Reference: m.ClassReference, File: c.GetFile()))))
            .Where(r => r.Reference is not null)
            .DistinctBy(l => l.File.Name + l.Reference.Start.Line);
    }

    public static IEnumerable<(DataFlowReference Reference, ModelFile File)> GetDataFlowReferences(this ModelStore modelStore, DataFlow dataFlow)
    {
        return modelStore.DataFlows.SelectMany(d => d.DependsOn.Where(dd => dd == dataFlow)
            .Select(dd => (d.DependsOnReference.First(dr => dr.ReferenceName == dd.Name), d.GetFile())));
    }

    public static IEnumerable<(DecoratorReference Reference, ModelFile File)> GetDecoratorReferences(this ModelStore modelStore, Decorator decorator)
    {
        return modelStore.Classes.Where(c => c.Decorators.Select(d => d.Decorator).Contains(decorator))
            .Select(c => (
                Reference: c.DecoratorReferences.First(dr => dr.ReferenceName == decorator.Name),
                File: c.GetFile()))
            .Concat(modelStore.Endpoints.Where(e => e.Decorators.Select(d => d.Decorator).Contains(decorator))
            .Select(e => (
                Reference: e.DecoratorReferences.First(dr => dr.ReferenceName == decorator.Name),
                File: e.GetFile())))
            .DistinctBy(l => l.File.Name + l.Reference.Start.Line);
    }

    public static IEnumerable<(DomainReference Reference, ModelFile File)> GetDomainReferences(this ModelStore modelStore, Domain domain)
    {
        return modelStore.Classes.SelectMany(c => c.Properties)
            .Concat(modelStore.Classes.SelectMany(c => c.FromMapperProperties))
            .Concat(modelStore.Decorators.SelectMany(c => c.Properties))
            .Concat(modelStore.Endpoints.SelectMany(e => e.Properties))
            .Where(p =>
                p is RegularProperty rp && rp.Domain == domain
                || p is AliasProperty alp && alp.DomainReference != null && alp.Domain == domain
                || p is CompositionProperty cp && cp.Domain == domain)
            .Select(p =>
            {
                return (Reference: p switch
                {
                    RegularProperty rp => rp.DomainReference,
                    AliasProperty alp => alp.DomainReference!,
                    CompositionProperty cp => cp.DomainReference!,
                    _ => null! // Impossible
                }, File: p.GetFile());
            })
            .Concat(modelStore.Converters.SelectMany(c => c.DomainsFromReferences.Union(c.DomainsToReferences).Select(d => (Reference: d, File: c.ModelFile))).Where(r => r.Reference.ReferenceName == domain.Name))
            .Concat(modelStore.Domains.Values.SelectMany(d => d.AsDomainReferences.Values.Select(adr => (Reference: adr, File: d.GetFile()))).Where(r => r.Reference.ReferenceName == domain.Name))
            .Where(l => l.Reference is not null)
            .DistinctBy(l => l.File.Name + l.Reference.Start.Line);
    }

    public static ModelFile GetFile(this object? objet)
    {
        return objet switch
        {
            ModelFile file => file,
            Class classe => classe.ModelFile,
            Endpoint endpoint => endpoint.ModelFile,
            IProperty { Decorator: Decorator decorator } => decorator.ModelFile,
            IProperty { Class: Class classe } => classe.ModelFile,
            IProperty { Endpoint: Endpoint endpoint } => endpoint.ModelFile,
            IProperty { PropertyMapping: PropertyMapping param } => param.FromMapper.Class.ModelFile,
            Domain domain => domain.ModelFile,
            Converter converter => converter.ModelFile,
            Decorator decorator => decorator.ModelFile,
            DataFlow dataFlow => dataFlow.ModelFile,
            (Decorator decorator, _) => decorator.ModelFile,
            Keyword keyword => keyword.ModelFile,
            ClassValue classValue => classValue.Class.ModelFile,
            _ => throw new ArgumentException("Type d'objet non supporté.")
        };
    }

    public static Reference? GetLocation(this object? objet)
    {
        return objet switch
        {
            Class c => c.Location,
            Endpoint e => e.Location,
            RegularProperty p => p.Location,
            AssociationProperty p => p.Location,
            CompositionProperty p => p.Location,
            AliasProperty { PropertyReference: Reference pr } => pr,
            AliasProperty p => p.Location,
            Domain d => d.Location,
            LocatedString l => l.Location,
            Decorator d => d.Location,
            (Decorator d, _) => d.Location,
            DataFlow d => d.Location,
            FromMapper m => m.Reference.Location,
            ClassMappings c => c.Name.Location,
            PropertyMapping p => p.Property.GetLocation(),
            OneOf<ClassMappings, PropertyMapping> p => p.Match(c => c.GetLocation(), p => p.GetLocation()),
            Converter c => c.Location,
            _ => null
        };
    }

    public static IEnumerable<(Reference Reference, ModelFile File)> GetPropertyReferences(this ModelStore modelStore, IProperty property, bool includeTransitive = false)
    {
        return modelStore.GetPropertyReferencesCore(property, includeTransitive, includeTransitive).Distinct();
    }

    private static IEnumerable<(Reference Reference, ModelFile File)> GetPropertyReferencesCore(this ModelStore modelStore, IProperty property, bool collectBackward = false, bool collectForward = false)
    {
        if (collectBackward && property is AliasProperty ap)
        {
            yield return (ap.OriginalProperty!.GetLocation()!, ap.OriginalProperty.GetFile());

            foreach (var result in modelStore.GetPropertyReferencesCore(ap.OriginalProperty!, collectBackward: true))
            {
                yield return result;
            }
        }

        if (property.Class != null)
        {
            foreach (var mapping in property.Class.FromMappers.SelectMany(fm => fm.PropertyParams))
            {
                if (mapping.TargetPropertyReference != null && mapping.TargetProperty == property)
                {
                    yield return (mapping.TargetPropertyReference, mapping.TargetProperty.GetFile());
                }
            }
        }

        foreach (var alp in modelStore.Files.SelectMany(c => c.Properties).OfType<AliasProperty>())
        {
            if (alp.OriginalProperty == property)
            {
                var reference = alp.PropertyReference ?? alp.Reference;
                if (reference != null)
                {
                    yield return (reference, alp.GetFile());
                }

                if (collectForward)
                {
                    foreach (var result in modelStore.GetPropertyReferencesCore(alp, collectForward: true))
                    {
                        yield return result;
                    }
                }
            }
            else if (alp.OriginalProperty?.Class == property.Class)
            {
                var excludeReference = alp.OriginalAliasProperty?.Reference?.ExcludeReferences.FirstOrDefault(er => er.ReferenceName == property.Name);
                if (excludeReference != null)
                {
                    yield return (excludeReference, alp.GetFile());
                }
            }
        }

        if (property.Class != null)
        {
            foreach (var uk in property.Class.UniqueKeyReferences)
            {
                foreach (var prop in uk)
                {
                    if (prop.ReferenceName == property.Name)
                    {
                        yield return (prop, property.Class.GetFile());
                    }
                }
            }

            if (property.Class.DefaultPropertyReference?.ReferenceName == property.Name)
            {
                yield return (property.Class.DefaultPropertyReference, property.Class.GetFile());
            }

            if (property.Class.OrderPropertyReference?.ReferenceName == property.Name)
            {
                yield return (property.Class.OrderPropertyReference, property.Class.GetFile());
            }

            if (property.Class.FlagPropertyReference?.ReferenceName == property.Name)
            {
                yield return (property.Class.FlagPropertyReference, property.Class.GetFile());
            }
        }

        foreach (var classe in modelStore.Classes)
        {
            foreach (var mappings in classe.FromMappers.SelectMany(m => m.ClassParams).Concat(classe.ToMappers))
            {
                if (mappings.Mappings.ContainsKey(property))
                {
                    var reference = mappings.MappingReferences.Keys.FirstOrDefault(f => f.ReferenceName == property.Name);
                    if (reference != null)
                    {
                        yield return (reference, classe.GetFile());
                    }
                }

                if (mappings.Mappings.ContainsValue(property))
                {
                    var reference = mappings.MappingReferences.Values.FirstOrDefault(f => f.ReferenceName == property.Name);
                    if (reference != null)
                    {
                        yield return (reference, classe.GetFile());
                    }
                }
            }
        }

        foreach (var dataFlow in modelStore.DataFlows)
        {
            if (dataFlow.ActiveProperty == property)
            {
                yield return (dataFlow.ActivePropertyReference!, dataFlow.GetFile());
            }

            foreach (var source in dataFlow.Sources)
            {
                if (source.JoinProperties.Contains(property))
                {
                    var reference = source.JoinPropertyReferences.FirstOrDefault(f => f.ReferenceName == property.Name);
                    if (reference != null)
                    {
                        yield return (reference, dataFlow.GetFile());
                    }
                }
            }
        }
    }
}
