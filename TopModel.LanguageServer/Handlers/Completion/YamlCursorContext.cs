using System.Globalization;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace TopModel.LanguageServer.Handlers;

/// <summary>
/// Bloc YAML courant (entre deux lignes <c>---</c>) lu avec YamlDotNet, avec une sentinelle insérée à la position du curseur.
/// La position de la sentinelle dans l'arbre donne le chemin YAML du curseur (<see cref="Path" />).
/// Si le bloc est invalide (saisie en cours), plusieurs variantes de la ligne du curseur sont essayées, et les autres
/// lignes en erreur sont neutralisées (vidées, en conservant leur indentation et leurs tirets).
/// </summary>
public sealed class YamlCursorContext
{
    /// <summary>
    /// Sentinelle insérée à la position du curseur.
    /// </summary>
    private const string Sentinel = "__tmdcursor__";

    private readonly YamlNode? root;

    private YamlCursorContext(YamlNode? root)
    {
        this.root = root;
        Path = root switch
        {
            null => null,
            _ => Locate(root, string.Empty),
        };
    }

    /// <summary>
    /// Chemin YAML du curseur. Le premier segment est l'objet racine du document courant.
    /// Les éléments de liste sont indicés à partir de 0 (<c>[n]</c>, collés au segment précédent), que la liste soit
    /// en bloc ou inline. Exemples : <c>class.properties[2].domain</c>, <c>class.unique[0][1]</c>,
    /// <c>class.values.A.Code</c>, <c>uses[1]</c>.
    /// Un point final (<c>class.properties[2].</c>) indique que le curseur est sur une clé de l'objet (nouvelle clé en
    /// cours de saisie). À la racine du document, ce cas est représenté par la chaîne ".".
    /// Dans le texte d'un scalaire multiligne, le chemin est celui de sa clé.
    /// Null si la ligne est un séparateur "---", si le curseur est dans un commentaire ou si le bloc est inexploitable.
    /// </summary>
    public string? Path { get; }

    /// <summary>
    /// Lit le document YAML contenant le curseur.
    /// </summary>
    /// <param name="text">Lignes du fichier.</param>
    /// <param name="line">Ligne du curseur (base 0).</param>
    /// <param name="character">Colonne du curseur (base 0).</param>
    /// <returns>Le contexte du curseur.</returns>
    public static YamlCursorContext Parse(string[] text, int line, int character)
    {
        // Extraction du document yaml du curseur
        if (line < 0 || line >= text.Length || character < 0 || text[line].StartsWith("---"))
        {
            return new(root: null);
        }

        var start = line;
        while (start > 0 && !text[start - 1].StartsWith("---"))
        {
            start--;
        }

        var end = line + 1;
        while (end < text.Length && !text[end].StartsWith("---"))
        {
            end++;
        }

        var lines = text[start..end].ToList();
        var cursor = line - start;
        // Variantes générées à la demande : la première suffit dans le cas courant.
        YamlCursorContext? TryVariants(List<string> blockLines)
        {
            foreach (var variant in GetCursorLineVariants(blockLines, cursor, character).Distinct())
            {
                if (TryLoadNeutralizing(blockLines, cursor, variant, out var loaded))
                {
                    return new(loaded);
                }
            }

            return null;
        }

        // Dernier recours : les "[" ou "{" non fermés avant le curseur font signaler l'erreur plus bas que sa cause.
        return TryVariants(lines)
            ?? TryVariants(
                lines
                    .Select((l, i) => i < cursor && GetClosers(l, out _) is { Length: > 0 } ? Neutralize(l) : l)
                    .ToList()
            )
            ?? new(root: null);
    }

    /// <summary>
    /// Trouve un noeud à partir d'un chemin YAML (<c>class.properties[0].alias.</c>). "." et "" désignent la racine.
    /// </summary>
    /// <param name="path">Chemin YAML.</param>
    /// <returns>Le noeud, ou null s'il est introuvable.</returns>
    public YamlNode? Find(string path)
    {
        var node = root;
        foreach (var segment in path.TrimEnd('.').Split('.', StringSplitOptions.RemoveEmptyEntries))
        {
            var bracket = segment.IndexOf('[');
            var key = bracket < 0 ? segment : segment[..bracket];
            if (key.Length > 0)
            {
                node = node is YamlMappingNode mapping ? Child(mapping, key) : null;
            }

            while (bracket >= 0)
            {
                var close = segment.IndexOf(']', bracket);
                if (
                    close < 0
                    || !int.TryParse(
                        segment[(bracket + 1)..close],
                        NumberStyles.None,
                        CultureInfo.InvariantCulture,
                        out var index
                    )
                )
                {
                    return null;
                }

                node =
                    node is YamlSequenceNode sequence && index < sequence.Children.Count
                        ? sequence.Children[index]
                        : null;
                bracket = segment.IndexOf('[', close);
            }

            if (node == null)
            {
                return null;
            }
        }

        return node;
    }

