using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace TopModel.Core
{
    /// <summary>
    /// Regroupe quelques utilitaires pour la génération TS.
    /// </summary>
    public static class ModelUtils
    {      
        /// <summary>
        /// Transforme le type en type Typescript.
        /// </summary>
        /// <param name="type">Le type d'entrée.</param>
        /// <param name="removeBrackets">Supprime la liste.</param>
        /// <returns>Le type en sortie.</returns>
        public static string CSharpToTSType(string type)
        {
            switch (type)
            {
                case "int":
                case "int?":
                case "decimal?":
                case "short?":
                case "TimeSpan?":
                    return "number";
                case "DateTime?":
                case "Guid?":
                case "string":
                    return "string";
                case "bool?":
                    return "boolean";
                default:
                    if (type?.StartsWith("ICollection") ?? false)
                    {
                        return $"{CSharpToTSType(Regex.Replace(type, ".+<(.+)>", "$1"))}[]";
                    }

                    return "any";
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
                .Replace("/-", "/");
        }

        /// <summary>
        /// Met la première lettre d'un string en minuscule.
        /// </summary>
        /// <param name="text">Le texte en entrée.</param>
        /// <returns>Le texte en sortie.</returns>
        public static string ToFirstLower(this string text)
        {
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
    }
}
