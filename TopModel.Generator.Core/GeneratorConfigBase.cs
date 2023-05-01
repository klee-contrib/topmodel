using System.Text.RegularExpressions;
using TopModel.Core;
using TopModel.Core.Model.Implementation;
using TopModel.Utils;

namespace TopModel.Generator.Core;

public abstract class GeneratorConfigBase
{
#nullable disable

    /// <summary>
    /// Racine du répertoire de génération.
    /// </summary>
    public string OutputDirectory { get; set; }

    /// <summary>
    /// Tags du générateur.
    /// </summary>
    public IList<string> Tags { get; set; }

    /// <summary>
    /// Langage du générateur, utilisé pour choisir l'implémentation correspondante des domaines, décorateurs et convertisseurs.
    /// </summary>
    public string Language { get; set; }
#nullable enable

    /// <summary>
    /// Générateurs désactivés.
    /// </summary>
    public IList<string>? Disable { get; set; }

    /// <summary>
    /// Variables globales du générateur.
    /// </summary>
    public Dictionary<string, string> Variables { get; set; } = new();

    /// <summary>
    /// Variables par tag du générateur.
    /// </summary>
    public Dictionary<string, Dictionary<string, string>> TagVariables { get; set; } = new();

    public IEnumerable<string> TagVariableNames => TagVariables.Values.SelectMany(v => v.Keys).Distinct();

    public IEnumerable<string> GlobalVariableNames => Variables.Select(v => v.Key).Except(TagVariableNames).Distinct();

    /// <summary>
    /// Propriétés qui supportent la variable "module".
    /// </summary>
    public virtual string[] PropertiesWithModuleVariableSupport => Array.Empty<string>();

    /// <summary>
    /// Propriétés qui supportent la variable "lang".
    /// </summary>
    public virtual string[] PropertiesWithLangVariableSupport => Array.Empty<string>();

    /// <summary>
    /// Propriétés qui supportent les variables par tag de la configuration courante.
    /// </summary>
    public virtual string[] PropertiesWithTagVariableSupport => Array.Empty<string>();

    protected virtual bool UseNamedEnums => true;

    protected virtual string NullValue => "null";

    /// <summary>
    /// Détermine si une classe peut utiliser une enum pour sa clé primaire.
    /// </summary>
    /// <param name="classe">Classe.</param>
    /// <param name="availableClasses">Classes disponibles.</param>
    /// <param name="prop">Propriété à vérifier (si c'est pas la clé primaire).</param>
    /// <returns>Oui/non.</returns>
    public virtual bool CanClassUseEnums(Class classe, IEnumerable<Class>? availableClasses = null, IFieldProperty? prop = null)
    {
        if (availableClasses != null && !availableClasses.Contains(classe))
        {
            return false;
        }

        prop ??= classe.EnumKey;

        bool CheckProperty(IFieldProperty fp)
        {
            return (fp == classe.EnumKey || classe.UniqueKeys.Where(uk => uk.Count == 1).Select(uk => uk.Single()).Contains(prop))
                && classe.Values.All(r => r.Value.ContainsKey(fp) && IsEnumNameValid(r.Value[fp].ToString()));
        }

        return classe.Enum && CheckProperty(prop!);
    }

    public string GetConvertedValue(string value, Domain? fromDomain, Domain? toDomain)
    {
        if (fromDomain != null && toDomain != null && fromDomain != toDomain)
        {
            var converter = fromDomain.ConvertersFrom.FirstOrDefault(c => c.From.Contains(fromDomain) && c.To.Contains(toDomain));
            var text = GetImplementation(converter)?.Text;
            if (text != null)
            {
                value = GetImplementation(converter)!.Text
                    .Replace("{value}", value)
                    .ParseTemplate(fromDomain, toDomain, Language);
            }
        }

        return value;
    }

