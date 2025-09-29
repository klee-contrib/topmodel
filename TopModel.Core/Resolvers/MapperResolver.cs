using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Core.Resolvers;

internal class MapperResolver(
    IList<ModelFile> modelFiles,
    IDictionary<string, Class> referencedClasses,
    IEnumerable<Converter> converters,
    bool useLegacyAssociationCompositionMappers
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
                        ErrorType.TMD0002,
                        classe,
                        "La classe '{0}' est introuvable dans le fichier ou l'une de ses dépendances.",
                        mappings.ClassReference
                    );
                    continue;
                }

                mappings.Class = mappedClass;

                mappings.Mappings.Clear();

                foreach (var mapping in mappings.MappingReferences)
                {
                    var currentProperty = classe.ExtendedProperties.FirstOrDefault(p =>
                        p.Name == mapping.Key.ReferenceName
                    );
                    if (currentProperty == null)
                    {
                        yield return new ModelError(
                            ErrorType.TMD0004,
                            classe,
                            $"La propriété '{{0}}' est introuvable sur la classe '{classe}'.",
                            mapping.Key
                        );
                    }

                    if (mapping.Value.ReferenceName == "false")
                    {
                        continue;
                    }

                    var mappedProperty = mappedClass.ExtendedProperties.FirstOrDefault(p =>
                        p.Name == mapping.Value.ReferenceName
                    );
                    if (mappedProperty == null)
                    {
                        yield return new ModelError(
                            ErrorType.TMD0004,
                            classe,
                            $"La propriété '{{0}}' est introuvable sur la classe '{mappedClass}'.",
                            mapping.Value
                        );
                    }

                    if (currentProperty != null && mappedProperty != null)
                    {
                        var sourceCp = currentProperty switch
                        {
                            CompositionProperty cp => cp,
                            AliasProperty { Property: CompositionProperty cp } => cp,
                            _ => null,
                        };

                        var mappedAp = mappedProperty switch
                        {
                            AssociationProperty ap => ap,
                            AliasProperty { Property: AssociationProperty ap } => ap,
                            _ => null,
                        };

                        mappings.Mappings.Add(currentProperty, mappedProperty);

                        if (mappings.To && mappedProperty.Readonly)
                        {
                            yield return new ModelError(
                                ErrorType.TMD8008,
                                classe,
                                $"La propriété '{mappedProperty.Name}' ne peut pas être la cible d'un mapping car elle a été marquée comme 'readonly'.",
                                mapping.Value
                            );
                        }
                        else if (!mappings.To && currentProperty.Readonly)
                        {
                            yield return new ModelError(
                                ErrorType.TMD8008,
                                classe,
                                $"La propriété '{currentProperty.Name}' ne peut pas être la cible d'un mapping car elle a été marquée comme 'readonly'.",
                                mapping.Key
                            );
                        }

                        if (
                            (sourceCp == null || mappedAp == null)
                            && currentProperty.Domain != mappedProperty.Domain
                            && !converters.Any(c =>
                                c.From.Any(cf => cf == (mappings.To ? currentProperty.Domain : mappedProperty.Domain))
                                && c.To.Any(ct => ct == (mappings.To ? mappedProperty.Domain : currentProperty.Domain))
                            )
                        )
                        {
                            yield return new ModelError(
                                ErrorType.TMD8001,
                                classe,
                                $"La propriété '{mappedProperty.Name}' ne peut pas être mappée à '{currentProperty.Name}' car elle n'a pas le même domaine ('{mappedProperty.Domain?.Name}' au lieu de '{currentProperty.Domain?.Name}') et qu'il n'existe pas de convertisseur entre les deux.",
                                mapping.Value
                            );
                        }

                        if (sourceCp != null)
                        {
                            if (mappedAp == null)
                            {
                                yield return new ModelError(
                                    ErrorType.TMD8004,
                                    classe,
                                    $"La propriété '{mappedProperty.Name}' ne peut pas être mappée à la composition '{currentProperty.Name}' car ce n'est pas une association.",
                                    mapping.Value
                                );
                            }
                            else if (
                                !useLegacyAssociationCompositionMappers
                                && (mappedAp.Type.IsToMany() || sourceCp.Domain != null)
                            )
                            {
                                yield return new ModelError(
                                    ErrorType.TMD8005,
                                    classe,
                                    $"L'association '{mappedProperty.Name}' ne peut pas être mappée à la composition '{currentProperty.Name}' car l'association et la composition doivent toutes les deux être simples.",
                                    mapping.Value
                                );
                            }
                            else if (
                                !useLegacyAssociationCompositionMappers
                                && sourceCp.CompositionPrimaryKey?.Domain != mappedAp.Domain
                                && !converters.Any(c =>
                                    c.From.Any(cf => cf == sourceCp.CompositionPrimaryKey?.Domain)
                                    && c.To.Any(ct => ct == mappedAp.Domain)
                                )
                            )
                            {
                                yield return new ModelError(
                                    ErrorType.TMD8006,
                                    classe,
                                    $"La propriété '{mappedProperty.Name}' ne peut pas être mappée à la composition '{currentProperty.Name}' car elle n'a pas le même domaine que la composition '{sourceCp.Composition.Name}' ('{mappedProperty.Domain?.Name}' au lieu de '{sourceCp.CompositionPrimaryKey?.Domain?.Name ?? string.Empty}').",
                                    mapping.Value
                                );
                            }
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
                                ErrorType.TMD0004,
                                classe,
                                $"La propriété '{{0}}' est introuvable sur la classe '{classe}'.",
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
                                ErrorType.TMD0004,
                                classe,
                                $"La propriété '{mapping.Property.Name}' est introuvable sur la classe '{classe}'.",
                                mapping.Property.GetLocation()
                            );
                        }

                        mapping.TargetProperty = mappedProperty;
                    }

                    if (mapping.TargetProperty != null)
                    {
                        var sourceCp = mapping.Property switch
                        {
                            CompositionProperty cp => cp,
                            AliasProperty { Property: CompositionProperty cp } => cp,
                            _ => null,
                        };
                        var targetCp = mapping.TargetProperty switch
                        {
                            CompositionProperty cp => cp,
                            AliasProperty { Property: CompositionProperty cp } => cp,
                            _ => null,
                        };

                        if (targetCp == null && sourceCp != null)
                        {
                            yield return new ModelError(
                                ErrorType.TMD8011,
                                classe,
                                $"La propriété '{mapping.Property.Name}' ne peut pas être une composition pour définir un mapping vers '{mapping.TargetProperty.Name}'.",
                                mapping.Property.GetLocation()
                            );
                        }

                        if (targetCp != null && (sourceCp == null || targetCp.Composition != sourceCp.Composition))
                        {
                            yield return new ModelError(
                                ErrorType.TMD8010,
                                classe,
                                $"La propriété '{mapping.Property.Name}' doit être une composition de la même classe que '{mapping.TargetProperty.Name}' pour définir un mapping entre les deux.",
                                mapping.Property.GetLocation()
                            );
                        }

                        if (
                            mapping.Property.Domain != mapping.TargetProperty.Domain
                            && !converters.Any(c =>
                                c.From.Any(cf => cf == mapping.Property.Domain)
                                && c.To.Any(ct => ct == mapping.TargetProperty.Domain)
                            )
                        )
                        {
                            yield return new ModelError(
                                ErrorType.TMD8001,
                                classe,
                                $"La propriété '{mapping.Property.Name}' ne peut pas être mappée à '{mapping.TargetProperty.Name}' car elle n'a pas le même domaine ('{mapping.Property.Domain?.Name}' au lieu de '{mapping.TargetProperty.Domain?.Name}') et qu'il n'existe pas de convertisseur entre les deux.",
                                mapping.Property.GetLocation()
                            );
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
                        ErrorType.TMD0001,
                        classe,
                        $"Le nom '{param.GetName()}' est déjà utilisé.",
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
                            var property in classe
                                .ExtendedProperties.OfType<AliasProperty>()
                                .Where(property =>
                                    !property.Readonly
                                    && !explicitMappings.Any(m => m.Key == property)
                                    && !param.MappingReferences.Any(m =>
                                        m.Key.ReferenceName == property.Name && m.Value.ReferenceName == "false"
                                    )
                                )
                        )
                        {
                            var matchingProperties = param.Class.ExtendedProperties.Where(p =>
                                property.Property == p
                                || p is AliasProperty alp && property == alp.Property
                                || p is AliasProperty alp2 && property.Property == alp2.Property
                            );
                            if (matchingProperties.Count() == 1)
                            {
                                var p = matchingProperties.First();
                                if (
                                    p.Domain != null
                                    && (
                                        p.Domain == property.Domain
                                        || converters.Any(c =>
                                            c.From.Any(cf => cf == p.Domain) && c.To.Any(ct => ct == property.Domain)
                                        )
                                    )
                                )
                                {
                                    param.Mappings.Add(property, p);
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
                            var property in classe.ExtendedProperties.Where(property =>
                                !property.Readonly
                                && !explicitAndAliasMappings.Any(m => m.Key == property)
                                && !param.MappingReferences.Any(m =>
                                    m.Key.ReferenceName == property.Name && m.Value.ReferenceName == "false"
                                )
                            )
                        )
                        {
                            foreach (var p in param.Class.ExtendedProperties)
                            {
                                if (
                                    !param.Mappings.ContainsKey(property)
                                    && p.Name == property.Name
                                    && p.Domain != null
                                    && (
                                        p.Domain == property.Domain
                                        || converters.Any(c =>
                                            c.From.Any(cf => cf == p.Domain) && c.To.Any(ct => ct == property.Domain)
                                        )
                                    )
                                )
                                {
                                    param.Mappings.Add(property, p);
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

                    if (finalMappings.All(mapping => mapping.Key != null))
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
                yield return new ModelError(
                    ErrorType.TMD0001,
                    classe,
                    $"Le nom '{mapper.Name}' est déjà utilisé.",
                    mapper.GetLocation()
                );
            }

            foreach (var mapper in classe.ToMappers.Where(m => m.Class != null))
            {
                var explicitMappings = mapper.Mappings.ToDictionary(p => p.Key, p => p.Value);

                foreach (
                    var property in classe
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
                        property.Property == p
                        || p is AliasProperty alp && property == alp.Property
                        || p is AliasProperty alp2 && property.Property == alp2.Property
                    );
                    if (matchingProperties.Count() == 1)
                    {
                        var p = matchingProperties.First();
                        if (
                            !p.Readonly
                            && p.Domain != null
                            && (
                                p.Domain == property.Domain
                                || converters.Any(c =>
                                    c.From.Any(cf => cf == p.Domain) && c.To.Any(ct => ct == property.Domain)
                                )
                            )
                        )
                        {
                            mapper.Mappings.Add(property, p);
                        }
                    }
                }

                var explicitAndAliasMappings = mapper.Mappings.ToDictionary(p => p.Key, p => p.Value);

                foreach (
                    var property in classe.ExtendedProperties.Where(property =>
                        !explicitAndAliasMappings.ContainsKey(property)
                        && !mapper.MappingReferences.Any(m =>
                            m.Key.ReferenceName == property.Name && m.Value.ReferenceName == "false"
                        )
                    )
                )
                {
                    foreach (var p in mapper.Class.ExtendedProperties)
                    {
                        if (
                            p.Name == property.Name
                            && p.Domain != null
                            && (
                                p.Domain == property.Domain
                                || converters.Any(c =>
                                    c.From.Any(cf => cf == p.Domain) && c.To.Any(ct => ct == property.Domain)
                                )
                            )
                            && !p.Readonly
                        )
                        {
                            mapper.Mappings.Add(property, p);
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
}
