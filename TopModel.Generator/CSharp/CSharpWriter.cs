using System.Text;
using Microsoft.Extensions.Logging;
using TopModel.Utils;

namespace TopModel.Generator.CSharp;

/// <summary>
/// FileWriter avec des méthodes spécialisées pour écrire du C#.
/// </summary>
public class CSharpWriter : IDisposable
{
    private readonly bool _useLatestCSharp;
    private readonly FileWriter _writer;

    public CSharpWriter(string name, ILogger logger, bool useLatestCSharp = false)
    {
        _useLatestCSharp = useLatestCSharp;
        _writer = new FileWriter(name, logger);
    }

    /// <inheritdoc cref="IDisposable.Dispose" />
    public void Dispose()
    {
        _writer.Dispose();
    }

    /// <summary>
    /// Ecrit du texte.
    /// </summary>
    /// <param name="text">Texte.</param>
    public void Write(string text)
    {
        Write(0, text);
    }

    /// <summary>
    /// Ecrit la chaine avec le niveau indenté.
    /// </summary>
    /// <param name="indentationLevel">Niveau d'indentation.</param>
    /// <param name="value">Valeur à écrire dans le flux.</param>
    public void Write(int indentationLevel, string value)
    {
        var indentValue = GetIdentValue(indentationLevel);
        value = value.Replace("\r\n", "\r\n" + indentValue);
        _writer.Write(indentValue + value);
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

        WriteLine(indentLevel, $@"[{attributeName}{aParams}]");
    }

    /// <summary>
    /// Retourne le code associé à la déclaration.
    /// </summary>
    /// <param name="name">Nom de la classe.</param>
    /// <param name="inheritedClass">Classe parente.</param>
    /// <param name="ifList">Liste des interfaces implémentées.</param>
    public void WriteClassDeclaration(string name, string? inheritedClass, params string[] ifList)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (ifList == null)
        {
            throw new ArgumentNullException(nameof(ifList));
        }

        var sb = new StringBuilder();

        sb.Append("public partial class ");
        sb.Append(name);
        if (!string.IsNullOrEmpty(inheritedClass) || ifList != null && ifList.Length > 0)
        {
            sb.Append(" : ");
            if (!string.IsNullOrEmpty(inheritedClass))
            {
                sb.Append(inheritedClass);
                if (ifList.Length > 0)
                {
                    sb.Append(", ");
                }
            }

            if (ifList.Length > 0)
            {
                var enumerator = ifList.GetEnumerator();
                for (var i = 0; i < ifList.Length; ++i)
                {
                    if (!enumerator.MoveNext())
                    {
                        throw new NotSupportedException();
                    }

                    sb.Append(enumerator.Current);
                    if (i < ifList.Length - 1)
                    {
                        sb.Append(", ");
                    }
                }
            }
        }

