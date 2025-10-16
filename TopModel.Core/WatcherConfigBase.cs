using System.Text.RegularExpressions;
using Spectre.Console;
using TopModel.Core.FileModel;
using TopModel.Core.Loaders.YamlUtils;
using TopModel.Core.Model;
using TopModel.Core.Model.Implementation;
using TopModel.Core.Utils;
using TopModel.Utils;
using YamlDotNet.Serialization;

namespace TopModel.Core;

public class WatcherConfigBase
{
    private HashSet<Class>? _classes = null;
    private HashSet<Endpoint>? _endpoints = null;

    /// <summary>
    /// Nom de la configuration.
    /// </summary>
    public virtual string? Name { get; set; }

    /// <summary>
    /// Tags du module.
    /// </summary>
    public virtual required IList<string> Tags { get; set; }

    /// <summary>
    /// Tags pour lesquels il ne faut pas générer les fichiers (surchage en CLI).
    /// </summary>
    public virtual IList<string> ExcludedTags { get; set; } = [];

    /// <summary>
    /// Tags d'autres configs dont les classes peuvent être référencées dans le code généré par les générateurs de ce module.
    ///
    /// Les valeurs sont les noms des configurations cibles pour chaque tag.
    /// </summary>
    public virtual IDictionary<string, string> ReferencedTags { get; set; } = new Dictionary<string, string>();

    /// <summary>
    /// Tags d'autres configs dont les classes peuvent être référencées dans le code généré par les générateurs de ce module.
    ///
    /// Les valeurs sont les configurations cibles pour chaque tag.
    /// </summary>
    public virtual IDictionary<string, WatcherConfigBase> ReferencedTagConfigs { get; } =
        new Dictionary<string, WatcherConfigBase>();

    /// <summary>
    /// Langages du module, utilisé en cascade pour choisir l'implémentation correspondante des domaines, décorateurs et convertisseurs.
    /// </summary>
    [YamlConverter(typeof(StringListTypeConverter))]
    public virtual IList<string> Language { get; set; } = [];

    /// <summary>
    /// Langage par défaut.
    /// </summary>
    public virtual string? DefaultLanguage { get; }

    /// <summary>
    /// Variables globales du module.
    /// </summary>
    public virtual IDictionary<string, string> Variables { get; set; } = new Dictionary<string, string>();

    /// <summary>
    /// Variables par tag du module.
    /// </summary>
    public virtual IDictionary<string, IDictionary<string, string>> TagVariables { get; set; } =
        new Dictionary<string, IDictionary<string, string>>();

    /// <summary>
    /// Définition du module racine, pour les différents regroupements à faire dessus (fichiers de traductions...).
    /// </summary>
    public virtual string RootModule { get; set; } = "{module:head}";

    /// <summary>
    /// Noms de toutes les variables par tag du module.
    /// </summary>
    public virtual IEnumerable<string> TagVariableNames => TagVariables.Values.SelectMany(v => v.Keys).Distinct();

    /// <summary>
    /// Noms de toutes les variables  globales du module.
    /// </summary>
    public virtual IEnumerable<string> GlobalVariableNames =>
        Variables.Select(v => v.Key).Except(TagVariableNames).Distinct();

    /// <summary>
    /// Propriétés qui supportent la variable "module".
    /// </summary>
    public virtual string[] PropertiesWithModuleVariableSupport => [];

    /// <summary>
    /// Propriétés qui supportent la variable "module".
    /// </summary>
    public virtual string[] PropertiesWithFileNameVariableSupport => [];

    /// <summary>
    /// Propriétés qui supportent la variable "lang".
    /// </summary>
    public virtual string[] PropertiesWithLangVariableSupport => [];

    /// <summary>
    /// Propriétés qui peuvent contenir des templates à ne pas interprêter dans la résolution des variables globales ou par tag.
    /// </summary>
    public virtual IDictionary<string, List<string>> TemplateAttributes => new Dictionary<string, List<string>>();

    /// <summary>
    /// Propriétés qui supportent les variables par tag de la configuration courante.
    /// </summary>
    public virtual string[] PropertiesWithTagVariableSupport => [];

    public virtual IDictionary<string, ModelFile> Files { get; } = new Dictionary<string, ModelFile>();

