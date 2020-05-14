using System;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;

namespace TopModel.Generator.Kasper
{
    /// <summary>
    /// FileWriter avec des méthodes spécialisées pour écrire du CJava
    /// </summary>
    public class JavaWriter : IDisposable
    {
        private const string IndentValue = "    ";

        private readonly FileWriter _writer;

        public JavaWriter(string name, ILogger logger)
        {
            _writer = new FileWriter(name, logger, CodePagesEncodingProvider.Instance.GetEncoding(1252)!);
        }

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            _writer.Dispose();
        }

        /// <summary>
        /// Ecrit un attribut de décoration.
        /// </summary>
        /// <param name="indentLevel">Indentation.</param>
        /// <param name="attributeName">Nom de l'attribut.</param>
        /// <param name="attributeParams">Paramètres.</param>
        public void WriteAttribute(int indentLevel, string attributeName, params string[] attributeParams)
        {
            var aParams = string.Empty;
            if (attributeParams.Any())
            {
                aParams = $@"({string.Join(", ", attributeParams)})";
            }

            if (attributeName.Contains("kasperx"))
            {
                var ident = string.Join(IndentValue, Enumerable.Range(0, indentLevel).Select(v => string.Empty));
                aParams = aParams.Replace(",", $",\n{ident}{IndentValue}").Replace("(", $" (\n {ident}{IndentValue}").Replace(")", $"\n{ident})");
            }

            WriteLine(indentLevel, $@"@{attributeName}{aParams}");
        }

        /// <summary>
        /// Retourne le code associé à la déclaration.
        /// </summary>
        /// <param name="name">Nom de la classe.</param>
        /// <param name="inheritedClass">Classe parente.</param>
        /// <param name="isAbstract">isAbstract</param>
        public void WriteClassDeclaration(string name, string? inheritedClass, bool isAbstract)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var sb = new StringBuilder();

            sb.Append($"public{(isAbstract ? " abstract" : string.Empty)} class ");
            sb.Append(name);
            if (!string.IsNullOrEmpty(inheritedClass))
            {
                sb.Append($" extends {inheritedClass}");
            }

            sb.Append(" {");
            WriteLine(0, sb.ToString());
        }

        /// <summary>
        /// Ecrit la chaine de caractère dans le flux.
        /// </summary>
        /// <param name="value">Valeur à écrire dans le flux.</param>
        public void WriteLine(string? value = null)
        {
            WriteLine(0, value ?? string.Empty);
        }

        /// <summary>
        /// Ecrit la chaine avec le niveau indenté.
        /// </summary>
        /// <param name="indentationLevel">Niveau d'indentation.</param>
        /// <param name="value">Valeur à écrire dans le flux.</param>
        public void WriteLine(int indentationLevel, string value)
        {
            var indentValue = string.Empty;
            for (var i = 0; i < indentationLevel; ++i)
            {
                indentValue += IndentValue;
            }

            value = value.Replace("\n", "\n" + indentValue);
            _writer.WriteLine(indentValue + value);
        }

        /// <summary>
        /// Ecrit le commentaire de parametre.
        /// </summary>
        /// <param name="paramName">Nom du paramètre.</param>
        /// <param name="value">Valeur du paramètre.</param>
        public void WriteParam(string paramName, string value)
        {
            if (!string.IsNullOrEmpty(paramName) && !string.IsNullOrEmpty(value))
            {
                WriteLine(1, LoadParam(paramName, value));
            }
        }

        /// <summary>
        /// Ecrit le commentaire de returns.
        /// </summary>
        /// <param name="indentationLevel">Niveau d'indention.</param>
        /// <param name="value">Description du returns.</param>
        public void WriteReturns(int indentationLevel, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                WriteLine(indentationLevel, LoadReturns(value));
            }
        }

        /// <summary>
        /// Ecrit le commentaire de throws.
        /// </summary>
        /// <param name="indentationLevel">Niveau d'indention.</param>
        /// <param name="value">Description du returns.</param>
        public void WriteThrows(int indentationLevel, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                WriteLine(indentationLevel, LoadThrows(value));
            }
        }

        /// <summary>
        /// Ecrit la valeur du résumé du commentaire..
        /// </summary>
        /// <param name="indentationLevel">Niveau d'indentation.</param>
        /// <param name="value">Valeur à écrire.</param>
        public void WriteDocStart(int indentationLevel, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                WriteLine(indentationLevel, LoadDocStart(value));
            }
        }

        public void WriteDocEnd(int indentationLevel)
        {
            WriteLine(indentationLevel, " */");
        }

        /// <summary>
        /// Retourne le commentaire du param formatté.
        /// </summary>
        /// <param name="paramName">Nom du paramètre.</param>
        /// <param name="value">Description du paramètre.</param>
        /// <returns>Code généré.</returns>
        private static string LoadParam(string paramName, string value)
        {
            if (string.IsNullOrEmpty(paramName))
            {
                throw new ArgumentNullException(nameof(paramName));
            }

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            var sb = new StringBuilder();
            sb.Append(" * @param ");
            sb.Append(paramName);
            sb.Append(" ");
            sb.Append(value);
            if (!value.EndsWith(".", StringComparison.OrdinalIgnoreCase))
            {
                sb.Append('.');
            }

            return sb.ToString();
        }

        /// <summary>
        /// Retourne le commentaire du returns formatté.
        /// </summary>
        /// <param name="value">Description de la valeur retournée.</param>
        /// <returns>Code généré.</returns>
        private static string LoadReturns(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            var sb = new StringBuilder();
            sb.Append(" * @return ");
            sb.Append(value);
            if (!value.EndsWith(".", StringComparison.OrdinalIgnoreCase))
            {
                sb.Append('.');
            }

            return sb.ToString();
        }

        /// <summary>
        /// Retourne le commentaire du returns formatté.
        /// </summary>
        /// <param name="value">Description de la valeur retournée.</param>
        /// <returns>Code généré.</returns>
        private static string LoadThrows(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            var sb = new StringBuilder();
            sb.Append(" * @throws ");
            sb.Append(value);
            if (!value.EndsWith(".", StringComparison.OrdinalIgnoreCase))
            {
                sb.Append('.');
            }

            return sb.ToString();
        }

        /// <summary>
        /// Retourne le commentaire du summary formatté.
        /// </summary>
        /// <param name="summary">Contenu du commentaire.</param>
        /// <returns>Code généré.</returns>
        private static string LoadDocStart(string summary)
        {
            if (string.IsNullOrEmpty(summary))
            {
                throw new ArgumentNullException(nameof(summary));
            }

            summary = summary.Trim();

            var sb = new StringBuilder();
            sb.Append("/**\n");
            sb.Append(" * " + summary.Replace("\n", "\n * "));
            if (!summary.EndsWith(".", StringComparison.OrdinalIgnoreCase))
            {
                sb.Append('.');
            }

            return sb.ToString();
        }
    }
}
