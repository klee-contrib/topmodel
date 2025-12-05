using System.Text.RegularExpressions;
using Spectre.Console;
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

    protected virtual bool UseNamedEnums => true;

    protected virtual string NullValue => "null";

    /// <summary>
    /// Détermine si une classe peut utiliser une enum pour sa clé primaire.
    /// </summary>
    /// <param name="classe">Classe.</param>
    /// <param name="prop">Propriété à vérifier (si c'est pas la clé primaire).</param>
    /// <returns>Oui/non.</returns>
    public virtual bool CanClassUseEnums(Class classe, IProperty? prop = null)
    {
        if (!AvailableClasses.Contains(classe))
        {
            return false;
        }

        prop ??= classe.EnumKey;

        bool CheckProperty(IProperty fp)
        {
            return (
                    fp == classe.EnumKey
                    || classe.UniqueKeys.Where(uk => uk.Count == 1).Select(uk => uk.Single()).Contains(prop)
                ) && classe.Values.All(r => r.Value.ContainsKey(fp) && IsEnumNameValid(r.Value[fp]));
        }

        return classe.Enum && CheckProperty(prop!);
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

    public virtual string GetBestClassTag(Class classe, string tag)
    {
        return classe.Tags.Contains(tag)
            ? tag
            : classe.Tags.Intersect(Tags).FirstOrDefault() ?? classe
                    .Tags.Intersect(ReferencedTagConfigs.Keys)
                    .FirstOrDefault()
                ?? tag;
    }

    public virtual string? GetClassExtends(Class classe, string tag)
    {
        return classe.Extends?.NamePascal
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
        if (property.Domain != null)
        {
            foreach (
                var import in GetImplementation(property.Domain)!
                    .Imports.Select(u => u.Value.ParseTemplate(property, this, tag))
            )
            {
                yield return import;
            }

            // TODO : Si c'est un alias avec As domain générique, il faut aller chercher les imports du domaine original aussi
        }
    }

    public virtual string GetEnumType(IProperty fp, bool isPrimaryKeyDef = false)
    {
        var op = fp switch
        {
            AssociationProperty a => a.Property,
            AliasProperty { Property: AssociationProperty a } => a.Property,
            AliasProperty alp => alp.Property,
            _ => fp,
        };

        return op is AssociationProperty ap ? GetEnumType(ap.Association.Name, ap.Property.Name, isPrimaryKeyDef)
            : op is RegularProperty rp ? GetEnumType(rp.Class?.Name ?? string.Empty, rp.Name, isPrimaryKeyDef)
            : string.Empty;
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
    /// <param name="useClassForAssociation">Utilise le type de la classe pour une association.</param>
    /// <returns>Le type.</returns>
    public virtual string GetType(IProperty property, bool useClassForAssociation = false)
    {
        return GetTypeCore(property, useClassForAssociation, isAlias: false, domainOverride: null);
    }

    /// <summary>
    /// Récupère la valeur d'une propriété.
    /// </summary>
    /// <param name="property">La propriété.</param>
    /// <param name="value">Valeur à utiliser, si non renseigné utilise la valeur par défaut de la propriété.</param>
    /// <returns>La valeur.</returns>
    public virtual string GetValue(IProperty property, string? value = null)
    {
        if (!IgnoreDefaultValues && property is not IProperty { Composition: not null })
        {
            value ??= property?.DefaultValue;
        }

        if (property == null || value == null || value == "null" || value == "undefined")
        {
            return NullValue;
        }

        var template = GetImplementation(property.Domain)?.GetValueTemplate(value);
        if (template != null)
        {
            return template.Value.Replace("{value}", value).ParseTemplate(property, this);
        }

        var prop = property is AliasProperty alp ? alp.Property : property;
        var ap = prop as AssociationProperty;

        var classe = ap != null ? ap.Association : prop.Class;
        var targetProp = ap != null ? ap.Property : prop;

        if (UseNamedEnums && classe != null && classe.Enum && AvailableClasses.Contains(classe))
        {
            if (CanClassUseEnums(classe, targetProp))
            {
                return $"{GetEnumType(classe.NamePascal, targetProp.NamePascal).TrimEnd('?')}.{value}";
            }
            else if (classe.EnumKey == targetProp)
            {
                var refName = classe.Values.SingleOrDefault(rv => rv.Value[targetProp] == value)?.Name;
                if (refName != null)
                {
                    return GetConstEnumName(classe.Name, refName);
                }
            }
        }

        if (ShouldQuoteValue(property))
        {
            return QuoteValue(value);
        }

        return value;
    }

    public virtual IEnumerable<string> GetValueImports(IProperty property, string? value = null)
    {
        if (!IgnoreDefaultValues && property is not IProperty { Composition: not null })
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

    protected virtual string GetConstEnumName(string className, string refName)
    {
        return $"{className.ToPascalCase(strictIfUppercase: true)}.{refName.ToPascalCase(strictIfUppercase: true)}";
    }

    protected abstract string GetEnumType(string className, string propName, bool isPrimaryKeyDef = false);

    protected virtual string GetTypeCore(
        IProperty property,
        bool useClassForAssociation,
        bool isAlias,
        Domain? domainOverride
    )
    {
        string GetTransformed(string type)
        {
            var domain = GetImplementation(domainOverride ?? property.Domain);
            return (domain?.GenericType?.Replace("{T}", type) ?? domain?.Type ?? string.Empty).ParseTemplate(
                property,
                this
            );
        }

        string HandleEnum(IProperty op)
        {
            string GetEnum(string className, string propName, bool isPrimaryKeyDef = false)
            {
                var op = property switch
                {
                    AssociationProperty ap => ap.Property,
                    _ => property,
                };
                return (GetImplementation(op.Domain)?.GenericType ?? "{T}")
                    .Replace("{T}", GetEnumType(className, propName, isPrimaryKeyDef))
                    .ParseTemplate(op, this);
            }

            var type =
                op is AssociationProperty ap ? GetEnum(ap.Association.Name, ap.Property.Name)
                : op is RegularProperty rp ? GetEnum(rp.Class.Name, rp.Name, !isAlias)
                : throw new InvalidOperationException();

            if (
                (domainOverride ?? property.Domain) != (op is AssociationProperty ap2 ? ap2.Property.Domain : op.Domain)
            )
            {
                return GetTransformed(type);
            }
            else
            {
                return type;
            }
        }

        return property switch
        {
            AssociationProperty ap when useClassForAssociation => ap.Property.Domain != ap.Domain
                ? GetTransformed(ap.Association.NamePascal)
                : ap.Association.NamePascal,

            AssociationProperty ap when CanClassUseEnums(ap.Association, ap.Property) => HandleEnum(ap),
            AssociationProperty ap when ap.Property.Domain != ap.Domain => GetTransformed(
                GetImplementation(domainOverride ?? ap.Property.Domain)?.Type ?? string.Empty
            ),
            RegularProperty { Class: not null } rp when CanClassUseEnums(rp.Class, rp) => HandleEnum(rp),
            IProperty { Composition: not null } when (domainOverride ?? property.Domain) is not null => (
                GetImplementation(domainOverride ?? property.Domain)?.GenericType ?? "{T}"
            )
                .Replace("{T}", "{composition.name}")
                .ParseTemplate(property, this),
            IProperty { Composition: Class c } => c.NamePascal,
            AliasProperty { As: not null } alp
                when domainOverride is null && GetImplementation(alp.Domain)?.GenericType != null => GetImplementation(
                alp.Domain
            )!
                .GenericType!.Replace(
                    "{T}",
                    GetTypeCore(alp.OriginalProperty!, useClassForAssociation, isAlias: true, domainOverride: null)
                ),
            AliasProperty { As: null } alp => GetTypeCore(
                alp.OriginalProperty!,
                useClassForAssociation,
                isAlias: true,
                domainOverride ?? alp.DomainOverride
            ),
            IProperty => (GetImplementation(domainOverride ?? property.Domain)?.Type ?? string.Empty).ParseTemplate(
                property,
                this
            ),
            _ => string.Empty,
        };
    }

    protected virtual bool IsEnumNameValid(string name)
    {
        return !Regex.IsMatch(name ?? string.Empty, "^\\d");
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
                        Target.Property => container is IProperty,
                        Target.AssociationProperty => container
                            is AssociationProperty
                                or AliasProperty { Property: AssociationProperty },
                        Target.CompositionProperty => container is IProperty { Composition: not null },
                        Target.RegularProperty => container
                            is RegularProperty
                                or AliasProperty { Property: RegularProperty },
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
