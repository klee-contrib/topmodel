using TopModel.Core.FileModel;
using TopModel.Core.Model;
using YamlDotNet.Core;

namespace TopModel.Core.Utils;

public static class ReferencesExtensions
{
    extension(ModelStore modelStore)
    {
        public IEnumerable<(AnnotationReference Reference, ModelFile File)> GetAnnotationReferences(
            Annotation annotation
        )
        {
            return modelStore
                .AnnotationContainers.Where(c => c.Annotations.Select(d => d.Annotation).Contains(annotation))
                .Select(c =>
                    (
                        Reference: c.AnnotationReferences.FirstOrDefault(dr => dr.Value == annotation.Name)!,
                        File: c.ModelFile
                    )
                )
                .Concat(
                    modelStore
                        .AnnotationContainers.Where(c =>
                            c.ExcludedAnnotations.Select(d => d.Annotation).Contains(annotation)
                        )
                        .Select(c =>
                            (
                                Reference: c.ExcludedAnnotationReferences.FirstOrDefault(dr =>
                                    dr.Value == annotation.Name
                                )!,
                                File: c.ModelFile
                            )
                        )
                )
                .Concat(
                    modelStore
                        .PropertyContainers.Where(c =>
                            c.PropertyAnnotations.Select(d => d.Annotation).Contains(annotation)
                        )
                        .Select(c =>
                            (
                                Reference: c.PropertyAnnotationReferences.FirstOrDefault(dr =>
                                    dr.Value == annotation.Name
                                )!,
                                File: c.ModelFile
                            )
                        )
                )
                .Where(r => r.Reference is not null)
                .DistinctBy(l => l.File.Name + l.Reference.Start.Line);
        }

