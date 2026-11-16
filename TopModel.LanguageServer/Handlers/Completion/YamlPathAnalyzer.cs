using System.Globalization;
using System.Text;

namespace TopModel.LanguageServer.Handlers;

/// <summary>
/// Analyse la position d'un curseur dans un document YAML TopModel pour en déduire un chemin YAML.
/// </summary>
public static class YamlPathAnalyzer
{
    /// <summary>
    /// Calcule le chemin YAML correspondant à la position du curseur.
    /// Le premier segment est l'objet racine du document courant (les documents sont séparés par "---").
    /// Les éléments de liste sont indicés à partir de 0 (<c>[n]</c>, collés au segment précédent), que la liste soit
    /// en bloc (<c>- a</c>) ou inline (<c>[a, b]</c>). Exemples : <c>class.properties[2].domain</c>,
    /// <c>class.unique[0][1]</c>, <c>class.values.A.Code</c>, <c>uses[1]</c>.
    /// Un point final (<c>class.properties[2].</c>) indique que le curseur est en train de saisir une nouvelle clé
    /// de l'objet précédent (pas de ':' avant le curseur, ou curseur sur la clé elle-même). À la racine du document,
    /// ce cas est représenté par la chaîne ".".
    /// Un élément de liste scalaire (<c>- foo</c>) donne le chemin se terminant par l'indice (<c>class.properties[2]</c>),
    /// même s'il s'agit d'une clé en cours de saisie après le tiret.
    /// Dans le texte d'un scalaire multiligne (<c>|</c>, <c>&gt;</c>), le chemin est celui de sa clé.
    /// Retourne null si la position est invalide, si la ligne est un séparateur "---" ou si le curseur est dans un commentaire.
    /// Limite connue : les structures inline sur plusieurs lignes ne sont pas gérées.
    /// </summary>
    /// <param name="text">Lignes du document.</param>
    /// <param name="line">Numéro de ligne (base 0).</param>
    /// <param name="character">Colonne du curseur (base 0).</param>
    /// <returns>Le chemin YAML, ou null.</returns>
    public static string? GetPath(string[] text, int line, int character)
    {
        if (line < 0 || line >= text.Length || character < 0 || text[line].StartsWith("---"))
        {
            return null;
        }

        var docStart = GetDocumentStart(text, line);

        var stack = new List<(int Indent, string? Key, int Index)>();

        // Indentation de la clé d'un scalaire multiligne ("|" ou ">") en cours, -1 sinon.
        var blockIndent = -1;
        for (var i = docStart; i < line; i++)
        {
            if (blockIndent >= 0 && IsInBlockScalar(text[i], blockIndent))
            {
                continue;
            }

            ProcessLine(stack, text[i], text[i].Length, isCurrent: false);
            blockIndent = IsBlockScalarStart(text[i]) && stack.Count > 0 ? stack[^1].Indent : -1;
        }

        if (blockIndent >= 0 && IsInBlockScalar(text[line], blockIndent))
        {
            return BuildPath(stack);
        }

        return ProcessLine(stack, text[line], Math.Min(character, text[line].Length), isCurrent: true);
    }

    /// <summary>
    /// Ligne de début du document contenant la ligne donnée (ligne qui suit le "---" précédent, 0 s'il n'y en a pas).
    /// </summary>
    /// <param name="text">Lignes du fichier.</param>
    /// <param name="line">Numéro de ligne (base 0).</param>
    /// <returns>La ligne de début du document.</returns>
    internal static int GetDocumentStart(string[] text, int line)
    {
        for (var i = Math.Min(line, text.Length) - 1; i >= 0; i--)
        {
            if (text[i].StartsWith("---"))
            {
                return i + 1;
            }
        }

        return 0;
    }

    private static string BuildPath(List<(int Indent, string? Key, int Index)> stack)
    {
        var sb = new StringBuilder();
        foreach (var entry in stack)
        {
            if (entry.Key == null)
            {
                sb.Append('[').Append(entry.Index.ToString(CultureInfo.InvariantCulture)).Append(']');
            }
            else
            {
                if (sb.Length > 0)
                {
                    sb.Append('.');
                }

                sb.Append(entry.Key);
            }
        }

        return sb.ToString();
    }

    private static int FindCommentStart(string line)
    {
        var quote = '\0';
        for (var i = 0; i < line.Length; i++)
        {
            var c = line[i];
            var afterBoundary = i == 0 || char.IsWhiteSpace(line[i - 1]) || line[i - 1] is '[' or '{' or ',';
            if (quote != '\0')
            {
                if (c == quote)
                {
                    quote = '\0';
                }
            }
            else if (c is '"' or '\'' && afterBoundary)
            {
                quote = c;
            }
            else if (c == '#' && afterBoundary)
            {
                return i;
            }
        }

        return -1;
    }