    /// <summary>
    /// Récupère la valeur par défaut d'une propriété.
    /// </summary>
    /// <param name="property">La propriété.</param>
    /// <param name="availableClasses">Classes disponibles dans le générateur.</param>
    /// <returns>La valeur par défaut.</returns>
    public string GetDefaultValue(IProperty property, IEnumerable<Class> availableClasses)
    {
        var fp = property as IFieldProperty;

        if (fp?.DefaultValue == null || fp.DefaultValue == "null" || fp.DefaultValue == "undefined")
        {
            return NullValue;
        }

        var prop = fp is AliasProperty alp ? alp.Property : fp;
        var ap = prop as AssociationProperty;

        var classe = ap != null ? ap.Association : prop.Class;
        var targetProp = ap != null ? ap.Property : prop;

        if (UseNamedEnums && classe.Enum && availableClasses.Contains(classe))
        {
            if (CanClassUseEnums(classe, availableClasses, targetProp))
            {
                return $"{GetEnumType(classe.NamePascal, targetProp.NamePascal).TrimEnd('?')}.{fp.DefaultValue}";
            }
            else
            {
                var refName = classe.Values.SingleOrDefault(rv => rv.Value[targetProp] == fp.DefaultValue)?.Name;
                if (refName != null)
                {
                    return GetConstEnumName(classe.Name, refName);
                }
            }
        }

        if (GetImplementation(fp.Domain)!.Type.ToLower() == "string")
        {
            return $@"""{fp.DefaultValue}""";
        }

        return fp.DefaultValue;
    }

    public IEnumerable<string> GetDomainAnnotations(IProperty property, string tag)
    {
        if (property is IFieldProperty fp)
        {
            foreach (var annotation in GetImplementation(fp.Domain)!.Annotations
                .Where(a => FilterAnnotations(a, fp, tag))
                .Select(a => a.Text.ParseTemplate(fp)))
            {
                yield return annotation;
            }
        }
        else if (property is CompositionProperty { DomainKind: not null } cp)
        {
            foreach (var annotation in GetImplementation(cp.DomainKind)!.Annotations
                .Where(a => FilterAnnotations(a, cp, tag))
                .Select(a => a.Text.ParseTemplate(cp)))
            {
                yield return annotation;
            }
        }
    }

    public IEnumerable<string> GetDomainImports(IProperty property, string tag, bool noAnnotations = false)
    {
        if (property is IFieldProperty fp)
        {
            foreach (var import in GetImplementation(fp.Domain)!.Imports.Select(u => u.ParseTemplate(fp)))
            {
                yield return import;
            }

            if (!noAnnotations)
            {
                foreach (var import in GetImplementation(fp.Domain)!.Annotations
                .Where(a => FilterAnnotations(a, fp, tag))
                .SelectMany(a => a.Imports)
                    .Select(u => u.ParseTemplate(fp)))
                {
                    yield return import;
                }
            }
        }
        else if (property is CompositionProperty { DomainKind: not null } cp)
        {
            foreach (var import in GetImplementation(cp.DomainKind)!.Imports.Select(u => u.ParseTemplate(cp)))
            {
                yield return import;
            }

            if (!noAnnotations)
            {
                foreach (var import in GetImplementation(cp.DomainKind)!.Annotations
                    .Where(a => FilterAnnotations(a, cp, tag))
                    .SelectMany(a => a.Imports)
                    .Select(u => u.ParseTemplate(cp)))
                {
                    yield return import;
                }
            }
        }
    }

    /// <summary>
    /// Récupère l'implémentation du domaine pour la config.
    /// </summary>
    /// <param name="domain">Décorateur.</param>
    /// <returns>Implémentation.</returns>
    public DomainImplementation? GetImplementation(Domain? domain)
    {
        return domain?.Implementations.GetValueOrDefault(Language);
    }

    /// <summary>
    /// Récupère l'implémentation du décorateur pour la config.
    /// </summary>
    /// <param name="decorator">Décorateur.</param>
    /// <returns>Implémentation.</returns>
    public DecoratorImplementation? GetImplementation(Decorator? decorator)
    {
        return decorator?.Implementations.GetValueOrDefault(Language);
    }

    /// <summary>
    /// Récupère l'implémentation du convertisseur pour la config.
    /// </summary>
    /// <param name="converter">Convertisseur.</param>
    /// <returns>Implémentation.</returns>
    public ConverterImplementation? GetImplementation(Converter? converter)
    {
        return converter?.Implementations.GetValueOrDefault(Language);
    }

