using System.Diagnostics.CodeAnalysis;
using System.Text;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Core.Model.Implementation;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Generator.Core;

public abstract class GeneratorConfigBase : WatcherConfigBase
{
    /// <summary>
    /// Racine du répertoire de génération.
    /// </summary>
    public virtual required string OutputDirectory { get; set; }

    /// <summary>
    /// Désactive la génération des valeurs par défaut des propriétés dans les classes et endpoints générés avec cette configuration.
    /// </summary>
    public virtual bool IgnoreDefaultValues { get; set; }

    /// <summary>
    /// Si les libellés des listes de références doivent être traduits.
    /// </summary>
    public virtual bool? TranslateReferences { get; set; }

    /// <summary>
    /// Si les libellés des propriétés doivent être traduits.
    /// </summary>
    public virtual bool? TranslateProperties { get; set; }

    /// <summary>
    /// Générateurs désactivés.
    /// </summary>
    public virtual IList<string>? Disable { get; set; }

    /// <summary>
    /// Si le langage cible de la configuration supporte les enums.
    /// </summary>
    public virtual bool HasEnumSupport => true;

    /// <summary>
    /// Mode de génération des valeurs de propriétés avec clé d'unicité.
    /// </summary>
    public virtual UniqueValueGenerationMode UniqueValueGeneration { get; set; } =
        UniqueValueGenerationMode.EnumOrConst;

    /// <summary>
    /// Mode de génération de l'API ("Client" ou "Server").
    /// </summary>
    public virtual string? ApiGeneration { get; set; }

    /// <summary>
    /// Utilise le nom de l'enum ou de la constante pour référencer une valeur.
    /// </summary>
    protected virtual bool UseValueNameForValues => true;

    protected virtual string NullValue => "null";

    /// <summary>
    /// Formatte une valeur de propriété.
    /// </summary>
    /// <param name="property">Propriété.</param>
    /// <param name="value">Valeur.</param>
    /// <returns>La valeur de la propriété.</returns>
    public string FormatValue(IProperty property, string value)
    {
        var template = GetImplementation(property.Domain)?.GetValueTemplate(value);
        if (template != null)
        {
            return template.Value.Replace("{value}", value).ParseTemplate(property, this);
        }

        if (ShouldQuoteValue(property))
        {
            return QuoteValue(value);
        }

        return value;
    }

    public virtual IEnumerable<ClassValue> GetAllValues(Class classe)
    {
        foreach (var value in classe.Values)
        {
            yield return value;
        }

        foreach (var child in AvailableClasses.Where(c => c.Extends == classe))
        {
            foreach (var value in GetAllValues(child))
            {
                yield return value;
            }
        }
    }

