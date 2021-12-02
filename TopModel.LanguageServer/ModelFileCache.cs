using System.Collections.Concurrent;

public class ModelFileCache
{
    private readonly ConcurrentDictionary<string, string> _fileCache = new();

    public string GetFile(string filePath)
    {
        return _fileCache[filePath];
    }

    public void UpdateFile(string filePath, string content)
    {
        _fileCache.AddOrUpdate(filePath, content, (_, _) => content);
    }
}
