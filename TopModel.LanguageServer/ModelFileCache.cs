using System.Collections.Concurrent;

namespace TopModel.LanguageServer;

public class ModelFileCache
{
    private readonly ConcurrentDictionary<string, string[]> _fileCache = new();

    public string[] GetFile(string filePath)
    {
        return _fileCache[filePath];
    }

    public void UpdateFile(string filePath, string content)
    {
        var lines = SplitToLines(content).ToArray();
        _fileCache.AddOrUpdate(filePath, lines, (_, _) => lines);
    }

    private static IEnumerable<string> SplitToLines(string input)
    {
        if (input == null)
        {
            yield break;
        }

        using var reader = new StringReader(input);
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            yield return line;
        }
    }
}