    public virtual IEnumerable<(string Annotation, IEnumerable<string> Imports)> GetAnnotations(
        IAnnotationContainer container,
        string tag
    )
    {
        IList<AnnotationInstance> annotations = [.. container.Annotations];

        if (container is IProperty prop)
        {
            foreach (var parentAnnotation in prop.Parent.PropertyAnnotations)
            {
                if (!annotations.Any(a => a.Annotation == parentAnnotation.Annotation))
                {
                    annotations.Add(parentAnnotation);
                }
            }

            foreach (var decoratorAnnotation in prop.SourceDecorator?.PropertyAnnotations ?? [])
            {
                if (!annotations.Any(a => a.Annotation == decoratorAnnotation.Annotation))
                {
                    annotations.Add(decoratorAnnotation);
                }
            }
        }

        foreach (var excludedAnnotation in container.ExcludedAnnotations)
        {
            var annotationsToRemove = annotations.Where(a => a.Annotation == excludedAnnotation.Annotation).ToList();
            foreach (var annotation in annotationsToRemove)
            {
                annotations.Remove(annotation);
            }
        }

        foreach (
            var (implementation, annotation, parameters) in annotations.SelectMany(a =>
                GetImplementation(a.Annotation)
                    .Select(i => (Implementation: i, a.Annotation, a.Parameters))
                    .Where(a => FilterAnnotations(a.Implementation, a.Annotation, container, tag))
            )
        )
        {
            if (container is IProperty p)
            {
                yield return (
                    Annotation: implementation.Text.Value.ParseTemplate(
                        p,
                        annotation.TemplateParameters,
                        parameters,
                        this,
                        tag
                    ),
                    Imports: implementation.Imports.Select(i =>
                        i.Value.ParseTemplate(p, annotation.TemplateParameters, parameters, this, tag)
                    )
                );
            }
            else if (container is IPropertyContainer c)
            {
                yield return (
                    Annotation: implementation.Text.Value.ParseTemplate(
                        c,
                        annotation.TemplateParameters,
                        parameters,
                        this,
                        tag
                    ),
                    Imports: implementation.Imports.Select(i =>
                        i.Value.ParseTemplate(c, annotation.TemplateParameters, parameters, this, tag)
                    )
                );
            }
        }

        if (container is IPropertyContainer pc)
        {
            foreach (
                var annotation in pc
                    .Decorators.SelectMany(d =>
                        GetDecoratorAnnotations(
                            pc,
                            d.Decorator,
                            d.Parameters,
                            container.ExcludedAnnotations.Select(e => e.Annotation),
                            tag
                        )
                    )
                    .Distinct()
            )
            {
                yield return annotation;
            }
        }

        if (container is IProperty { Domain: not null } property)
        {
            foreach (
                var (implementation, annotation, parameters) in property.Domain.Annotations.SelectMany(a =>
                    GetImplementation(a.Annotation)
                        .Select(i => (Implementation: i, a.Annotation, a.Parameters))
                        .Where(a => FilterAnnotations(a.Implementation, a.Annotation, property, tag))
                )
            )
            {
                if (
                    !annotations.Any(a => a.Annotation == annotation)
                    && !container.ExcludedAnnotations.Any(e => e.Annotation == annotation)
                )
                {
                    var resolvedParameters = parameters.ToDictionary(
                        p => p.Key,
                        p => p.Value.ParseTemplate(property, this, tag)
                    );
                    yield return (
                        Annotation: implementation.Text.Value.ParseTemplate(
                            property,
                            annotation.TemplateParameters,
                            resolvedParameters,
                            this,
                            tag
                        ),
                        Imports: implementation.Imports.Select(i =>
                            i.Value.ParseTemplate(
                                property,
                                annotation.TemplateParameters,
                                resolvedParameters,
                                this,
                                tag
                            )
                        )
                    );
                }
            }
        }
    }

    /// <summary>
    /// Récupère le mode de génération des endpoints pour un tag donné.
    /// </summary>
    /// <param name="tag">Le tag.</param>
    /// <returns>Le mode de génération.</returns>
    public ApiGenerationMode GetApiGenerationMode(string tag)
    {
        if (ApiGeneration == null)
        {
            return ApiGenerationMode.None;
        }

        return ResolveVariables(ApiGeneration, tag) switch
        {
            nameof(ApiGenerationMode.Client) => ApiGenerationMode.Client,
            nameof(ApiGenerationMode.Server) => ApiGenerationMode.Server,
            _ => ApiGenerationMode.None,
        };
    }

    public virtual string GetBestClassTag(Class classe, string tag)
    {
        return classe.Tags.Contains(tag)
            ? tag
            : classe.Tags.Intersect(Tags).FirstOrDefault()
                ?? classe.Tags.Intersect(ReferencedTagConfigs.Keys).FirstOrDefault()
                ?? tag;
    }

    public virtual string? GetClassExtends(Class classe, string tag)
    {
        return GetTypeName(classe.Extends)
            ?? classe
                .Decorators.SelectMany(d =>
                    GetDecoratorImplementationValues(i => i.Extends, classe, d.Decorator, d.Parameters, tag)
                )
                .SingleOrDefault(e => e != null);
    }