        sb.Append("\r\n{");
        WriteLine(1, sb.ToString());
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
        var indentValue = GetIdentValue(indentationLevel);
        value = value.Replace("\r\n", "\r\n" + indentValue);
        _writer.WriteLine(indentValue + value);
    }

    /// <summary>
    /// Retourne le code associé à la déclaration d'un namespace.
    /// </summary>
    /// <param name="value">Valeur du namespace.</param>
    public void WriteNamespace(string value)
    {
        Write($"namespace {value}");
        if (_useLatestCSharp)
        {
            WriteLine(";");
            WriteLine();
        }
        else
        {
            WriteLine();
            WriteLine("{");
        }
    }

    /// <summary>
    /// Retourne le code associé à la fin d'un namespace.
    /// </summary>
    public void WriteNamespaceEnd()
    {
        if (!_useLatestCSharp)
        {
            WriteLine("}");
        }
    }

    /// <summary>
    /// Ecrit le commentaire de paramètre.
    /// </summary>
    /// <param name="paramName">Nom du paramètre.</param>
    /// <param name="value">Valeur du paramètre.</param>
    public void WriteParam(string paramName, string value)
    {
        if (!string.IsNullOrEmpty(paramName) && !string.IsNullOrEmpty(value))
        {
            WriteLine(2, LoadParam(paramName, value, "param"));
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
    /// Ecrit la valeur du résumé du commentaire..
    /// </summary>
    /// <param name="indentationLevel">Niveau d'indentation.</param>
    /// <param name="value">Valeur à écrire.</param>
    public void WriteSummary(int indentationLevel, string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            WriteLine(indentationLevel, LoadSummary(value));
        }
    }

    /// <summary>
    /// Ecrit le commentaire de paramètre de type.
    /// </summary>
    /// <param name="indentationLevel">Niveau d'indentation.</param>
    /// <param name="paramName">Nom du paramètre.</param>
    /// <param name="value">Valeur du paramètre.</param>
    public void WriteTypeParam(int indentationLevel, string paramName, string value)
    {
        if (!string.IsNullOrEmpty(paramName) && !string.IsNullOrEmpty(value))
        {
            WriteLine(indentationLevel, LoadParam(paramName, value, "typeparam"));
        }
    }

    /// <summary>
    /// Retourne le code associé à la déclaration d'un Using.
    /// </summary>
    /// <param name="nsNames">Nom de la classe/namespace à importer.</param>
    public void WriteUsings(params string[] nsNames)
    {
        var systemUsings = nsNames.Where(name => name.StartsWith("System"));
        var otherUsings = nsNames.Except(systemUsings);

        foreach (var nsName in systemUsings.OrderBy(x => x).Concat(otherUsings.OrderBy(x => x)))
        {
            WriteLine($"using {nsName};");
        }
    }

    /// <summary>
    /// Retourne le commentaire du param formatté.
    /// </summary>
    /// <param name="paramName">Nom du paramètre.</param>
    /// <param name="value">Description du paramètre.</param>
    /// <param name="tag">Tag XML.</param>
    /// <returns>Code généré.</returns>
    private static string LoadParam(string paramName, string value, string tag)
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
        sb.Append($"/// <{tag} name=\"");
        sb.Append(paramName);
        sb.Append("\">");
        sb.Append(value.Replace("<", "&lt;").Replace(">", "&gt;"));
        if (!value.EndsWith(".", StringComparison.OrdinalIgnoreCase))
        {
            sb.Append('.');
        }

        sb.Append($"</{tag}>");
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
        sb.Append("/// <returns>");
        sb.Append(value.Replace("<", "&lt;").Replace(">", "&gt;"));
        if (!value.EndsWith(".", StringComparison.OrdinalIgnoreCase))
        {
            sb.Append('.');
        }

        sb.Append("</returns>");
        return sb.ToString();
    }

    /// <summary>
    /// Retourne le commentaire du summary formatté.
    /// </summary>
    /// <param name="summary">Contenu du commentaire.</param>
    /// <returns>Code généré.</returns>
    private static string LoadSummary(string summary)
    {
        if (string.IsNullOrEmpty(summary))
        {
            throw new ArgumentNullException(nameof(summary));
        }

        summary = summary.Trim();

        var sb = new StringBuilder();
        sb.Append("/// <summary>\r\n");
        sb.Append("/// " + summary.Replace("\r\n", "\r\n/// ").Replace("\n", "\r\n/// ").Replace("<", "&lt;").Replace(">", "&gt;"));
        if (!summary.EndsWith(".", StringComparison.OrdinalIgnoreCase))
        {
            sb.Append('.');
        }

        sb.Append("\r\n/// </summary>");
        return sb.ToString();
    }

    /// <summary>
    /// Calcule l'identation nécessaire.
    /// </summary>
    /// <param name="indentationLevel">Niveau d'indention.</param>
    /// <returns>Identation.</returns>
    private string GetIdentValue(int indentationLevel)
    {
        if (_useLatestCSharp && indentationLevel > 0)
        {
            indentationLevel--;
        }

        var indentValue = string.Empty;
        for (var i = 0; i < indentationLevel; ++i)
        {
            indentValue += _writer.IndentValue;
        }

        return indentValue;
    }
}