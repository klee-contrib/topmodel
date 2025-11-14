using Microsoft.Extensions.Localization;
using Spectre.Console;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Core.Resolvers;

internal class PropertyResolver(
    IStringLocalizer localizer,
    IList<ModelFile> modelFiles,
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
        foreach (var classe in modelFiles.SelectMany(mf => mf.Classes))
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

        foreach (var endpoint in modelFiles.SelectMany(mf => mf.Endpoints))
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

        foreach (var decorator in modelFiles.SelectMany(mf => mf.Decorators))
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
        var aliasedProperties = modelFiles.SelectMany(mf => mf.Properties).OfType<AliasProperty>().Where(filter);
        var sortedAliases = CoreUtils.Sort<AliasProperty>(
            aliasedProperties,
            a =>
                aliasedProperties
                    .Where(b =>
                        (a.Class != b.Class || a.Endpoint != b.Endpoint || a.Decorator != b.Decorator)
                        && (
                            b.Class?.Name != null && b.Class?.Name == a.Reference?.ClassReference?.ReferenceName
                            || b.Endpoint?.Name != null
                                && b.Endpoint?.Name == a.Reference?.EndpointReference?.ReferenceName
                            || b.Decorator?.Name != null
                                && b.Decorator?.Name == a.Reference?.DecoratorReference?.ReferenceName
                        )
                    )
                    .ToList()
        );
        foreach (var alp in sortedAliases)
        {
            IPropertyContainer propertyContainer;

            if (alp.Reference?.ClassReference != null)
            {
                if (!referencedClasses!.TryGetValue(alp.Reference.ClassReference.ReferenceName, out var aliasedClass))
                {
                    yield return new ModelError(
                        localizer,
                        ErrorType.TMD0002,
                        [alp.Reference.ClassReference.ReferenceName],
                        alp,
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
                        localizer,
                        ErrorType.TMD0006,
                        [alp.Reference.EndpointReference.ReferenceName],
                        alp,
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
                        localizer,
                        ErrorType.TMD0005,
                        [alp.Reference.DecoratorReference.ReferenceName],
                        alp,
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
                        localizer,
                        ErrorType.TMD0004,
                        [propReference.ReferenceName, propertyContainer.Name],
                        alp,
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
                    alp,
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
                    alp,
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
                        alp,
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
                    var index = alp
                        .PropertyMapping.FromMapper.Params.ToList()
                        .FindIndex(param => param.TryPickT1(out var pm, out var _) && pm.Property == alp);
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
                foreach (
                    var param in alp
                        .PropertyMapping.FromMapper.Params.Where(param =>
                            param.TryPickT1(out var pm, out var _) && pm.Property == alp
                        )
                        .ToList()
                )
                {
                    alp.PropertyMapping.FromMapper.Params.Remove(param);
                }
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
            var ap in modelFiles
                .SelectMany(mf => mf.Properties.OfType<AssociationProperty>())
                .Where(ap => ap.Association != null)
        )
        {
            if (ap.PropertyReference == null && !ap.Association.ExtendedProperties.Any(p => p.PrimaryKey))
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
                && ap.Association.Properties.Count(p => p.PrimaryKey) > 1
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

            if (ap.Type.IsToMany() && !(ap.Property?.Domain?.AsDomains.ContainsKey(ap.As) ?? false))
            {
                yield return new ModelError(
                    ErrorType.TMD9003,
                    ap,
                    $@"Cette association ne peut pas avoir le type {ap.Type} car le domaine {ap.Property?.Domain} ne contient pas de définition de domaine 'as' pour '{ap.As}'.",
                    ap.Reference
                );
                continue;
            }

            if (
                ap.WithReverse != null
                && !ap.Type.IsToMany()
                && !(ap.Class.PrimaryKey.FirstOrDefault()?.Domain?.AsDomains.ContainsKey(ap.As) ?? false)
            )
            {
                yield return new ModelError(
                    ErrorType.TMD9003,
                    ap,
                    $@"Cette association ne peut pas définir d'association réciproque car le domaine {ap.Class.PrimaryKey.FirstOrDefault()?.Domain} ne contient pas de définition de domaine 'as' pour '{ap.As}'.",
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
                        localizer,
                        ErrorType.TMD0004,
                        [ap.PropertyReference.ReferenceName, ap.Association.Name],
                        ap,
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
        var classes = modelFiles.SelectMany(mf => mf.Classes);

        foreach (var classe in classes)
        {
            foreach (var rap in classe.Properties.OfType<ReverseAssociationProperty>().ToList())
            {
                classe.Properties.Remove(rap);
            }
        }

        foreach (var prop in modelFiles.SelectMany(mf => mf.Properties).Where(p => p.SourceDecorator is null))
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
                            localizer,
                            ErrorType.TMD0003,
                            [rp.DomainReference?.ReferenceName ?? string.Empty],
                            rp,
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
                    if (ap.ExplicitType != null)
                    {
                        var warning = "La propriété `type` est dépréciée et sera retirée en 4.0.";

                        switch (ap.Type)
                        {
                            case AssociationType.ManyToOne:
                                warning += " 'manyToOne' est la valeur par défaut, vous pouvez la retirer.";
                                break;
                            case AssociationType.OneToOne:
                                warning +=
                                    " Vous pouvez définir une 'oneToOne' en retirant la définition de 'type' et en ajoutant une contrainte d'unicité sur la propriété.";
                                break;
                            case AssociationType.OneToMany:
                                warning +=
                                    " Vous devez définir l'association réciproque sur la classe cible pour définir une 'oneToMany'.";
                                break;
                            case AssociationType.ManyToMany:
                                warning +=
                                    " Vous devez définir une classe d'association explicite pour votre `manyToMany`.";
                                break;
                        }

                        yield return new ModelError(ErrorType.TMD9008, ap, warning, ap.ExplicitType, isError: false);
                    }

                    if (
                        (
                            ap.Class == null
                            || (ap.Class.Extends == null || !ap.Class.IsPersistent) && ap.Class.PrimaryKey.Count() != 1
                        ) && ap.Type.IsToMany()
                    )
                    {
                        yield return new ModelError(
                            ErrorType.TMD9005,
                            ap,
                            $"Il est impossible de définir une association oneToMany ou manyToMany sur classe sans clé primaire simple.",
                            ap.Reference
                        );

                        if (ap.WithReverse != null)
                        {
                            yield return new ModelError(
                                ErrorType.TMD9006,
                                ap,
                                $"Une association réciproque ne peut être définie que dans une classe avec une clé primaire simple.",
                                ap.Reference
                            );
                        }

                        break;
                    }

                    if (!referencedClasses.TryGetValue(ap.Reference.ReferenceName, out var association))
                    {
                        yield return new ModelError(
                            localizer,
                            ErrorType.TMD0002,
                            [ap.Reference.ReferenceName],
                            ap,
                            ap.Reference
                        );
                        break;
                    }

                    ap.Association = association;

                    if (ap.WithReverse != null && ap.Class != null)
                    {
                        if (!classes.Contains(association))
                        {
                            yield return new ModelError(
                                ErrorType.TMD9007,
                                ap,
                                "Le fichier de la classe cible doit référencer le fichier courant pour définir une association réciproque.",
                                ap.Reference
                            );
                            break;
                        }
                        ap.ReverseProperty = new ReverseAssociationProperty
                        {
                            Class = association,
                            ReverseProperty = ap,
                        };
                        association.Properties.Add(ap.ReverseProperty);
                    }

                    break;

                case CompositionProperty cp:
                    if (!referencedClasses.TryGetValue(cp.Reference.ReferenceName, out var composition))
                    {
                        yield return new ModelError(
                            localizer,
                            ErrorType.TMD0002,
                            [cp.Reference.ReferenceName],
                            cp,
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
                                localizer,
                                ErrorType.TMD0003,
                                [cp.DomainReference.ReferenceName],
                                cp,
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
                            localizer,
                            ErrorType.TMD0003,
                            [alp.DomainReference.ReferenceName],
                            alp,
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

    private IEnumerable<ModelError> CheckDomainParameters(IProperty property, DomainReference domainRef, Domain domain)
    {
        foreach (
            var extraParameter in domainRef.ParameterReferences.Keys.Where(pr =>
                !domain.TemplateParameters.Any(tp => tp.Name == pr.ReferenceName)
            )
        )
        {
            yield return new ModelError(
                localizer,
                ErrorType.TMD0007,
                [extraParameter.ReferenceName, domain.Name],
                property,
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
                localizer,
                ErrorType.TMD0008,
                [missingParameter.Name, domain.Name],
                property,
                domainRef
            );
        }
    }
}
