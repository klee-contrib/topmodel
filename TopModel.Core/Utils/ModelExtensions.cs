using OneOf;
using TopModel.Core.FileModel;
using TopModel.Core.Model;

namespace TopModel.Core.Utils;

public static class ModelExtensions
{
    public static IEnumerable<(AnnotationReference Reference, ModelFile File)> GetAnnotationReferences(
        this ModelStore modelStore,
        Annotation annotation
    )
    {
        return modelStore
            .AnnotationContainers.Where(c => c.Annotations.Select(d => d.Annotation).Contains(annotation))
            .Select(c =>
                (
                    Reference: c.AnnotationReferences.FirstOrDefault(dr => dr.ReferenceName == annotation.Name)!,
                    File: c.GetFile()
                )
            )
            .Concat(
                modelStore
                    .PropertyContainers.Where(c => c.PropertyAnnotations.Select(d => d.Annotation).Contains(annotation))
                    .Select(c =>
                        (
                            Reference: c.PropertyAnnotationReferences.FirstOrDefault(dr =>
                                dr.ReferenceName == annotation.Name
                            )!,
                            File: c.GetFile()
                        )
                    )
            )
            .Where(r => r.Reference is not null)
            .DistinctBy(l => l.File.Name + l.Reference.Start.Line);
    }

    public static IEnumerable<(ClassReference Reference, ModelFile File)> GetClassReferences(
        this ModelStore modelStore,
        Class classe
    )
    {
        return modelStore
            .Properties.Where(p =>
                p is AliasProperty alp && alp.OriginalProperty?.Class == classe
                || p is AssociationProperty ap && ap.Association == classe
                || p is CompositionProperty cp && cp.Composition == classe
            )
            .Select(p =>
            {
                return (
                    Reference: p switch
                    {
                        AssociationProperty ap => ap.Reference,
                        CompositionProperty cp => cp.Reference,
                        AliasProperty alp => alp.Reference!.ClassReference!,
                        _ => null!, // Impossible
                    },
                    File: p.GetFile()
                );
            })
            .Concat(
                modelStore
                    .Classes.Where(c => c.Extends == classe)
                    .Select(c => (Reference: c.ExtendsReference!, File: c.GetFile()))
            )
            .Concat(
                modelStore
                    .DataFlows.Where(d => d.Class == classe)
                    .Select(d => (Reference: d.ClassReference, File: d.GetFile()))
            )
            .Concat(
                modelStore.DataFlows.SelectMany(d =>
                    d.Sources.Where(s => s.Class == classe)
                        .Select(s => (Reference: s.ClassReference, File: d.GetFile()))
                )
            )
            .Concat(
                modelStore.Classes.SelectMany(c =>
                    c.FromMappers.SelectMany(c => c.ClassParams)
                        .Concat(c.ToMappers)
                        .Where(m => m.Class == classe)
                        .Select(m => (Reference: m.ClassReference, File: c.GetFile()))
                )
            )
            .Where(r => r.Reference is not null)
            .DistinctBy(l => l.File.Name + l.Reference.Start.Line);
    }

    public static IEnumerable<(DataFlowReference Reference, ModelFile File)> GetDataFlowReferences(
        this ModelStore modelStore,
        DataFlow dataFlow
    )
    {
        return modelStore.DataFlows.SelectMany(d =>
            d.DependsOn.Where(dd => dd == dataFlow)
                .Select(dd => (d.DependsOnReference.First(dr => dr.ReferenceName == dd.Name), d.GetFile()))
        );
    }

    public static IEnumerable<(DecoratorReference Reference, ModelFile File)> GetDecoratorReferences(
        this ModelStore modelStore,
        Decorator decorator
    )
    {
        return modelStore
            .PropertyContainers.Where(c => c.Decorators.Select(d => d.Decorator).Contains(decorator))
            .Select(c =>
                (Reference: c.DecoratorReferences.First(dr => dr.ReferenceName == decorator.Name), File: c.GetFile())
            )
            .Concat(
                modelStore
                    .Properties.OfType<AliasProperty>()
                    .Where(alp => alp.OriginalProperty?.Decorator == decorator)
                    .Select(alp => (Reference: alp.Reference?.DecoratorReference!, File: alp.GetFile()))
            )
            .Where(r => r.Reference is not null)
            .DistinctBy(l => l.File.Name + l.Reference.Start.Line);
    }