    public virtual IEnumerable<string> GetClassImplements(Class classe, string tag)
    {
        return classe
            .Decorators.SelectMany(d =>
                GetDecoratorImplementationValues(i => i.Implements, classe, d.Decorator, d.Parameters, tag)
            )
            .Distinct();
    }

    public virtual string GetConvertedValue(string value, Domain? fromDomain, Domain? toDomain)
    {
        var converter = fromDomain.GetConverter(toDomain);
        if (converter != null && fromDomain != null && toDomain != null)
        {
            var text = GetImplementation(converter)?.Text;
            if (text != null)
            {
                value = GetImplementation(converter)!
                    .Text.Replace("{value}", value)
                    .ParseTemplate(fromDomain, toDomain, this);
            }
        }

        return value;
    }

    public virtual IEnumerable<string> GetConverterImports(Domain? fromDomain, Domain? toDomain)
    {
        if (fromDomain != null && toDomain != null && fromDomain != toDomain)
        {
            var converter = fromDomain.GetConverter(toDomain);
            if (converter != null)
            {
                var imports = GetImplementation(converter)?.Imports;
                if (imports != null)
                {
                    foreach (var import in imports)
                    {
                        yield return import;
                    }
                }
            }
        }
    }

    public virtual IEnumerable<string> GetDecoratorImports(Class classe, string tag)
    {
        foreach (
            var import in classe
                .Decorators.SelectMany(d =>
                    GetDecoratorImplementationValues(i => i.Imports, classe, d.Decorator, d.Parameters, tag)
                )
                .Distinct()
        )
        {
            yield return import;
        }
    }

    public virtual IEnumerable<string> GetDecoratorImports(Endpoint endpoint, string tag)
    {
        foreach (
            var import in endpoint
                .Decorators.SelectMany(d =>
                    GetDecoratorImplementationValues(i => i.Imports, endpoint, d.Decorator, d.Parameters, tag)
                )
                .Distinct()
        )
        {
            yield return import;
        }
    }

    public virtual IEnumerable<string> GetDomainImports(IProperty property, string tag)
    {
        foreach (var (domain, generic) in property.DomainChain)
        {
            foreach (
                var import in GetImplementation(domain)!.Imports.Select(u => u.Value.ParseTemplate(property, this, tag))
            )
            {
                yield return import;
            }

            if (!generic)
            {
                break;
            }
        }
    }

    /// <summary>
    /// Récupère le nom du type d'enum associé à la propriété demandée.
    /// </summary>
    /// <param name="prop">La propriété.</param>
    /// <param name="internalReference">S'il s'agit d'une référence à la propriété depuis sa classe.</param>
    /// <returns>Le nom du type.</returns>
    public virtual string GetEnumType(IProperty prop, bool internalReference = false)
    {
        if (prop.UniqueValuedProperty == null)
        {
            return string.Empty;
        }

        var className = prop.UniqueValuedProperty?.Class?.NamePascal ?? string.Empty;
        var propName = prop.UniqueValuedProperty?.NamePascal ?? string.Empty;

        if (prop.UniqueValuedProperty?.Class?.Enum == EnumMode.Enum)
        {
            return className;
        }

        return GetEnumInEnumClassType(className, propName, internalReference);
    }

    public virtual string GetReadonlyEnumClassInstanceName(Class classe, string refName, bool internalReference = false)
    {
        var sb = new StringBuilder();

        if (!internalReference)
        {
            sb.Append($"{classe.NamePascal}.");
        }

        sb.Append(refName.ToPascalCase(strictIfUppercase: true));
        return sb.ToString();
    }

    /// <summary>
    /// Récupère le module racine pour un namespace.
    /// </summary>
    /// <param name="ns">Namespace.</param>
    /// <returns>Module racine.</returns>
    public virtual string GetRootModule(Namespace ns)
    {
        return ResolveVariables(RootModule, module: ns.Module);
    }

