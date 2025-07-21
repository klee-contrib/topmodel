using System.Text.RegularExpressions;
using Spectre.Console;
using TopModel.Core;
using TopModel.Core.Model.Implementation;
using TopModel.Core.Templating;
using TopModel.Utils;

namespace TopModel.Generator.Core;

public abstract class GeneratorConfigBase : WatcherConfigBase
{
    /// <summary>
    /// Racine du répertoire de génération.
    /// </summary>
    public required string OutputDirectory { get; set; }

    /// <summary>
    /// Tags pour lesquels il ne faut pas générer les fichiers (surchage en CLI).
    /// </summary>
    public IList<string> ExcludedTags { get; set; } = [];

    /// <summary>
    /// Désactive la génération des valeurs par défaut des propriétés dans les classes et endpoints générés avec cette configuration.
    /// </summary>
    public virtual bool IgnoreDefaultValues { get; set; }

    /// <summary>
    /// Si les libellés des listes de références doivent être traduits.
    /// </summary>
    public bool? TranslateReferences { get; set; }

    /// <summary>
    /// Si les libellés des propriétés doivent être traduits.
    /// </summary>
    public bool? TranslateProperties { get; set; }

    /// <summary>
    /// Générateurs désactivés.
    /// </summary>
    public IList<string>? Disable { get; set; }

    protected virtual bool UseNamedEnums => true;

    protected virtual string NullValue => "null";

    /// <summary>
    /// Détermine si une classe peut utiliser une enum pour sa clé primaire.
    /// </summary>
    /// <param name="classe">Classe.</param>
    /// <param name="availableClasses">Classes disponibles.</param>
    /// <param name="prop">Propriété à vérifier (si c'est pas la clé primaire).</param>
    /// <returns>Oui/non.</returns>
    public virtual bool CanClassUseEnums(Class classe, IEnumerable<Class>? availableClasses = null, IProperty? prop = null)
    {
        if (availableClasses != null && !availableClasses.Contains(classe))
        {
            return false;
        }

        prop ??= classe.EnumKey;

        bool CheckProperty(IProperty fp)
        {
            return (fp == classe.EnumKey || classe.UniqueKeys.Where(uk => uk.Count == 1).Select(uk => uk.Single()).Contains(prop))
                && classe.Values.All(r => r.Value.ContainsKey(fp) && IsEnumNameValid(r.Value[fp].ToString()));
        }

        return classe.Enum && CheckProperty(prop!);
    }

    public IEnumerable<(string Annotation, List<string> Imports)> GetAnnotationsAndImports(Class classe, string tag)
    {
        return classe.Decorators
            .SelectMany(GetDecoratorImplementations)
            .SelectMany(d =>
            {
                var (implementation, decorator, parameters) = d;
                return implementation.Annotations.Select(a => (
                    Annotation: a.ParseTemplate(decorator, classe, parameters, this, tag),
                    Imports: implementation.Imports.Select(i => i.ParseTemplate(decorator, classe, parameters, this, tag)).ToList()));
            });
    }

    public string? GetClassExtends(Class classe)
    {
        var extendsDecorator = classe.Decorators.SelectMany(GetDecoratorImplementations).SingleOrDefault(d => d.Implementation.Extends != null);
        return classe.Extends?.NamePascal ?? GetImplementation(extendsDecorator.Decorator)?.Extends!.ParseTemplate(extendsDecorator.Decorator, classe, extendsDecorator.Parameters, this);
    }

    public IEnumerable<string> GetClassImplements(Class classe)
    {
        return classe.Decorators
            .SelectMany(GetDecoratorImplementations)
            .SelectMany(d => (d.Implementation.Implements ?? [])
                .Select(i => i.ParseTemplate(d.Decorator, classe, d.Parameters, this)))
            .Distinct();
    }

    public string GetConvertedValue(string value, Domain? fromDomain, Domain? toDomain)
    {
        var converter = GetConverter(fromDomain, toDomain);
        if (converter != null && fromDomain != null && toDomain != null)
        {
            var text = GetImplementation(converter)?.Text;
            if (text != null)
            {
                value = GetImplementation(converter)!.Text
                    .Replace("{value}", value)
                    .ParseTemplate(fromDomain, toDomain, this);
            }
        }

        return value;
    }

    public Converter? GetConverter(Domain? fromDomain, Domain? toDomain)
    {
        if (fromDomain != null && toDomain != null && fromDomain != toDomain)
        {
            return fromDomain.ConvertersFrom.FirstOrDefault(c => c.From.Contains(fromDomain) && c.To.Contains(toDomain));
        }

        return null;
    }