        public IEnumerable<(ClassReference Reference, ModelFile File)> GetClassReferences(
            Class classe,
            bool includeTransitive = false
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
                            AliasProperty alp => alp.Reference?.ClassReference!,
                            _ => null!, // Impossible
                        },
                        File: p.ModelFile
                    );
                })
                .Concat(
                    includeTransitive
                        ? modelStore
                            .GetNameOccurrences(classe)
                            .Select(o =>
                            {
                                var (start, end, name) = SubRange(o.Reference, o.Offset, o.Length);
                                return (
                                    Reference: new ClassReference
                                    {
                                        Start = start,
                                        End = end,
                                        Value = name,
                                    },
                                    o.File
                                );
                            })
                        : []
                )
                .Concat(
                    modelStore
                        .Properties.OfType<AliasProperty>()
                        .Where(alp => alp.CompositionReference != null && alp.Composition == classe)
                        .Select(p =>
                        {
                            return (Reference: p.CompositionReference!, File: p.ModelFile);
                        })
                )
                .Concat(
                    modelStore
                        .Classes.Where(c => c.Extends == classe)
                        .Select(c => (Reference: c.ExtendsReference!, File: c.ModelFile))
                )
                .Concat(
                    modelStore
                        .Classes.Where(c => c.Implements.Contains(classe))
                        .Select(c =>
                            (Reference: c.ImplementReferences.First(dr => dr.Value == classe.Name), File: c.ModelFile)
                        )
                )
                .Concat(
                    modelStore
                        .DataFlows.Where(d => d.Class == classe)
                        .Select(d => (Reference: d.ClassReference, File: d.ModelFile))
                )
                .Concat(
                    modelStore.DataFlows.SelectMany(d =>
                        d.Sources.Where(s => s.Class == classe)
                            .Select(s => (Reference: s.ClassReference, File: d.ModelFile))
                    )
                )
                .Concat(
                    modelStore.Classes.SelectMany(c =>
                        c.FromMappers.SelectMany(c => c.ClassParams)
                            .Concat(c.ToMappers)
                            .Where(m => m.Class == classe)
                            .Select(m => (Reference: m.ClassReference, File: c.ModelFile))
                    )
                )
                .Where(r => r.Reference is not null)
                .DistinctBy(l => l.File.Name + l.Reference.Start.Line);
        }

        public IEnumerable<(DataFlowReference Reference, ModelFile File)> GetDataFlowReferences(DataFlow dataFlow)
        {
            return modelStore.DataFlows.SelectMany(d =>
                d.DependsOn.Where(dd => dd == dataFlow)
                    .Select(dd => (d.DependsOnReference.First(dr => dr.Value == dd.Name), d.ModelFile))
            );
        }

        public IEnumerable<(DecoratorReference Reference, ModelFile File)> GetDecoratorReferences(Decorator decorator)
        {
            return modelStore
                .PropertyContainers.Where(c => c.Decorators.Select(d => d.Decorator).Contains(decorator))
                .Select(c =>
                    (Reference: c.DecoratorReferences.First(dr => dr.Value == decorator.Name), File: c.ModelFile)
                )
                .Concat(
                    modelStore
                        .Properties.OfType<AliasProperty>()
                        .Where(alp => alp.OriginalProperty?.Decorator == decorator)
                        .Select(alp => (Reference: alp.Reference?.DecoratorReference!, File: alp.ModelFile))
                )
                .Where(r => r.Reference is not null)
                .DistinctBy(l => l.File.Name + l.Reference.Start.Line);
        }

        public IEnumerable<(DomainReference Reference, ModelFile File)> GetDomainReferences(Domain domain)
        {
            return modelStore
                .Properties.Where(p =>
                    p is RegularProperty rp && rp.Domain?.Domain == domain
                    || p is AliasProperty alp && alp.DomainReference != null && alp.Domain?.Domain == domain
                    || p is CompositionProperty cp && cp.Domain?.Domain == domain
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
                        File: p.ModelFile
                    );
                })
                .Concat(
                    modelStore
                        .Converters.SelectMany(c =>
                            c.DomainsFromReferences.Union(c.DomainsToReferences)
                                .Select(d => (Reference: d, File: c.ModelFile))
                        )
                        .Where(r => r.Reference.Value == domain.Name)
                )
                .Concat(
                    modelStore
                        .Domains.Values.SelectMany(d =>
                            d.AsDomainReferences.Values.Select(adr => (Reference: adr, File: d.ModelFile))
                        )
                        .Where(r => r.Reference.Value == domain.Name)
                )
                .Where(l => l.Reference is not null)
                .DistinctBy(l => l.File.Name + l.Reference.Start.Line);
        }

        public IEnumerable<(EndpointReference Reference, ModelFile File)> GetEndpointReferences(Endpoint endpoint)
        {
            return modelStore
                .Properties.OfType<AliasProperty>()
                .Where(alp => alp.OriginalProperty?.Endpoint == endpoint)
                .Select(alp => (Reference: alp.Reference?.EndpointReference!, File: alp.ModelFile))
                .Where(r => r.Reference is not null)
                .DistinctBy(l => l.File.Name + l.Reference.Start.Line);
        }

        public IEnumerable<(ParameterReference Reference, ModelFile File)> GetParameterReferences(
            TemplateParameter parameter
        )
        {
            return modelStore
                .VariableContainers.SelectMany(vc =>
                    vc.Variables.Values.Where(v => v.TemplateParameter == parameter)
                        .Select(v =>
                            (
                                Reference: vc.VariableReferences.FirstOrDefault(vr =>
                                    vr.Value == v.TemplateParameter!.Name
                                )!,
                                File: v.ModelFile
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
                                            .AnnotationReferences.FirstOrDefault(ac => ac.Value == a.Annotation.Name)
                                            ?.ParameterReferences.Keys.FirstOrDefault(pr => pr.Value == tp.Name)!,
                                        File: ac.ModelFile
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
                                            .DecoratorReferences.FirstOrDefault(ac => ac.Value == d.Decorator.Name)
                                            ?.ParameterReferences.Keys.FirstOrDefault(pr => pr.Value == tp.Name)!,
                                        File: pc.ModelFile
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
                                                ac.Value == d.Annotation.Name
                                            )
                                            ?.ParameterReferences.Keys.FirstOrDefault(pr => pr.Value == tp.Name)!,
                                        File: pc.ModelFile
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
                                        pr.Value == tp.Name
                                    )!,
                                    File: ac.ModelFile
                                )
                            )
                    )
                )
                .Where(r => r.Reference is not null)
                .DistinctBy(l => l.File.Name + l.Reference.Start.Line);
        }

        public IEnumerable<(PropertyReference Reference, ModelFile File)> GetPropertyReferences(
            IProperty property,
            bool includeTransitive = false
        )
        {
            return modelStore
                .GetPropertyReferencesCore(property, includeTransitive)
                .Concat(
                    includeTransitive
                        ? modelStore
                            .GetNameOccurrences(property)
                            .Select(o =>
                            {
                                var (start, end, name) = SubRange(o.Reference, o.Offset, o.Length);
                                return (
                                    Reference: new PropertyReference
                                    {
                                        Start = start,
                                        End = end,
                                        Value = name,
                                    },
                                    o.File
                                );
                            })
                        : []
                )
                .Distinct();
        }

        private IEnumerable<(PropertyReference Reference, ModelFile File, int Offset, int Length)> GetNameOccurrences(
            object target
        )
        {
            foreach (var property in modelStore.Properties)
            {
                var portions = property.NameSegments.Targets.Where(t => t.Target == target).ToList();
                if (portions.Count == 0)
                {
                    continue;
                }

                var name = property.Name;
                foreach (
                    var (reference, file) in modelStore.GetPropertyReferencesCore(property, includeTransitive: true)
                )
                {
                    // Si le texte de la référence ne correspond pas au nom calculé, les décalages seraient faux.
                    if (reference.Value != name)
                    {
                        continue;
                    }

                    foreach (var (offset, length, _) in portions)
                    {
                        yield return (reference, file, offset, length);
                    }
                }
            }
        }

        private IEnumerable<(PropertyReference Reference, ModelFile File)> GetPropertyReferencesCore(
            IProperty property,
            bool includeTransitive
        )
        {
            if (property.Decorator != null)
            {
                foreach (
                    var p in modelStore.Properties.Where(p =>
                        p.SourceContainer == property.Decorator && p.Name == property.Name
                    )
                )
                {
                    foreach (var result in modelStore.GetPropertyReferencesCore(p, includeTransitive))
                    {
                        yield return result;
                    }
                }
            }

            if (property.Class != null)
            {
                foreach (var mapping in property.Class.FromMappers.SelectMany(fm => fm.PropertyParams))
                {
                    if (mapping.TargetPropertyReference != null && mapping.TargetProperty == property)
                    {
                        yield return (mapping.TargetPropertyReference, mapping.TargetProperty.ModelFile);
                    }
                }

                if (property.Class.Type == ClassType.Interface)
                {
                    foreach (
                        var p in modelStore.Properties.Where(p =>
                            p.SourceContainer == property.Class && p.Name == property.Name
                        )
                    )
                    {
                        foreach (var result in modelStore.GetPropertyReferencesCore(p, includeTransitive))
                        {
                            yield return result;
                        }
                    }
                }
            }

            foreach (var alp in modelStore.Files.SelectMany(c => c.Properties).OfType<AliasProperty>())
            {
                if (alp.OriginalProperty == property)
                {
                    var reference = alp.PropertyReference;
                    if (reference != null)
                    {
                        yield return (reference, alp.ModelFile);
                    }

                    if (includeTransitive)
                    {
                        foreach (var result in modelStore.GetPropertyReferencesCore(alp, includeTransitive))
                        {
                            yield return result;
                        }
                    }
                }
                else if (alp.OriginalProperty?.Class == property.Class)
                {
                    var excludeReference = alp.OriginalAliasProperty?.Reference?.ExcludeReferences.FirstOrDefault(er =>
                        er.Value == property.Name
                    );
                    if (excludeReference != null)
                    {
                        yield return (excludeReference, alp.ModelFile);
                    }
                }
            }

            if (property.Class != null)
            {
                foreach (var index in property.Class.Indexes)
                {
                    foreach (var prop in index.PropertyReferences)
                    {
                        if (prop.Value == property.Name)
                        {
                            yield return (prop, property.Class.ModelFile);
                        }
                    }
                }

                if (property.Class.DefaultPropertyReference?.Value == property.Name)
                {
                    yield return (property.Class.DefaultPropertyReference, property.Class.ModelFile);
                }

                if (property.Class.OrderPropertyReference?.Value == property.Name)
                {
                    yield return (property.Class.OrderPropertyReference, property.Class.ModelFile);
                }

                if (property.Class.FlagPropertyReference?.Value == property.Name)
                {
                    yield return (property.Class.FlagPropertyReference, property.Class.ModelFile);
                }

                if (property.Class.LocalePropertyReference?.Value == property.Name)
                {
                    yield return (property.Class.LocalePropertyReference, property.Class.ModelFile);
                }

                if (property.Class.DiscriminatorPropertyReference?.Value == property.Name)
                {
                    yield return (property.Class.DiscriminatorPropertyReference, property.Class.ModelFile);
                }
            }

            foreach (var classe in modelStore.Classes)
            {
                foreach (var mappings in classe.FromMappers.SelectMany(m => m.ClassParams).Concat(classe.ToMappers))
                {
                    if (mappings.Mappings.ContainsKey(property))
                    {
                        var reference = mappings.MappingReferences.Keys.FirstOrDefault(f => f.Value == property.Name);
                        if (reference != null)
                        {
                            yield return (reference, classe.ModelFile);
                        }
                    }

                    if (mappings.Mappings.Any(m => m.Value == property))
                    {
                        var reference = mappings.MappingReferences.Values.FirstOrDefault(f => f.Value == property.Name);
                        if (reference != null)
                        {
                            yield return (reference, classe.ModelFile);
                        }
                    }
                }
            }

            foreach (var dataFlow in modelStore.DataFlows)
            {
                if (dataFlow.ActiveProperty == property)
                {
                    yield return (dataFlow.ActivePropertyReference!, dataFlow.ModelFile);
                }

                foreach (var source in dataFlow.Sources)
                {
                    if (source.JoinProperties.Contains(property))
                    {
                        var reference = source.JoinPropertyReferences.FirstOrDefault(f => f.Value == property.Name);
                        if (reference != null)
                        {
                            yield return (reference, dataFlow.ModelFile);
                        }
                    }
                }
            }
        }
    }

    private static (Mark Start, Mark End, string Name) SubRange(Reference reference, int offset, int length)
    {
        return (
            new Mark(reference.Start.Index + offset, reference.Start.Line, reference.Start.Column + offset),
            new Mark(
                reference.Start.Index + offset + length,
                reference.Start.Line,
                reference.Start.Column + offset + length
            ),
            reference.Value.Substring(offset, length)
        );
    }
}
