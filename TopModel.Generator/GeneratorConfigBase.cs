#nullable disable
using System.Text.RegularExpressions;
using TopModel.Core;

namespace TopModel.Generator;

public abstract class GeneratorConfigBase
{
    /// <summary>
    /// Racine du répertoire de génération.
    /// </summary>
    public string OutputDirectory { get; set; }

    /// <summary>
    /// Tags du générateur.
    /// </summary>
    public IList<string> Tags { get; set; }

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
    /// Propriétés qui supportent les variables par tag de la configuration courante.
    /// </summary>
    public virtual string[] PropertiesWithTagVariableSupport => Array.Empty<string>();

    /// <summary>
    /// Résout les variables globales.
    /// </summary>
    /// <param name="number">Numéro du générateur.</param>
    public void ResolveVariables(int number)
    {
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
            var value = (string)property.GetValue(this);
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
                    if (varName == "app" || varName == "module")
                    {
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

        if (hasMissingVar)
        {
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Résout les variables de tag dans un chaîne de caractère.
    /// </summary>
    /// <param name="tag">Nom du tag.</param>
    /// <param name="value">Chaîne de caractères.</param>
    /// <returns>Value avec les variables remplacées..</returns>
    public virtual string ResolveTagVariables(string tag, string value)
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
        return Regex.Replace(value, $$"""\{{{varName}}(:\w+)?\}""", m => m.Value.Trim('{', '}').GetTransformation()(varValue));
    }
}