    /// <summary>
    /// Classes concernées par cette configuration.
    /// </summary>
    public virtual IEnumerable<Class> Classes
    {
        get
        {
            if (_classes == null)
            {
                _classes = Files
                    .SelectMany(f =>
                        f.Value.Classes.Where(c => Tags.Intersect(c.Tags).Any()).Concat(GetExtraClasses(f.Value))
                    )
                    .Distinct()
                    .ToHashSet();
            }

            return _classes;
        }
    }

    /// <summary>
    /// Classes disponibles pour cette configuration. Peut contenir des classes d'autres configurations.
    /// </summary>
    public virtual IEnumerable<Class> AvailableClasses =>
        Classes
            .Concat(ReferencedTagConfigs.SelectMany(rtc => rtc.Value.Classes.Where(c => c.Tags.Contains(rtc.Key))))
            .Distinct();

    /// <summary>
    /// Endpoints concernés par cette configuration.
    /// </summary>
    public virtual IEnumerable<Endpoint> Endpoints
    {
        get
        {
            if (_endpoints == null)
            {
                _endpoints = Files
                    .SelectMany(f => f.Value.Endpoints.Where(c => Tags.Intersect(c.Tags).Any()))
                    .Distinct()
                    .ToHashSet();
            }

            return _endpoints;
        }
    }

    protected virtual bool PersistentOnly => false;

    public virtual IEnumerable<Class> GetExtraClasses(ModelFile file)
    {
        return [];
    }

    /// <summary>
    /// Récupère les implémentations de l'annotation pour la config.
    /// </summary>
    /// <param name="annotation">Annotation.</param>
    /// <returns>Implémentations.</returns>
    public virtual IList<AnnotationImplementation> GetImplementation(Annotation? annotation)
    {
        return GetImplementation(annotation?.Implementations) ?? [];
    }

    /// <summary>
    /// Récupère l'implémentation du domaine pour la config.
    /// </summary>
    /// <param name="domain">Décorateur.</param>
    /// <returns>Implémentation.</returns>
    public virtual DomainImplementation? GetImplementation(Domain? domain)
    {
        return GetImplementation(domain?.Implementations);
    }

    /// <summary>
    /// Récupère l'implémentation du décorateur pour la config.
    /// </summary>
    /// <param name="decorator">Décorateur.</param>
    /// <returns>Implémentation.</returns>
    public virtual DecoratorImplementation? GetImplementation(Decorator? decorator)
    {
        return GetImplementation(decorator?.Implementations);
    }

    /// <summary>
    /// Récupère l'implémentation du convertisseur pour la config.
    /// </summary>
    /// <param name="converter">Convertisseur.</param>
    /// <returns>Implémentation.</returns>
    public virtual ConverterImplementation? GetImplementation(Converter? converter)
    {
        return GetImplementation(converter?.Implementations);
    }

