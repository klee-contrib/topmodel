using Microsoft.Extensions.Localization;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Core.Resolvers;

internal class MapperResolver(
    IStringLocalizer localizer,
    IList<ModelFile> modelFiles,
    IDictionary<string, Class> referencedClasses,
    IEnumerable<Converter> converters
)
{
    /// <summary>
    /// Résout les mappers.
    /// </summary>
    /// <returns>Erreurs.</returns>
    public IEnumerable<ModelError> ResolveMappers()
    {
        foreach (var classe in modelFiles.SelectMany(mf => mf.Classes))
        {
            foreach (var mappings in classe.FromMappers.SelectMany(m => m.ClassParams).Concat(classe.ToMappers))
            {
                if (!referencedClasses.TryGetValue(mappings.ClassReference.ReferenceName, out var mappedClass))
                {
                    yield return new ModelError(
                        localizer,
                        ErrorType.TMD0002,
                        [mappings.ClassReference.ReferenceName],
                        classe,
                        mappings.ClassReference
                    );
                    continue;
                }

                mappings.Class = mappedClass;
            }
        }

        foreach (var classe in modelFiles.SelectMany(mf => mf.Classes))
        {
            foreach (var mappings in classe.FromMappers.SelectMany(m => m.ClassParams).Concat(classe.ToMappers))
            {
                mappings.Mappings.Clear();

                foreach (var mapping in mappings.MappingReferences)
                {
                    var currentProperty = classe.ExtendedProperties.FirstOrDefault(p =>
                        p.Name == mapping.Key.ReferenceName
                    );
                    if (currentProperty == null)
                    {
                        yield return new ModelError(
                            localizer,
                            ErrorType.TMD0004,
                            [mapping.Key.ReferenceName, classe.Name],
                            classe,
                            mapping.Key
                        );
                    }

                    if (mapping.Value.ReferenceName == "false")
                    {
                        continue;
                    }

                    var mappedProperty = mappings.Class.ExtendedProperties.FirstOrDefault(p =>
                        p.Name == mapping.Value.ReferenceName
                    );

                    if (mappedProperty == null)
                    {
                        yield return new ModelError(
                            localizer,
                            ErrorType.TMD0004,
                            [mapping.Value.ReferenceName, mappings.Class.Name],
                            classe,
                            mapping.Value
                        );
                    }

                    if (currentProperty != null && mappedProperty != null)
                    {
                        mappings.Mappings.Add(currentProperty, mappedProperty);

                        foreach (
                            var error in CheckValidMapping(
                                classe,
                                mappings.To ? currentProperty : mappedProperty,
                                mappings.To ? mappedProperty : currentProperty,
                                mapping.Value
                            )
                        )
                        {
                            yield return error;
                        }
                    }
                }
            }

            foreach (var mapping in classe.FromMappers.SelectMany(fm => fm.PropertyParams))
            {
                if (mapping.Property != null)
                {
                    if (mapping.TargetPropertyReference != null)
                    {
                        var currentProperty = classe.ExtendedProperties.FirstOrDefault(p =>
                            p.Name == mapping.TargetPropertyReference.ReferenceName
                        );
                        if (currentProperty == null)
                        {
                            yield return new ModelError(
                                localizer,
                                ErrorType.TMD0004,
                                [mapping.TargetPropertyReference.ReferenceName, classe.Name],
                                classe,
                                mapping.TargetPropertyReference
                            );
                        }

                        mapping.TargetProperty = currentProperty;
                    }
                    else
                    {
                        var mappedProperty = classe.ExtendedProperties.FirstOrDefault(p =>
                            p.Name == mapping.Property.Name
                        );
                        if (mappedProperty == null)
                        {
                            yield return new ModelError(
                                localizer,
                                ErrorType.TMD0004,
                                [mapping.Property.Name, classe.Name],
                                classe,
                                mapping.Property.GetLocation()
                            );
                        }

                        mapping.TargetProperty = mappedProperty;
                    }

                    if (mapping.TargetProperty != null)
                    {
                        foreach (
                            var error in CheckValidMapping(
                                classe,
                                mapping.Property,
                                mapping.TargetProperty,
                                mapping.Property.GetLocation()
                            )
                        )
                        {
                            yield return error;
                        }
                    }
                }
            }

            foreach (var mapper in classe.FromMappers)
            {
                foreach (
                    var param in mapper.Params.Where(
                        (e, i) => mapper.Params.Where((p, j) => p.GetName() == e.GetName() && j < i).Any()
                    )
                )
                {
                    yield return new ModelError(
                        localizer,
                        ErrorType.TMD0001,
                        [param.GetName()],
                        classe,
                        param.GetLocation()
                    );
                }

                var mappedProperties = mapper.Params.SelectMany(p =>
                    p.Match(
                        p =>
                            p.MappingReferences.Where(e => e.Value.ReferenceName != "false")
                                .Select(e => (e.Key.ReferenceName, Reference: e.Key)),
                        p =>
                        {
                            if (p.TargetPropertyReference != null)
                            {
                                return [(p.TargetPropertyReference.ReferenceName, p.TargetPropertyReference)];
                            }

                            return [(p.Property.Name, p.Property.GetLocation() ?? new Reference())];
                        }
                    )
                );

                var hasDoublon = false;
                foreach (
                    var mapping in mappedProperties.Where(
                        (e, i) => mappedProperties.Where((p, j) => p.ReferenceName == e.ReferenceName && j < i).Any()
                    )
                )
                {
                    hasDoublon = true;
                    yield return new ModelError(
                        ErrorType.TMD8002,
                        classe,
                        $"La propriété '{mapping.ReferenceName}' est déjà initialisée dans ce mapper.",
                        mapping.Reference
                    );
                }

                if (!hasDoublon)
                {
                    var explicitMappings = mapper
                        .ClassParams.SelectMany(p => p.Mappings)
                        .Concat(
                            mapper.PropertyParams.Select(p => new KeyValuePair<IProperty, IProperty>(
                                p.TargetProperty,
                                p.Property
                            ))
                        )
                        .ToList();

                    foreach (var param in mapper.ClassParams.Where(p => p.Class != null))
                    {
                        foreach (
                            var currentProperty in classe
                                .ExtendedProperties.OfType<AliasProperty>()
                                .Where(property =>
                                    !property.Readonly
                                    && !explicitMappings.Exists(m => m.Key == property)
                                    && !param.MappingReferences.Any(m =>
                                        m.Key.ReferenceName == property.Name && m.Value.ReferenceName == "false"
                                    )
                                )
                        )
                        {
                            var matchingProperties = param.Class.ExtendedProperties.Where(p =>
                                currentProperty.Property == p
                                || p is AliasProperty alp && currentProperty == alp.Property
                                || p is AliasProperty alp2 && currentProperty.Property == alp2.Property
                            );
                            if (matchingProperties.Count() == 1)
                            {
                                var mappedProperty = matchingProperties.Single();
                                if (CheckPossibleMapping(mappedProperty, currentProperty))
                                {
                                    param.Mappings.Add(currentProperty, mappedProperty);
                                }
                            }
                        }
                    }

                    var explicitAndAliasMappings = mapper
                        .ClassParams.SelectMany(p => p.Mappings)
                        .Concat(
                            mapper.PropertyParams.Select(p => new KeyValuePair<IProperty, IProperty>(
                                p.TargetProperty,
                                p.Property
                            ))
                        )
                        .ToList();

                    foreach (var param in mapper.ClassParams.Where(p => p.Class != null))
                    {
                        foreach (
                            var currentProperty in classe.ExtendedProperties.Where(property =>
                                !property.Readonly
                                && !explicitAndAliasMappings.Exists(m => m.Key == property)
                                && !param.MappingReferences.Any(m =>
                                    m.Key.ReferenceName == property.Name && m.Value.ReferenceName == "false"
                                )
                            )
                        )
                        {
                            foreach (var mappedProperty in param.Class.ExtendedProperties)
                            {
                                if (
                                    !param.Mappings.ContainsKey(currentProperty)
                                    && mappedProperty.Name == currentProperty.Name
                                    && CheckPossibleMapping(mappedProperty, currentProperty)
                                )
                                {
                                    param.Mappings.Add(currentProperty, mappedProperty);
                                }
                            }
                        }
                    }

                    var finalMappings = mapper
                        .ClassParams.SelectMany(p => p.Mappings)
                        .Concat(
                            mapper.PropertyParams.Select(p => new KeyValuePair<IProperty, IProperty>(
                                p.TargetProperty,
                                p.Property
                            ))
                        )
                        .ToList();

                    if (finalMappings.TrueForAll(mapping => mapping.Key != null))
                    {
                        foreach (
                            var mapping in finalMappings.Where(
                                (e, i) => finalMappings.Where((p, j) => p.Key == e.Key && j < i).Any()
                            )
                        )
                        {
                            yield return new ModelError(
                                ErrorType.TMD8003,
                                classe,
                                $"Plusieurs propriétés de la classe peuvent être mappées sur '{mapping.Key.Name}' : {string.Join(", ", mapper.ClassParams.SelectMany(p => p.Mappings.Where(m => m.Key == mapping.Key).Select(m => $"'{p.Name}.{m.Value}'")))}.",
                                mapper.GetLocation()
                            );
                        }

                        foreach (
                            var param in mapper.Params.Where(
                                (p, i) =>
                                    p.GetRequired() && mapper.Params.Where((q, j) => !q.GetRequired() && j < i).Any()
                            )
                        )
                        {
                            var previousRequired = mapper
                                .Params.Where((q, j) => !q.GetRequired() && j < mapper.Params.IndexOf(param))
                                .Select(p => p.GetName());
                            yield return new ModelError(
                                ErrorType.TMD8012,
                                classe,
                                $"Le paramètre '{param.GetName()}' du mapper ne peut pas être obligatoire si l'un des paramètres précédents ({string.Join(", ", previousRequired)}) ne l'est pas.",
                                param.GetLocation()
                            );
                        }
                    }
                }
            }

            foreach (
                var mapper in classe.ToMappers.Where(
                    (e, i) => classe.ToMappers.Where((p, j) => p.Name == e.Name && j < i).Any()
                )
            )
            {
                yield return new ModelError(localizer, ErrorType.TMD0001, [mapper.Name], classe, mapper.GetLocation());
            }

            foreach (var mapper in classe.ToMappers.Where(m => m.Class != null))
            {
                var explicitMappings = mapper.Mappings.ToDictionary(p => p.Key, p => p.Value);

                foreach (
                    var currentProperty in classe
                        .ExtendedProperties.OfType<AliasProperty>()
                        .Where(property =>
                            !explicitMappings.ContainsKey(property)
                            && !mapper.MappingReferences.Any(m =>
                                m.Key.ReferenceName == property.Name && m.Value.ReferenceName == "false"
                            )
                        )
                )
                {
                    var matchingProperties = mapper.Class.ExtendedProperties.Where(p =>
                        currentProperty.Property == p
                        || p is AliasProperty alp && currentProperty == alp.Property
                        || p is AliasProperty alp2 && currentProperty.Property == alp2.Property
                    );
                    if (matchingProperties.Count() == 1)
                    {
                        var mappedProperty = matchingProperties.Single();
                        if (!mappedProperty.Readonly && CheckPossibleMapping(currentProperty, mappedProperty))
                        {
                            mapper.Mappings.Add(currentProperty, mappedProperty);
                        }
                    }
                }

                var explicitAndAliasMappings = mapper.Mappings.ToDictionary(p => p.Key, p => p.Value);

                foreach (
                    var currentProperty in classe.ExtendedProperties.Where(property =>
                        !explicitAndAliasMappings.ContainsKey(property)
                        && !mapper.MappingReferences.Any(m =>
                            m.Key.ReferenceName == property.Name && m.Value.ReferenceName == "false"
                        )
                    )
                )
                {
                    foreach (var mappedProperty in mapper.Class.ExtendedProperties)
                    {
                        if (
                            !mappedProperty.Readonly
                            && mappedProperty.Name == currentProperty.Name
                            && CheckPossibleMapping(currentProperty, mappedProperty)
                        )
                        {
                            mapper.Mappings.Add(currentProperty, mappedProperty);
                        }
                    }
                }

                foreach (
                    var mapping in mapper.Mappings.Where(
                        (e, i) => mapper.Mappings.Where((p, j) => p.Value == e.Value && j < i).Any()
                    )
                )
                {
                    yield return new ModelError(
                        ErrorType.TMD8003,
                        classe,
                        $"Plusieurs propriétés de la classe peuvent être mappées sur '{mapper.Class}.{mapping.Value?.Name}' : {string.Join(", ", mapper.Mappings.Where(p => p.Value == mapping.Value).Select(p => $"'{p.Key.Name}'"))}.",
                        mapper.GetLocation()
                    );
                }
            }
        }

        // Vérification qu'aucun mapper n'est vide
        foreach (var classe in modelFiles.SelectMany(mf => mf.Classes))
        {
            foreach (var mapper in classe.FromMappers)
            {
                if (!mapper.ClassParams.SelectMany(p => p.Mappings).Any() && !mapper.PropertyParams.Any())
                {
                    yield return new ModelError(
                        ErrorType.TMD8009,
                        classe,
                        "Aucun mapping n'a été trouvé sur ce mapper.",
                        mapper.GetLocation()
                    );
                }
            }

            foreach (var mapper in classe.ToMappers)
            {
                if (mapper.Mappings.Count == 0)
                {
                    yield return new ModelError(
                        ErrorType.TMD8009,
                        classe,
                        "Aucun mapping n'a été trouvé sur ce mapper.",
                        mapper.GetLocation()
                    );
                }
            }
        }
    }

    private bool CheckDomains(Domain? sourceDomain, Domain? targetDomain)
    {
        return sourceDomain == targetDomain
            || converters.Any(c => c.From.Any(cf => cf == sourceDomain) && c.To.Any(ct => ct == targetDomain));
    }

    private bool CheckPossibleMapping(IProperty sourceProperty, IProperty targetProperty)
    {
        // Mapping primitif => primitif
        if (
            sourceProperty.MappingType.IsT0
            && targetProperty.MappingType.IsT0
            && CheckDomains(sourceProperty.Domain, targetProperty.Domain)
        )
        {
            return true;
        }

        // Mapping classe => classe
        if (
            sourceProperty.MappingType.TryPickT1(out var st1, out _)
            && targetProperty.MappingType.TryPickT1(out var tt1, out _)
            && (st1.Class == tt1.Class || st1.Class.GetMapperTo(tt1.Class) != null)
            && (CheckDomains(st1.Domain, tt1.Domain) || st1.Domain?.Generic != true && tt1.Domain?.Generic != true)
        )
        {
            return true;
        }

        // Mapping collection classe => collection classe
        if (
            sourceProperty.MappingType.TryPickT2(out var st2, out _)
            && targetProperty.MappingType.TryPickT2(out var tt2, out _)
            && (st2.Class == tt2.Class || st2.Class.GetMapperTo(tt2.Class) != null)
            && (CheckDomains(st2.Domain, tt2.Domain) || st2.Domain?.Generic != true && tt2.Domain?.Generic != true)
        )
        {
            return true;
        }

        // Mapping classe => propriété
        if (
            targetProperty.MappingType.TryPickT0(out var mp, out _)
            && (
                sourceProperty.MappingType.TryPickT1(out var cp1, out _)
                    && CheckDomains(cp1.Property?.Domain, mp.Domain)
                || sourceProperty.MappingType.TryPickT2(out var cp2, out _)
                    && mp.ItemDomain != null
                    && CheckDomains(cp2.Property?.Domain, mp.ItemDomain)
            )
        )
        {
            return true;
        }

        return false;
    }

    private IEnumerable<ModelError> CheckValidMapping(
        Class classe,
        IProperty sourceProperty,
        IProperty targetProperty,
        Reference? propRef
    )
    {
        if (targetProperty.Readonly)
        {
            yield return new ModelError(
                ErrorType.TMD8008,
                classe,
                $"La propriété '{targetProperty.Name}' ne peut pas être la cible d'un mapping car elle a été marquée comme 'readonly'.",
                propRef
            );
        }

        if (!CheckPossibleMapping(sourceProperty, targetProperty))
        {
            yield return new ModelError(
                ErrorType.TMD8001,
                classe,
                $"La propriété '{sourceProperty.Name}' ne peut pas être mappée à '{targetProperty.Name}'.",
                propRef
            );
        }
    }
}
