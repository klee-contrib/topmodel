using System.Text;
using TopModel.Utils;

namespace TopModel.Generator.Csharp;

/// <summary>
/// FileWriter avec des méthodes spécialisées pour écrire du C#.
/// </summary>
public class CSharpWriter(IFileWriter writer) : IDisposable
{
    private readonly List<WriterLine> _lines = [];
    private readonly List<string> _usings = [];

    private WriterLine? _currentLine;

    public bool EnableHeader
    {
        get => writer.EnableHeader;
        set => writer.EnableHeader = value;
    }

    public string HeaderMessage
    {
        get => writer.HeaderMessage;
        set => writer.HeaderMessage = value;
    }

    /// <summary>
    /// Ajoute un using au fichier.
    /// </summary>
    /// <param name="nsName">Nom de la classe/namespace à importer.</param>
    public void AddUsing(string nsName)
    {
        _usings.Add(nsName);
    }

    /// <summary>
    /// Ajoute des usings au fichier.
    /// </summary>
    /// <param name="nsNames">Noms des classes/namespaces à importer.</param>
    public void AddUsings(IEnumerable<string> nsNames)
    {
        _usings.AddRange(nsNames);
    }

    /// <inheritdoc cref="IDisposable.Dispose" />
    public void Dispose()
    {
        if (_usings.Count > 0)
        {
            var systemUsings = _usings.Where(name => name.StartsWith("System"));
            var otherUsings = _usings.Except(systemUsings);

            foreach (
                var nsName in systemUsings
                    .Order()
                    .Concat(otherUsings.Order())
                    .Where(u => !string.IsNullOrWhiteSpace(u))
                    .Distinct()
            )
            {
                writer.WriteLine($"using {nsName};");
            }

            writer.WriteLine();
        }

        foreach (var line in _lines)
        {
            writer.WriteLine(line.Indent, line.Line);
        }

        writer.Dispose();
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
    public virtual void Write(int indentationLevel, string value)
    {
        if (_currentLine == null)
        {
            _currentLine = new() { Indent = indentationLevel, Line = value };
        }
        else
        {
            _currentLine.Line += value;
        }
    }

    /// <summary>
    /// Ecrit un attribut de décoration.
    /// </summary>
    /// <param name="attributeName">Nom de l'attribut.</param>
    /// <param name="attributeParams">Paramètres.</param>
    public void WriteAttribute(string attributeName, params string[] attributeParams)
    {
        WriteAttribute(0, attributeName, attributeParams);
    }

    /// <summary>
    /// Ecrit un attribut de décoration.
    /// </summary>
    /// <param name="indentLevel">Indentation.</param>
    /// <param name="attributeName">Nom de l'attribut.</param>
    /// <param name="attributeParams">Paramètres.</param>
    public virtual void WriteAttribute(int indentLevel, string attributeName, params string[] attributeParams)
    {
        var aParams = string.Empty;
        if (attributeParams.Length > 0)
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
    /// <param name="isRecord">Génère un record au lieu d'une classe.</param>
    /// <param name="ifList">Liste des interfaces implémentées.</param>
    /// <param name="parameters">Paramètres (si constructeur principal).</param>
    /// <param name="parameters">Paramètres de la classe parente (si constructeur principal).</param>
    public virtual void WriteClassDeclaration(
        string name,
        string? inheritedClass,
        bool isRecord,
        string[]? ifList = null,
        string? parameters = null,
        string? baseParameters = null
    )
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        var sb = new StringBuilder();

        sb.Append("public partial ");
        if (isRecord)
        {
            sb.Append("record ");
        }
        else
        {
            sb.Append("class ");
        }

        sb.Append(name);

        if (parameters != null)
        {
            sb.Append($"({parameters})");
        }

        if (!string.IsNullOrEmpty(inheritedClass) || ifList != null && ifList.Length > 0)
        {
            if (baseParameters != null)
            {
                sb.AppendLine();
                sb.Append("    ");
            }

            sb.Append(" : ");
            if (!string.IsNullOrEmpty(inheritedClass))
            {
                sb.Append(inheritedClass);

                if (baseParameters != null)
                {
                    sb.Append($"({baseParameters})");
                }

                if (ifList != null && ifList.Length > 0)
                {
                    sb.Append(", ");
                }
            }

            if (ifList != null && ifList.Length > 0)
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
        WriteLine(sb.ToString());
    }

    /// <summary>
    /// Ecrit la valeur de l'example du commentaire..
    /// </summary>
    /// <param name="value">Valeur à écrire.</param>
    public void WriteExample(string value)
    {
        WriteExample(0, value);
    }

    /// <summary>
    /// Ecrit la valeur de l'example du commentaire.
    /// </summary>
    /// <param name="indentationLevel">Niveau d'indentation.</param>
    /// <param name="value">Valeur à écrire.</param>
    public virtual void WriteExample(int indentationLevel, string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            WriteLine(indentationLevel, LoadExample(value));
        }
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
    public virtual void WriteLine(int indentationLevel, string value)
    {
        if (_currentLine != null)
        {
            _currentLine.Line += value;
            _lines.Add(_currentLine);
            _currentLine = null;
        }
        else
        {
            _lines.Add(new() { Indent = indentationLevel, Line = value });
        }
    }

    /// <summary>
    /// Retourne le code associé à la déclaration d'un namespace.
    /// </summary>
    /// <param name="value">Valeur du namespace.</param>
    public virtual void WriteNamespace(string value)
    {
        WriteLine($"namespace {value};");
        WriteLine();
    }

    /// <summary>
    /// Ecrit le commentaire de paramètre.
    /// </summary>
    /// <param name="paramName">Nom du paramètre.</param>
    /// <param name="value">Valeur du paramètre.</param>
    /// <param name="indent">Niveau d'indentation.</param>
    public virtual void WriteParam(string paramName, string value, int indent = 1)
    {
        if (!string.IsNullOrEmpty(paramName) && !string.IsNullOrEmpty(value))
        {
            WriteLine(indent, LoadParam(paramName, value, "param"));
        }
    }

    /// <summary>
    /// Ecrit le commentaire de returns.
    /// </summary>
    /// <param name="indentationLevel">Niveau d'indention.</param>
    /// <param name="value">Description du returns.</param>
    public virtual void WriteReturns(int indentationLevel, string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            WriteLine(indentationLevel, LoadReturns(value));
        }
    }

    /// <summary>
    /// Ecrit la valeur du résumé du commentaire..
    /// </summary>
    /// <param name="value">Valeur à écrire.</param>
    public void WriteSummary(string value)
    {
        WriteSummary(0, value);
    }

    /// <summary>
    /// Ecrit la valeur du résumé du commentaire..
    /// </summary>
    /// <param name="indentationLevel">Niveau d'indentation.</param>
    /// <param name="value">Valeur à écrire.</param>
    public virtual void WriteSummary(int indentationLevel, string value)
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
    public virtual void WriteTypeParam(int indentationLevel, string paramName, string value)
    {
        if (!string.IsNullOrEmpty(paramName) && !string.IsNullOrEmpty(value))
        {
            WriteLine(indentationLevel, LoadParam(paramName, value, "typeparam"));
        }
    }

    /// <summary>
    /// Retourne le commentaire de l'exemple formatté.
    /// </summary>
    /// <param name="value">Description de l'exemple.</param>
    /// <returns>Code généré.</returns>
    protected static string LoadExample(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException(nameof(value));
        }

        var sb = new StringBuilder();
        sb.Append("/// <example>");
        sb.Append(value.Replace("<", "&lt;").Replace(">", "&gt;"));
        sb.Append("</example>");
        return sb.ToString();
    }