    public IEnumerable<string> GetConverterImports(Domain? fromDomain, Domain? toDomain)
    {
        if (fromDomain != null && toDomain != null && fromDomain != toDomain)
        {
            var converter = GetConverter(fromDomain, toDomain);
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

    public IEnumerable<string> GetDecoratorAnnotations(Class classe, string tag)
    {
        return classe.Decorators
            .SelectMany(GetDecoratorImplementations)
            .SelectMany(d => (d.Implementation.Annotations ?? [])
                .Select(a => a.ParseTemplate(d.Decorator, classe, d.Parameters, this, tag)))
            .Distinct();
    }

    public IEnumerable<string> GetDecoratorAnnotations(Endpoint endpoint, string tag)
    {
        return endpoint.Decorators
            .SelectMany(GetDecoratorImplementations)
            .SelectMany(d => (d.Implementation.Annotations ?? [])
                .Select(a => a.ParseTemplate(d.Decorator, endpoint, d.Parameters, this, tag)))
            .Distinct();
    }

    public IEnumerable<string> GetDecoratorImports(Class classe, string tag)
    {
        return classe.Decorators
            .SelectMany(GetDecoratorImplementations)
            .SelectMany(d => (d.Implementation.Imports ?? [])
                .Select(i => i.ParseTemplate(d.Decorator, classe, d.Parameters, this, tag)))
            .Distinct();
    }

    public IEnumerable<string> GetDecoratorImports(Endpoint endpoint, string tag)
    {
        return endpoint.Decorators
            .SelectMany(GetDecoratorImplementations)
            .SelectMany(d => (d.Implementation.Imports ?? [])
                .Select(i => i.ParseTemplate(d.Decorator, endpoint, d.Parameters, this, tag)))
            .Distinct();
    }

    public IEnumerable<(string Annotation, IEnumerable<string> Imports)> GetDomainAnnotations(IProperty property, string tag)
    {
        if (property.Domain is not null)
        {
            foreach (var annotation in GetImplementation(property.Domain)!.Annotations
               .Where(a => FilterAnnotations(a, property, tag)))
            {
                yield return (Annotation: annotation.Text.ParseTemplate(property, this, tag), Imports: annotation.Imports.Select(i => i.ParseTemplate(property, this, tag)));
            }
        }
    }

    public IEnumerable<string> GetDomainImports(IProperty property, string tag)
    {
        if (property.Domain != null)
        {
            foreach (var import in GetImplementation(property.Domain)!.Imports.Select(u => u.ParseTemplate(property, this, tag)))
            {
                yield return import;
            }

            foreach (var import in GetDomainAnnotations(property, tag).SelectMany(a => a.Imports))
            {
                yield return import;
            }
        }
    }

    public string GetEnumType(IProperty fp, bool isPrimaryKeyDef = false)
    {
        var op = fp switch
        {
            AssociationProperty a => a.Property,
            AliasProperty { Property: AssociationProperty a } => a.Property,
            AliasProperty alp => alp.Property,
            _ => fp
        };

        return op is AssociationProperty ap
            ? GetEnumType(ap.Association.Name, ap.Property.Name, isPrimaryKeyDef)
            : op is RegularProperty rp
            ? GetEnumType(rp.Class?.Name ?? string.Empty, rp.Name, isPrimaryKeyDef)
            : string.Empty;
    }

    /// <summary>
    /// Récupère le type d'une propriété.
    /// </summary>
    /// <param name="property">Domaine.</param>
    /// <param name="availableClasses">Classes disponibles.</param>
    /// <param name="useClassForAssociation">Utilise le type de la classe pour une association.</param>
    /// <returns>Le type.</returns>
    public string GetType(IProperty property, IEnumerable<Class>? availableClasses = null, bool useClassForAssociation = false)
    {
        string GetEnum(string className, string propName, bool isPrimaryKeyDef = false)
        {
            var op = property switch
            {
                AssociationProperty ap => ap.Property,
                AliasProperty { Property: AssociationProperty ap } => ap.Property,
                AliasProperty alp => alp.Property,
                _ => property
            };

            return (GetImplementation(op.Domain)?.GenericType ?? "{T}").Replace("{T}", GetEnumType(className, propName, isPrimaryKeyDef)).ParseTemplate(op, this);
        }

        string GetTransformed(string type)
        {
            var domain = GetImplementation(property.Domain);
            return (domain?.GenericType?.Replace("{T}", type) ?? domain?.Type ?? string.Empty).ParseTemplate(property, this);
        }

        string HandleAUC(AssociationProperty ap)
        {
            return ap.Property.Domain != ap.Domain
                ? GetTransformed(ap.Association.NamePascal)
                : ap.Association.NamePascal;
        }

        string HandleEnum(IProperty op)
        {
            var type = op is AssociationProperty ap
                ? GetEnum(ap.Association.Name, ap.Property.Name)
                : op is RegularProperty rp
                ? GetEnum(rp.Class.Name, rp.Name, rp == property)
                : throw new InvalidOperationException();

            if (property.Domain != (op is AssociationProperty ap2 ? ap2.Property.Domain : op.Domain))
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
            AssociationProperty ap when useClassForAssociation => HandleAUC(ap),
            AliasProperty { Property: AssociationProperty ap } when useClassForAssociation => HandleAUC(ap),
            AssociationProperty ap when CanClassUseEnums(ap.Association, availableClasses, ap.Property) => HandleEnum(ap),
            AliasProperty { Property: AssociationProperty ap } when CanClassUseEnums(ap.Association, availableClasses, ap.Property) => HandleEnum(ap),
            RegularProperty { Class: not null } rp when CanClassUseEnums(rp.Class, availableClasses, rp) => HandleEnum(rp),
            AliasProperty { Property: RegularProperty { Class: not null } rp } when CanClassUseEnums(rp.Class, availableClasses, rp) => HandleEnum(rp),
            AliasProperty { As: not null } alp when GetImplementation(alp.Domain)?.GenericType != null => GetImplementation(alp.Domain)!.GenericType!.Replace("{T}", GetType(alp.OriginalProperty!, availableClasses, useClassForAssociation)),
            CompositionProperty { Domain: not null } => (GetImplementation(property.Domain)?.GenericType ?? "{T}").Replace("{T}", "{composition.name}").ParseTemplate(property, this),
            AliasProperty { Property: CompositionProperty { Domain: not null } } => (GetImplementation(property.Domain)?.GenericType ?? "{T}").Replace("{T}", "{composition.name}").ParseTemplate(property, this),
            CompositionProperty cp => cp.Composition.NamePascal,
            AliasProperty { Property: CompositionProperty cp } => cp.Composition.NamePascal,
            IProperty => (GetImplementation(property.Domain)?.Type ?? string.Empty).ParseTemplate(property, this),
            _ => string.Empty
        };
    }

    /// <summary>
    /// Récupère la valeur d'une propriété.
    /// </summary>
    /// <param name="property">La propriété.</param>
    /// <param name="availableClasses">Classes disponibles dans le générateur.</param>
    /// <param name="value">Valeur à utiliser, si non renseigné utilise la valeur par défaut de la propriété.</param>
    /// <returns>La valeur.</returns>
    public virtual string GetValue(IProperty property, IEnumerable<Class> availableClasses, string? value = null)
    {
        if (!IgnoreDefaultValues && property is not CompositionProperty and not AliasProperty { Property: CompositionProperty })
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

        if (UseNamedEnums && classe != null && classe.Enum && availableClasses.Contains(classe))
        {
            if (CanClassUseEnums(classe, availableClasses, targetProp))
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

    public IEnumerable<string> GetValueImports(IProperty property, string? value = null)
    {
        if (!IgnoreDefaultValues && property is not CompositionProperty and not AliasProperty { Property: CompositionProperty })
        {
            value ??= property.DefaultValue;
        }

        var template = value != null ? GetImplementation(property.Domain)?.GetValueTemplate(value) : null;
        if (template != null)
        {
            return template.Imports.Select(i => i.ParseTemplate(property, this));
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

    protected virtual bool IsEnumNameValid(string name)
    {
        return !Regex.IsMatch(name ?? string.Empty, "^\\d");
    }

    protected virtual string QuoteValue(string value)
    {
        return $@"""{value}""";
    }

    private bool FilterAnnotations(TargetedText annotation, IProperty property, string tag)
    {
        return property.Class != null && !property.Class.Abstract && (
            (annotation.Target & Target.Dto) > 0 && !IsPersistent(property.Class, tag)
            || (annotation.Target & Target.Persisted) > 0 && IsPersistent(property.Class, tag))
        || (annotation.Target & Target.Api) > 0 && property.Endpoint != null;
    }

    private IEnumerable<(DecoratorImplementation Implementation, Decorator Decorator, string[] Parameters)> GetDecoratorImplementations((Decorator Decorator, string[] Parameters) d)
    {
        var implementation = GetImplementation(d.Decorator);
        if (implementation != null)
        {
            yield return (implementation, d.Decorator, d.Parameters);
        }

        foreach (var subD in d.Decorator.Decorators)
        {
            foreach (var subImplementation in GetDecoratorImplementations(subD))
            {
                yield return subImplementation;
            }
        }
    }
}