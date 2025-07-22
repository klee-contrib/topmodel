using System.Text.RegularExpressions;
using Spectre.Console;
using TopModel.Core.Loaders.YamlUtils;
using TopModel.Core.Model.Implementation;
using TopModel.Core.Templating;
using YamlDotNet.Serialization;

namespace TopModel.Core;

public class WatcherConfigBase
{
    /// <summary>
    /// Tags du module.
    /// </summary>
    public required IList<string> Tags { get; set; }

    /// <summary>
    /// Langages du module, utilisé en cascade pour choisir l'implémentation correspondante des domaines, décorateurs et convertisseurs.
    /// </summary>
    [YamlConverter(typeof(StringListTypeConverter))]
    public IList<string> Language { get; set; } = [];

    /// <summary>
    /// Setter pour le language par défaut.
    /// </summary>
    public string DefaultLanguage
    {
        set
        {
            if (Language.Count == 0)
            {
                Language.Add(value);
            }
        }
    }

    /// <summary>
    /// Variables globales du module.
    /// </summary>
    public Dictionary<string, string> Variables { get; set; } = [];

    /// <summary>
    /// Variables par tag du module.
    /// </summary>
    public Dictionary<string, Dictionary<string, string>> TagVariables { get; set; } = [];

    /// <summary>
    /// Noms de toutes les variables par tag du module.
    /// </summary>
    public IEnumerable<string> TagVariableNames => TagVariables.Values.SelectMany(v => v.Keys).Distinct();

    /// <summary>
    /// Noms de toutes les variables  globales du module.
    /// </summary>
    public IEnumerable<string> GlobalVariableNames => Variables.Select(v => v.Key).Except(TagVariableNames).Distinct();

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
    public virtual Dictionary<string, List<string>> TemplateAttributes => [];

    /// <summary>
    /// Propriétés qui supportent les variables par tag de la configuration courante.
    /// </summary>
    public virtual string[] PropertiesWithTagVariableSupport => [];

    /// <summary>
    /// Récupère les implémentations de l'annotation pour la config.
    /// </summary>
    /// <param name="annotation">Annotation.</param>
    /// <returns>Implémentations.</returns>
    public IList<AnnotationImplementation> GetImplementation(Annotation? annotation)
    {
        return GetImplementation(annotation?.Implementations) ?? [];
    }

    /// <summary>
    /// Récupère l'implémentation du domaine pour la config.
    /// </summary>
    /// <param name="domain">Décorateur.</param>
    /// <returns>Implémentation.</returns>
    public DomainImplementation? GetImplementation(Domain? domain)
    {
        return GetImplementation(domain?.Implementations);
    }

    /// <summary>
    /// Récupère l'implémentation du décorateur pour la config.
    /// </summary>
    /// <param name="decorator">Décorateur.</param>
    /// <returns>Implémentation.</returns>
    public DecoratorImplementation? GetImplementation(Decorator? decorator)
    {
        return GetImplementation(decorator?.Implementations);
    }

    /// <summary>
    /// Récupère l'implémentation du convertisseur pour la config.
    /// </summary>
    /// <param name="converter">Convertisseur.</param>
    /// <returns>Implémentation.</returns>
    public ConverterImplementation? GetImplementation(Converter? converter)
    {
        return GetImplementation(converter?.Implementations);
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
                    TagVariables[tag] = [];
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
        foreach (var property in GetType().GetProperties().Where(p => p.PropertyType == typeof(string) && p.CanWrite && p.Name != nameof(DefaultLanguage)))
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
                            "module" => PropertiesWithModuleVariableSupport,
                            "lang" => PropertiesWithLangVariableSupport,
                            "fileName" => PropertiesWithFileNameVariableSupport,
                            _ => null!
                        };

                        if (!supportedProperties.Contains(property.Name))
                        {
                            hasMissingVar = true;
                            AnsiConsole.MarkupLine($"[yellow]{Emoji.Known.Warning} {{{GetType().Name}[[{number}]].{property.Name}}} - La variable '{{{varName}}}' n'est pas supportée par cette propriété.[/]");
                        }

                        continue;
                    }

                    var hasTagSupport = PropertiesWithTagVariableSupport.Contains(property.Name);

                    if (!hasTagSupport)
                    {
                        hasMissingVar = true;
                        AnsiConsole.MarkupLine($"[yellow]{Emoji.Known.Warning} {{{GetType().Name}[[{number}]].{property.Name}}} - La variable globale '{{{varName}}}' n'est pas définie pour ce générateur.[/]");
                    }
                    else if (!TagVariableNames.Contains(varName))
                    {
                        hasMissingVar = true;
                        AnsiConsole.MarkupLine($"[yellow]{Emoji.Known.Warning}  {{{GetType().Name}[[{number}]].{property.Name}}} - La variable '{{{varName}}}' n'est pas définie pour ce générateur.[/]");
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
