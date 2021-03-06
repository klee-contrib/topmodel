using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace TopModel.Utils;

/// <summary>
/// Regroupe quelques utilitaires pour la génération.
/// </summary>
public static class ModelUtils
{
    public static void CombinePath<T>(string directoryName, T classe, Expression<Func<T, string?>> getter)
    {
        var property = (PropertyInfo)((MemberExpression)getter.Body).Member;

        if (property.GetValue(classe) != null)
        {
            property.SetValue(classe, Path.GetFullPath(Path.Combine(directoryName, (string)property.GetValue(classe)!)));
        }
    }

    /// <summary>
    /// Convertit un text en dash-case.
    /// </summary>
    /// <param name="text">Le texte en entrée.</param>
    /// <param name="upperStart">Texte commençant par une majuscule.</param>
    /// <returns>Le texte en sortie.</returns>
    public static string ToDashCase(this string text, bool upperStart = true)
    {
        return Regex.Replace(text, @"\p{Lu}", m => "-" + m.Value)
            .ToLowerInvariant()
            .Substring(upperStart ? 1 : 0)
            .Replace("/-", "/")
            .Replace("\\-", "\\");
    }

    /// <summary>
    /// Convertit un text en PascalCase.
    /// </summary>
    /// <param name="text">Le texte en entrée.</param>
    /// <returns>Le texte en sortie.</returns>
    public static string ToPascalCase(this string text)
    {
        var invalidCharsRgx = new Regex("[^_a-zA-Z0-9]");
        var whiteSpace = new Regex(@"(?<=\s)");
        var startsWithLowerCaseChar = new Regex("^[a-z]");
        var firstCharFollowedByUpperCasesOnly = new Regex("(?<=[A-Z])[A-Z0-9]+$");
        var lowerCaseNextToNumber = new Regex("(?<=[0-9])[a-z]");
        var upperCaseInside = new Regex("(?<=[A-Z])[A-Z]+?((?=[A-Z][a-z])|(?=[0-9]))");

        // replace white spaces with undescore, then replace all invalid chars with empty string
        var pascalCase = invalidCharsRgx.Replace(whiteSpace.Replace(text, "_"), string.Empty)

            // split by underscores
            .Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries)

            // set first letter to uppercase
            .Select(w => startsWithLowerCaseChar.Replace(w, m => m.Value.ToUpper()))

            // replace second and all following upper case letters to lower if there is no next lower (ABC -> Abc)
            .Select(w => firstCharFollowedByUpperCasesOnly.Replace(w, m => m.Value.ToLower()))

            // set upper case the first lower case following a number (Ab9cd -> Ab9Cd)
            .Select(w => lowerCaseNextToNumber.Replace(w, m => m.Value.ToUpper()))

            // lower second and next upper case letters except the last if it follows by any lower (ABcDEf -> AbcDef)
            .Select(w => upperCaseInside.Replace(w, m => m.Value.ToLower()));

        return string.Concat(pascalCase);
    }

    /// <summary>
    /// Met la première lettre d'un string en minuscule.
    /// </summary>
    /// <param name="text">Le texte en entrée.</param>
    /// <returns>Le texte en sortie.</returns>
    public static string ToFirstLower(this string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        return char.ToLower(text[0]) + text.Substring(1);
    }

    /// <summary>
    /// Met la première lettre d'un string en majuscule.
    /// </summary>
    /// <param name="text">Le texte en entrée.</param>
    /// <returns>Le texte en sortie.</returns>
    public static string ToFirstUpper(this string text)
    {
        return char.ToUpper(text[0]) + text.Substring(1);
    }

    /// <summary>
    /// Convertit un nom avec la syntaxe C#.
    /// </summary>
    /// <param name="name">Nom au format C#.</param>
    /// <returns>Nom base de données.</returns>
    public static string ConvertCsharp2Bdd(string name)
    {
        if (name.Contains("_"))
        {
            return name.ToUpperInvariant();
        }

        var sb = new StringBuilder();
        var c = name.ToCharArray();
        var lastIsUp = true;
        var anteLastIsUp = false;
        for (var i = 0; i < c.Length; ++i)
        {
            var upperChar = new string(c[i], 1).ToUpper(CultureInfo.CurrentCulture);
            if (i > 0)
            {
                var isLastCaracter = i == c.Length - 1;
                var nextIsMinus = !isLastCaracter && !new string(c[i + 1], 1).ToUpper(CultureInfo.CurrentCulture).Equals(new string(c[i + 1], 1));

                if (upperChar.Equals(new string(c[i], 1)))
                {
                    if (!lastIsUp || anteLastIsUp ||
                        !lastIsUp && isLastCaracter ||
                        lastIsUp && nextIsMinus)
                    {
                        sb.Append('_');
                        anteLastIsUp = false;
                        lastIsUp = true;
                    }
                    else
                    {
                        anteLastIsUp = lastIsUp;
                        lastIsUp = true;
                    }
                }
                else
                {
                    anteLastIsUp = lastIsUp;
                    lastIsUp = false;
                }
            }

            sb.Append(upperChar);
        }

        return sb.ToString();
    }

    public static string ToRelative(this string path, string? relativeTo = null)
    {
        var relative = Path.GetRelativePath(relativeTo ?? Directory.GetCurrentDirectory(), path);
        if (!relative.StartsWith("."))
        {
            relative = $".\\{relative}";
        }

        return relative;
    }
}