    /// <summary>
    /// Récupère le type d'une propriété.
    /// </summary>
    /// <param name="property">Domaine.</param>
    /// <param name="forceAssociationPropertyType">Pour une association, retourne toujours le type de la propriété cible.</param>
    /// <param name="skipChain">Récupère le type à l'index demandé dans la hiérarchie de domaine.</param>
    /// <returns>Le type.</returns>
    public virtual string GetType(IProperty property, bool forceAssociationPropertyType = false, int skipChain = 0)
    {
        string GetType(IEnumerable<(Domain Domain, bool Generic)> domainChain)
        {
            var queue = new Queue<(Domain Domain, bool As)>(domainChain);

            var (domain, generic) = queue.Dequeue();

            if (queue.Count == 0)
            {
                if (
                    property.Association != null
                    && property.UseClassForAssociation
                    && !forceAssociationPropertyType
                    && AvailableClasses.Contains(property.Association)
                )
                {
                    return GetTypeName(property.Association!);
                }
                else if (
                    HasEnumSupport
                    && property is { EnumLikeProperty: IProperty elp }
                    && AvailableClasses.Contains(elp.Class)
                    && (UniqueValueGeneration.CanEnum || elp.Class.Enum == EnumMode.Enum)
                    && (!UseValueNameForValues || elp == property.EnumProperty)
                )
                {
                    return (GetImplementation(elp.Domain)?.GenericType ?? "{T}")
                        .Replace("{T}", GetEnumType(elp, elp.Class == property.Class))
                        .ParseTemplate(property, this);
                }
            }

            var impl = GetImplementation(domain);

            if (generic && queue.Count > 0 && impl?.GenericType != null)
            {
                return impl.GenericType.Replace("{T}", GetType(queue)).ParseTemplate(property, this);
            }

            return (GetImplementation(domain)?.Type ?? string.Empty).ParseTemplate(property, this);
        }

        return property switch
        {
            { Composition: Class c, Domain: Domain domain } => (GetImplementation(domain)?.GenericType ?? "{T}")
                .Replace("{T}", GetTypeName(c))
                .ParseTemplate(property, this),
            { Composition: Class c } => GetTypeName(c),
            _ => GetType(property.DomainChain.Skip(skipChain)),
        };
    }

    /// <summary>
    /// Récupère le nom du type pour une classe.
    /// </summary>
    /// <param name="classe">Classe.</param>
    /// <returns>Le nom du type.</returns>
    [return: NotNullIfNotNull(nameof(classe))]
    public virtual string? GetTypeName(Class? classe)
    {
        return classe?.NamePascal;
    }

    public virtual string GetUniqueValuedName(IProperty property, string refName, bool internalReference = false)
    {
        var sb = new StringBuilder();

        if (!internalReference)
        {
            sb.Append($"{property.Class.NamePascal}.");
        }

        sb.Append(refName.ToPascalCase(strictIfUppercase: true));
        sb.Append(property.NamePascal);

        return sb.ToString();
    }