    /// <summary>
    /// Initialise les variables globales, et par tag manquantes.
    /// </summary>
    /// <param name="app">Valeur de la variable 'app'.</param>
    /// <param name="number">Numéro du générateur.</param>
    public virtual void InitVariables(string app, int number)
    {
        if (Language.Count == 0 && DefaultLanguage != null)
        {
            Language.Add(DefaultLanguage);
        }

        if (!Variables.ContainsKey("app"))
        {
            Variables["app"] = app;
        }

        // Si on a défini au moins une variable par tag, alors on s'assure qu'elle est définie pour tous les tags (et on y met "" si ce n'est pas une variable globale).
        if (TagVariableNames.Any())
        {
            foreach (var tag in Tags.Where(tag => !TagVariables.ContainsKey(tag)))
            {
                TagVariables[tag] = new Dictionary<string, string>();
            }

            foreach (var variables in TagVariables.Values)
            {
                foreach (var varName in TagVariableNames.Where(varName => !variables.ContainsKey(varName)))
                {
                    Variables.TryGetValue(varName, out var globalVariable);
                    variables[varName] = globalVariable ?? string.Empty;
                }
            }
        }

        var hasMissingVar = false;
        foreach (var property in GetType().GetProperties().Where(p => p.PropertyType == typeof(string) && p.CanWrite))
        {
            var value = (string?)property.GetValue(this);
            if (value != null)
            {
                value = ResolveGlobalVariables(value);
                property.SetValue(this, value);

                foreach (var match in Regex.Matches(value, @"\{([$a-zA-Z0-9_-]+)(:\w+)?\}").Cast<Match>())
                {
                    var varName = match.Groups[1].Value;
                    if (TemplateAttributes.TryGetValue(property.Name, out var ta) && ta.Contains(varName))
                    {
                        continue;
                    }

                    if (varName == "module" || varName == "lang" || varName == "fileName")
                    {
                        var supportedProperties = varName switch
                        {
                            "module" => [.. PropertiesWithModuleVariableSupport, nameof(RootModule)],
                            "lang" => PropertiesWithLangVariableSupport,
                            "fileName" => PropertiesWithFileNameVariableSupport,
                            _ => null!,
                        };

                        if (!supportedProperties.Contains(property.Name))
                        {
                            hasMissingVar = true;
                            AnsiConsole.MarkupLine(
                                $"[yellow]{Emoji.Known.Warning} {{{GetType().Name}[[{number}]].{property.Name}}} - La variable '{{{varName}}}' n'est pas supportée par cette propriété.[/]"
                            );
                        }

                        continue;
                    }

                    var hasTagSupport = PropertiesWithTagVariableSupport.Contains(property.Name);

                    if (!hasTagSupport)
                    {
                        hasMissingVar = true;
                        AnsiConsole.MarkupLine(
                            $"[yellow]{Emoji.Known.Warning} {{{GetType().Name}[[{number}]].{property.Name}}} - La variable globale '{{{varName}}}' n'est pas définie pour ce générateur.[/]"
                        );
                    }
                    else if (!TagVariableNames.Contains(varName))
                    {
                        hasMissingVar = true;
                        AnsiConsole.MarkupLine(
                            $"[yellow]{Emoji.Known.Warning}  {{{GetType().Name}[[{number}]].{property.Name}}} - La variable '{{{varName}}}' n'est pas définie pour ce générateur.[/]"
                        );
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
            AnsiConsole.WriteLine();
        }
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

    internal IEnumerable<ModelError> CheckDomainImplementations(IEnumerable<ModelFile> files)
    {
        var handledFiles = files.Where(file => Tags.Intersect(file.AllTags.Except(ExcludedTags)).Any());

        if (Language.Count > 0)
        {
            foreach (
                var domain in handledFiles
                    .SelectMany(f => f.Properties)
                    .Where(fp => !PersistentOnly || (fp.Class?.IsPersistent ?? false))
                    .Select(fp => fp.Domain)
                    .Concat(
                        PersistentOnly
                            ? []
                            : handledFiles
                                .SelectMany(f => f.Properties)
                                .OfType<CompositionProperty>()
                                .Select(fp => fp.Domain!)
                    )
                    .Where(domain => domain != null && GetImplementation(domain) == null)
                    .Distinct()
            )
            {
                yield return new ModelError(
                    ErrorType.TMD6003,
                    domain,
                    $"La configuration '{Name}' requiert que le domaine '{domain}' ait une implémentation pour l'un des languages suivants : {string.Join(", ", Language.Select(l => $"'{l}'"))}."
                );
            }
        }
    }

    internal void OnFileChanged()
    {
        _classes = null;
        _endpoints = null;
    }

    internal string ResolveGlobalVariables(string input)
    {
        foreach (var varName in GlobalVariableNames)
        {
            input = ReplaceVariable(input, varName, Variables[varName]);
        }

        return input;
    }

    /// <summary>
    /// Résout les variables de tag dans une chaîne de caractère.
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
        string MatchEvaluator(Match m) => varValue.Transform(m.Value.Trim('{', '}'));
        return Regex.Replace(value, $"\\{{{varName}(:\\w+)*\\}}", MatchEvaluator);
    }

    /// <summary>
    /// Pour un dictionnaire d'implémentations, retourne la première valeur qui match avec un langages
    /// </summary>
    /// <typeparam name="T">Type d'implémentation</typeparam>
    /// <param name="implementations">Dictionnaire de toutes les implémentations</param>
    /// <returns>L'implémentation sélectionnée si elle existe</returns>
    private T? GetImplementation<T>(IDictionary<string, T>? implementations)
    {
        foreach (var language in Language)
        {
            if (implementations?.ContainsKey(language) ?? false)
            {
                return implementations[language];
            }
        }

        return default;
    }
}