    private static int FindKeyColon(string content)
    {
        for (var i = 0; i < content.Length; i++)
        {
            if (content[i] == ':' && (i == content.Length - 1 || content[i + 1] == ' '))
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// La ligne se termine-t-elle par un indicateur de scalaire multiligne ("key: |", "key: >-", "- |"...) ?
    /// </summary>
    private static bool IsBlockScalarStart(string line)
    {
        var commentStart = FindCommentStart(line);
        var content = (commentStart >= 0 ? line[..commentStart] : line).TrimEnd();
        var end = content.Length;
        while (end > 0 && (char.IsAsciiDigit(content[end - 1]) || content[end - 1] is '+' or '-'))
        {
            end--;
        }

        return end >= 3
            && content[end - 1] is '|' or '>'
            && content[end - 2] == ' '
            && content[..(end - 2)].TrimEnd() is var before
            && (before.EndsWith(':') || before.Trim() == "-");
    }

    private static bool IsDash(string content)
    {
        return content.Length > 0 && content[0] == '-' && (content.Length == 1 || content[1] == ' ');
    }

    /// <summary>
    /// La ligne fait-elle partie du scalaire multiligne dont la clé est à l'indentation donnée ?
    /// </summary>
    private static bool IsInBlockScalar(string line, int blockIndent)
    {
        return string.IsNullOrWhiteSpace(line) || line.Length - line.TrimStart().Length > blockIndent;
    }

    private static string ParseFlow(string prefix)
    {
        if (prefix.Length == 0 || prefix[0] is not ('{' or '['))
        {
            return string.Empty;
        }

        var levels = new List<FlowLevel>();
        var quote = '\0';
        foreach (var c in prefix)
        {
            var current = levels.Count > 0 ? levels[^1] : null;
            if (quote != '\0')
            {
                quote = c == quote ? '\0' : quote;
                current?.Buffer.Append(c);
                continue;
            }

            switch (c)
            {
                case '{':
                case '[':
                    levels.Add(new FlowLevel(c == '{'));
                    break;
                case '}':
                case ']':
                    if (current != null)
                    {
                        levels.RemoveAt(levels.Count - 1);
                    }

                    break;
                case ':' when current is { IsMap: true, Key: null }:
                    current.Key = current.Buffer.ToString().Trim();
                    current.Buffer.Clear();
                    break;
                case ',' when current != null:
                    current.Key = null;
                    current.Index++;
                    current.Buffer.Clear();
                    break;
                case '"' or '\'' when current != null && current.Buffer.ToString().Trim().Length == 0:
                    quote = c;
                    break;
                default:
                    current?.Buffer.Append(c);
                    break;
            }
        }

        var tail = new StringBuilder();
        for (var i = 0; i < levels.Count; i++)
        {
            var level = levels[i];
            if (!level.IsMap)
            {
                tail.Append('[').Append(level.Index.ToString(CultureInfo.InvariantCulture)).Append(']');
            }
            else if (level.Key != null)
            {
                tail.Append('.').Append(level.Key);
            }
            else if (i == levels.Count - 1)
            {
                tail.Append('.');
            }
        }

        return tail.ToString();
    }

    /// <summary>
    /// Dépile jusqu'au tiret d'un élément de liste et retourne l'indice du nouvel élément.
    /// Une clé de même indentation est conservée (liste au même niveau que sa clé).
    /// </summary>
    private static int PopForListItem(List<(int Indent, string? Key, int Index)> stack, int indent)
    {
        while (stack.Count > 0 && stack[^1].Indent > indent)
        {
            stack.RemoveAt(stack.Count - 1);
        }

        if (stack.Count > 0 && stack[^1].Key == null && stack[^1].Indent == indent)
        {
            var index = stack[^1].Index + 1;
            stack.RemoveAt(stack.Count - 1);
            return index;
        }

        return 0;
    }

    private static void PopFrom(List<(int Indent, string? Key, int Index)> stack, int indent)
    {
        while (stack.Count > 0 && stack[^1].Indent >= indent)
        {
            stack.RemoveAt(stack.Count - 1);
        }
    }

    /// <summary>
    /// Met à jour la pile de segments avec une ligne. Pour la ligne du curseur, retourne le chemin complet.
    /// </summary>
    private static string? ProcessLine(
        List<(int Indent, string? Key, int Index)> stack,
        string rawLine,
        int cursor,
        bool isCurrent
    )
    {
        var line = rawLine;
        var commentStart = FindCommentStart(rawLine);
        if (commentStart >= 0)
        {
            if (isCurrent && cursor > commentStart)
            {
                return null;
            }

            line = rawLine[..commentStart];
        }

        if (string.IsNullOrWhiteSpace(line))
        {
            if (!isCurrent)
            {
                return null;
            }

            // Ligne vide en cours de saisie : nouvelle clé à l'indentation du curseur.
            PopFrom(stack, cursor);
            return BuildPath(stack) + ".";
        }

        var indent = line.Length - line.TrimStart().Length;
        var content = line[indent..];
        var rel = Math.Max(cursor, indent) - indent;
        var isListItem = false;
        while (IsDash(content))
        {
            isListItem = true;
            stack.Add((indent, null, PopForListItem(stack, indent)));
            var consumed = 1;
            while (consumed < content.Length && content[consumed] == ' ')
            {
                consumed++;
            }

            if (isCurrent && rel < consumed)
            {
                return BuildPath(stack);
            }

            indent += consumed;
            content = content[consumed..];
            rel -= consumed;
        }

        if (content.Length == 0)
        {
            return isCurrent ? BuildPath(stack) : null;
        }

        if (content[0] is '{' or '[')
        {
            return isCurrent ? BuildPath(stack) + ParseFlow(content[..rel]) : null;
        }

        var colon = FindKeyColon(content);
        if (colon < 0)
        {
            if (isListItem)
            {
                return isCurrent ? BuildPath(stack) : null;
            }

            if (!isCurrent)
            {
                return null;
            }

            PopFrom(stack, indent);
            return BuildPath(stack) + ".";
        }

        PopFrom(stack, indent);
        if (isCurrent && rel <= colon)
        {
            return BuildPath(stack) + ".";
        }

        stack.Add((indent, content[..colon].Trim(), -1));
        return isCurrent ? BuildPath(stack) + ParseFlow(content[(colon + 1)..rel].TrimStart()) : null;
    }

    private sealed class FlowLevel(bool isMap)
    {
        public StringBuilder Buffer { get; } = new();

        public int Index { get; set; }

        public bool IsMap { get; } = isMap;

        public string? Key { get; set; }
    }
}