    /// <summary>
    /// Récupère la valeur d'une propriété.
    /// </summary>
    /// <param name="property">La propriété.</param>
    /// <param name="value">Valeur à utiliser, si non renseigné utilise la valeur par défaut de la propriété.</param>
    /// <returns>La valeur.</returns>
    public virtual string GetValue(IProperty property, string? value = null)
    {
        if (!IgnoreDefaultValues && property is not { Composition: not null })
        {
            value ??= property?.DefaultValue;
        }

        if (property == null || value == null || value == "null" || value == "undefined")
        {
            return NullValue;
        }

        if (
            UseValueNameForValues
            && property is { ReadonlyEnumClassAssociation: Class a }
            && AvailableClasses.Contains(a)
        )
        {
            var refName = property
                .UniqueValuedProperty!.Class.Values.SingleOrDefault(rv =>
                    rv.Value[property.UniqueValuedProperty] == value
                )
                ?.Name;
            if (refName != null)
            {
                var instanceName = GetReadonlyEnumClassInstanceName(a, refName);
                if (!string.IsNullOrEmpty(instanceName))
                {
                    return instanceName;
                }
            }
        }
        else if (
            HasEnumSupport
            && UseValueNameForValues
            && property.EnumProperty != null
            && AvailableClasses.Contains(property.EnumProperty!.Class)
            && (UniqueValueGeneration.CanEnum || property.EnumProperty?.Class.Enum == EnumMode.Enum)
        )
        {
            return $"{GetEnumType(property.EnumProperty!).TrimEnd('?')}.{value}";
        }
        else if (
            UseValueNameForValues
            && UniqueValueGeneration.CanConst
            && property.UniqueValuedProperty != null
            && AvailableClasses.Contains(property.UniqueValuedProperty!.Class)
        )
        {
            var refName = property
                .UniqueValuedProperty!.Class.Values.SingleOrDefault(rv =>
                    rv.Value[property.UniqueValuedProperty] == value
                )
                ?.Name;
            if (refName != null)
            {
                return GetUniqueValuedName(property.UniqueValuedProperty!, refName);
            }
        }

        return FormatValue(property, value);
    }

    public virtual IEnumerable<string> GetValueImports(IProperty property, string? value = null)
    {
        if (!IgnoreDefaultValues && property is not { Composition: not null })
        {
            value ??= property.DefaultValue;
        }

        var template = value != null ? GetImplementation(property.Domain)?.GetValueTemplate(value) : null;
        if (template != null)
        {
            return template.Imports.Select(i => i.Value.ParseTemplate(property, this));
        }
        else
        {
            return [];
        }
    }

    public virtual bool IsPersistent(Class classe, string tag)
    {
        return classe.IsPersistent;
    }

    /// <summary>
    /// Détermine si une valeur de cette propriété doit être mise entre guillemets.
    /// </summary>
    /// <param name="property">Propriété.</param>
    /// <returns>Oui/non.</returns>
    public virtual bool ShouldQuoteValue(IProperty property)
    {
        return GetImplementation(property.Domain)?.Type?.ToLower() == "string";
    }

    protected virtual string GetEnumInEnumClassType(string className, string propName, bool internalReference = false)
    {
        return $"{className}{propName}";
    }

    protected virtual string QuoteValue(string value)
    {
        return $@"""{value}""";
    }

    private bool FilterAnnotations(
        AnnotationImplementation implementation,
        Annotation annotation,
        IAnnotationContainer container,
        string tag
    )
    {
        return (
                annotation.Target.Count == 0
                || annotation.Target.Any(t =>
                    t switch
                    {
                        Target.Class => container is Class,
                        Target.Endpoint => container is Endpoint,
                        Target.ClientEndpoint => container is Endpoint
                            && GetApiGenerationMode(tag) == ApiGenerationMode.Client,
                        Target.ServerEndpoint => container is Endpoint
                            && GetApiGenerationMode(tag) == ApiGenerationMode.Server,
                        Target.Property => container is IProperty,
                        Target.AssociationProperty => container is IProperty { Association: not null },
                        Target.CompositionProperty => container is IProperty { Composition: not null },
                        Target.RegularProperty => container is IProperty { Association: null, Composition: null },
                        _ => true,
                    }
                )
            )
            && implementation.When.All(ac =>
                ac switch
                {
                    AnnotationConstraint.NonPersisted => container is Endpoint
                        || container is Class c && !IsPersistent(c, tag)
                        || container is IProperty { Endpoint: not null }
                        || container is IProperty { Class: Class { Abstract: false } pc } && !IsPersistent(pc, tag),
                    AnnotationConstraint.Persisted => container is Class c && IsPersistent(c, tag)
                        || container is IProperty { Class: Class { Abstract: false } pc } && IsPersistent(pc, tag),
                    AnnotationConstraint.ClassProperty => container is IProperty { Class: Class { Abstract: false } },
                    AnnotationConstraint.EndpointParam => container is IProperty { Endpoint: Endpoint e } p
                        && e.Params.Contains(p),
                    AnnotationConstraint.PrimaryKey => container is IProperty { PrimaryKey: true },
                    _ => true,
                }
            );
    }

