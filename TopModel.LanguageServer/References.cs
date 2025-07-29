using TopModel.Core.FileModel;

namespace TopModel.LanguageServer;

public class References(object? objet, IEnumerable<(Reference Reference, ModelFile File)> enumerable)
    : List<(Reference Reference, ModelFile File)>(enumerable)
{
    public object Objet { get; } = objet ?? new();
}
