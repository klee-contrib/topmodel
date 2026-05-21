using System.Text.RegularExpressions;

namespace TopModel.Utils.Cli;

/// <summary>
/// Localise les fichiers de configuration TopModel dans un répertoire.
/// Logique partagée entre TopModel.Generator et TopModel.LanguageServer.
/// </summary>
public static partial class ConfigUtils
{
    /// <summary>
    /// Cherche les fichiers de config depuis un répertoire racine.
    /// Stratégie :
    ///   1. Descend récursivement jusqu'à profondeur 3.
    ///   2. Si rien trouvé, remonte l'arbre jusqu'au premier niveau qui en contient.
    /// </summary>
    public static IEnumerable<FileInfo> FindConfigFiles(string rootPath, Regex pattern)
    {
        var found = new List<FileInfo>();

        SearchDescending(pattern, rootPath, found, depth: 0);

        if (found.Count == 0)
        {
            var dir = rootPath;
            while (dir != null)
            {
                dir = Directory.GetParent(dir)?.FullName;
                if (dir == null)
                    break;

                var matches = Directory
                    .EnumerateFiles(dir)
                    .Where(f => pattern.IsMatch(f))
                    .Select(f => new FileInfo(f))
                    .ToList();

                if (matches.Count > 0)
                {
                    found.AddRange(matches);
                    break;
                }
            }
        }

        return found;
    }

    private static void SearchDescending(Regex configPattern, string dirName, List<FileInfo> found, int depth)
    {
        if (depth > 3)
            return;

        foreach (var entryName in Directory.EnumerateFileSystemEntries(dirName))
        {
            if (Directory.Exists(entryName))
            {
                SearchDescending(configPattern, entryName, found, depth + 1);
            }
            else if (configPattern.IsMatch(entryName))
            {
                found.Add(new FileInfo(entryName));
            }
        }
    }
}