    /// <summary>
    /// Clés de l'objet désigné par le chemin, hors clé sous le curseur.
    /// </summary>
    /// <param name="objectPath">Chemin de l'objet, point final inclus.</param>
    /// <returns>Les clés présentes.</returns>
    public ISet<string> GetKeys(string objectPath)
    {
        var keys = new HashSet<string>();
        if (Find(objectPath) is not YamlMappingNode mapping)
        {
            return keys;
        }

        foreach (var (keyNode, _) in mapping.Children)
        {
            if (keyNode is YamlScalarNode { Value: { Length: > 0 } key } && !key.Contains(Sentinel))
            {
                keys.Add(key);
            }
        }

        return keys;
    }

    /// <summary>
    /// Valeur scalaire d'une clé de l'objet désigné par le chemin (sans la sentinelle si elle est en cours de saisie).
    /// </summary>
    /// <param name="objectPath">Chemin de l'objet, point final inclus.</param>
    /// <param name="key">Nom de la clé.</param>
    /// <returns>La valeur, ou null si elle est absente ou vide.</returns>
    public string? GetScalar(string objectPath, string key)
    {
        return
            Find(objectPath) is YamlMappingNode mapping
            && Child(mapping, key) is YamlScalarNode { Value: { } value }
            && value.Replace(Sentinel, string.Empty) is { Length: > 0 } scalar
            ? scalar
            : null;
    }

    private static YamlNode? Child(YamlMappingNode mapping, string key)
    {
        return mapping.Children.FirstOrDefault(kv => kv.Key is YamlScalarNode { Value: var v } && v == key).Value;
    }

    /// <summary>
    /// Index de la fin de l'indentation et des tirets de liste en tête de ligne.
    /// </summary>
    private static int ContentStart(string line, bool skipDashes)
    {
        var i = 0;
        while (i < line.Length && line[i] == ' ')
        {
            i++;
        }

        while (skipDashes && i < line.Length && line[i] == '-' && (i + 1 == line.Length || line[i + 1] == ' '))
        {
            i++;
            while (i < line.Length && line[i] == ' ')
            {
                i++;
            }
        }

        return i;
    }

    /// <summary>
    /// Indique si le texte se termine dans une variable de template ouverte (<c>List&lt;{T</c>, <c>({value:</c>) :
    /// un "{" non refermé collé au caractère qui le précède. Un "{" en début de valeur ou après un séparateur ouvre au
    /// contraire un flux YAML.
    /// </summary>
    private static bool EndsInTemplateVariable(string text)
    {
        var open = text.LastIndexOf('{');
        return open > 0 && text.IndexOf('}', open) < 0 && text[open - 1] is not (' ' or '[' or '{' or ',' or ':');
    }

    /// <summary>
    /// "]" et "}" à ajouter pour fermer les "[" et "{" ouverts dans le texte (guillemets et commentaires ignorés).
    /// </summary>
    /// <param name="text">Texte à analyser.</param>
    /// <param name="quote">Guillemet resté ouvert, ou null.</param>
    /// <returns>Les fermants, dans l'ordre.</returns>
    private static string GetClosers(string text, out char? quote)
    {
        var closers = new Stack<char>();
        quote = null;
        var escaped = false;
        for (var i = 0; i < text.Length; i++)
        {
            var c = text[i];
            if (quote != null)
            {
                if (escaped)
                {
                    // Caractère échappé ("\x" entre guillemets doubles, "''" entre apostrophes) : ignoré.
                    escaped = false;
                }
                else if (
                    c == '\\' && quote == '"'
                    || c == '\'' && quote == '\'' && i + 1 < text.Length && text[i + 1] == '\''
                )
                {
                    escaped = true;
                }
                else
                {
                    quote = c == quote ? null : quote;
                }
            }
            else if (c is '"' or '\'' && (i == 0 || text[i - 1] is ' ' or '[' or '{' or ',' or ':'))
            {
                quote = c;
            }
            else if (c == '#' && (i == 0 || text[i - 1] == ' '))
            {
                break;
            }
            else if (c is '{' or '[')
            {
                closers.Push(c == '{' ? '}' : ']');
            }
            else if (c is '}' or ']' && closers.Count > 0 && closers.Peek() == c)
            {
                closers.Pop();
            }
        }

        return string.Concat(closers);
    }