    /// <summary>
    /// Récupère le type d'une propriété.
    /// </summary>
    /// <param name="property">Domaine.</param>
    /// <param name="availableClasses">Classes disponibles.</param>
    /// <param name="useClassForAssociation">Utilise le type de la classe pour une association.</param>
    /// <param name="useIterable">Pour si c'est une composition liste, utiliser le type itérable au lieu de collection.</param>
    /// <returns>Le type.</returns>
    public string GetType(IProperty property, IEnumerable<Class>? availableClasses = null, bool useClassForAssociation = false, bool useIterable = true)
    {
        return property switch
        {
            AssociationProperty { Association: Class assoc } ap when useClassForAssociation => ap.Type.IsToMany() ? GetListType(assoc.NamePascal) : ap.Association.NamePascal,
            AliasProperty { Property: AssociationProperty { Association: Class assoc } ap, AsList: var asList } when useClassForAssociation => asList || ap.Type.IsToMany() ? GetListType(assoc.NamePascal) : ap.Association.NamePascal,
            AssociationProperty { Association: Class assoc } ap when CanClassUseEnums(assoc, availableClasses, ap.Property) => GetEnumType(assoc.Name, ap.Property.Name, ap.Type.IsToMany()),
            AliasProperty { Property: AssociationProperty { Association: Class assoc } ap, AsList: var asList } when CanClassUseEnums(assoc, availableClasses) => GetEnumType(assoc.Name, ap.Property.Name, asList || ap.Type.IsToMany()),
            RegularProperty { Class: Class classe } rp when CanClassUseEnums(classe, availableClasses, rp) => GetEnumType(rp.Class.Name, rp.Name, false, true),
            AliasProperty { Property: RegularProperty { Class: Class alClass } rp, AsList: var asList } when CanClassUseEnums(alClass, availableClasses, rp) => GetEnumType(alClass.Name, rp.Name, asList),
            IFieldProperty fp => GetImplementation(fp.Domain)!.Type.ParseTemplate(fp),
            CompositionProperty { Kind: "object" } cp => cp.Composition.NamePascal,
            CompositionProperty { Kind: "list" } cp => GetListType(cp.Composition.NamePascal, useIterable),
            CompositionProperty { DomainKind: Domain domain } cp => GetImplementation(domain)!.Type switch
            {
                string s when s.Contains("{composition.name}") => s.ParseTemplate(cp),
                string s => $"{s}<{{composition.name}}>".ParseTemplate(cp)
            },
            _ => string.Empty
        };
    }

    /// <summary>
    /// Initialise les variables globales, et par tag manquantes.
    /// </summary>
    /// <param name="app">Valeur de la variable 'app'.</param>
    /// <param name="number">Numéro du générateur.</param>
    public void InitVariables(string app, int number)
    {
        if (!Variables.ContainsKey("app"))
        {
            Variables["app"] = app;
        }

        // Si on a défini au moins une variable par tag, alors on s'assure qu'elle est définie pour tous les tags (et on y met "" si ce n'est pas une variable globale).
        if (TagVariableNames.Any())
        {
            foreach (var tag in Tags)
            {
                if (!TagVariables.ContainsKey(tag))
                {
                    TagVariables[tag] = new();
                }
            }

            foreach (var variables in TagVariables.Values)
            {
                foreach (var varName in TagVariableNames)
                {
                    if (!variables.ContainsKey(varName))
                    {
                        Variables.TryGetValue(varName, out var globalVariable);
                        variables[varName] = globalVariable ?? string.Empty;
                    }
                }
            }
        }

        var hasMissingVar = false;

        foreach (var property in GetType().GetProperties().Where(p => p.PropertyType == typeof(string)))
        {
            var value = (string?)property.GetValue(this);
            if (value != null)
            {
                foreach (var varName in GlobalVariableNames)
                {
                    value = ReplaceVariable(value, varName, Variables[varName]);
                }

                property.SetValue(this, value);

                foreach (var match in Regex.Matches(value, @"\{([$a-zA-Z0-9_-]+)(:\w+)?\}").Cast<Match>())
                {
                    var varName = match.Groups[1].Value;
                    if (varName == "module" || varName == "lang")
                    {
                        var supportedProperties = varName switch
                        {
                            "module" => PropertiesWithModuleVariableSupport,
                            "lang" => PropertiesWithLangVariableSupport,
                            _ => null!
                        };

                        if (!supportedProperties.Contains(property.Name))
                        {
                            hasMissingVar = true;
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"/!\\ {{{GetType().Name}[{number}].{property.Name}}} - La variable '{{{varName}}}' n'est pas supportée par cette propriété.");
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }

                        continue;
                    }

                    var hasTagSupport = PropertiesWithTagVariableSupport.Contains(property.Name);

                    if (!hasTagSupport)
                    {
                        hasMissingVar = true;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"/!\\ {{{GetType().Name}[{number}].{property.Name}}} - La variable globale '{{{varName}}}' n'est pas définie pour ce générateur.");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    else if (!TagVariableNames.Contains(varName))
                    {
                        hasMissingVar = true;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"/!\\ {{{GetType().Name}[{number}].{property.Name}}} - La variable '{{{varName}}}' n'est pas définie pour ce générateur.");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }
            }
        }

