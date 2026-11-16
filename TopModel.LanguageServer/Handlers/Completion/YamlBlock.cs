using System.Globalization;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace TopModel.LanguageServer.Handlers;

/// <summary>
/// Bloc YAML courant (entre deux lignes <c>---</c>) lu avec YamlDotNet en mode "best effort" :
/// si le bloc est invalide (saisie en cours), on répare la ligne du curseur, puis on retire des lignes (celle du
/// curseur, puis à partir de la ligne en erreur) jusqu'à obtenir un YAML valide.
/// </summary>
public sealed class YamlBlock
{
    private readonly int character;
    private readonly int cursorLine;
    private readonly YamlNode? root;
    private readonly bool truncatedBeforeCursor;

    /// <summary>
    /// Constructeur privé : un bloc se construit avec <see cref="Parse" />.
    /// </summary>
    /// <param name="root">Noeud racine du bloc, ou null s'il est inexploitable.</param>
    /// <param name="cursorLine">Indice de la ligne du curseur dans le bloc, ou -1 si cette ligne a été retirée.</param>
    /// <param name="character">Colonne du curseur.</param>
    /// <param name="truncatedBeforeCursor">Vrai si des lignes précédant le curseur ont dû être retirées.</param>
    private YamlBlock(YamlNode? root, int cursorLine, int character, bool truncatedBeforeCursor)
    {
        this.character = character;
        this.cursorLine = cursorLine;
        this.root = root;
        this.truncatedBeforeCursor = truncatedBeforeCursor;
    }

    /// <summary>
    /// Lit le bloc YAML contenant le curseur.
    /// </summary>
    /// <param name="text">Lignes du fichier.</param>
    /// <param name="line">Ligne du curseur (base 0).</param>
    /// <param name="character">Colonne du curseur (base 0).</param>
    /// <returns>Le bloc lu.</returns>
    public static YamlBlock Parse(string[] text, int line, int character)
    {
        if (line < 0 || line >= text.Length)
        {
            return new(root: null, cursorLine: -1, character, truncatedBeforeCursor: false);
        }

        var start = YamlPathAnalyzer.GetDocumentStart(text, line);

        var end = text.Length;
        for (var i = line + 1; i < text.Length; i++)
        {
            if (text[i].StartsWith("---"))
            {
                end = i;
                break;
            }
        }

        var lines = text[start..end].ToList();
        var cursor = line - start;

        // 1. Bloc tel quel.
        if (TryLoad(lines, out var root, out var errorLine))
        {
            return new(root, cursor, character, truncatedBeforeCursor: false);
        }

        // 2. Ligne du curseur réparée (coupée au séparateur précédent, avec fermeture des "{" et "[" ouverts).
        var repaired = RepairLine(lines[cursor], character);
        if (repaired != null)
        {
            var original = lines[cursor];
            lines[cursor] = repaired;
            if (TryLoad(lines, out root, out _))
            {
                return new(root, cursor, character, truncatedBeforeCursor: false);
            }

            lines[cursor] = original;
        }

        // 3. Sans la ligne du curseur.
        var withoutCursor = lines.Where((_, i) => i != cursor).ToList();
        if (TryLoad(withoutCursor, out root, out _))
        {
            return new(root, cursorLine: -1, character, truncatedBeforeCursor: false);
        }

        // 4. Tronqué à la ligne en erreur (puis, si besoin, ligne par ligne).
        var count = lines.Count;
        while (count > 0)
        {
            count = errorLine >= 0 && errorLine < count ? errorLine : count - 1;
            if (TryLoad(lines.Take(count).ToList(), out root, out errorLine))
            {
                return new(
                    root,
                    cursorLine: count > cursor ? cursor : -1,
                    character,
                    truncatedBeforeCursor: count < cursor
                );
            }
        }

        // 5. Bloc inexploitable.
        return new(root: null, cursorLine: -1, character, truncatedBeforeCursor: false);
    }

    /// <summary>
    /// Trouve un noeud à partir d'un chemin YAML (<c>class.properties[0].alias.</c>). "." et "" désignent la racine.
    /// </summary>
    /// <param name="path">Chemin YAML.</param>
    /// <returns>Le noeud, ou null s'il est introuvable.</returns>
    public YamlNode? Find(string path)
    {
        if (root == null || (truncatedBeforeCursor && path.Contains('[')))
        {
            return null;
        }

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
        if (FindMapping(objectPath) is not { } mapping)
        {
            return keys;
        }

        foreach (var (keyNode, _) in mapping.Children)
        {
            if (keyNode is not YamlScalarNode { Value: { Length: > 0 } key })
            {
                continue;
            }

            // Les Mark sont en base 1.
            var underCursor =
                cursorLine >= 0
                && keyNode.Start.Line - 1 == cursorLine
                && keyNode.Start.Column - 1 <= character
                && character <= keyNode.End.Column - 1;
            if (!underCursor)
            {
                keys.Add(key);
            }
        }

        return keys;
    }

    /// <summary>
    /// Valeur scalaire d'une clé de l'objet désigné par le chemin.
    /// </summary>
    /// <param name="objectPath">Chemin de l'objet, point final inclus.</param>
    /// <param name="key">Nom de la clé.</param>
    /// <returns>La valeur, ou null si elle est absente ou vide.</returns>
    public string? GetScalar(string objectPath, string key)
    {
        return FindMapping(objectPath) switch
        {
            null => null,
            var mapping => Child(mapping, key) is YamlScalarNode { Value: { Length: > 0 } scalar } ? scalar : null,
        };
    }

    private static YamlNode? Child(YamlMappingNode mapping, string key)
    {
        return mapping.Children.FirstOrDefault(kv => kv.Key is YamlScalarNode { Value: var v } && v == key).Value;
    }

    /// <summary>
    /// Coupe une ligne au dernier séparateur (<c>,</c>, <c>{</c>, <c>[</c> ou <c>:</c>) avant le curseur et ferme les
    /// "{" et "[" restés ouverts.
    /// </summary>
    /// <returns>La ligne réparée, ou null si elle ne contient aucun séparateur avant le curseur.</returns>
    private static string? RepairLine(string line, int character)
    {
        var prefix = line[..Math.Min(character, line.Length)];
        var cut = prefix.LastIndexOfAny([',', '{', '[', ':']);
        if (cut < 0)
        {
            return null;
        }

        prefix = prefix[..(cut + 1)];
        var closers = new Stack<char>();
        char? quote = null;
        foreach (var c in prefix)
        {
            if (quote != null)
            {
                quote = c == quote ? null : quote;
            }
            else if (c is '"' or '\'')
            {
                quote = c;
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

        return quote != null ? null : $"{prefix} {string.Concat(closers)}";
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
            return true;
        }
        catch (Exception e) when (e is YamlException or InvalidOperationException)
        {
            root = null;
            errorLine = e is YamlException { Start.Line: > 0 } ye ? (int)ye.Start.Line - 1 : -1;
            return false;
        }
    }

    private YamlMappingNode? FindMapping(string path) => Find(path) as YamlMappingNode;
}