    private IEnumerable<(string Annotation, IEnumerable<string> Imports)> GetDecoratorAnnotations(
        IPropertyContainer container,
        Decorator decorator,
        IDictionary<string, string> parameters,
        IEnumerable<Annotation> excludedAnnotations,
        string tag
    )
    {
        foreach (
            var (implementation, annotation, annotationParameters) in decorator
                .Annotations.Where(a => !excludedAnnotations.Contains(a.Annotation))
                .SelectMany(a =>
                    GetImplementation(a.Annotation).Select(i => (Implementation: i, a.Annotation, a.Parameters))
                )
                .Where(a => FilterAnnotations(a.Implementation, a.Annotation, container, tag))
        )
        {
            if (!container.Annotations.Any(a => a.Annotation == annotation))
            {
                var resolvedParameters = annotationParameters.ToDictionary(
                    p => p.Key,
                    p => p.Value.ParseTemplate(container, decorator.TemplateParameters, parameters, this, tag)
                );
                yield return (
                    Annotation: implementation.Text.Value.ParseTemplate(
                        container,
                        annotation.TemplateParameters,
                        resolvedParameters,
                        this,
                        tag
                    ),
                    Imports: implementation.Imports.Select(i =>
                        i.Value.ParseTemplate(container, annotation.TemplateParameters, resolvedParameters, this, tag)
                    )
                );
            }
        }

        foreach (var subD in decorator.Decorators)
        {
            foreach (
                var values in GetDecoratorAnnotations(
                    container,
                    subD.Decorator,
                    subD.Parameters.ToDictionary(
                        p => p.Key,
                        p => p.Value.ParseTemplate(container, decorator.TemplateParameters, parameters, this, tag)
                    ),
                    excludedAnnotations.Concat(decorator.ExcludedAnnotations.Select(e => e.Annotation)),
                    tag
                )
            )
            {
                yield return values;
            }
        }
    }

    private IEnumerable<string> GetDecoratorImplementationValues(
        Func<DecoratorImplementation, StringWithVariables?> getter,
        IPropertyContainer container,
        Decorator decorator,
        IDictionary<string, string> parameters,
        string tag
    )
    {
        var implementation = GetImplementation(decorator);
        if (implementation != null)
        {
            var value = getter(implementation);
            if (value != null)
            {
                yield return value.Value.ParseTemplate(container, decorator.TemplateParameters, parameters, this, tag);
            }
        }

        foreach (var subD in decorator.Decorators)
        {
            foreach (
                var values in GetDecoratorImplementationValues(
                    getter,
                    container,
                    subD.Decorator,
                    subD.Parameters.ToDictionary(
                        p => p.Key,
                        p => p.Value.ParseTemplate(container, decorator.TemplateParameters, parameters, this, tag)
                    ),
                    tag
                )
            )
            {
                yield return values;
            }
        }
    }

    private IEnumerable<string> GetDecoratorImplementationValues(
        Func<DecoratorImplementation, IEnumerable<StringWithVariables>> getter,
        IPropertyContainer container,
        Decorator decorator,
        IDictionary<string, string> parameters,
        string tag
    )
    {
        var implementation = GetImplementation(decorator);
        if (implementation != null)
        {
            foreach (var value in getter(implementation))
            {
                yield return value.Value.ParseTemplate(container, decorator.TemplateParameters, parameters, this, tag);
            }
        }

        foreach (var subD in decorator.Decorators)
        {
            foreach (
                var values in GetDecoratorImplementationValues(
                    getter,
                    container,
                    subD.Decorator,
                    subD.Parameters.ToDictionary(
                        p => p.Key,
                        p => p.Value.ParseTemplate(container, decorator.TemplateParameters, parameters, this, tag)
                    ),
                    tag
                )
            )
            {
                yield return values;
            }
        }
    }
}
