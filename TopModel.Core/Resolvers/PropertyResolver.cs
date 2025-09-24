using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Core.Resolvers;

internal class PropertyResolver(
    ModelFile modelFile,
    IDictionary<string, Domain> domains,
    IDictionary<string, Class> referencedClasses,
    IDictionary<string, Endpoint> referencedEndpoints,
    IDictionary<string, Decorator> referencedDecorators
)
{
    /// <summary>
    /// Réinitialise les alias déjà résolus sur les classes/endpoints/décorateurs/mappers (pour le watch).
    /// </summary>
    public void ResetAliases()
    {
        foreach (var classe in modelFile.Classes)
        {
            foreach (var alp in classe.Properties.OfType<AliasProperty>().ToList())
            {
                if (alp.OriginalAliasProperty is not null)
                {
                    var index = classe.Properties.IndexOf(alp);
                    classe.Properties.RemoveAt(index);
                    if (!classe.Properties.Contains(alp.OriginalAliasProperty))
                    {
                        classe.Properties.Insert(index, alp.OriginalAliasProperty);
                    }
                }
            }

            foreach (var alp in classe.FromMapperProperties.OfType<AliasProperty>().ToList())
            {
                if (alp.OriginalAliasProperty is not null)
                {
                    var index = alp.PropertyMapping.FromMapper.Params.IndexOf(alp.PropertyMapping);
                    alp.PropertyMapping.FromMapper.Params.RemoveAt(index);
                    if (!alp.PropertyMapping.FromMapper.Params.Contains(alp.OriginalAliasProperty.PropertyMapping))
                    {
                        alp.PropertyMapping.FromMapper.Params.Insert(
                            index,
                            new PropertyMapping
                            {
                                FromMapper = alp.PropertyMapping.FromMapper,
                                Property = alp.OriginalAliasProperty,
                                TargetProperty = alp.PropertyMapping.TargetProperty,
                                TargetPropertyReference = alp.PropertyMapping.TargetPropertyReference,
                            }
                        );
                    }
                }
            }
        }

        foreach (var endpoint in modelFile.Endpoints)
        {
            foreach (var alp in endpoint.Params.OfType<AliasProperty>().ToList())
            {
                if (alp.OriginalAliasProperty is not null)
                {
                    var index = endpoint.Params.IndexOf(alp);
                    endpoint.Params.RemoveAt(endpoint.Params.IndexOf(alp));
                    if (!endpoint.Params.Contains(alp.OriginalAliasProperty))
                    {
                        endpoint.Params.Insert(index, alp.OriginalAliasProperty);
                    }
                }
            }

            if (endpoint.Returns is AliasProperty ralp && ralp.OriginalAliasProperty is not null)
            {
                endpoint.Returns = ralp.OriginalAliasProperty;
            }
        }

        foreach (var decorator in modelFile.Decorators)
        {
            foreach (var alp in decorator.Properties.OfType<AliasProperty>().ToList())
            {
                if (alp.OriginalAliasProperty is not null)
                {
                    var index = decorator.Properties.IndexOf(alp);
                    decorator.Properties.RemoveAt(decorator.Properties.IndexOf(alp));
                    if (!decorator.Properties.Contains(alp.OriginalAliasProperty))
                    {
                        decorator.Properties.Insert(index, alp.OriginalAliasProperty);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Résout les alias d'un fichier.
    /// </summary>
    /// <param name="filter">Aliases à prendre en compte dans le fichier.</param>
    /// <returns>Erreurs.</returns>
    public IEnumerable<ModelError> ResolveAliases(Func<AliasProperty, bool> filter)
    {
        foreach (var alp in modelFile.Properties.OfType<AliasProperty>().Where(filter))
        {
            IPropertyContainer propertyContainer;

            if (alp.Reference?.ClassReference != null)
            {
                if (!referencedClasses!.TryGetValue(alp.Reference.ClassReference.ReferenceName, out var aliasedClass))
                {
                    yield return new ModelError(
                        ErrorType.TMD0002,
                        alp,
                        "La classe '{0}' est introuvable dans le fichier ou l'une de ses dépendances.",
                        alp.Reference.ClassReference
                    );
                    continue;
                }

                propertyContainer = aliasedClass;
            }
            else if (alp.Reference?.EndpointReference != null)
            {
                if (
                    !referencedEndpoints!.TryGetValue(
                        alp.Reference.EndpointReference.ReferenceName,
                        out var aliasedEndpoint
                    )
                )
                {
                    yield return new ModelError(
                        ErrorType.TMD0006,
                        alp,
                        "L'endpoint '{0}' est introuvable dans le fichier ou l'une de ses dépendances.",
                        alp.Reference.EndpointReference
                    );
                    continue;
                }

                propertyContainer = aliasedEndpoint;
            }
            else if (alp.Reference?.DecoratorReference != null)
            {
                if (
                    !referencedDecorators!.TryGetValue(
                        alp.Reference.DecoratorReference.ReferenceName,
                        out var aliasedDecorator
                    )
                )
                {
                    yield return new ModelError(
                        ErrorType.TMD0005,
                        alp,
                        "Le décorateur '{0}' est introuvable dans le fichier ou l'une de ses dépendances.",
                        alp.Reference.DecoratorReference
                    );
                    continue;
                }

                propertyContainer = aliasedDecorator;
            }
            else
            {
                // Impossible
                continue;
            }

            var shouldBreak = false;
            foreach (var propReference in alp.Reference.IncludeReferences.Concat(alp.Reference.ExcludeReferences))
            {
                var aliasedProperty = propertyContainer.Properties.FirstOrDefault(p =>
                    p.Name == propReference.ReferenceName
                );
                if (aliasedProperty == null)
                {
                    yield return new ModelError(
                        ErrorType.TMD0004,
                        alp,
                        $"La propriété '{{0}}' est introuvable sur la classe '{propertyContainer}'.",
                        propReference
                    );
                    shouldBreak = true;
                }
            }

            foreach (
                var include in alp.Reference.IncludeReferences.Where(
                    (e, i) =>
                        alp
                            .Reference.IncludeReferences.Where((p, j) => p.ReferenceName == e.ReferenceName && j < i)
                            .Any()
                )
            )
            {
                yield return new ModelError(
                    ErrorType.TMD9001,
                    modelFile,
                    $"La propriété '{include.ReferenceName}' est déjà référencée dans la définition de l'alias.",
                    include
                );
                shouldBreak = true;
            }

            foreach (
                var exclude in alp.Reference.ExcludeReferences.Where(
                    (e, i) =>
                        alp
                            .Reference.ExcludeReferences.Where((p, j) => p.ReferenceName == e.ReferenceName && j < i)
                            .Any()
                )
            )
            {
                yield return new ModelError(
                    ErrorType.TMD9001,
                    modelFile,
                    $"La propriété '{exclude.ReferenceName}' est déjà référencée dans la définition de l'alias.",
                    exclude
                );
                shouldBreak = true;
            }

            if (shouldBreak)
            {
                continue;
            }

            var propertiesToAlias = (
                alp.Reference.IncludeReferences.Count > 0
                    ? alp.Reference.IncludeReferences.Select(p =>
                        propertyContainer.Properties.First(prop => prop.Name == p.ReferenceName)
                    )
                    : propertyContainer.Properties.Where(prop =>
                        !alp.Reference.ExcludeReferences.Select(p => p.ReferenceName).Contains(prop.Name)
                    )
            ).Reverse();

            foreach (var property in propertiesToAlias)
            {
                var prop = alp.Clone(
                    property,
                    alp.Reference.IncludeReferences.FirstOrDefault(ir => ir.ReferenceName == property.Name)
                );

                if (prop.As != null && prop.Domain == null)
                {
                    yield return new ModelError(
                        ErrorType.TMD9004,
                        modelFile,
                        $"Le domaine '{prop.OriginalProperty?.Domain}' doit définir un domaine 'as' pour '{prop.As}' pour définir un alias '{prop.As}' sur la propriété '{prop.OriginalProperty}' de la classe '{prop.OriginalProperty?.Class}'",
                        prop.PropertyReference ?? prop.Reference?.ContainerReference
                    );
                }

                if (alp.Class != null)
                {
                    var index = alp.Class.Properties.IndexOf(alp);
                    if (index >= 0)
                    {
                        alp.Class.Properties.Insert(index + 1, prop);
                    }
                }
                else if (alp.Endpoint?.Params.Contains(alp) ?? false)
                {
                    var index = alp.Endpoint.Params.IndexOf(alp);
                    if (index >= 0)
                    {
                        alp.Endpoint.Params.Insert(index + 1, prop);
                    }
                }
                else if (alp.Endpoint?.Returns == alp)
                {
                    alp.Endpoint.Returns = prop;
                }
                else if (alp.Decorator != null)
                {
                    var index = alp.Decorator.Properties.IndexOf(alp);
                    if (index >= 0)
                    {
                        alp.Decorator.Properties.Insert(index + 1, prop);
                    }
                }
                else if (alp.PropertyMapping != null)
                {
                    var index = alp.PropertyMapping.FromMapper.Params.FindIndex(param =>
                        param.TryPickT1(out var pm, out var _) && pm.Property == alp
                    );
                    if (index >= 0)
                    {
                        var mapping = new PropertyMapping
                        {
                            FromMapper = alp.PropertyMapping.FromMapper,
                            Property = prop,
                            TargetProperty = alp.PropertyMapping.TargetProperty,
                            TargetPropertyReference = alp.PropertyMapping.TargetPropertyReference,
                        };
                        prop.PropertyMapping = mapping;
                        alp.PropertyMapping.FromMapper.Params.Insert(index + 1, mapping);
                    }
                }
            }

            if (alp.Class != null)
            {
                alp.Class.Properties.Remove(alp);
            }
            else if (alp.Endpoint?.Params.Contains(alp) ?? false)
            {
                alp.Endpoint.Params.Remove(alp);
            }
            else if (alp.PropertyMapping != null)
            {
                alp.PropertyMapping.FromMapper.Params.RemoveAll(param =>
                    param.TryPickT1(out var pm, out var _) && pm.Property == alp
                );
            }
            else
            {
                alp.Decorator?.Properties.Remove(alp);
            }
        }
    }

    /// <summary>
    /// Résout les propriétés cible (la FK) pour les associations.
    /// </summary>
    /// <returns>Erreurs.</returns>
    public IEnumerable<ModelError> ResolveAssociationProperties()
    {
        foreach (
            var ap in modelFile
                .Classes.SelectMany(c => c.Properties.OfType<AssociationProperty>())
                .Where(ap => ap.Association != null)
        )
        {
            if (ap.Type.IsToMany() && !(ap.Property?.Domain?.AsDomains.ContainsKey(ap.As) ?? false))
            {
                yield return new ModelError(
                    ErrorType.TMD9003,
                    ap,
                    $@"Cette association ne peut pas avoir le type {ap.Type} car le domain {ap.Property?.Domain} ne contient pas de définition de domaine 'as' pour '{ap.As}'.",
                    ap.Reference
                );
                continue;
            }

            if (ap.PropertyReference != null)
            {
                var referencedProperty = ap.Association.ExtendedProperties.FirstOrDefault(p =>
                    p.Name == ap.PropertyReference!.ReferenceName
                );
                if (referencedProperty == null)
                {
                    yield return new ModelError(
                        ErrorType.TMD0004,
                        ap,
                        $"La propriété '{{0}}' est introuvable sur la classe '{ap.Association}'.",
                        ap.PropertyReference
                    );
                }

                ap.Property = referencedProperty;
            }
        }
    }

    /// <summary>
    /// Résolutions des références sur les propriétés (hors alias).
    /// On ne touche pas aux propriétés liées à une classe et un décorateur en même temps car
    /// ces propriétés sont déjà résolues sur les décorateurs avant d'être recopiées sur les classes.
    /// </summary>
    /// <returns>Erreurs.</returns>
    public IEnumerable<ModelError> ResolveNonAliasProperties()
    {
        foreach (var prop in modelFile.Properties.Where(p => p.SourceDecorator is null))
        {
            switch (prop)
            {
                case RegularProperty rp:
                    if (
                        rp.DomainReference == null
                        || !domains.TryGetValue(rp.DomainReference.ReferenceName, out var domain)
                    )
                    {
                        yield return new ModelError(
                            ErrorType.TMD0003,
                            rp,
                            "Le domaine '{0}' est introuvable.",
                            rp.DomainReference
                        );
                        break;
                    }

                    foreach (var error in CheckDomainParameters(rp, rp.DomainReference, domain))
                    {
                        yield return error;
                    }

                    rp.Domain = domain;
                    rp.DomainParameters = rp.DomainReference.ParameterReferences.ToDictionary(
                        pr => pr.Key.ReferenceName,
                        pr => pr.Value.Value
                    );
                    break;

                case AssociationProperty ap:
                    if (!referencedClasses.TryGetValue(ap.Reference.ReferenceName, out var association))
                    {
                        yield return new ModelError(
                            ErrorType.TMD0002,
                            ap,
                            "La classe '{0}' est introuvable dans le fichier ou l'une de ses dépendances.",
                            ap.Reference
                        );
                        break;
                    }

                    if (ap.PropertyReference == null && !association.ExtendedProperties.Any(p => p.PrimaryKey))
                    {
                        yield return new ModelError(
                            ErrorType.TMD9002,
                            ap,
                            "La classe '{0}' doit avoir au moins une clé primaire pour être référencée dans une association.",
                            ap.Reference
                        );
                        break;
                    }

                    if (
                        ap.PropertyReference == null
                        && association.Properties.Count(p => p.PrimaryKey) > 1
                        && ap.PropertyReference == null
                    )
                    {
                        yield return new ModelError(
                            ErrorType.TMD9002,
                            ap,
                            "La classe '{0}' a plusieurs clés primaires, vous devez obligatoirement référencer une propriété cible.",
                            ap.Reference
                        );
                        break;
                    }

                    ap.Association = association;

                    if (ap.HasReverse && (ap.Type == AssociationType.OneToOne || !(ap.Class?.IsPersistent ?? false)))
                    {
                        yield return new ModelError(
                            ErrorType.TMD9005,
                            ap,
                            $"Il sera impossible de générer une association réciproque pour cette association.",
                            ap.Reference
                        );
                    }

                    break;

                case CompositionProperty cp:
                    if (!referencedClasses.TryGetValue(cp.Reference.ReferenceName, out var composition))
                    {
                        yield return new ModelError(
                            ErrorType.TMD0002,
                            cp,
                            "La classe '{0}' est introuvable dans le fichier ou l'une de ses dépendances.",
                            cp.Reference
                        );
                        break;
                    }

                    cp.Composition = composition;

                    if (cp.DomainReference != null)
                    {
                        if (!domains.TryGetValue(cp.DomainReference.ReferenceName, out var cpDomain))
                        {
                            yield return new ModelError(
                                ErrorType.TMD0003,
                                cp,
                                "Le domaine '{0}' est introuvable.",
                                cp.DomainReference
                            );
                            break;
                        }

                        foreach (var error in CheckDomainParameters(cp, cp.DomainReference, cpDomain))
                        {
                            yield return error;
                        }

                        cp.Domain = cpDomain;
                        cp.DomainParameters = cp.DomainReference.ParameterReferences.ToDictionary(
                            pr => pr.Key.ReferenceName,
                            pr => pr.Value.Value
                        );
                    }

                    break;

                case AliasProperty alp when alp.DomainReference != null:
                    if (!domains.TryGetValue(alp.DomainReference.ReferenceName, out var aliasDomain))
                    {
                        yield return new ModelError(
                            ErrorType.TMD0003,
                            alp,
                            "Le domaine '{0}' est introuvable.",
                            alp.DomainReference
                        );
                        break;
                    }

                    foreach (var error in CheckDomainParameters(alp, alp.DomainReference, aliasDomain))
                    {
                        yield return error;
                    }

                    alp.Domain = aliasDomain;
                    alp.DomainParameters = alp.DomainReference.ParameterReferences.ToDictionary(
                        pr => pr.Key.ReferenceName,
                        pr => pr.Value.Value
                    );
                    break;
            }
        }
    }

    private static IEnumerable<ModelError> CheckDomainParameters(
        IProperty property,
        DomainReference domainRef,
        Domain domain
    )
    {
        foreach (
            var extraParameter in domainRef.ParameterReferences.Keys.Where(pr =>
                !domain.TemplateParameters.Any(tp => tp.Name == pr.ReferenceName)
            )
        )
        {
            yield return new ModelError(
                ErrorType.TMD0007,
                property,
                $"Le paramètre '{extraParameter.ReferenceName}' n'existe pas sur le domaine '{domain.Name}'.",
                extraParameter
            );
        }

        foreach (
            var missingParameter in domain.TemplateParameters.Where(tp =>
                tp.Required && !domainRef.ParameterReferences.Any(pr => pr.Key.ReferenceName == tp.Name)
            )
        )
        {
            yield return new ModelError(
                ErrorType.TMD0008,
                property,
                $"Le paramètre '{missingParameter.Name}' du domaine '{domain.Name}' est obligatoire.",
                domainRef
            );
        }
    }
}