    public static IEnumerable<(DomainReference Reference, ModelFile File)> GetDomainReferences(
        this ModelStore modelStore,
        Domain domain
    )
    {
        return modelStore
            .Properties.Where(p =>
                p is RegularProperty rp && rp.Domain == domain
                || p is AliasProperty alp && alp.DomainReference != null && alp.Domain == domain
                || p is CompositionProperty cp && cp.Domain == domain
            )
            .Select(p =>
            {
                return (
                    Reference: p switch
                    {
                        RegularProperty rp => rp.DomainReference,
                        AliasProperty alp => alp.DomainReference!,
                        CompositionProperty cp => cp.DomainReference!,
                        _ => null!, // Impossible
                    },
                    File: p.GetFile()
                );
            })
            .Concat(
                modelStore
                    .Converters.SelectMany(c =>
                        c.DomainsFromReferences.Union(c.DomainsToReferences)
                            .Select(d => (Reference: d, File: c.ModelFile))
                    )
                    .Where(r => r.Reference.ReferenceName == domain.Name)
            )
            .Concat(
                modelStore
                    .Domains.Values.SelectMany(d =>
                        d.AsDomainReferences.Values.Select(adr => (Reference: adr, File: d.GetFile()))
                    )
                    .Where(r => r.Reference.ReferenceName == domain.Name)
            )
            .Where(l => l.Reference is not null)
            .DistinctBy(l => l.File.Name + l.Reference.Start.Line);
    }