        foreach (var tagVariables in TagVariables.Values)
        {
            foreach (var tagVarName in tagVariables.Keys)
            {
                foreach (var varName in GlobalVariableNames)
                {
                    tagVariables[tagVarName] = ReplaceVariable(tagVariables[tagVarName], varName, Variables[varName]);
                }
            }
        }

        if (hasMissingVar)
        {
            Console.WriteLine();
        }
    }

    public virtual bool IsPersistent(Class classe, string tag)
    {
        return classe.IsPersistent;
    }

    /// <summary>
    /// Résout toutes les variables pour une valeur donnée.
    /// </summary>
    /// <param name="value">Valeur.</param>
    /// <param name="tag">Tag.</param>
    /// <param name="module">Module.</param>
    /// <param name="lang">Lang.</param>
    /// <returns>La valeur avec les variables résolues.</returns>
    public virtual string ResolveVariables(string value, string? tag = null, string? module = null, string? lang = null)
    {
        var result = value;

        if (tag != null)
        {
            result = ResolveTagVariables(result, tag);
        }

        if (module != null)
        {
            result = ReplaceVariable(result, "module", module);
        }

        if (lang != null)
        {
            result = ReplaceVariable(result, "lang", lang);
        }

        return result;
    }

    protected virtual string GetConstEnumName(string className, string refName)
    {
        return $"{className.ToPascalCase()}.{refName}";
    }

    protected abstract string GetEnumType(string className, string propName, bool asList = false, bool isPrimaryKeyDef = false);

    protected abstract string GetListType(string name, bool useIterable = true);

    protected virtual bool IsEnumNameValid(string name)
    {
        return !Regex.IsMatch(name ?? string.Empty, "^\\d");
    }

    /// <summary>
    /// Résout les variables de tag dans un chaîne de caractère.
    /// </summary>
    /// <param name="value">Chaîne de caractères.</param>
    /// <param name="tag">Nom du tag.</param>
    /// <returns>Value avec les variables remplacées..</returns>
    protected virtual string ResolveTagVariables(string value, string tag)
    {
        if (TagVariables.TryGetValue(tag, out var tagVariables))
        {
            foreach (var (varName, varValue) in tagVariables)
            {
                value = ReplaceVariable(value, varName, varValue);
            }
        }

        return value;
    }

    private static string ReplaceVariable(string value, string varName, string varValue)
    {
        return Regex.Replace(value, $"\\{{{varName}(:\\w+)?\\}}", m => m.Value.Trim('{', '}').GetTransformation()(varValue));
    }

    private bool FilterAnnotations(TargetedText annotation, IProperty property, string tag)
    {
        return property.Class != null && !property.Class.Abstract && (
            (annotation.Target & Target.Dto) > 0 && !IsPersistent(property.Class, tag)
            || (annotation.Target & Target.Persisted) > 0 && IsPersistent(property.Class, tag))
        || (annotation.Target & Target.Api) > 0 && property.Endpoint != null;
    }
}