    /// <summary>
    /// Retourne le commentaire du param formatté.
    /// </summary>
    /// <param name="paramName">Nom du paramètre.</param>
    /// <param name="value">Description du paramètre.</param>
    /// <param name="tag">Tag XML.</param>
    /// <returns>Code généré.</returns>
    protected static string LoadParam(string paramName, string value, string tag)
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

        value = value.Replace("<", "&lt;").Replace(">", "&gt;").ReplaceLineEndings();
        if (!value.EndsWith(".", StringComparison.OrdinalIgnoreCase))
        {
            value += ".";
        }

        if (value.Contains(Environment.NewLine))
        {
            sb.Append("\r\n");
            foreach (var line in value.Split(Environment.NewLine))
            {
                sb.Append("///");
                if (!string.IsNullOrWhiteSpace(line))
                {
                    sb.Append($" {line}");
                }

                sb.Append("\r\n");
            }

            sb.Append("/// ");
        }
        else
        {
            sb.Append(value);
        }

        sb.Append($"</{tag}>");
        return sb.ToString();
    }

    /// <summary>
    /// Retourne le commentaire du returns formatté.
    /// </summary>
    /// <param name="value">Description de la valeur retournée.</param>
    /// <returns>Code généré.</returns>
    protected static string LoadReturns(string value)
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
    protected static string LoadSummary(string summary)
    {
        if (string.IsNullOrEmpty(summary))
        {
            throw new ArgumentNullException(nameof(summary));
        }

        summary = summary.Trim().Replace("<", "&lt;").Replace(">", "&gt;").ReplaceLineEndings();
        if (!summary.EndsWith(".", StringComparison.OrdinalIgnoreCase))
        {
            summary += ".";
        }

        var sb = new StringBuilder();
        sb.Append("/// <summary>\r\n");

        foreach (var line in summary.Split(Environment.NewLine))
        {
            sb.Append("///");
            if (!string.IsNullOrWhiteSpace(line))
            {
                sb.Append($" {line}");
            }

            sb.Append("\r\n");
        }

        sb.Append("/// </summary>");
        return sb.ToString();
    }
}