    /// <summary>
    /// Variantes de la ligne du curseur avec la sentinelle, par ordre de préférence.
    /// </summary>
    private static IEnumerable<string> GetCursorLineVariants(List<string> lines, int cursor, int character)
    {
        var line = lines[cursor];

        // Ligne vide : nouvelle clé (ou valeur) à l'indentation du curseur.
        if (string.IsNullOrWhiteSpace(line))
        {
            var padding = new string(' ', character);
            yield return $"{padding}{Sentinel}:";
            yield return $"{padding}{Sentinel}";
            yield break;
        }

        var contentStart = ContentStart(line, skipDashes: true);
        var position = Math.Max(Math.Min(character, line.Length), contentStart);
        var prefix = line[..position];
        var suffix = line[position..];
        // Espace après le ':' d'une clé ou après un tiret, mais pas après le ':' d'une transformation ("{value:").
        var sentinel =
            prefix.EndsWith(':') && !EndsInTemplateVariable(prefix) || prefix.Trim() == "-" ? $" {Sentinel}" : Sentinel;

        // Clé en cours de saisie (pas de ':' de clé, ni de tiret, ni de flux).
        // (un élément de liste après un tiret reste ambigu : scalaire ou début d'objet, on garde le scalaire).
        var content = line[ContentStart(line, skipDashes: false)..];
        if (content[0] is not ('-' or '[' or '{' or '#') && !content.Contains(": ") && !content.EndsWith(':'))
        {
            yield return $"{prefix}{sentinel}{suffix}:";
        }

        yield return $"{prefix}{sentinel}{suffix}";

        // Suite de la ligne retirée et flux ouverts refermés (sur la ligne, puis depuis le début du bloc).
        var closers = GetClosers(prefix, out var quote);
        yield return $"{prefix}{sentinel}{quote}{closers}";

        var blockClosers = GetClosers(string.Join('\n', lines.Take(cursor).Append(prefix)), out _);
        yield return $"{prefix}{sentinel}{quote}{blockClosers}";
    }

    /// <summary>
    /// Chemin de la sentinelle dans le noeud, ou null si elle n'y est pas.
    /// </summary>
    private static string? Locate(YamlNode node, string path)
    {
        switch (node)
        {
            case YamlScalarNode { Value: { } value } when value.Contains(Sentinel):
                return path;
            case YamlMappingNode mapping:
                foreach (var (key, value) in mapping.Children)
                {
                    var keyName = key is YamlScalarNode { Value: { } k } ? k : string.Empty;
                    if (keyName.Contains(Sentinel))
                    {
                        return $"{path}.";
                    }

                    if (Locate(value, path.Length == 0 ? keyName : $"{path}.{keyName}") is { } found)
                    {
                        return found;
                    }
                }

                return null;
            case YamlSequenceNode sequence:
                for (var i = 0; i < sequence.Children.Count; i++)
                {
                    if (
                        Locate(sequence.Children[i], string.Create(CultureInfo.InvariantCulture, $"{path}[{i}]")) is
                        { } found
                    )
                    {
                        return found;
                    }
                }

                return null;
            default:
                return null;
        }
    }

    /// <summary>
    /// Vide une ligne en conservant son indentation et ses tirets de liste (pour ne pas décaler les indices).
    /// </summary>
    private static string Neutralize(string line)
    {
        return line[..ContentStart(line, skipDashes: true)];
    }

    /// <summary>
    /// Charge le YAML. YamlDotNet lève parfois une <see cref="InvalidOperationException" /> (au lieu d'une
    /// <see cref="YamlException" />) sur certaines saisies incomplètes, par exemple une séquence inline non fermée.
    /// </summary>
    /// <param name="lines">Lignes à charger.</param>
    /// <param name="root">Noeud racine.</param>
    /// <param name="errorLine">Ligne de l'erreur (base 0), ou -1 si elle est inconnue.</param>
    private static bool TryLoad(List<string> lines, out YamlNode? root, out int errorLine)
    {
        try
        {
            var stream = new YamlStream();
            stream.Load(new StringReader(string.Join('\n', lines)));
            root = stream.Documents.Count > 0 ? stream.Documents[0].RootNode : null;
            errorLine = -1;
            return root != null;
        }
        catch (Exception e) when (e is YamlException or InvalidOperationException)
        {
            root = null;
            errorLine = e is YamlException { Start.Line: > 0 } ye ? (int)ye.Start.Line - 1 : -1;
            return false;
        }
    }

    /// <summary>
    /// Charge le bloc avec une variante de la ligne du curseur, en neutralisant les autres lignes en erreur.
    /// </summary>
    private static bool TryLoadNeutralizing(List<string> lines, int cursor, string cursorLine, out YamlNode? root)
    {
        var work = new List<string>(lines) { [cursor] = cursorLine };
        var neutralized = new HashSet<int>();
        while (true)
        {
            if (TryLoad(work, out root, out var errorLine))
            {
                return true;
            }

            if (errorLine < 0 || errorLine >= work.Count || errorLine == cursor || !neutralized.Add(errorLine))
            {
                return false;
            }

            work[errorLine] = Neutralize(work[errorLine]);
        }
    }
}