    public static IEnumerable<(EndpointReference Reference, ModelFile File)> GetEndpointReferences(
        this ModelStore modelStore,
        Endpoint endpoint
    )
    {
        return modelStore
            .Properties.OfType<AliasProperty>()
            .Where(alp => alp.OriginalProperty?.Endpoint == endpoint)
            .Select(alp => (Reference: alp.Reference?.EndpointReference!, File: alp.GetFile()))
            .Where(r => r.Reference is not null)
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
            DecoratorInstance { Decorator: Decorator decorator } => decorator.ModelFile,
            Annotation annotation => annotation.ModelFile,
            AnnotationInstance { Annotation: Annotation annotation } => annotation.ModelFile,
            DataFlow dataFlow => dataFlow.ModelFile,
            Keyword keyword => keyword.ModelFile,
            ClassValue classValue => classValue.Class.ModelFile,
            TemplateParameter templateParameter => templateParameter.Domain?.ModelFile
                ?? templateParameter.Decorator?.ModelFile
                ?? templateParameter.Annotation!.ModelFile,
            Variable { TemplateParameter: TemplateParameter templateParameter } => templateParameter.Domain?.ModelFile
                ?? templateParameter.Decorator?.ModelFile
                ?? templateParameter.Annotation!.ModelFile,
            Variable => new ModelFile { Name = string.Empty },
            ReverseAssociationDefinition { Property.Decorator: Decorator decorator } => decorator.ModelFile,
            ReverseAssociationDefinition { Property.Class: Class classe } => classe.ModelFile,
            ReverseAssociationDefinition { Property.Endpoint: Endpoint endpoint } => endpoint.ModelFile,
            ReverseAssociationDefinition { Property.PropertyMapping: PropertyMapping param } => param
                .FromMapper
                .Class
                .ModelFile,
            _ => throw new InvalidOperationException("Type d'objet non supporté."),
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
            DecoratorInstance { Decorator: Decorator d } => d.Location,
            Annotation a => a.Location,
            AnnotationInstance { Annotation: Annotation a } => a.Location,
            DataFlow d => d.Location,
            FromMapper m => m.Reference.Location,
            ClassMappings c => c.Name.Location,
            PropertyMapping p => p.Property.GetLocation(),
            OneOf<ClassMappings, PropertyMapping> p => p.Match(c => c.GetLocation(), p => p.GetLocation()),
            Converter c => c.Location,
            TemplateParameter t => t.Name.Location,
            Variable { TemplateParameter: TemplateParameter t } => t.Name.Location,
            ReverseAssociationDefinition rad => rad.Property.Location,
            _ => null,
        };
    }

    public static IEnumerable<(ParameterReference Reference, ModelFile File)> GetParameterReferences(
        this ModelStore modelStore,
        TemplateParameter parameter
    )
    {
        return modelStore
            .VariableContainers.SelectMany(vc =>
                vc.Variables.Values.Where(v => v.TemplateParameter == parameter)
                    .Select(v =>
                        (
                            Reference: vc.VariableReferences.FirstOrDefault(vr =>
                                vr.ReferenceName == v.TemplateParameter!.Name
                            )!,
                            File: v.GetFile()!
                        )
                    )
            )
            .Concat(
                modelStore.AnnotationContainers.SelectMany(ac =>
                    ac.Annotations.SelectMany(a =>
                        a.Annotation.TemplateParameters.Where(tp => tp == parameter)
                            .Select(tp =>
                                (
                                    Reference: ac
                                        .AnnotationReferences.FirstOrDefault(ac =>
                                            ac.ReferenceName == a.Annotation.Name
                                        )
                                        ?.ParameterReferences.Keys.FirstOrDefault(pr => pr.ReferenceName == tp.Name)!,
                                    File: ac.GetFile()!
                                )
                            )
                    )
                )
            )
            .Concat(
                modelStore.PropertyContainers.SelectMany(pc =>
                    pc.Decorators.SelectMany(d =>
                        d.Decorator.TemplateParameters.Where(tp => tp == parameter)
                            .Select(tp =>
                                (
                                    Reference: pc
                                        .DecoratorReferences.FirstOrDefault(ac => ac.ReferenceName == d.Decorator.Name)
                                        ?.ParameterReferences.Keys.FirstOrDefault(pr => pr.ReferenceName == tp.Name)!,
                                    File: pc.GetFile()!
                                )
                            )
                    )
                )
            )
            .Concat(
                modelStore.PropertyContainers.SelectMany(pc =>
                    pc.PropertyAnnotations.SelectMany(d =>
                        d.Annotation.TemplateParameters.Where(tp => tp == parameter)
                            .Select(tp =>
                                (
                                    Reference: pc
                                        .PropertyAnnotationReferences.FirstOrDefault(ac =>
                                            ac.ReferenceName == d.Annotation.Name
                                        )
                                        ?.ParameterReferences.Keys.FirstOrDefault(pr => pr.ReferenceName == tp.Name)!,
                                    File: pc.GetFile()!
                                )
                            )
                    )
                )
            )
            .Concat(
                modelStore.Properties.SelectMany(ac =>
                    (ac.Domain?.TemplateParameters ?? [])
                        .Where(tp => tp == parameter)
                        .Select(tp =>
                            (
                                Reference: ac.DomainReference?.ParameterReferences.Keys.FirstOrDefault(pr =>
                                    pr.ReferenceName == tp.Name
                                )!,
                                File: ac.GetFile()!
                            )
                        )
                )
            )
            .Where(r => r.Reference is not null)
            .DistinctBy(l => l.File.Name + l.Reference.Start.Line);
    }

    public static IEnumerable<(Reference Reference, ModelFile File)> GetPropertyReferences(
        this ModelStore modelStore,
        IProperty property,
        bool includeTransitive = false
    )
    {
        return modelStore.GetPropertyReferencesCore(property, includeTransitive, includeTransitive).Distinct();
    }

    public static IEnumerable<Reference> GetUselessImports(this ModelStore modelStore, ModelFile modelFile)
    {
        return modelFile.Uses.Where(use =>
            use.ReferenceName == modelFile.Name
            || (!modelFile.References.Values.Select(r => r.GetFile().Name).Contains(use.ReferenceName))
                && modelStore.Files.Any(d => d.Name == use.ReferenceName)
                && !modelStore.Files.Any(mf =>
                    mf.Name == use.ReferenceName
                    && mf.Properties.OfType<AssociationProperty>()
                        .Any(ap => ap.Association?.ModelFile == modelFile && ap.ReverseProperty != null)
                )
        );
    }

    private static IEnumerable<(Reference Reference, ModelFile File)> GetPropertyReferencesCore(
        this ModelStore modelStore,
        IProperty property,
        bool collectBackward = false,
        bool collectForward = false
    )
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
                var reference = alp.PropertyReference ?? alp.Reference?.ContainerReference;
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
                var excludeReference = alp.OriginalAliasProperty?.Reference?.ExcludeReferences.FirstOrDefault(er =>
                    er.ReferenceName == property.Name
                );
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
                    var reference = mappings.MappingReferences.Keys.FirstOrDefault(f =>
                        f.ReferenceName == property.Name
                    );
                    if (reference != null)
                    {
                        yield return (reference, classe.GetFile());
                    }
                }

                if (mappings.Mappings.Any(m => m.Value == property))
                {
                    var reference = mappings.MappingReferences.Values.FirstOrDefault(f =>
                        f.ReferenceName == property.Name
                    );
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
